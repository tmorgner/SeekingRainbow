using System;
using System.Collections.Generic;

namespace SeekingRainbow.Scripts
{
  public class WeakEvent<T>
  {
    List<WeakReference<Action<T>>> listeners;

    public WeakEvent()
    {
    }

    public void Invoke(T value)
    {
      if (listeners == null)
      {
        return;
      }

      for (var i = listeners.Count - 1; i >= 0; i--)
      {
        var r = listeners[i];
        Action<T> storedListener;
        if (r.TryGetTarget(out storedListener))
        {
          storedListener.Invoke(value);
        }
        else
        {
          listeners.RemoveAt(i);
        }
      }
    }

    public void AddListener(Action<T> l)
    {
      if (listeners == null)
      {
        listeners = new List<WeakReference<Action<T>>>();
      }

      listeners.Add(new WeakReference<Action<T>>(l));
    }

    public void RemoveListener(Action<T> l)
    {
      if (listeners == null)
      {
        return;
      }

      for (var i = listeners.Count - 1; i >= 0; i--)
      {
        var r = listeners[i];
        Action<T> storedListener;
        if (r.TryGetTarget(out storedListener))
        {
          if (ReferenceEquals(storedListener, l))
          {
            listeners.RemoveAt(i);
            return;
          }
        }
        else
        {
          listeners.RemoveAt(i);
        }
      }
    }

    public void RemoveListeners()
    {
      listeners?.Clear();
    }
  }
}
