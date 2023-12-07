using System;
using System.Linq.Expressions;
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

    public static void AddComponent<T>(this VisualElement element, IRenderTreeNode parent) where T : class, IRenderTreeNode
    {
      T component = InstantiationHelper.CreateInstance<T>(element);
      component.RootVisualElement = element;
      component.Parent = parent;
      component.RootStateHost = parent.RootStateHost;
      component.Initialize();
      parent.Children.Add(component);
    }

    public static void AddComponent<T>(this VisualElement element, IStateHost parent) where T : class, IRenderTreeNode
    {
      T component = InstantiationHelper.CreateInstance<T>(element);
      component.RootVisualElement = element;
      component.RootStateHost = parent;
      component.Initialize();
      parent.RenderTreeManager.AddNode(component);
    }
  }
}