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

### State Batching

The system now supports automatic state batching by default. State modifications no longer trigger immediate re-renders; instead, they are grouped together, combining consecutive state changes into a single re-render. This aims to simplify the update workflow by automatically supporting non-continuous but consecutive updates and reducing unnecessary re-renders.

###### Manual Batching

If need be, manual state batching *can* be enabled by opting out of automatic state batching. Just set the protected variable `useAutomaticBatching` to `false` in the `Init()` method and manually batch consecutive state updates into one re-render, but comes with the drawback, that non-continuous but consecutive state update logic is now required to be moved into the callback.

```C#
BatchStateUpdates(() => {
  flag.Set(true);
  displayMode.Set(DisplayMode.Display);
}))
```



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

- The `uxmlSource` instance field allows you to specify the path to a `uxml` file, serving as the new root for the editor's element tree `<sup><sub>`*(Experimental)*`</sub></sup>`
- Concrete plans to abstract away more boilerplate from the window creation exist, and will likely be realized soon™.
