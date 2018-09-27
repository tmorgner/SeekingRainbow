using UnityEngine;

namespace SeekingRainbow.Scripts
{
  public class PlayerBasePowerInput : MonoBehaviour
  {
    public GameSession Session;

    void Update()
    {
      foreach (var power in Session.AvailablePowers)
      {
        if (Input.GetKeyDown(power.TriggerKey))
        {
          if (Session.SelectedPowers.Contains(power))
          {
            Session.Deselect(power);
          }
          else
          {
            Session.Select(power);
          }
        }
      }
    }
  }
}