using UnityEngine;

namespace SeekingRainbow.Scripts
{
  /// <summary>
  ///  Attached to tile game objects created by an ability effect. This component is
  ///  used to indicate that a propagating ability already visited a certain field.
  /// </summary>
  public class AbilityEffectMarker : MonoBehaviour
  {
    public AbilityEffect Creator;
  }
}