using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainbuttontesti : MonoBehaviour {

	RectTransform panel;

	// Use this for initialization
	void Start () {
		panel = this.transform as RectTransform;
	}
	
	// Update is called once per frame
	void Update () {
		panel.SetAsLastSibling ();
	}
}
