using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SeekingRainbow.Scripts
{
  public class StartAnimationBehaviour: MonoBehaviour
  {
    private int index;
    private float nextEvent;
    public List<Sprite> Sprites;
    public List<float> Times;
    public SceneReference NextScene;

    void Start()
    {
      index = -1;
      nextEvent = 0;
    }

    private void Update()
    {
      if (Time.time >= nextEvent || Input.GetKeyDown(KeyCode.Space))
      {
        index += 1;
        if (index >= Math.Min(Times.Count, Sprites.Count))
        {
          SceneManager.LoadScene(NextScene.ScenePath);
          return;
        };

        GetComponent<SpriteRenderer>().sprite = Sprites[index];
        nextEvent = Time.time + Times[index];
      }
    }
  }
}
