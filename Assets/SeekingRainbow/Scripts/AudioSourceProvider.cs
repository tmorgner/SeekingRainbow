using UnityEngine;

namespace SeekingRainbow.Scripts
{
  public class AudioSourceProvider : MonoBehaviour
  {
    public SoundVarianceSettings soundSettings;
    public AudioSource audioSource;

    void Awake()
    {
      if (audioSource == null)
      {
        audioSource = GetComponent<AudioSource>();
      }
    }

    void OnEnable()
    {
      if (!ReferenceEquals(audioSource, null))
      {
        soundSettings.Register(audioSource);
      }
    }

    void OnDisable()
    {
      if (!ReferenceEquals(audioSource, null))
      {
        soundSettings.Unregister(audioSource);
      }
    }
  }
}