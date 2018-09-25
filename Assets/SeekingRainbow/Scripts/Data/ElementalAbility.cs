using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    
    public List<ElementalBasePower> Requirements;

    public bool CanActivate(ICollection<ElementalBasePower> powers)
    {
      if (Requirements.Count == 0)
      {
        return true;
      }

      foreach (var req in Requirements)
      {
        if (!powers.Contains(req))
        {
          return false;
        }
      }

      return true;
    }
  }
}