using System;
using UnityEngine;

namespace AbandonedCrypt.EditorState
{
  public abstract class StatefulPopupWindow<T> : StatefulPopupWindow
  {
    protected abstract Action<T> Callback { get; set; }

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
        Callback = cb;
      }
    }

    public override void OnClose()
    {
      InvokeCallbackOnClose();
      base.OnClose();
    }
  }
}