using System;
using System.Collections.Generic;
using UnityEngine;

namespace SeekingRainbow.Scripts
{
  public class PlayerAbilityEffects : MonoBehaviour
  {
    public GameSession Session;

    readonly Dictionary<ElementalAbility, GameObject> effectsPool;
    readonly Dictionary<ElementalAbility, GameObject> activeEffects;

    readonly Action<ElementalBasePower> onBasePowersChangedDelegate;
    readonly Action<ElementalAbility> onAbilitiesChangedDelegate;

    public PlayerAbilityEffects()
    {
      onAbilitiesChangedDelegate = OnAbilitiesChanged;
      onBasePowersChangedDelegate = OnBasePowersChanged;
      effectsPool = new Dictionary<ElementalAbility, GameObject>();
      activeEffects = new Dictionary<ElementalAbility, GameObject>();
    }

    void OnDisable()
    {
      Session.BasePowerSelected.RemoveListener(onBasePowersChangedDelegate);
      Session.BasePowerDeselected.RemoveListener(onBasePowersChangedDelegate);
      Session.AbilitySelected.RemoveListener(onAbilitiesChangedDelegate);
      Session.AbilityDeselected.RemoveListener(onAbilitiesChangedDelegate);
    }

    void OnEnable()
    {
      Session.BasePowerSelected.AddListener(onBasePowersChangedDelegate);
      Session.BasePowerDeselected.AddListener(onBasePowersChangedDelegate);
      Session.AbilitySelected.AddListener(onAbilitiesChangedDelegate);
      Session.AbilityDeselected.AddListener(onAbilitiesChangedDelegate);
    }

    void OnBasePowersChanged(ElementalBasePower obj)
    {
      OnAbilitiesChanged();
    }

    void OnAbilitiesChanged(ElementalAbility obj)
    {
      OnAbilitiesChanged();
    }

    void OnAbilitiesChanged()
    {
      if (Session.AvailableAbilities.Count > 0 && Session.AutoSelectAbility)
      {
        var ability = Session.AvailableAbilities[0];
        if (!IsSelectedAbility(ability))
        {
          // remove all currently selected abilities 
          foreach (var selected in Session.SelectedAbilities)
          {
            Session.Deselect(selected);
          }

          // so that the first ability is the only one selected.
          Session.Select(ability);
        }
      }

      ResyncEffects();
    }

    void ResyncEffects()
    {
      var selected = Session.SelectedAbilities;
      Debug.Log("Selected abilitites: " + string.Join(", ", selected));
      // first remove all abilities that are no longer selected but still have 
      // an effect running.
      foreach (var ab in Session.Abilities)
      {
        if (selected.Contains(ab))
        {
          continue;
        }

        GameObject go;
        if (activeEffects.TryGetValue(ab, out go))
        {
          go.SetActive(false);
          activeEffects.Remove(ab);
        }
        else
        {
          Debug.Log("Did not find game object for " + ab);
        }
      }

      // then add the newly selected abilities ..
      foreach (var ab in Session.SelectedAbilities)
      {
        GameObject go;
        if (!activeEffects.TryGetValue(ab, out go))
        {
          if (!effectsPool.TryGetValue(ab, out go))
          {
            var effect = ab.ActivationEffect;
            if (effect != null)
            {
              go = Instantiate(effect, transform);
              go.transform.position = transform.position;
              go.transform.rotation = transform.rotation;
              effectsPool[ab] = go;
            }
          }

          if (go != null)
          {
            activeEffects[ab] = go;
            go.SetActive(true);
          }
        }
      }
    }

    bool IsSelectedAbility(ElementalAbility ability)
    {
      return Session.SelectedAbilities.Count == 1 && Session.SelectedAbilities.Contains(ability);
    }

    void OnDestroy()
    {
      effectsPool.Clear();
      activeEffects.Clear();
    }
  }
}
