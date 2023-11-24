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

    public StateVar<T> Retrieve<T>(string name)
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