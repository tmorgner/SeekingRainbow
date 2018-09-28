using System.Collections;
using UnityEngine;

namespace SeekingRainbow.Scripts
{
  [CreateAssetMenu]
  public class ReplaceTileAbilityEffect : AbilityEffect
  {
    public LayerMask itemsMask;
    public GameObject replacementTile;

    protected override void ApplyAbility(AbilityEffector source, Vector2Int start, Vector2Int position)
    {
      if (replacementTile == null)
      {
        return;
      }

      var effectMarker = source.GetComponentInChildren<AbilityEffectMarker>();
      if (effectMarker != null)
      {
        if (RequireEmpty)
        {
          return;
        }

        if (Requirement != null && effectMarker.Creator != Requirement)
        {
          return;
        }
      }
      else if (Requirement != null)
      {
        return;
      }

      source.StartCoroutine(ReplaceTile(source.transform, start + position));
    }


    IEnumerator ReplaceTile(Transform targetTile, Vector2Int start)
    {
      yield return new WaitForEndOfFrame();
      Debug.Log("Doing work");

      var go = Instantiate(replacementTile);
      go.transform.SetParent(targetTile.parent, false);
      go.transform.SetPositionAndRotation(targetTile.position, targetTile.rotation);

      var effectMarker = go.GetComponentInChildren<AbilityEffectMarker>();
      if (effectMarker == null)
      {
        var childObject = new GameObject();
        effectMarker = childObject.AddComponent<AbilityEffectMarker>();
        childObject.transform.SetParent(go.transform, false);
      }

      effectMarker.Creator = this;

      Destroy(targetTile.gameObject);
      
      PropagateEffectFromHere(start);
    }
  }
}
