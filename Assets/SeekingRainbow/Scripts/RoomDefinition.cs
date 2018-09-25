using UnityEngine;
using UnityEngine.SceneManagement;

namespace SeekingRainbow.Scripts
{
  public class RoomDefinition: MonoBehaviour
  {
    [Tooltip("Describe the player's goal in this room")]
    [Multiline(10)]
    public string DeveloperNote;
    public SceneReference North;
    public SceneReference East;
    public SceneReference South;
    public SceneReference West;

    void GoTo(SceneReference reference, string member)
    {
      if (string.IsNullOrEmpty(reference.ScenePath))
      {
        Debug.LogWarning("Requested scene not defined in reference " + member);
        return;
      }

      SceneManager.LoadScene(reference.ScenePath);
    }

    public void GoNorth()
    {
      GoTo(North, "North");
    }

    public void GoSouth()
    {
      GoTo(South, "South");
    }

    public void GoWest()
    {
      GoTo(West, "West");
    }

    public void GoEast()
    {
      GoTo(East, "East");
    }
  }
}
