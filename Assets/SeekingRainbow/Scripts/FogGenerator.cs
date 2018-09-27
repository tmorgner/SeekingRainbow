using UnityEngine;

namespace SeekingRainbow.Scripts
{
  /// <summary>
  ///  Generates fog tiles. This script locates all walls (objects marked with WallMarker) and
  ///  computes a bound box of the area they cover. The insde of that box is then filled with
  ///  FogTemplate clones.
  /// </summary>
  public class FogGenerator: MonoBehaviour
  {
    public GameObject FogTemplate;
    RectInt rect;

    public void Start()
    {
      rect = WallMarker.FindBoundaries();
      Debug.Log("Found boundaries: " + rect);

      rect = WallMarker.Shrink(rect);

      if (FogTemplate == null)
      {
        return;
      }

      foreach (var pos in rect.allPositionsWithin)
      {
        var go = Instantiate(FogTemplate, transform);
        go.transform.position = new Vector3(pos.x, pos.y, transform.position.z);
      }
    }
  }
}
