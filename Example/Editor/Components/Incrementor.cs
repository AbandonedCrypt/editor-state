using AbandonedCrypt.EditorState;
using UnityEngine;
using UnityEngine.UIElements;

namespace AbandonedCrypt.Example
{
  public class Incrementor : EditorComponent
  {
    private StateVar<int> _count;

    public Incrementor(VisualElement root) : base(root)
    {
    }

    protected override void Init()
    {
      var stateRepo = StateContext.Find(typeof(ExampleEditor).Name);
      _count = stateRepo.Retrieve<int>("Count", this);
    }

    protected override VisualElement Render()
    {
      component.style.height = Length.Percent(40);
      component.style.justifyContent = Justify.Center;
      component.style.alignItems = Align.Center;

      var button = new Button
      {
        text = "+",
      };
      button.clicked += () =>
      {
        _count.Set(_count.Value + 1);
      };

      component.Add(button);

      return component;
    }
  }
}