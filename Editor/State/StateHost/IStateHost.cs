namespace AbandonedCrypt.EditorState
{
  public interface IStateHost : ITreeHost
  {
    internal void ReRender();
    internal StateManager StateManager { get; }
    internal bool UseAutomaticStateBatching { get; }
    public StateRepository StateRepository { get; }
  }
}