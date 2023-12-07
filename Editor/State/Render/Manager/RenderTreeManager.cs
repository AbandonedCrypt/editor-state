
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UIElements;

namespace AbandonedCrypt.EditorState
{
  internal class RenderTreeManager
  {
    private List<IRenderTreeNode> Nodes { get; }
    private VisualElement hostRootElement;


    public RenderTreeManager()
    {
      Nodes = new();
    }

    public void Initialize(IStateHost host)
    {
      hostRootElement = host.RootVisualElement;
    }

    public void AddNode(IRenderTreeNode component)
    {
      Nodes.Add(component);
    }

    internal void InitiateRender()
    {
      if (hostRootElement == null)
      {
        Debug.LogError("RenderTreeManager was not initialized, rootVisualElement is null.");
        return;
      }

      var previousRoot = hostRootElement;
      TraverseDown(Nodes, (node) =>
      {
        if (node.IsDirty)
        {
          node.ReRender();
        }
        previousRoot.Add(node.RenderedComponent);
        previousRoot = node.RootVisualElement;
      });
    }

    /// <summary>
    /// Recursively iterate the component tree and execute an action on each node.
    /// </summary>
    /// <param name="action"></param>
    internal void TraverseDown(List<IRenderTreeNode> nodes, Action<IRenderTreeNode> action)
    {
      foreach (var child in nodes)
      {
        if (child != null)
        {
          action(child);
          TraverseDown(child.Children, action);
        }
      }
    }
  }
}