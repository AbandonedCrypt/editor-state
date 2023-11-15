using System.Threading;

namespace AbandonedCrypt.EditorState
{
  internal class StateManager
  {
    private IStateHost stateHost;

    private bool defering;

    private Timer timer;
    private readonly int stateDelay = 4; // default delay in ms

    public StateManager(IStateHost stateHost)
    {
      this.stateHost = stateHost;
    }

    public void InitiateStateChange()
    {
      if (defering)
      {
        timer.Change(Timeout.Infinite, Timeout.Infinite);
        timer.Dispose();
      }

      timer = new Timer(Flush, null, stateDelay, Timeout.Infinite);

      defering = true;
    }

    private void Flush(object state)
    {
      stateHost.ReRender();
      timer.Dispose();
      defering = false;
    }
  }
}