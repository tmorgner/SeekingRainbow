using UnityEngine;

namespace SeekingRainbow.Scripts
{
  public class ReplaceSprite : MonoBehaviour
  {
    public Sprite sprite;

    public void DoReplace()
    {
      Debug.Log("Changed sprite");
      GetComponent<SpriteRenderer>().sprite = sprite;
    }
  }
}