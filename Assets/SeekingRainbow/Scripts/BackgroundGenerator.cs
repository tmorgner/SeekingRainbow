using System.Collections.Generic;
using UnityEngine;

namespace SeekingRainbow.Scripts
{
  public class BackgroundGenerator: MonoBehaviour
  {
    public List<GameObject> Items;
    
    void Start()
    {
      if (Items.Count == 0)
      {
        return;
      }

      var rect = WallMarker.FindBoundaries();
      var camRect = ComputeViewport();
//      camRect = WallMarker.Expand(camRect);
      foreach (var pos in camRect.allPositionsWithin)
      {
        if (!rect.Contains(pos))
        {
          var item = PickRandom(Items);
          var go = Instantiate(item, transform);
          go.transform.position = new Vector3(pos.x, pos.y, transform.position.z);
          var wm = go.GetComponent<WallMarker>();
          wm.enabled = false;
        }
      }
    }

    static T PickRandom<T>(IReadOnlyList<T> items)
    {
      var randomIndex = Random.Range(0, items.Count);
      return items[randomIndex];
    }

    static RectInt ComputeViewport()
    {
      var mainCam = Camera.main;
      var size = mainCam.orthographicSize;
      var aspectRatio = mainCam.aspect;
      var width = size * aspectRatio;
      var posX = mainCam.transform.position.x - width;
      var posY = mainCam.transform.position.y - size;

      // add some extra padding so that we can guarantee that under no normal circumstances
      // the player notices that the world ends outside of the viewable area.
      var camRect = new RectInt(Mathf.FloorToInt(posX) - 1, 
                                Mathf.FloorToInt(posY) - 1, 
                                Mathf.CeilToInt(width * 2) + 2, 
                                Mathf.CeilToInt(size * 2) + 2);
      return camRect;
    }
  }
}
