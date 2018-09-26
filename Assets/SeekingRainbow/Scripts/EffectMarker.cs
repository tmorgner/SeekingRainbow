using System;
using System.Collections.Generic;
using UnityEngine;

namespace SeekingRainbow.Scripts
{
  public class EffectMarker: MonoBehaviour
  {
    [Multiline(10)]
    public string DeveloperNote;
    public ElementalAbility Source;
    public List<ElementalAbility> RequiredEffects;
  }
}
