using System.Collections;
using UnityEngine;

namespace SeekingRainbow.Scripts
{
  [CreateAssetMenu]
  public class FillTileAbilityEffect : AbilityEffect
  {
    public AbilityEffectMarker Effect;
    public float Delay;

    protected override void ApplyAbility(AbilityEffector source, Vector2Int start, Vector2Int position)
    {
      Debug.Log("Attempt to apply effect " + name);
      var marker = source.GetComponentInChildren<AbilityEffectMarker>();
      if (marker != null)
      {
        if (RequireEmpty)
        {
          Debug.Log("Found a marker but require empty. marker was " + marker.Creator); 
          return;
        }

        if (Requirement != null && marker.Creator != Requirement)
        {
          Debug.Log("Found a marker but requirements do not match. marker was " + marker.Creator); 
          return;
        }
        Destroy(marker.gameObject);
      }
      else if (Requirement != null)
      {
        Debug.Log("Found no marker but require one."); 
        return;
      }

      if (Effect != null)
      {
        var go = Instantiate(Effect);
        go.Creator = this;
        go.transform.SetParent(source.transform, false);
        go.transform.position = source.transform.position;

        if (PropagateEffect)
        {
          go.StartCoroutine(PerformPropagateEffect(start + position));
        }
      }
      else
      {
        var go = new GameObject();
        go.transform.SetParent(source.transform, false);

        var next = go.AddComponent<AbilityEffectMarker>();
        next.Creator = this;
        if (PropagateEffect)
        {
          next.StartCoroutine(PerformPropagateEffect(start + position));
        }
      }
    }

    IEnumerator PerformPropagateEffect(Vector2Int position)
    {
      yield return new WaitForSeconds(Delay);

      PropagateEffectFromHere(position);
    }
  }
}
