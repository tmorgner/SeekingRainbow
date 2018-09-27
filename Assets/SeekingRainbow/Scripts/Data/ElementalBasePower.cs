using UnityEngine;

namespace SeekingRainbow.Scripts
{
  [CreateAssetMenu]
  public class ElementalBasePower: ScriptableObject
  {
    public string Name;
    public KeyCode TriggerKey;
    public Sprite Sprite;
    public Sprite SpriteSelected;
    public Sprite SpriteNotAvailable;
  }
}
