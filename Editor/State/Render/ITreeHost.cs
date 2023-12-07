using System.Collections.Generic;
using UnityEngine.UIElements;

namespace AbandonedCrypt.EditorState
{
  public interface ITreeHost
  {
    internal bool UseRenderTree { get; }
    internal VisualElement RootVisualElement { get; }
    internal RenderTreeManager RenderTreeManager { get; }
    internal void AddComponent(IRenderTreeNode component);
  }
}