using UnityEngine;

namespace SeekingRainbow.Scripts
{
  /// <summary>
  ///  Something happens when I walked on that tile. (Unity does not like interfaces  that much).
  /// </summary>
  public abstract class TileEffect: MonoBehaviour
  {
    public abstract void OnEffectTriggered(PlayerBehaviour playerBehaviour);
  }
}
