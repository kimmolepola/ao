using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChatInputField : MonoBehaviour {

	public GameObject controller;

	private InputField input;
	private bool gameOver;
	private bool placeholderIsActive;

	// Use this for initialization
	void Start () {
		input = GetComponent<InputField> ();
		var se = new InputField.SubmitEvent ();
		se.AddListener (SubmitCommand);
		input.onEndEdit = se;
		ActivatePlaceholder ();
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
			input.text = "Enter text...";
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
			if (controller != null) {
				if (!gameOver) {
					controller.GetComponent<ChatParser> ().Parse (arg0);
				}
				input.text = "";
				input.ActivateInputField ();
			}
		}
	}

	// Update is called once per frame
	void Update () {
	}

	public void setGameOver (bool b) {
		gameOver = b;
	}
}
