using System;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine.UIElements;

namespace AbandonedCrypt.EditorState
{
  public abstract class TreeComponent : IRenderTreeNode, IComponentStateHost
  {
    internal Guid Guid { get; } = Guid.NewGuid();

    internal IRenderTreeNode Parent { get; private set; }
    internal List<IRenderTreeNode> Children { get; } = new();
    internal List<IRenderTreeNode> StubNodes { get; } = new();

    internal bool IsDirty { get; set; }

    internal VisualElement renderedComponent;
    internal List<IStateVar> stateVars = new();

    internal IStateHost RootStateHost { get; set; }

    // ! we attach the renderedElement to this! we do not manipulate it or return it as part of renderedElement
    internal VisualElement root;

    protected int HierarchyDepth { get; private set; }
    protected VisualElement component = new();

    protected Action reRenderHook;

    VisualElement IRenderTreeNode.RootVisualElement { get => root; set => root = value; }
    VisualElement IRenderTreeNode.RenderedComponent => renderedComponent;

    IRenderTreeNode IRenderTreeNode.Parent { get => Parent; set => Parent = value; }
    List<IRenderTreeNode> IRenderTreeNode.Children { get => Children; }

    bool IRenderTreeNode.IsDirty => IsDirty;
    Guid IRenderTreeNode.Guid => Guid;

    IStateHost IComponentStateHost.RootStateHost { get => RootStateHost; set => RootStateHost = value; }
    List<IStateVar> IComponentStateHost.StateVars => stateVars;

    int IRenderTreeNode.HierarchyDepth => HierarchyDepth;

    protected TreeComponent(VisualElement componentRoot) : this()
    {
      ValidateComponentRoot(componentRoot);
      root = componentRoot;
    }

    internal TreeComponent()
    {
    }

    internal void Initialize()
    {
      Init();
      InitialRender();
    }

    ~TreeComponent()
    {
      OnDestroy();
    }

    protected abstract void Init();

    protected abstract VisualElement Render();

    protected void AddComponent(TreeComponent component)
    {
      component.Parent = this;
      component.RootStateHost = RootStateHost;
      Children.Add(component);
    }

    private void ValidateComponentRoot(VisualElement componentRoot)
    {
      if (componentRoot == null)
        throw new ComponentRootNotFoundException($"The provided VisualElement could not be found in the parent element tree.");

      HierarchyDepth = componentRoot.GetDepth();

      if (Parent == null) return;

      if (HierarchyDepth >= Parent.RootVisualElement.GetDepth())
        throw new ComponentRootHierarchyException(
          "Invalid Component Hierarchy: The root element of a component must be a descendant of the parent component's root element.\n" +
          "The current component root element is positioned higher in the element tree than its parent component. " +
          "Please ensure that the current component root is correctly nested within the parent component's structure."
          );
    }

    private void OnDestroy()
    {
      if (Parent != null)
        _ = Parent.Children.RemoveAll(child => Guid.Equals(child.Guid, Guid));
      stateVars.ForEach(sv => sv.UnsubscribeComponent(this));
    }

    internal void InitialRender()
    {
      renderedComponent = Render();
      // GetStubNodes();
    }

    internal void ReRender()
    {
      ClearTree();
      reRenderHook?.Invoke();
      renderedComponent = Render();
      // GetStubNodes();
      IsDirty = false;
    }

    /// <summary>
    /// Marks the current and all child nodes dirty for rerender
    /// </summary>
    internal void SetDirty()
    {
      IsDirty = true;
      //TraverseDown((node) => node.SetDirty());
    }

    private void ClearTree()
    {
      root.Remove(component);
      component.Clear();
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

    /// <summary>
    /// Only called by reflection instantiation to emulate internal constructor.
    /// </summary>
    void IRenderTreeNode.Initialize() => Initialize();
  }
}