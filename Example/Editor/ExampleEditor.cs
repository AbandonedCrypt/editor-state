using AbandonedCrypt.EditorState;
using AbandonedCrypt.Example;
using UnityEditor;
using UnityEngine;

public class ExampleEditor : StatefulEditorWindow
{
  /*
  StateVar are declared in the class' global scope
  */
  private StateVar<int> _count;

  [MenuItem("AbandonedCrypt/EditorState/ExampleEditor")]
  public static void ShowExample()
  {
    ExampleEditor wnd = GetWindow<ExampleEditor>();
    wnd.titleContent = new GUIContent("ExampleEditor");
  }

  protected override void Init()
  {
    useRenderTree = true;

    /*
    StateVar are initialized in Init(), as they require a reference to their parent state host.
    */
    _count = new(this, 3, "Count");
  }

  protected override void Render()
  {
    /*
    (Recommended for parameterless components)
    Use the VisualElement extension method AddComponent<T>() to add a component to the render tree.
    This approach helps avoid potential side effects by ensuring correct parent-child relationships in the element tree.
    Incorrect parent elements can lead to unexpected behavior in components.
    */
    rootVisualElement.AddComponent<Counter>(this);
    rootVisualElement.AddComponent<Incrementor>(this);

    /*
    (Required for components with constructor arguments)
    Alternatively, you can employ the AddComponent() methods from StatefulWindow and EditorComponent.
    These methods achieve the same result as the VisualElement overload but require you to provide
    the parent element to the constructor. This enables passing down any necessary parameters via ctor args.
    */
    AddComponent(new Title(rootVisualElement, "I am a bottom title."));
  }
}
