namespace AbandonedCrypt.EditorState
{
  public class StateVarNotInitializedException : System.Exception
  {
    public StateVarNotInitializedException(string message) : base(message) { }
  }
}