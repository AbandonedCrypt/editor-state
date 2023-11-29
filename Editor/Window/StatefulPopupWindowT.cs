using System.Globalization;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Build;

namespace AbandonedCrypt.EditorState
{
  public abstract class StatefulPopupWindow<T> : StatefulPopupWindow
  {
    protected Action<T> callback;

    public static void Show<U>(Rect position, Action<T> callback) where U : StatefulPopupWindow<T>, new()
    {
      U window = new();
      window.SetCallback(callback);
      UnityEditor.PopupWindow.Show(position, window);
    }

    protected abstract void InvokeCallbackOnClose();

    private void SetCallback(Action<T> cb)
    {
      {
        callback = cb;
      }
    }

    public override void OnClose()
    {
      InvokeCallbackOnClose();
      base.OnClose();
    }
  }
}