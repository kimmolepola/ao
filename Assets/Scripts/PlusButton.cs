using UnityEngine;

public class PlusButton : MonoBehaviour {

	public GameObject MainCamera;

	private bool buttonDown;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (buttonDown) {
			MainCamera.GetComponent<MainCamera> ().ZoomIn ();
		}
	}

	public void OnButtonDown () {
		buttonDown = true;
	}

	public void OnButtonUp () {
		buttonDown = false;
	}
}
