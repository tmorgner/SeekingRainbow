using UnityEngine;
using UnityEngine.Events;

namespace SeekingRainbow.Scripts
{
  public class AbilityEffector: MonoBehaviour
  {
    public ElementalAbility trigger;
    public AbilityEffect effect;
    public UnityEvent effectTriggered;
  }
}
