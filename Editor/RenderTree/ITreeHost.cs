using System.Collections.Generic;
using UnityEngine.UIElements;

namespace AbandonedCrypt.EditorState
{
  internal interface ITreeHost
  {
    internal RenderTreeManager RenderTreeManager { get; }
  }
}