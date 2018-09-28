using UnityEngine;

namespace SeekingRainbow.Scripts
{
  public abstract class AbilityEffect : ScriptableObject
  {
    protected static readonly Vector2Int[] neighbours =
    {
      new Vector2Int(-1, 0),
      new Vector2Int(0, -1),
      new Vector2Int(0, 1),
      new Vector2Int(1, 0),
    };

    public bool PropagateEffect;
    public bool RequireEmpty;
    public AbilityEffect Requirement;

    protected abstract void ApplyAbility(AbilityEffector source, Vector2Int start, Vector2Int position);

    protected void PropagateEffectFromHere(Vector2Int start)
    {
      if (PropagateEffect)
      {
        foreach (var n in neighbours)
        {
          var direction = n;
          PropagateEffectTo(start, direction);
        }
      }
    }

    void PropagateEffectTo(Vector2Int start, Vector2Int input)
    {
      var end = start + input;
      var castingStart = Vector2.Lerp(start, end, 0.6f);
      var targets = Physics2D.LinecastAll(castingStart, end);
      if (targets.Length == 0)
      {
        Debug.Log("No targets in that direction");
      }

      foreach (var t in targets)
      {
        var abilityEffectors = t.transform.GetComponents<AbilityEffector>();
        if (abilityEffectors.Length == 0)
        {
          Debug.Log("No targets in that direction [0]");
        }

        foreach (var effector in abilityEffectors)
        {
          if (effector.effect == this)
          {
            var effect = effector.effect;
            if (effect == null)
            {
              Debug.LogWarning("Configuration Error: No effect defined on effector.");
              continue;
            }

            Debug.Log("Starting effect trigger " + effector);
            effect.ApplyAbility(effector, start, input);
          }
          else
          {
            Debug.Log("Trigger does not match " + effector);
          }
        }
      }
    }

    /// <summary>
    ///  Let a player cast an ability on to a given tile.
    /// </summary>
    /// <param name="ab"></param>
    /// <param name="start"></param>
    /// <param name="input"></param>
    public static void PerformAbilityAt(ElementalAbility ab, Vector2Int start, Vector2Int input)
    {
      Debug.Log("Attempt to cast an ability " + ab + " from " + start + " in direction " + input);

      var end = start + input;
      var castingStart = Vector2.Lerp(start, end, 0.6f);
      var targets = Physics2D.LinecastAll(castingStart, end);
      if (targets.Length == 0)
      {
        Debug.Log("No targets in that direction");
        return;
      }

      foreach (var t in targets)
      {
        var abilityEffectors = t.transform.GetComponents<AbilityEffector>();
        if (abilityEffectors.Length == 0)
        {
          Debug.Log("No targets in that direction [0]");
        }

        foreach (var effector in abilityEffectors)
        {
          if (effector == null)
          {
            continue;
          }

          if (effector.trigger == ab)
          {
            var effect = effector.effect;
            if (effect == null)
            {
              Debug.LogWarning("Configuration Error: No effect defined on effector.");
              continue;
            }

            Debug.Log("Starting effect trigger " + effector);
            effect.ApplyAbility(effector, start, input);
          }
          else
          {
            Debug.Log("Trigger does not match " + effector);
          }
        }
      }
    }
  }
}
