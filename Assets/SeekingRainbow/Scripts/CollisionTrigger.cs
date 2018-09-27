using UnityEngine;
using UnityEngine.Events;

namespace SeekingRainbow.Scripts
{
  public class CollisionTrigger: MonoBehaviour 
  {
    public LayerMask CollideWith;
    public UnityEvent OnCollision;

    void OnTriggerEnter2D(Collider2D other)
    {
      OnCollision.Invoke();
    }
  }
}
