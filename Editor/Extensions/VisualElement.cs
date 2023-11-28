using UnityEngine.UIElements;

namespace AbandonedCrypt.EditorState
{
  public static class VisualElementExtensions
  {
    public static int GetDepth(this VisualElement element)
    {
      int depth = 0;
      var current = element;
      while (current.parent != null)
      {
        depth++;
        current = current.parent;
      }
      return depth;
    }
  }
}