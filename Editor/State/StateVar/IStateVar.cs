namespace AbandonedCrypt.EditorState
{
  public interface IStateVar
  {
    internal string Name { get; }
    internal void UnsubscribeComponent(IRenderTreeNode component);
    IStateHost GetStateHost();
  }
}