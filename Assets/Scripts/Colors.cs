using UnityEngine;

public class Colors : ScriptableObject {

	private string[] colors;
	private bool isSetUp;

	// Use this for initialization
	void Start () {
		SetUpColors ();
		isSetUp = true;
	}

	// Update is called once per frame
	void Update () {

	}

	public string PickARandomColor () {
		if (!isSetUp) {
			SetUpColors ();
		}
		return "<color=" + colors[UnityEngine.Random.Range (0, colors.Length)] + ">";
	}

	void SetUpColors () {
		colors = new string[] {
			"#330000",
			"#330d00",
			"#331a00",
			"#332600",
			"#333300",
			"#263300",
			"#1a3300",
			"#0d3300",
			"#003300",
			"#00330d",
			"#00331a",
			"#003326",
			"#003333",
			"#002633",
			"#001a33",
			"#000d33",
			"#000033",
			"#0d0033",
			"#1a0033",
			"#260033",
			"#330033",
			"#330026",
			"#33001a",
			"#33000d",
			"#330000",
			"#660000",
			"#661a00",
			"#663300",
			"#664d00",
			"#666600",
			"#4d6600",
			"#336600",
			"#1a6600",
			"#006600",
			"#00661a",
			"#006633",
			"#00664d",
			"#006666",
			"#004d66",
			"#003366",
			"#001a66",
			"#000066",
			"#1a0066",
			"#330066",
			"#4d0066",
			"#660066",
			"#66004d",
			"#660033",
			"#66001a",
			"#660000"
		};
	}
}
