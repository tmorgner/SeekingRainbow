using System.Collections;
using UnityEngine;

namespace SeekingRainbow.Scripts
{
  public class FillWaterTileAbilityEffect : AbilityEffect
  {
    static readonly Vector2Int[] neighbours = {
      new Vector2Int(-1, 0),
      new Vector2Int(0, -1),
      new Vector2Int(0, 1),
      new Vector2Int(1, 0),
    };

    public float Delay;
    public EffectMarker Effect;

    public override void PerformAbility(ElementalAbility a, Vector2Int start, Vector2Int position)
    {
      if (Effect == null)
      {
        return;
      }

      var c = GetComponentInChildren<EffectMarker>();
      if (c != null)
      {
        if (c.Source == a)
        {
          return;
        }
      }
      

      var go = Instantiate(Effect);
      go.Source = a;
      go.transform.SetParent(this.transform, false);
      go.transform.position = transform.position;

      StartCoroutine(PropagateEffect(a, start + position));
    }

    IEnumerator PropagateEffect(ElementalAbility elementalAbility, Vector2Int position)
    {
      yield return new WaitForSeconds(Delay);

      foreach (var n in neighbours)
      {
        var target = n;
        PerformAbilityAt(elementalAbility, position, target);
      }
    }
  }
}
