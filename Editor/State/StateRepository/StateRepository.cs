using System;
using System.Collections.Generic;

namespace AbandonedCrypt.EditorState
{
  public sealed class StateRepository
  {
    private readonly Dictionary<string, WeakReference<IStateVar>> repository = new();

    internal void Add(IStateVar stateVar)
    {
      Cleanup();
      if (repository.ContainsKey(stateVar.Name))
        repository.Remove(stateVar.Name);
      repository.Add(stateVar.Name, new WeakReference<IStateVar>(stateVar));
    }

    private StateVar<T> Retrieve<T>(string name, bool priv)
    {
      Cleanup();
      if (repository.TryGetValue(name, out var weakReference))
      {
        if (weakReference.TryGetTarget(out var stateVar) && stateVar is StateVar<T> typedStateVar)
        {
          return typedStateVar;
        }
      }

      throw new StateVarNotFoundException($"A StateVar with the name '{name}' was not found in the StateRepository, has a different type, or has been garbage collected.");
    }

    public StateVar<T> Retrieve<T>(string name)
    {
      var stateVar = Retrieve<T>(name, true);
      if (stateVar.ComponentLevel)
      {
        throw new StateVarInvalidAccessException(
          "Invalid StateVar access. StateVar was initialized inside a component and may only be retrieved from inside such - and only with a component reference.\nIf you are accessing it from within a component, please use the respective `Retrieve<T>` overload."
        );
      }
      return stateVar;
    }

    public StateVar<T> Retrieve<T>(string name, IRenderTreeNode host)
    {
      var stateVar = Retrieve<T>(name, true);

      if (stateVar.ComponentLevel && host.HierarchyDepth >= stateVar.ComponentHierarchyLevel)
      {
        throw new StateVarInvalidAccessException(
          "Invalid StateVar access. StateVar was declared lower in the component tree, than the component trying to access it."
          );
      }

      stateVar.SubscribeComponent(host);
      host.StateVars.Add(stateVar);
      return stateVar;
    }

    private void Cleanup()
    {
      List<string> keysToRemove = new();
      foreach (var kvp in repository)
        if (!kvp.Value.TryGetTarget(out _))
          keysToRemove.Add(kvp.Key);

      foreach (var key in keysToRemove)
        repository.Remove(key);
    }
  }
}