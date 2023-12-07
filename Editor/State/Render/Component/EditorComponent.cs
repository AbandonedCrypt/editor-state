using System.Diagnostics;
using UnityEngine.UIElements;

namespace AbandonedCrypt.EditorState
{
  public abstract class EditorComponent : TreeComponent
  {
    protected EditorComponent(VisualElement root) : base(root) { }
  }
}