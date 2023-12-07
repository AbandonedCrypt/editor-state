using System;
using System.Reflection;
using UnityEngine.UIElements;

namespace AbandonedCrypt.EditorState
{
  public static class InstantiationHelper
  {
    /// <summary>
    /// Tries to find the first constructor that only takes the root VisualElement and creates an instance from it.
    /// </summary>
    /// <typeparam name="T">The type to create an instance from, usually TreeComponent derived</typeparam>
    /// <param name="root">the root visual element of the component</param>
    /// <returns>Instance of T</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static T CreateInstance<T>(VisualElement root) where T : class, IRenderTreeNode
    {
      ConstructorInfo constructor = typeof(T).GetConstructor(new[] { typeof(VisualElement) });
      if (constructor != null)
      {
        return (T)constructor.Invoke(new object[] { root });
      }
      else
      {
        throw new InvalidOperationException($"Constructor with VisualElement parameter not found in {typeof(T).Name}");
      }
    }
  }
}