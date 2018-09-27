using System.Collections.Generic;
using UnityEngine;

namespace SeekingRainbow.Scripts
{
  [CreateAssetMenu]
  public class ElementalAbility : ScriptableObject
  {
    public string Name;
    public Sprite Sprite;
    public Sprite SpriteSelected;
    public Sprite SpriteNotAvailable;
    public GameObject ActivationEffect;

    public List<ElementalBasePower> Requirements;

    public bool CanActivate(HashSet<ElementalBasePower> powers)
    {
      if (Requirements.Count == 0)
      {
        return true;
      }

      if (Requirements.Count != powers.Count)
      {
        return false;
      }

      foreach (var req in Requirements)
      {
        if (!powers.Contains(req))
        {
          return false;
        }
      }

      foreach (var req in powers)
      {
        if (!Requirements.Contains(req))
        {
          return false;
        }
      }

      return true;
    }
  }
}