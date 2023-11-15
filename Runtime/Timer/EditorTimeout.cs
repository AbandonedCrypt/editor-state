using System;
using UnityEditor;
using UnityEngine;

namespace AbandonedCrypt.EditorState
{
  /// <summary>
  /// Timeout abstraction to do timeouts on the unity editor thread
  /// </summary>
  public class EditorTimeout
  {
    private readonly Action callback;
    private readonly float timeout;

    private double started;
    private bool running;

    public EditorTimeout(float timeoutS, Action onFinish)
    {
      timeout = timeoutS / 1000f;
      callback = onFinish;
      Start();
    }

    private void Start()
    {
      started = EditorApplication.timeSinceStartup;
      EditorApplication.update += CheckTimeout;
      running = true;
    }

    private void Stop()
    {
      EditorApplication.update -= CheckTimeout;
      running = false;
    }

    public void Reset()
    {
      if (running)
      {
        Stop();
        Start();
      }
      else
      {
        Start();
      }
    }

    public void Dispose()
    {
      if (running) Stop();
    }

    private void CheckTimeout()
    {
      double elapsed = EditorApplication.timeSinceStartup - started;
      if (elapsed >= timeout)
      {
        Stop();
        callback();
      }
    }
  }
}