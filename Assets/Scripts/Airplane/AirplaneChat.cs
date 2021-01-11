using UnityEngine;
using System.Collections.Generic;
using System;

public class AirplaneChat : MonoBehaviour {

	private ChatText chatTextScript;
	private List<string> chatCommentsList;
	private bool overrideChatIdString;
	private bool newChatComments;
	private string iDColor;
	private float delayedChatCommentTime;

	// Use this for initialization
	void Start () {
		chatCommentsList = new List<string> ();
		overrideChatIdString = true;
		CheckIn ();
	}

	// Update is called once per frame
	void Update () {
		if (newChatComments) {
			if (Time.time > delayedChatCommentTime) {
				SendChatCommentsToChat (chatCommentsList);
				newChatComments = false;
				chatCommentsList.Clear ();
			}
		}
	}

	public void Construct (string iDColr, ChatText chatTextScrpt) {
		chatTextScript = chatTextScrpt;
		iDColor = iDColr;
	}

	private string SelectCheckInCompliment () {
		int nowHour = int.Parse (DateTime.Now.ToString ("HH"));
		if (nowHour >= 18 || nowHour < 3) {
			return ", good evening";
		} else if (nowHour >= 3 && nowHour < 12) {
			return ", good morning";
		}
		return ", good afternoon";
	}

	private void CheckIn () {
		string request = "";
		if (GetComponent<AirplaneMain> ().GetMode () == "standby") {
			request = ", requesting takeoff";
		}
		AddToChatList ("Tower, " + GetComponent<AirplaneMain> ().GetId () + SelectCheckInCompliment () + request);
	}

	private void SendChatCommentsToChat (List<string> chatComments) {
		chatTextScript.StartNewLine (iDColor);
		chatTextScript.EnableBold ();
		foreach (string s in chatComments) {
			chatTextScript.AddText (s);
		}
		if (!overrideChatIdString) {
			chatTextScript.AddComma ();
			chatTextScript.AddText (GetComponent<AirplaneMain> ().GetId ().ToString ());
		}
		overrideChatIdString = false;
		chatTextScript.AddDot ();
		chatTextScript.DisableBold ();
		chatTextScript.EndLine ();
	}

	public void AddToChatList (string s) {
		if (chatCommentsList.Count == 0) {
			s = char.ToUpper (s[0]) + s.Substring (1);
		}
		chatCommentsList.Add (s);
		delayedChatCommentTime = Time.time + UnityEngine.Random.Range (1.5f, 8f);
		newChatComments = true;
	}

	public void OverrideChatList (string s) {
		chatCommentsList.Clear ();
		s = char.ToUpper (s[0]) + s.Substring (1);
		chatCommentsList.Add (s);
		delayedChatCommentTime = Time.time + UnityEngine.Random.Range (1.5f, 8f);
		newChatComments = true;
	}

	public string GetIDColor () {
		return iDColor;
	}
	public void OverrideChatIDString () {
		overrideChatIdString = true;
	}

	private string SelectCheckOutCompliment () {
		int nowHour = int.Parse (DateTime.Now.ToString ("HH"));
		if (nowHour >= 19) {
			return ", good evening";
		} else if (nowHour < 5) {
			return ", good night";
		} else if (nowHour >= 5 && nowHour < 9) {
			return ", good morning";
		}
		return ", good day";
	}

	public void ExecuteLandingCompletedMessage () {
		chatTextScript.StartNewLine (iDColor);
		chatTextScript.EnableBold ();
		chatTextScript.AddText ("landing completed, " + GetComponent<AirplaneMain> ().GetId () + SelectCheckOutCompliment ());
		chatTextScript.DisableBold ();
		chatTextScript.EndLine ();
	}

	public void ExecuteLandingMessage () {
		chatTextScript.StartNewLine (iDColor);
		chatTextScript.EnableBold ();
		chatTextScript.AddText ("landing, " + GetComponent<AirplaneMain> ().GetId () + ".");
		chatTextScript.DisableBold ();
		chatTextScript.EndLine ();
	}

	public void ExecuteFuelMessage (int fuelLevel) {
		chatTextScript.StartNewLine (iDColor);
		chatTextScript.EnableBold ();
		chatTextScript.AddText ("fuel " + fuelLevel + ",  " + GetComponent<AirplaneMain> ().GetId () + ".");
		chatTextScript.DisableBold ();
		chatTextScript.EndLine ();
	}
}
