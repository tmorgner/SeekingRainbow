using UnityEngine;
using UnityEngine.Events;

namespace SeekingRainbow.Scripts
{
  public class PlayerTileEffect: TileEffect
  {
    public UnityEvent OnEffectActivated;

    public override void OnEffectTriggered(PlayerBehaviour playerBehaviour)
    {
      OnEffectActivated.Invoke();
    }
  }
}
