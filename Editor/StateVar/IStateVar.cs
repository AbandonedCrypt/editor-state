namespace AbandonedCrypt.EditorState
{
  public interface IStateVar
  {
    string Name { get; }
    IStateHost GetStateHost();
  }
}