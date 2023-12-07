using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AbandonedCrypt.EditorState
{
  public abstract class StatefulEditorWindow : EditorWindow, IStateHost, ITreeHost
  {
    // & core
    [SerializeField]
    protected VisualTreeAsset m_VisualTreeAsset = default;
    protected VisualElement root;

    /// <summary>
    /// undocumented
    /// </summary>
    protected Action reRenderHook;

    /// <summary>
    /// Path to the UXML file containing the desired root visual tree of this editor.<br/>
    /// <i>Starting at "Assets/..."</i>
    /// </summary>
    protected string uxmlSource = "";

    // & State Management
    /// <summary>
    /// Controls whether automatic state updating batching is used.<br/>
    /// Manual state update batching is now obsolete and automatic batching is therefore <b>opt-out</b>.<br/><br/>
    /// <i>Set this to false if you must use manual batching, this should only be necessary if you run into performance issues with automatic batching,<br/>which should basically never happen.</i>
    /// </summary>
    protected bool useAutomaticBatching = true;
    bool IStateHost.UseAutomaticStateBatching => useAutomaticBatching;

    protected bool useRenderTree = false;

    private bool _batching;
    private readonly StateManager _stateManager;
    StateManager IStateHost.StateManager => _stateManager;

    // & State Repository
    private readonly StateRepository _stateRepository = new();
    public StateRepository StateRepository => _stateRepository;

    // & Render Tree
    private readonly RenderTreeManager renderTreeManager;

    internal RenderTreeManager RenderTreeManager => renderTreeManager;
    RenderTreeManager ITreeHost.RenderTreeManager => renderTreeManager;

    StateRepository IStateHost.StateRepository => StateRepository;

    bool ITreeHost.UseRenderTree => useRenderTree;

    VisualElement ITreeHost.RootVisualElement => root;

    protected StatefulEditorWindow()
    {
      _stateManager = new(this);
      renderTreeManager = new();
      StateContext.Register(GetType().Name, _stateRepository);
      StateHostLocator.Register(this);
    }

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
    /// Perform any cleanup logic here, will be called by OnDestroy().
    /// </summary>
    protected virtual void OnClose() { }

    protected void AddComponent(EditorComponent component)
    {
      component.RootStateHost = this;
      component.Initialize();
      RenderTreeManager.AddNode(component);
    }

    private void OnDestroy()
    {
      StateContext.Unregister(this.GetType().Name);
      StateHostLocator.Unregister(this);
      OnClose();
    }

    /// <summary>
    /// Reconstructs the whole editor's DOM.<br/><br/>
    /// <i>Implicitly called by stateful variables, or explicitly if desired.</i>
    /// </summary>
    internal void ReRender()
    {
      if (_batching) return;
      reRenderHook?.Invoke();
      rootVisualElement.Clear();
      rootVisualElement.Add(m_VisualTreeAsset.Instantiate());
      if (useRenderTree)
        RenderTreeManager.InitiateRender();
      else
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
        // TODO: Error Handling
        m_VisualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/PopupExample.uxml");
        m_VisualTreeAsset.CloneTree(rootVisualElement);
      }
      root = rootVisualElement;
      renderTreeManager.Initialize(this);
      Init();
      rootVisualElement.Add(m_VisualTreeAsset.Instantiate());
      if (useRenderTree)
      {
        Render();
        RenderTreeManager.InitiateRender();
      }
      else
        Render();
    }

    /// <summary>
    /// Call StartBatching before calling more than 1 StateVar update in a row.<br/>
    /// After setting those StateVars, you must call StopBatching!
    /// </summary>
    private void StartBatching()
    {
      if (useAutomaticBatching) throw new StateUpdateBatchingException("Manual state update batching is prohibited when automatic batching is enabled.");
      _batching = true;
    }

    /// <summary>
    /// Ends StateVar batching and performs a ReRender.
    /// </summary>
    private void StopBatching()
    {
      _batching = false;
      ReRender();
    }

    /// <summary>
    /// All StateVar assignments made in <paramref name="batchOperation"/> will be batched into a single rerender.
    /// </summary>
    /// <param name="batchOperation">Consecutive state variable assignments</param>
    [Obsolete("Manual state update batching has been superseded by automatic batching. Only use manual batching, if you run into performance issues with useAutomaticBatching = true.")]
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

    void ITreeHost.AddComponent(IRenderTreeNode component)
    {
      AddComponent((EditorComponent)component);
    }
  }
}