using UnityEngine;

namespace SeekingRainbow.Scripts
{
  public abstract class AbilityEffect : ScriptableObject
  {
    public EffectMarker EffectPrefab;

    public abstract void ApplyAbility(AbilityEffector source, ElementalAbility a, Vector2Int start, Vector2Int position);

    public bool IsValid(AbilityEffector source)
    {
      var em = source.GetComponentInChildren<EffectMarker>();
      return IsValid(em);
    }

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

    public static void PerformAbilityAt(ElementalAbility ab, Vector2Int start, Vector2Int input)
    {
      Debug.Log("Attempt to cast ability " + ab + " from " + start + " in direction " + input);

      var end = start + input;
      var castingStart = Vector2.Lerp(start, end, 0.6f);
      var targets = Physics2D.LinecastAll(castingStart, end);
      if (targets.Length == 0)
      {
        Debug.Log("No targets in that direction");
      }
      foreach (var t in targets)
      {
        var aes = t.transform.GetComponents<AbilityEffector>();
        if (aes.Length == 0)
        {
          Debug.Log("No targets in that direction [0]");
        }
        foreach (var ae in aes)
        {
          if (ae == null)
          {
            continue;
          }

          if (ae.effect == null)
          {
            Debug.Log("No effect");
            continue;
          }

          if (ae.trigger == ab)
          {
            Debug.Log("Starting effect trigger " + ae);
            var effect = ae.effect;
            effect.ApplyAbility(ae, ab, start, input);
          }
          else
          {
            Debug.Log("Trigger does not match " + ae);
          }
        }
      }
    }
  }
}
