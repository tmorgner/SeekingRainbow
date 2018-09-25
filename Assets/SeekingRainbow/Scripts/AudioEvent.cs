using System.Collections.Generic;
using UnityEngine;

namespace SeekingRainbow.Scripts
{
  public class AudioEvent: MonoBehaviour
  {
    public List<AudioClip> Clips;
    public SoundVarianceSettings Target;

    public void PlayOne()
    {
      Target?.RandomizeSfx(Clips);
    }
  }
}
