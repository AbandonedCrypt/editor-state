using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace AbandonedCrypt.EditorState
{
  public abstract class TreeComponent : IRenderTreeNode
  {
    internal Guid Guid { get; } = Guid.NewGuid();

    internal IRenderTreeNode Parent { get; private set; }
    internal List<IRenderTreeNode> Children { get; } = new();
    internal List<IRenderTreeNode> StubNodes { get; } = new();

    internal bool IsDirty { get; set; }
    internal VisualElement renderedComponent;

    protected VisualElement root;

    VisualElement IRenderTreeNode.rootVisualElement => root;
    IRenderTreeNode IRenderTreeNode.Parent { get => Parent; set => Parent = value; }
    List<IRenderTreeNode> IRenderTreeNode.Children { get => Children; }

    bool IRenderTreeNode.IsDirty => IsDirty;
    Guid IRenderTreeNode.Guid => Guid;

    public TreeComponent(string rootElementName) : this()
    {
      GetRootVisualElement(rootElementName);
    }

    internal TreeComponent()
    {
      Init();
    }

    ~TreeComponent()
    {
      OnDestroy();
    }

    protected abstract void Init();

    protected abstract VisualElement Render();

    protected void AddComponent(TreeComponent component)
    {
      Children.Add(component);
      component.Parent = this;
    }

    private void GetRootVisualElement(string rootElementName)
    {
      root = Parent.rootVisualElement.Q<VisualElement>(rootElementName);
      if (root == null)
        throw new ComponentRootNotFoundException($"A VisualElement with the name {rootElementName} could not be found in the provided element tree.");
      if (root.GetDepth() >= Parent.rootVisualElement.GetDepth())
        throw new ComponentRootHierarchyException(
          "Invalid Component Hierarchy: The root element of a component must be a descendant of the parent component's root element.\n" +
          "The current component root element is positioned higher in the element tree than its parent component. " +
          "Please ensure that the current component root is correctly nested within the parent component's structure."
          );
    }

    private void OnDestroy()
    {
      _ = Parent.Children.RemoveAll(child => Guid.Equals(child.Guid, Guid));
    }

    internal void InitialRender()
    {
      renderedComponent = Render();
      // GetStubNodes();
    }

    internal void ReRender()
    {
      ClearTree();
      renderedComponent = Render();
      // GetStubNodes();
    }

    /// <summary>
    /// Marks the current and all child nodes dirty for rerender
    /// </summary>
    internal void SetDirty()
    {
      IsDirty = true;
      TraverseDown((node) => node.SetDirty());
    }

    private void ClearTree()
    {
      root.Clear();
    }

    private void GetStubNodes()
    {
      TraverseDown((node) =>
      {
        if (node.Children.Count == 0)
          StubNodes.Add(node);
      });
    }

    internal void TraverseDown(Action<IRenderTreeNode> action)
    {
      foreach (var child in Children)
      {
        action(child);
        TraverseDown(action);
      }
    }

    void IRenderTreeNode.ReRender() => ReRender();
    void IRenderTreeNode.SetDirty() => SetDirty();
  }
}