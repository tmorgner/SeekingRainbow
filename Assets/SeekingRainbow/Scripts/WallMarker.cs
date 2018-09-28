using Completed;
using UnityEngine;

namespace SeekingRainbow.Scripts
{
  [RequireComponent(typeof(BoxCollider2D))]
  public class WallMarker : MonoBehaviour
  {
    public static RectInt FindBoundaries()
    {
      var markers = FindObjectsOfType<Wall>();
      if (markers.Length == 0)
      {
        // no markers.
        return new RectInt();
      }

      var rect = new RectInt();
      bool first = true;
      foreach (var m in markers)
      {
        if (!m.enabled)
        {
          continue;
        }
        var position = m.transform.position;
        if (first)
        {
          rect = new RectInt(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), 0, 0);
          first = false;
        }
        else
        {
          rect.x = Mathf.Min(rect.x, Mathf.FloorToInt(position.x));
          rect.y = Mathf.Min(rect.y, Mathf.FloorToInt(position.y));
          var right = Mathf.Max(rect.xMax, Mathf.FloorToInt(position.x));
          rect.width = right - rect.x;
          var top = Mathf.Max(rect.yMax, Mathf.FloorToInt(position.y));
          rect.height = top - rect.y;
        }
      }


      rect.width += 1;
      rect.height += 1;
      Debug.Log("Walls found at " + rect);
      return rect;
    }

    public static RectInt Shrink(RectInt rect, int size = 1)
    {
      if (rect.width > size * 2)
      {
        rect.x += size;
        rect.width -= size * 2;
      }

      if (rect.height > size * 2)
      {
        rect.y += size;
        rect.height -= size * 2;
      }

      return rect;
    }

    public static RectInt Expand(RectInt rect, int size = 1)
    {
      rect.x -= size;
      rect.width += size * 2;
      rect.y -= size;
      rect.height += size * 2;
      return rect;
    }
  }
}
