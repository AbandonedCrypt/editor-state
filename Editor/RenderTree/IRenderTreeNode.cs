using System.Collections.Generic;
using UnityEngine.UIElements;

namespace AbandonedCrypt.EditorState
{
  internal interface IRenderTreeNode
  {
    internal IRenderTreeNode Parent { get; set; }
    internal List<IRenderTreeNode> Children { get; }
    internal VisualElement rootVisualElement { get; }
    internal void ReRender();
    internal void SetDirty();
  }
}