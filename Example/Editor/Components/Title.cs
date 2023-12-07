using AbandonedCrypt.EditorState;
using UnityEngine;
using UnityEngine.UIElements;

namespace AbandonedCrypt.Example
{
  public class Title : EditorComponent
  {
    private string _title;

    public Title(VisualElement root, string title) : base(root)
    {
      _title = title;
    }

    protected override void Init()
    {
      /*
      This component does not declare, read or set any state, so it will not be rerendered.
      */

      /*
      The reRenderHook provides a way for you to execute logic on re-renders, but you should avoid using it.
      In this case, we expect to never see the Log printed to console, as this component should never be marke dirty.      
      */
      reRenderHook += () => Debug.Log("Title component re-rendered.");
    }

    protected override VisualElement Render()
    {
      component.style.height = Length.Percent(20);
      var label = new Label(_title);
      label.style.unityFontStyleAndWeight = FontStyle.Bold;
      label.style.unityTextAlign = TextAnchor.UpperCenter;
      label.style.fontSize = 20f;

      var subLabel = new Label("I will not be re-rendered.");
      subLabel.style.unityFontStyleAndWeight = FontStyle.Italic;
      subLabel.style.unityTextAlign = TextAnchor.UpperCenter;
      subLabel.style.fontSize = 10f;

      component.Add(label);
      component.Add(subLabel);

      return component;
    }
  }
}