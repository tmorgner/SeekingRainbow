using System.Collections.Generic;
using UnityEngine;

namespace SeekingRainbow.Scripts.UI
{
  public class PowerSelector: MonoBehaviour
  {
    public GameSession Session;
    public SpriteButton Template;
    bool initialized;
    RectTransform templateRect;
    Dictionary<ElementalBasePower, SpriteButton> buttons;

    public PowerSelector()
    {
      buttons = new Dictionary<ElementalBasePower, SpriteButton>();
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
    }

    void Update()
    {
      if (!initialized)
      {
        int count = 0;
        foreach (var ability in Session.Powers)
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
        if (!Session.AvailablePowers.Contains(ability))
        {
          //Debug.Log("Power " + ability + " is not here");
          spriteButton.SetUp(ability.Name, ability.SpriteNotAvailable, SpriteButtonState.Disabled);
        }
        else if (Session.SelectedPowers.Contains(ability))
        {
          //Debug.Log("Power " + ability + " is currently selected");
          spriteButton.SetUp(ability.Name, ability.SpriteSelected, SpriteButtonState.Selected);
        }
        else
        {
//          Debug.Log("Power " + ability + " can be selected");
          spriteButton.SetUp(ability.Name, ability.Sprite, SpriteButtonState.Enabled);
        }
      }
    }

    void OnAbilitySelectedInUI(ElementalBasePower ability)
    {
      if (Session.SelectedPowers.Contains(ability))
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
