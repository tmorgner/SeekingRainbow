using System.Collections.Generic;
using UnityEngine;

namespace SeekingRainbow.Scripts.UI
{
  public class AbilitySelector: MonoBehaviour
  {
    public GameSession Session;
    public SpriteButton Template;
    bool initialized;
    RectTransform templateRect;
    Dictionary<ElementalAbility, SpriteButton> buttons;

    public AbilitySelector()
    {
      buttons = new Dictionary<ElementalAbility, SpriteButton>();
    }

    void Awake()
    {
      if (Template == null)
      {
        Template = GetComponentInChildren<SpriteButton>();
      }

      if (Template != null)
      {
        Template.gameObject.SetActive(false);
        templateRect = Template.GetComponent<RectTransform>();
      }

      Session.AbilityDeselected.AddListener(OnAbilityDeselected);
      Session.AbilitySelected.AddListener(OnAbilitySelected);
    }

    void OnAbilitySelected(ElementalAbility obj)
    {
      
    }

    void OnAbilityDeselected(ElementalAbility obj)
    {
      
    }

    void Update()
    {
      if (!initialized)
      {
        int count = 0;
        foreach (var ability in Session.Abilities)
        {
          var name = ability.Name;
          var sprite = ability.Sprite;
          
          var go = Instantiate(Template);
          go.SetUp(name, sprite, SpriteButtonState.Disabled);
          go.gameObject.name = $"{ability} Button";
          go.OnClick.AddListener(() => OnAbilitySelectedInUI(ability));
          buttons.Add(ability, go);
          var rt = go.GetComponent<RectTransform>();
          var pos = templateRect.anchoredPosition3D;
          pos.x += (templateRect.anchoredPosition3D.x + templateRect.sizeDelta.x) * count;
          rt.position = pos;

          go.gameObject.SetActive(true);
          go.transform.SetParent(transform, false);

          count += 1;
        }

        initialized = true;
      }

      foreach (var pair in buttons)
      {
        var ability = pair.Key;
        var spriteButton = pair.Value;
        if (!Session.AvailableAbilities.Contains(ability))
        {
          spriteButton.SetUp(ability.Name, ability.SpriteNotAvailable, SpriteButtonState.Disabled);
        }
        else if (Session.SelectedAbilities.Contains(ability))
        {
          spriteButton.SetUp(ability.Name, ability.SpriteSelected, SpriteButtonState.Selected);
        }
        else
        {
          spriteButton.SetUp(ability.Name, ability.Sprite, SpriteButtonState.Enabled);
        }
      }
    }

    void OnAbilitySelectedInUI(ElementalAbility ability)
    {
      if (Session.SelectedAbilities.Contains(ability))
      {
        Session.Deselect(ability);
      }
      else
      {
        Session.Select(ability);
      }
    }

    void OnDestroy()
    {
      buttons.Clear();
    }
  }
}
