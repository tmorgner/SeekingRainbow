using UnityEngine;

namespace SeekingRainbow.Scripts
{
  public class RotateOverTime: MonoBehaviour
  {
    public float AngularVelocity;

    void Update()
    {
      transform.Rotate(0, 0, AngularVelocity * Time.deltaTime);
    }
  }
}
