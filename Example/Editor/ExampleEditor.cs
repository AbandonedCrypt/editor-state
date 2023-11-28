using AbandonedCrypt.EditorState;
using UnityEditor;
using UnityEngine;

public class ExampleEditor : StatefulEditorWindow
{
  private StateVar<int> count;

  [MenuItem("EditorState/ExampleEditor")]
  public static void ShowExample()
  {
    ExampleEditor wnd = GetWindow<ExampleEditor>();
    wnd.titleContent = new GUIContent("ExampleEditor");
  }

  protected override void Init()
  {
    count = new(this, 0, "Count");
  }

  protected override void Render()
  {

  }
}
