using UnityEngine;
using UnityEngine.UI;

public class RadarButton : MonoBehaviour {

	public GameObject controller;
	public GameObject satelliteButton;

	// Use this for initialization
	void Start () {
		GetComponent<Button> ().onClick.AddListener (() => {
			SwitchButtonOn ();
		});
		GetComponent<Image> ().color = new Color32 (135, 255, 135, 255);
	}

	// Update is called once per frame
	void Update () {

	}

	void SwitchButtonOn () {
		controller.GetComponent<ControllerMain> ().switchDisplay ("radar");
		GetComponent<Image> ().color = new Color32 (135, 255, 135, 255);
		satelliteButton.GetComponent<Image> ().color = new Color32 (185, 210, 235, 255);
	}

	public void SwitchButtonOff () {
		GetComponent<Image> ().color = new Color32 (185, 210, 235, 255);
	}
}


