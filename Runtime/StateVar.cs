using System;
using System.Reflection;

namespace AbandonedCrypt.EditorState
{
  /// <summary>
  /// Stateful variable that re-renders the provided editor window when its value changes.<br/><br/>
  /// - <i>It will not perform a re-render if a value has been assigned, but that value is identical to the previous one.</i>
  /// <br/><br/>
  /// - <i>You</i> <b>must</b> <i>instantiate this in Init().</i>
  /// <br/><br/>
  /// - implicitly converts and compares to the target type<br/><br/>
  /// <i>!Developers notice!<br/>
  /// This is a highly experimental, rudimentary, personal project, aimed at a specific use-case.<br/>
  /// There is no focus on safety, best practices or any consideration of possible side effects.<br/>
  /// It is merely a highly specific tool to aid a personal use case.</i><br/><br/>
  /// <b>Using it as a general purpose solution is almost guaranteed to run you into bugs galore,<br/>
  /// as it is not tested against any other editor-flows than basic utility editors.
  /// </b>
  /// </summary>
  public sealed class StateVar<T>
  {
    private readonly MethodInfo reRenderMethod;
    private readonly StatefulEditorWindow stateHost;

    private T _value;

    public T Value
    {
      get => _value;
      set
      {
        if (_value.Equals(value)) return;
        _value = value;
        ReRenderHost();
      }
    }

    public StateVar(StatefulEditorWindow stateHost)
    {
      if (stateHost == null)
      {
        throw new ArgumentNullException(nameof(stateHost));
      }
      this.stateHost = stateHost;
      reRenderMethod = stateHost.GetType().GetMethod("ReRender", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    public void ReRenderHost()
    {
      reRenderMethod.Invoke(stateHost, null);
    }

    public StateVar(StatefulEditorWindow stateHost, T defaultValue) : this(stateHost)
    {
      _value = defaultValue;
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
      return left.Value.Equals(right);
    }

    public static bool operator !=(StateVar<T> left, T right)
    {
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
  }
}