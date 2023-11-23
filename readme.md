# Unity Editor State

## Overview

The Editor State Framework is a custom `EditorWindow` abstraction designed to simplify state management and re-rendering within your custom editor, while trying to mimic a loosely react-ish base approach.

It is designed with *code-first* in mind, as that is my preferred approach in UITK, but is perfectly usable in conjunction with UXML through queries.

It comes with a generic `OnChange` value binding method for VisualElements, which works for all input elements.

##### Disclaimer

It is worthy to note, that this is by no means a professional-level, maintained, general-purpose framework, but rather a tool born out of laziness and necessity in a rather specific use case.

I have some plans for quality of life features or general feature extensions and intentionally left this open for extension.

Hence there are no guarantees made: *Side effects are to be expected, if used outside of the scope of custom utility editors and issues will likely remain unresolved if they do not tangent my editor scripting experience.*

## Concept

### `StatefulEditorWindow`

The `StatefulEditorWindow` is an abstraction layer over Unity's `EditorWindow`. It hides away a lot of the boilerplate coming with creating editor windows and provides a concise number of semantically sound and intuitive base methods to facilitate a more efficient and straight-forward implementation process.

It establishes a state-driven rendering paradigm, where UI logic is encapsulated within the render method. Re-renders are triggered by changes to stateful variables, to create a straightforward development flow.

It is also called `StateHost`, as the core driving 'engine' behind this framework.

### `StateVar`

A stateful variable of generic type, comparable to `useState` in a single field. Modifying it will invoke a re-render.

## Getting started

### Installation

1. Open your unity package manager (`Window > Package Manager`)
2. Click `+ > Add package from git url`
3. Enter `https://github.com/AbandonedCrypt/editor-state.git`
4. You're Done

### Usage

1. Create a new Editor
2. Inherit from `StatefulEditorWindow`
3. Remove everything but the decorated static `Show()` method.
4. Implement the abstract methods `Init()` and `Render()`
5. Add stateful variables

```C#
public class MyEditor : StatefulEditorWindow
{
  // state variables
  private StateVar<bool> flag;
  private StateVar<DisplayMode> displayMode;

  [MenuItem("Your/Custom/Editor")]
  public static void ShowEditor()
  {
    MyEditor wnd = GetWindow<MyEditor>();
    wnd.titleContent = new GUIContent("My Editor");
    wnd.minSize = new Vector2(250f, 360f);
  }

  protected override void Init()
  {
    // StateVar must be assigned in Init()!
    flag = new StateVar<bool>(this, false);
    displayMode = new StateVar<DisplayMode>(this, DisplayMode.Edit);

    // initialize anything else here
  }

  protected override void Render()
  {
    // the root visual element is available as root or rootVisualElement
    rootVisualElement.Add(new Label("Top Label"));
    root.Add(new Label("Bottom Label"));

    // modifying a StateVar will re-render your editor
    if(flag) //StateVars implicitly convert to their assigned type
      root.Add(new Label("I will be visible when 'flag' is assigned 'true'"));
  }
}
```

## Features

### State Update Batching

The system now supports automatic state update batching by default. State modifications no longer trigger immediate re-renders; instead, they are grouped together, combining consecutive state changes into a single re-render. This aims to simplify the update workflow by automatically supporting non-continuous but consecutive updates and reducing unnecessary re-renders.

###### Manual Batching

If need be, manual state batching *can* be enabled by opting out of automatic state batching. Just set the protected variable `useAutomaticBatching` to `false` in the `Init()` method and manually batch consecutive state updates into one re-render, but comes with the drawback, that non-continuous but consecutive state update logic is now required to be moved into the callback.

```C#
BatchStateUpdates(() => {
  flag.Set(true);
  displayMode.Set(DisplayMode.Display);
}))
```

### State Repository

In order to avoid having to pass down state vars by "prop-drilling" in increasingly bloated constructors or method parameters, or breaking your encapsulation by having to make `StateVar` public to access them from children,  `StateVar` can now be initialized as *Repository State Variables*. That means they will automatically be added to the `StateRepository` of their state host, but continue to work as expected. You create a *Repository State Variable* by giving it a `name` at instantiation.

```C#
private StateVar<float> _someFloat = new(parent, .1f, "SomeFloat");
```

After which it will be available in the parents' state repository for retrieval in any child.

```C#
StateVar<float> someFloat = parent.StateRepository.Retrieve<float>("SomeFloat");
```

This way you will now only need to pass down either the State Host (StatefulEditorWindow, as `IStateHost` e.g.) or the `StateRepository` itself.

Each StatefulEditorWindow has a dedicated state repository, to which all of its associated StateVars will be added, as long as they are *repository state vars*.

#### StateContext as a static Repository locator

*You can now use a static service locator `StateContext` to retrieve a state repository instance from the parent without having to pass down any instance of  `IStateHost` or `StateRepository`*

You can find your editor's StateRepository by querying the `StateContext` for the name of your editor from any child component.

```C#
// assume your editor is called MyEditor
var stateRepository = StateContext.Find("MyEditor");

// or more future-proof
var stateRepository = StateContext.Find(typeof(MyEditor).Name);

// retrieve your StateVar from the repository
var someFloat = stateRepository.Retrieve<float>("SomeFloat");
```

---

*Draw-backs by nature of the design are, that stale references to StateVars will not reliably be garbage collected on time. So a reference to a stale StateVar, from an inactive sub-host, might still return its instance. This side-effect is negligible though, if StateVar etiquette is followed* (*don't try to access, what should not be available in your child hierarchy*).

*Yes, there is no access control, so you could use this to re-render / modify any editor window from any other editor window. Should you? No idea. Is it kind of cool? I guess so.*

### StateHost Locator

Passing a state host around to e.g. create `StateVar` at lower hierarchy levels¹ is cumbersome. That's why you can just use the `StateHostLocator` to find a state host at any level in your code.

State host registration to the locator is automatically managed by your `StatefulEditorWindow`.

```C#
// Find by string name of your editor
var stateHost = StateHostLocator.Find("MyEditor");

// or more future proof
var stateHost = StateHostLocator.Find(typeof(MyEditor).Name);

// use your state host however you want then
var newStatefulFloat = new StateVar<float>(stateHost, .5f);
```

*¹ Disclaimer: Lower child hierarchy level `StateVar` are not functionally supported yet. They will be overwritten and recreated at rerender. It's on the board with high priority, though.*


### Generic Value Binding

A generic extension method for `VisualElements` is provided, that offers a declarative `OnChange` method on all derived elements, specifically useful for any type of input field.

```C#
// text field
var input = new TextField();
input.value = someObject.Name;
input.OnChange(value => someObject.Name = value);

// color field
var color = new ColorField();
color.value = someObject.ErrorColor;
color.OnChange(value => someObject.ErrorColor = value);
```

### Additional

- The `uxmlSource` instance field allows you to specify the path to a `uxml` file, serving as the new root for the editor's element tree *(Experimental)*
- Concrete plans to abstract away more boilerplate from the window creation exist, and will likely be realized soon™.
