using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SeekingRainbow.Scripts.UI
{
  public class SpriteButton : MonoBehaviour
  {
    public UnityEvent OnClick;
    public Image image;
    public Button button;
    public Text text;
    [Header("Selected Colors")]
    public ColorBlock SelectedColors;
    [Header("Normal Colors")]
    public ColorBlock NormalColors;

    void Awake()
    {
      if (button == null)
      {
        button = GetComponent<Button>() ?? GetComponentInChildren<Button>();
      }

      if (button != null)
      {
        if (image == null)
        {
          image = button.GetComponentInChildren<Image>();
        }

        if (text == null)
        {
          text = button.GetComponentInChildren<Text>();
        }

      }
    }

    void Reset()
    {
      NormalColors = new ColorBlock()
      {
        colorMultiplier = 1,
        fadeDuration = 0.1f,
        normalColor = Color.white,
        highlightedColor = Color.white,
        pressedColor = Color.gray,
        disabledColor = Color.gray
      };

      SelectedColors = new ColorBlock()
      {
        colorMultiplier = 1,
        fadeDuration = 0.1f,
        normalColor = Color.yellow,
        highlightedColor = Color.yellow,
        pressedColor = Color.gray,
        disabledColor = Color.gray
      };
    }

    void OnEnable()
    {
      if (button != null)
      {
        button.onClick.AddListener(OnClickHandler);
      }
    }

    void OnDisable()
    {
      if (button != null)
      {
        button.onClick.RemoveListener(OnClickHandler);
      }
    }

    void OnDestroy()
    {
      OnClick.RemoveAllListeners();
    }

    void OnClickHandler()
    {
      OnClick.Invoke();
    }

    public void SetUp(string name, Sprite image, SpriteButtonState selected)
    {
      if (image == null)
      {
        this.image.enabled = false;
        this.text.text = name;
        this.text.enabled = true;
      }
      else
      {
        this.image.enabled = true;
        this.image.sprite = image;
        this.text.enabled = false;
      }

      Debug.Log("SetUp called on " + name + " with " + image);
      var buttonColors = this.button.colors;
      if (selected == SpriteButtonState.Selected)
      {
        buttonColors = SelectedColors;
      }
      else
      {
        buttonColors = NormalColors;
      }

      this.button.interactable = selected != SpriteButtonState.Disabled;
      this.button.colors = buttonColors;
    }
  }
}
