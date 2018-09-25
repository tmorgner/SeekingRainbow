using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SeekingRainbow.Scripts
{
  [CreateAssetMenu]
  public class SoundVarianceSettings : ScriptableObject
  {
    public float lowPitchRange = .95f; //The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.05f; //The highest a sound effect will be randomly pitched.

    public AudioSource Source { get; private set; }

    public void Reset()
    {
      Source = null;
      lowPitchRange = .95f;
      highPitchRange = 1.05f;
    }

    public void Register(AudioSource source)
    {
      Source = source;
    }

    public void Unregister(AudioSource source)
    {
      Source = null;
    }

    //RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
    public void RandomizeSfx(params AudioClip[] clips)
    {
      RandomizeSfx((IReadOnlyList<AudioClip>) clips);
    }

    public void RandomizeSfx(IReadOnlyList<AudioClip> clips)
    {
      //Generate a random number between 0 and the length of our array of clips passed in.
      int randomIndex = Random.Range(0, clips.Count);

      //Choose a random pitch to play back our clip at between our high and low pitch ranges.
      float randomPitch = Random.Range(lowPitchRange, highPitchRange);

      //Set the pitch of the audio source to the randomly chosen pitch.
      Source.pitch = randomPitch;

      //Set the clip to the clip at our randomly chosen index.
      Source.clip = clips[randomIndex];

      //Play the clip.
      Source.Play();
    }
  }
}
