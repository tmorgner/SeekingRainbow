using UnityEngine;
using UnityEngine.Events;

// Hi! This script presents the overlay info for our tutorial content, linking you back to the relevant page.
public class LaunchScreen : MonoBehaviour 
{
	// store the GameObject which renders the overlay info
	public GameObject overlay;
  public UnityEvent onGameStarting;
  
	void Awake()
	{
  	ShowLaunchScreen();
	}

	// show overlay info, pausing game time, disabling the audio listener 
	// and enabling the overlay info parent game object
	public void ShowLaunchScreen()
	{
		Time.timeScale = 0f;
		overlay.SetActive (true);
	}

	// continue to play, by ensuring the preference is set correctly, the overlay is not active, 
	// and that the audio listener is enabled, and time scale is 1 (normal)
	public void StartGame()
	{		
		overlay.SetActive (false);
		Time.timeScale = 1f;
    onGameStarting.Invoke();
	}
}
