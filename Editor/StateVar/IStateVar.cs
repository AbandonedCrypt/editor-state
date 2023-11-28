namespace AbandonedCrypt.EditorState
{
  public interface IStateVar
  {
    internal string Name { get; }
    IStateHost GetStateHost();
  }
}