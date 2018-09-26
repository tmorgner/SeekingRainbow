using System.Collections;
using UnityEngine;

namespace SeekingRainbow.Scripts
{
  [CreateAssetMenu]
  public class FillTileAbilityEffect : AbilityEffect
  {
    static readonly Vector2Int[] neighbours = {
      new Vector2Int(-1, 0),
      new Vector2Int(0, -1),
      new Vector2Int(0, 1),
      new Vector2Int(1, 0),
    };

    public float Delay;
    public EffectMarker EffectPrefab;

    public override void ApplyAbility(AbilityEffector source, ElementalAbility a, Vector2Int start, Vector2Int position)
    {
      if (EffectPrefab == null)
      {
        return;
      }

      if (source.GetComponentInChildren<EffectMarker>() != null)
      {
        return;
      }

      var go = Instantiate(EffectPrefab);
      go.Source = a;
      go.transform.SetParent(source.transform, false);
      go.transform.position = source.transform.position;

      go.StartCoroutine(PropagateEffect(a, start + position));
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
