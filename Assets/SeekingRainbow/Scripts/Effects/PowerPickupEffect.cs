using UnityEngine;

namespace SeekingRainbow.Scripts
{
  public class PowerPickupEffect : TileEffect
  {
    public ElementalBasePower power;
    public GameSession session;

    public override void OnEffectTriggered(PlayerBehaviour playerBehaviour)
    {
      session.ReceivePower(power);
      gameObject.SetActive(false);
    }
  }
}