using System;
using UnityEngine.UIElements;

namespace AbandonedCrypt.EditorState
{
  public static class ValueBinding
  {
    /// <summary>
    /// Generic value binding extension method for visual elements. Functional on all input fields.
    /// <br/><br/>
    /// For further instructions refer to the <seealso href="https://github.com/AbandonedCrypt/editor-state">documentation</seealso>.
    /// <br/><br/>
    /// <i>If intellisense complains, let it autocomplete the method.</i>
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="visualObject"></param>
    /// <param name="onChange"></param>
    public static void OnChange<U, T>(this U visualObject, Action<T> onChange) where U : VisualElement
    {
      visualObject.RegisterCallback<ChangeEvent<T>>(evt => onChange(evt.newValue));
    }
  }
}