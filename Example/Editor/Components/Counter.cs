using AbandonedCrypt.EditorState;
using UnityEngine;
using UnityEngine.UIElements;

namespace AbandonedCrypt.Example
{
  public class Counter : EditorComponent
  {
    private StateVar<int> _count;

    public Counter(VisualElement root) : base(root)
    {
    }

    protected override void Init()
    {
      var stateRepository = StateContext.Find(typeof(ExampleEditor).Name);

      // when retrieving inside a component, we must include the component instance in the retrieve call
      _count = stateRepository.Retrieve<int>("Count", this);
    }

    protected override VisualElement Render()
    {
      component.style.height = Length.Percent(40);
      component.style.justifyContent = Justify.Center;
      component.style.alignItems = Align.Center;

      var label = new Label(_count.Value.ToString());

      component.Add(label);
      return component;
    }
  }
}