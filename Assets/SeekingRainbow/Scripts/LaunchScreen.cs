using UnityEngine;
using UnityEngine.Events;

// Hi! This script presents the overlay info for our tutorial content, linking you back to the relevant page.
public class LaunchScreen : MonoBehaviour 
{
	// allow user to choose whether to show this menu 
	public bool showAtStart = true;

	// store the GameObject which renders the overlay info
	public GameObject overlay;

	// string to store Prefs Key with name of preference for showing the overlay info
	public static string showAtStartPrefsKey = "showLaunchScreen";

	// used to ensure that the launch screen isn't more than once per play session if the project reloads the main scene
	private static bool alreadyShownThisSession = false;

  public UnityEvent onGameStarting;
  
	void Awake()
	{
		// have we already shown this once?
		if(alreadyShownThisSession)
		{
			StartGame();
		}
		else
		{
			alreadyShownThisSession = true;

			// Check player prefs for show at start preference
			if (PlayerPrefs.HasKey(showAtStartPrefsKey))
			{
				showAtStart = PlayerPrefs.GetInt(showAtStartPrefsKey) == 1;
			}

			// show the overlay info or continue to play the game
			if (showAtStart) 
			{
				ShowLaunchScreen();
			}
			else 
			{
				StartGame ();
			}	
		}
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
