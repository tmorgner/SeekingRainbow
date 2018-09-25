using UnityEngine;

namespace SeekingRainbow.Scripts
{
  public abstract class AbilityEffect : MonoBehaviour
  {
    public abstract void PerformAbility(ElementalAbility a, Vector2Int from, Vector2Int position);

    public static void PerformAbilityAt(ElementalAbility ab, Vector2Int start, Vector2Int input)
    {
      Debug.Log("Attempt to cast ability " + ab + " from " + start + " in direction " + input);

      var end = start + input;
      var castingStart = Vector2.Lerp(start, end, 0.6f);
      var targets = Physics2D.LinecastAll(castingStart, end);
      foreach (var t in targets)
      {
        var ae = t.transform.GetComponent<AbilityEffect>();
        if (ae != null)
        {
          Debug.Log("Found " + t);
          ae.PerformAbility(ab, start, input);
        }
      }
    }
  }
}
