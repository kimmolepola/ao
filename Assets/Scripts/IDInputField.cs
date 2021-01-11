using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IDInputField : MonoBehaviour {

	public GameObject controller;

	private InputField input;
	private bool placeholderIsActive;
	private string text;
	ColorBlock cb;

	// Use this for initialization
	void Start () {
		input = GetComponent<InputField> ();
		var se = new InputField.SubmitEvent ();
		se.AddListener (SubmitCommand);
		input.onEndEdit = se;
		ActivatePlaceholder ();
		CheckIfChanged ();
	}

	// Update is called once per frame
	void Update () {

	}

	public void DeselectText () {
		StartCoroutine (MoveTextEnd_NextFrame ());
	}

	IEnumerator MoveTextEnd_NextFrame () {
		yield return 0;
		input.MoveTextEnd (false);
	}

	public void ActivatePlaceholder () {
		if (input.text == "") {
			input.text = "Set ID";
			placeholderIsActive = true;
		}
	}

	public void DeactivatePlaceholder () {
		if (placeholderIsActive) {
			input.text = "";
			placeholderIsActive = false;
		}
	}

	private void SubmitCommand (string arg0) {
		if (Input.GetKeyDown (KeyCode.Return)) {
			text = arg0;
			input.ActivateInputField ();
			SendIdTextAndCheckIfValid ();
			DeselectText ();
		}
	}

	public void CheckIfChanged () {
		//cb = input.colors;
		//if (placeholderIsActive) {
		//	cb.highlightedColor = Color.white;
		//} else {
		//	if (input.text == text) {
		//		cb.highlightedColor = Color.blue;
		//	} else {
		//		cb.highlightedColor = Color.white;
		//	}
		//}
		//input.colors = cb;
	}

	private void SendIdTextAndCheckIfValid () {
		cb = input.colors;
		if (controller.GetComponent<IDParser> ().SetIdIfValid (text)) {
			cb.highlightedColor = Color.green;
		} else {
			if (text == "") {
				cb.highlightedColor = Color.white;
			} else {
				cb.highlightedColor = Color.yellow;
			}
		}
		input.colors = cb;
	}
}
