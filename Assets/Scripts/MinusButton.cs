using UnityEngine;

public class MinusButton : MonoBehaviour {

	public GameObject MainCamera;

	private bool buttonDown;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (buttonDown) {
			MainCamera.GetComponent<MainCamera> ().ZoomOut ();
		}
	}

	public void OnButtonDown () {
		buttonDown = true;
	}

	public void OnButtonUp () {
		buttonDown = false;
	}
}
