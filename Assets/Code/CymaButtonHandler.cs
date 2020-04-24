using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if (UNITY_2018_3_OR_NEWER)
using UnityEngine.Android;
#endif
public class CymaButtonHandler : MonoBehaviour
{
	static AgoraInterface app = null;
	// Start is called before the first frame update
	void Start()
	{
#if (UNITY_2018_3_OR_NEWER)
			if (Permission.HasUserAuthorizedPermission(Permission.Microphone))
			{
			}
			else
			{
				Permission.RequestUserPermission(Permission.Microphone);
			}
			if (Permission.HasUserAuthorizedPermission(Permission.Camera))
			{
			}
			else
			{
				Permission.RequestUserPermission(Permission.Camera);
			}
#endif
	}
	// Update is called once per frame
	void Update()
	{
	}
	public void OnButtonClick()
	{
		Debug.Log("Button Clicked: " + name);
		// determine which button
		if (name.CompareTo("JoinButton") == 0)
		{
			// join chat
			OnJoinButtonClicked();
		}
		else if (name.CompareTo("LeaveButton") == 0)
		{
			// leave chat
			OnLeaveButtonClicked();
		}
	}
	private void OnJoinButtonClicked()
	{
		Debug.Log("Join button clicked");
		// get channel name from text input
		GameObject go = GameObject.Find("ChannelName");
		InputField input = go.GetComponent<InputField>();
		// init Agora Engine
		if (ReferenceEquals(app, null))
		{
			app = new AgoraInterface();
			app.loadEngine();
		}
		// join channel
		app.joinChannel(input.text);
		SceneManager.sceneLoaded += OnSceneFinishedLoading;
		SceneManager.LoadScene("ChatScene", LoadSceneMode.Single);
	}
	private void OnLeaveButtonClicked()
	{
		Debug.Log("Leave button clicked");
		if (!ReferenceEquals(app, null))
		{
			app.leaveChannel();
			app.unloadEngine();
			app = null;
			SceneManager.LoadScene("WelcomeScene", LoadSceneMode.Single);
		}
	}
	public void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		if (scene.name.CompareTo("ChatScene") == 0)
		{
			if (!ReferenceEquals(app, null))
			{
				app.OnChatSceneLoaded();
			}
			SceneManager.sceneLoaded -= OnSceneFinishedLoading;
		}
	}
}
