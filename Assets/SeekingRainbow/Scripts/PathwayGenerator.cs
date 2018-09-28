using UnityEngine;

namespace SeekingRainbow.Scripts
{
  public class PathwayGenerator: MonoBehaviour
  {
    public GameObject Template;
    public LayerMask layers;
    RectInt rect;

    public void Start()
    {
      rect = WallMarker.FindBoundaries();
      Debug.Log("Found boundaries: " + rect);

      rect = WallMarker.Shrink(rect);

      if (Template == null)
      {
        return;
      }

      foreach (var start in rect.allPositionsWithin)
      {
        var end = start + new Vector2(0.2f, 0.2f);
        var targets = Physics2D.Linecast(start, end, layers);
        if (targets.transform != null)
        {
          continue;
        }

        var go = Instantiate(Template, transform);
        go.transform.position = new Vector3(start.x, start.y, transform.position.z);
      }
    }
  }
}