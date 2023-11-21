namespace AbandonedCrypt.EditorState
{
  public interface IStateHost
  {
    internal void ReRender();
    internal StateManager StateManager { get; }
    internal bool UseAutomaticStateBatching { get; }
    public StateRepository StateRepository { get; }
  }
}