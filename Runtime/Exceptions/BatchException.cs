namespace AbandonedCrypt.EditorState
{
  public class StateUpdateBatchingException : System.Exception
  {
    public StateUpdateBatchingException(string message) : base(message) { }
  }
}