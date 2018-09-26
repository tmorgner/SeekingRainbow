﻿using System.Collections;
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
    public bool Propagate;

    public bool IsValid(EffectMarker marker)
    {
      if (EffectPrefab == null)
      {
        Debug.Log("No Prefab");
        return false;
      }

      if (marker == null)
      {
        if (EffectPrefab.RequiredEffects.Count == 0)
        {
          Debug.Log("OK: No Prefab and none needed");
          return true;
        }

        Debug.Log("No marker found, but effect requires previous stuff");
        return false;
      }

      Debug.Log("No marker found, but effect requires previous stuff");
      return EffectPrefab.RequiredEffects.Contains(marker.Source);
    }

    public override void ApplyAbility(AbilityEffector source, ElementalAbility a, Vector2Int start, Vector2Int position)
    {

      if (!IsValid(source.GetComponentInChildren<EffectMarker>()))
      {
        return;
      }

      var go = Instantiate(EffectPrefab);
      go.Source = a;
      go.transform.SetParent(source.transform, false);
      go.transform.position = source.transform.position;

      if (Propagate)
      {
        go.StartCoroutine(PropagateEffect(a, start + position));
      }
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
