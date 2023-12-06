using System;
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

    private T _value;
    private string _name;

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

    public void ReRenderHost()
    {
      if (stateHost.UseAutomaticStateBatching)
        stateHost.StateManager.InitiateStateChange();
      else
        stateHost.ReRender();
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

    public void Set(T value) => Value = value;

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

    IStateHost IStateVar.GetStateHost()
    {
      throw new NotImplementedException();
    }
  }
}