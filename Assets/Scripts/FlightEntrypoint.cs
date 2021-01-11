using UnityEngine;

public class FlightEntrypoint : ScriptableObject {

	Vector2 position;
	int direction;
	string id;
	bool takeoff;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void SetPosition (Vector2 pos) {
		position = pos;
	}

	public void SetDirection (int dir) {
		direction = dir;
	}

	public void SetId (string entrypointId) {
		id = entrypointId;
	}

	public void SetTakeoff (bool b) {
		takeoff = b;
	}

	public Vector2 GetPosition () {
		return position;
	}

	public int GetDirection () {
		return direction;
	}

	public string GetId () {
		return id;
	}

	public bool GetTakeoff () {
		return takeoff;
	}
}
