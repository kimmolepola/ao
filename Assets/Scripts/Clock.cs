using UnityEngine;
using System;
using UnityEngine.UI;

public class Clock : MonoBehaviour {

	private int landed;

	// Use this for initialization
	void Start () {
		landed = 0;
	}

	// Update is called once per frame
	void Update () {
		GetComponent<Text> ().text = "" + DateTime.Now.ToString ("HH:mm:ss") + "\nLanded: " + landed;
	}

	public void SetLanded (int i) {
		landed = i;
	}
}
