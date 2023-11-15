using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AbandonedCrypt.EditorState
{
  /// <summary>
  /// Stateful editor abstraction allowing for <b>StateVar</b> to be used and controlling re-renders.<br/>
  /// Also provides a more clear editor logic structure.<br/><br/>
  /// <i>Replaces</i> <b>EditorWindow</b> <i>in your custom editor inheritance.</i>
  /// <br/><br/><br/>
  /// <i>!Developers notice!<br/>
  /// This is a highly experimental, rudimentary, personal project, aimed at a specific use-case.<br/>
  /// There is no focus on safety, best practices or any consideration of possible side effects.<br/>
  /// It is merely a highly specific tool to aid a personal use case.</i><br/><br/>
  /// <b>Using it as a general purpose solution is almost guaranteed to run you into bugs galore,<br/>
  /// as it is not tested against any other editor-flows than my own.
  /// </b>
  /// </summary>
  public abstract class StatefulEditorWindow : EditorWindow, IStateHost
  {
    [SerializeField]
    protected VisualTreeAsset m_VisualTreeAsset = default;
    protected VisualElement root;

    /// <summary>
    /// TODO: implement<br/>
    /// will be used to control whether batchmanager takes effect (will limit "framerate")<br/>
    /// vs using manual Batching [StartBatching() / StopBatching()]
    /// </summary>
    protected bool useAutomaticBatching = false;

    /// <summary>
    /// Path to the UXML file containing the desired root visual tree of this editor.<br/>
    /// <i>Starting at "Assets/..."</i>
    /// </summary>
    protected string uxmlSource = "";

    protected bool batching;

    /// <summary>
    /// Perform all your initialization code in here.<br/>
    /// Stateful variables should be initialized here.<br/><br/>
    /// <i>Runs once on window creation.</i>
    /// </summary>
    protected abstract void Init();

    /// <summary>
    /// Place all rendering code in here.<br/><br/>
    /// <i>Runs on re-render, or automatically after a <b><see cref="StateVar">StateVar</see></b> change.</i> 
    /// </summary>
    protected abstract void Render();

    /// <summary>
    /// Reconstructs the whole editor's DOM.<br/><br/>
    /// <i>Implicitly called by stateful variables, or explicitly if desired.</i>
    /// </summary>
    internal void ReRender()
    {
      if (batching) return;
      Debug.Log("Rerender");
      rootVisualElement.Clear();
      rootVisualElement.Add(m_VisualTreeAsset.Instantiate());
      Render();
    }

    /// <summary>
    /// Creates the GUI and initializes all user data.<br/><br/>
    /// <i>If</i>  "<see cref="StatefulEditorWindow.uxmlSource"/>"  <i>has been specified, it will load the root visual tree from that file.</i>
    /// </summary>
    public void CreateGUI()
    {
      if (uxmlSource != "")
      {
        m_VisualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/PopupExample.uxml");
        m_VisualTreeAsset.CloneTree(rootVisualElement);
      }
      root = rootVisualElement;
      Init();
      rootVisualElement.Add(m_VisualTreeAsset.Instantiate());
      Render();
    }

    /// <summary>
    /// Call StartBatching before calling more than 1 StateVar update in a row.<br/>
    /// After setting those StateVars, you must call StopBatching!
    /// </summary>
    private void StartBatching()
    {
      if (useAutomaticBatching) throw new StateUpdateBatchingException("Manual state update batching is prohibited when automatic batching is enabled.");
      batching = true;
    }

    /// <summary>
    /// Ends StateVar batching and performs a ReRender.
    /// </summary>
    private void StopBatching()
    {
      batching = false;
      ReRender();
    }

    /// <summary>
    /// All StateVar assignments made in <paramref name="batchOperation"/> will be batched into a single rerender.
    /// </summary>
    /// <param name="batchOperation">Consecutive state variable assignments</param>
    protected void BatchStateUpdates(Action batchOperation)
    {
      StartBatching();
      batchOperation();
      StopBatching();
    }

    void IStateHost.ReRender()
    {
      ReRender();
    }
  }
}