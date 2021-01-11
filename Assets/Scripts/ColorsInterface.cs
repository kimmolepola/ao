using UnityEngine;

public class ColorsInterface : MonoBehaviour {

	Colors colors;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

	public string PickARandomColor () {
		if (!colors) {
			colors = ScriptableObject.CreateInstance<Colors> ();
		}
		return colors.PickARandomColor ();
	}
}
