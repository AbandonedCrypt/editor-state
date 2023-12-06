
using System.Collections.Generic;

namespace AbandonedCrypt.EditorState
{
  internal class RenderTreeManager
  {
    internal List<IRenderTreeNode> Nodes { get; } = new();
    //! TODO: ReRender
  }
}