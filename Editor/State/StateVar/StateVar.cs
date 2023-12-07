using System;
using System.Collections.Generic;
using Codice.Client.Common;

namespace AbandonedCrypt.EditorState
{
  /// <summary>
  /// Stateful variable that re-renders the provided editor window when its value changes.<br/><br/>
  /// - <i>You</i> <b>must</b> <i>instantiate this in Init().</i>
  /// <br/><br/>
  /// - implicitly converts and compares to the target type<br/><br/>
  /// </summary>
  public sealed class StateVar<T> : IStateVar
  {
    private readonly IStateHost stateHost;

    private readonly List<IRenderTreeNode> implementingComponents = new();

    private T _value;
    private string _name;
    public bool ComponentLevel { get; private set; } = false;
    public int ComponentHierarchyLevel { get; private set; }

    public T Value
    {
      get => _value;
      private set
      {
        _value = value;
        ReRenderHost();
      }
    }

    internal string Name { get => _name; }

    string IStateVar.Name => Name;

    private StateVar(IStateHost stateHost)
    {
      this.stateHost = stateHost ?? throw new ArgumentNullException();
    }

    public StateVar(IStateHost stateHost, T defaultValue) : this(stateHost)
    {
      _value = defaultValue;
    }

    public StateVar(IStateHost stateHost, T defaultValue, string name) : this(stateHost)
    {
      _value = defaultValue;
      _name = name;

      stateHost.StateRepository.Add(this);
    }

    public StateVar(IRenderTreeNode stateHost, T defaultValue) : this(stateHost.RootStateHost)
    {
      _value = defaultValue;
      ComponentLevel = true;
      ComponentHierarchyLevel = stateHost.HierarchyDepth;

      stateHost.StateVars.Add(this);
      SubscribeComponent(stateHost);
    }

    public StateVar(IRenderTreeNode stateHost, T defaultValue, string name) : this(stateHost.RootStateHost)
    {
      _value = defaultValue;
      _name = name;
      ComponentLevel = true;
      ComponentHierarchyLevel = stateHost.HierarchyDepth;

      stateHost.StateVars.Add(this);
      SubscribeComponent(stateHost);

      stateHost.RootStateHost.StateRepository.Add(this);
    }

    public void Set(T value) => Value = value;

    public void ReRenderHost()
    {
      if (stateHost.UseRenderTree)
      {
        implementingComponents.ForEach(cmp => cmp.SetDirty());
        stateHost.RenderTreeManager.InitiateRender();
        return;
      }

      if (stateHost.UseAutomaticStateBatching)
        stateHost.StateManager.InitiateStateChange();
      else
        stateHost.ReRender();
    }

    internal void SubscribeComponent(IRenderTreeNode component)
    {
      implementingComponents.Add(component);
    }

    internal void UnsubscribeComponent(IRenderTreeNode component)
    {
      implementingComponents.Remove(component);
    }

    public override bool Equals(object obj)
    {
      if (Value == null || obj == null || (obj as StateVar<T>).Value == null)
        return false;
      return Value.Equals((obj as StateVar<T>).Value);
    }

    public static bool operator ==(StateVar<T> left, T right)
    {
      if (left == null)
        throw new StateVarNotInitializedException("Can not implicitly compare StateVar value, as it has not been initialized. Please make sure your StateVar has been initialized in Init().");
      if (right == null)
        return left.Value == null;
      return left.Value.Equals(right);
    }

    public static bool operator !=(StateVar<T> left, T right)
    {
      if (left == null)
        throw new StateVarNotInitializedException("Can not implicitly compare StateVar value, as it has not been initialized. Please make sure your StateVar has been initialized in Init().");
      if (right == null)
        return left.Value != null;
      return !left.Value.Equals(right);
    }

    public static implicit operator T(StateVar<T> stateVar)
    {
      return stateVar.Value;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public IStateHost GetStateHost() => stateHost;

    IStateHost IStateVar.GetStateHost() => GetStateHost();

    void IStateVar.UnsubscribeComponent(IRenderTreeNode component) => UnsubscribeComponent(component);
  }
}