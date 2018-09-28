using System.Collections;
using UnityEngine;

namespace SeekingRainbow.Scripts
{
  [CreateAssetMenu]
  public class ReplaceTileAbilityEffect : AbilityEffect
  {
    static readonly Vector2Int[] neighbours =
    {
      new Vector2Int(-1, 0),
      new Vector2Int(0, -1),
      new Vector2Int(0, 1),
      new Vector2Int(1, 0),
    };

    public LayerMask itemsMask;
    public GameObject replacementTile;
    public bool Propagate;

    public override void ApplyAbility(AbilityEffector source, ElementalAbility a, Vector2Int start, Vector2Int position)
    {
      if (!IsValid(source))
      {
        return;
      }

      if (replacementTile == null)
      {
        return;
      }

      source.StartCoroutine(ReplaceTile(start + position, source.trigger));
    }


    IEnumerator ReplaceTile(Vector2Int start, ElementalAbility sourceTrigger)
    {
      var tile = Physics2D.Linecast(start, Vector2.zero, itemsMask);
      if (tile.transform == null)
      {
        Debug.Log("No target here");
        yield break;
      }

      yield return new WaitForEndOfFrame();
      Debug.Log("Doing work");

      var go = Instantiate(replacementTile);
      go.transform.SetParent(tile.transform.parent, false);
      go.transform.SetPositionAndRotation(tile.transform.position, tile.transform.rotation);
      Destroy(tile.transform.gameObject);

      if (Propagate)
      {
        foreach (var n in neighbours)
        {
          var target = n;
          PerformAbilityAt(sourceTrigger, start, target);
        }
      }
    }
  }
}
