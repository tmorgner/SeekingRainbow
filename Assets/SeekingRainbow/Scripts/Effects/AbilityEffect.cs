using UnityEngine;

namespace SeekingRainbow.Scripts
{
  public abstract class AbilityEffect : ScriptableObject
  {
    public abstract void ApplyAbility(AbilityEffector source, ElementalAbility a, Vector2Int start, Vector2Int position);

    public static void PerformAbilityAt(ElementalAbility ab, Vector2Int start, Vector2Int input)
    {
      Debug.Log("Attempt to cast ability " + ab + " from " + start + " in direction " + input);

      var end = start + input;
      var castingStart = Vector2.Lerp(start, end, 0.6f);
      var targets = Physics2D.LinecastAll(castingStart, end);
      foreach (var t in targets)
      {
        var ae = t.transform.GetComponent<AbilityEffector>();
        if (ae != null && ae.effect != null)
        {
          var effect = ae.effect;
          Debug.Log("Found " + t);
          effect.ApplyAbility(ae, ab, start, input);
        }
      }
    }
  }
}
