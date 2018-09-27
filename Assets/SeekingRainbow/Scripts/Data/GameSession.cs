using System;
using System.Collections.Generic;
using UnityEngine;

namespace SeekingRainbow.Scripts
{
  /// <summary>
  ///  Simple synchronization point so that we can avoid singletons. 
  ///  This class does not contain any serializable properties, everything
  ///  is handled as RuntimeValues.
  /// </summary>
  [CreateAssetMenu]
  public class GameSession : ScriptableObject
  {
    [Multiline(10)] public string DeveloperNote;
    [Tooltip("Add all powers that should be available in the game. This affects the UI.")]
    public List<ElementalBasePower> Powers;
    [Tooltip("Add all abilities that should be available in the game. This affects the UI.")]
    public List<ElementalAbility> Abilities;

    /// <summary>
    ///  Which powers did the player pick up? These will show up in the UI as buttons.
    /// </summary>
    public List<ElementalBasePower> AvailablePowers { get; private set; }

    public List<ElementalAbility> AvailableAbilities { get; private set; }

    /// <summary>
    ///  Which powers are currently selected. These will be used to compute the abilities that can be used.
    /// </summary>
    public HashSet<ElementalBasePower> SelectedPowers { get; private set; }

    public HashSet<ElementalAbility> SelectedAbilities { get; private set; }

    public bool AutoSelectAbility;

    [NonSerialized] public bool IsPlaythrough;

    public WeakEvent<ElementalBasePower> BasePowerReceived;
    public WeakEvent<ElementalBasePower> BasePowerLost;
    public WeakEvent<ElementalBasePower> BasePowerSelected;
    public WeakEvent<ElementalBasePower> BasePowerDeselected;
    public WeakEvent<ElementalAbility> AbilitySelected;
    public WeakEvent<ElementalAbility> AbilityDeselected;

    public GameSession()
    {
      AvailablePowers = new List<ElementalBasePower>();
      AvailableAbilities = new List<ElementalAbility>();
      SelectedPowers = new HashSet<ElementalBasePower>();
      SelectedAbilities = new HashSet<ElementalAbility>();

      BasePowerLost = new WeakEvent<ElementalBasePower>();
      BasePowerReceived = new WeakEvent<ElementalBasePower>();
      BasePowerSelected = new WeakEvent<ElementalBasePower>();
      BasePowerDeselected = new WeakEvent<ElementalBasePower>();
      AbilityDeselected = new WeakEvent<ElementalAbility>();
      AbilitySelected = new WeakEvent<ElementalAbility>();
    }

    void OnDisable()
    {
      Application.quitting -= Reset;
    }

    void OnEnable()
    {
      Application.quitting += Reset;
    }

    /// <summary>
    ///  This is mainly a Editor helper, as ScriptableObject instances survive 
    ///  exiting the play mode.
    /// </summary>
    public void Reset()
    {
      AvailablePowers.Clear();
      AvailableAbilities.Clear();
      SelectedPowers.Clear();
      SelectedAbilities.Clear();
      BasePowerReceived.RemoveListeners();
      BasePowerLost.RemoveListeners();
      BasePowerSelected.RemoveListeners();
      BasePowerDeselected.RemoveListeners();
      AbilityDeselected.RemoveListeners();
      AbilitySelected.RemoveListeners();
    }

    public void ReceivePower(ElementalBasePower power)
    {
      if (!AvailablePowers.Contains(power))
      {
        AvailablePowers.Add(power);
        UpdateAvailableAbilities();
        BasePowerReceived.Invoke(power);
      }
    }

    public void LosePower(ElementalBasePower power)
    {
      if (AvailablePowers.Contains(power))
      {
        AvailablePowers.Remove(power);
        UpdateAvailableAbilities();
        BasePowerLost.Invoke(power);
      }
    }

    public void Select(ElementalBasePower power)
    {
      if (SelectedPowers.Add(power))
      {
        UpdateAvailableAbilities();
        BasePowerSelected.Invoke(power);
      }
    }

    public void Deselect(ElementalBasePower power)
    {
      if (SelectedPowers.Remove(power))
      {
        UpdateAvailableAbilities();
        BasePowerDeselected.Invoke(power);
      }
    }

    void UpdateAvailableAbilities()
    {
      for (var i = AvailableAbilities.Count - 1; i >= 0; i--)
      {
        var a = AvailableAbilities[i];
        if (!a.CanActivate(SelectedPowers))
        {
          AvailableAbilities.RemoveAt(i);
          Deselect(a);
        }
      }

      foreach (var a in Abilities)
      {
        if (a.CanActivate(SelectedPowers))
        {
          if (!AvailableAbilities.Contains(a))
          {
            AvailableAbilities.Add(a);
          }
        }
      }
    }

    public void Select(ElementalAbility power)
    {
      if (SelectedAbilities.Add(power))
      {
        AbilitySelected.Invoke(power);
      }
    }

    public void Deselect(ElementalAbility power)
    {
      if (SelectedAbilities.Remove(power))
      {
        AbilityDeselected.Invoke(power);
      }
    }
  }
}
