using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UIElements;

namespace AbandonedCrypt.EditorState
{
  public abstract class StatefulPopupWindow : PopupWindowContent, IStateHost
  {
    protected VisualElement root = new();

    private readonly StateRepository stateRepo = new();
    private readonly StateManager stateManager;

    public StateRepository StateRepository => stateRepo;

    protected abstract Vector2 WindowSize { get; }

    // & StateHost stuff
    StateManager IStateHost.StateManager => stateManager;
    bool IStateHost.UseAutomaticStateBatching => true;
    StateRepository IStateHost.StateRepository => stateRepo;

    public StatefulPopupWindow()
    {
      stateManager = new(this);
    }

    protected abstract void Init();
    protected abstract void Render();
    protected abstract void OnExit();

    internal void ReRender()
    {
      root.Clear();
      Render();
    }

    public override Vector2 GetWindowSize()
    {
      return WindowSize;
    }

    public void Show(Rect position)
    {
      UnityEditor.PopupWindow.Show(position, this);
    }

    public static void Show<T>(Rect position) where T : StatefulPopupWindow, new()
    {
      UnityEditor.PopupWindow.Show(position, new T());
    }

    public override void OnOpen()
    {
      StateContext.Register(GetType().Name, stateRepo);
      Init();
      Render();
    }

    public override void OnClose()
    {
      StateContext.Unregister(GetType().Name);
      OnExit();
    }

    public override void OnGUI(Rect rect) { }

    public void Close()
    {
      if (editorWindow == null) return;
      editorWindow.Close();
    }

    void IStateHost.ReRender()
    {
      ReRender();
    }
  }
}