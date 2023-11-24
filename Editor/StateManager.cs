using System.Threading;
using UnityEngine;

namespace AbandonedCrypt.EditorState
{
  /// <summary>
  /// StateManager that batches consecutive StateVar changes into a single rerender.<br/>
  /// StateVar changes get a minimum buffer of "stateBufferDelay" milliseconds.<br/><br/>
  /// <i>As the timing is strongly tied to the EditorApplication.update interval, fine grained intervals can not be guaranteed to be checked.</i><br/>
  /// <i>If your editor update cycles turn out to be too long, consider using manual state batching for reliable updates.</i><br/>
  /// </summary>
  internal class StateManager
  {
    private readonly IStateHost stateHost;

    private bool defering;

    private EditorTimeout timer;
    private readonly int stateDelay; // default delay in ms

    public StateManager(IStateHost stateHost, int stateBufferDelay = 2)
    {
      this.stateHost = stateHost;
      stateDelay = stateBufferDelay;
    }

    public void InitiateStateChange()
    {
      if (!defering)
      {
        timer = new(stateDelay, Flush);
      }

      timer.Reset();

      defering = true;
    }

    private void Flush()
    {
      timer.Dispose();
      timer = null;
      defering = false;
      stateHost.ReRender();
    }
  }
}