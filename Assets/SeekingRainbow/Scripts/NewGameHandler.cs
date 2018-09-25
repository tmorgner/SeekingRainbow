using UnityEngine;
using UnityEngine.SceneManagement;

namespace SeekingRainbow.Scripts
{
  public class NewGameHandler: MonoBehaviour
  {
    public SceneReference gameScene;
    public GameSession session;

    public void OnStartGame()
    {
      session.Reset();
      session.IsPlaythrough = true;

      // todo: In a clean world this should be done by showing a loading screen and
      // then loading the scene asynchronously. We are not clean here.
      SceneManager.LoadScene(gameScene.ScenePath);

    }
  }
}
