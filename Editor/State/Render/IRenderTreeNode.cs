using System;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine.UIElements;

namespace AbandonedCrypt.EditorState
{
  public interface IRenderTreeNode : IComponentStateHost
  {
    internal Guid Guid { get; }
    internal bool IsDirty { get; }
    internal int HierarchyDepth { get; }
    internal IRenderTreeNode Parent { get; set; }
    internal List<IRenderTreeNode> Children { get; }
    internal VisualElement RootVisualElement { get; set; }
    internal VisualElement RenderedComponent { get; }
    internal void ReRender();
    internal void SetDirty();
    /// <summary>
    /// Manual call to constructor initialization logic.
    /// Use only on reflection instantiation.
    /// </summary>
    internal void Initialize();
  }
}