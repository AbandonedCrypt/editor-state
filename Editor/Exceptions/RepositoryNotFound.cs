namespace AbandonedCrypt.EditorState
{
  public class RepositoryNotFoundException : System.Exception
  {
    public RepositoryNotFoundException(string message) : base(message) { }
  }
}