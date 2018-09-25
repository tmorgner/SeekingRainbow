using System.Collections.Generic;
using UnityEngine;

namespace SeekingRainbow.Scripts
{
  public class PlayerDesignerPowers: MonoBehaviour
  {
    public GameSession Session;
    public List<ElementalBasePower> Powers;

    void Awake()
    {
      if (!Session.IsPlaythrough)
      {
        foreach (var p in Powers)
        {
          Session.ReceivePower(p);
        }
      }
    }
  }
}
