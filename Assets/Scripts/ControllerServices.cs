using UnityEngine;
using System.Collections.Generic;

public class ControllerServices : MonoBehaviour {

	public GameObject clock;
	public GameObject inputField;

	private List<int> airplanesTooClose = new List<int> ();
	private int landed;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void ProcessOutOfFuel (int airplaneId, ChatText chatTextScript) {
		inputField.GetComponent<ChatInputField> ().setGameOver (true);
		chatTextScript.StartNewLine ("<color=red>");
		chatTextScript.EnableBold ();
		chatTextScript.AddText ("Airplane " + airplaneId + " out of fuel.");
		chatTextScript.AddText ("YOU ARE FIRED");
		chatTextScript.DisableBold ();
		chatTextScript.EndLine ();
	}

	public void ProcessAirplanesTooClose (int airplaneId, ChatText chatTextScript) {
		airplanesTooClose.Add (airplaneId);
		if (airplanesTooClose.Count == 2) {
			inputField.GetComponent<ChatInputField> ().setGameOver (true);
			chatTextScript.StartNewLine ("<color=red>");
			chatTextScript.EnableBold ();
			chatTextScript.AddText ("Airplanes " + airplanesTooClose[0] + " and " + airplanesTooClose[1] + " too close.");
			chatTextScript.AddText ("YOU ARE FIRED");
			chatTextScript.DisableBold ();
			chatTextScript.EndLine ();
		}
	}

	public string CreateThreeLetterId () {
		string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		return "" + letters[UnityEngine.Random.Range (0, letters.Length)] + letters[UnityEngine.Random.Range (0, letters.Length)] + letters[UnityEngine.Random.Range (0, letters.Length)];
	}

	public void addLanded () {
		landed++;
		clock.GetComponent<Clock> ().SetLanded (landed);
	}

	public void RemoveAirplane (int i, Schedule scheduleScript) {
		ControllerMain controllerScript = GetComponent<ControllerMain> ();
		controllerScript.GetAirplanesList ().Remove (controllerScript.GetAirplanesDictionary ()[i]);
		controllerScript.GetAirplanesDictionary ().Remove (i);
		scheduleScript.RemoveIdFromActiveIds (i);
	}
}
