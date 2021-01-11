using UnityEngine;

public class WorldPositionAndId : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public Vector3 GetWorldPosition () {
		if (GetComponent<AirplaneMain> ()) {
			return transform.position;
		}
		if (GetComponent<Beacon> ()) {
			return GetComponent<Beacon> ().GetWorldPosition ();
		}
		return new Vector3 (0, 0, 0);
	}

	public string GetId () {
		if (GetComponent<AirplaneMain> ()) {
			return GetComponent<AirplaneMain> ().GetId ();
		}
		if (GetComponent<Beacon> ()) {
			return GetComponent<Beacon> ().GetId ();
		}
		return null;
	}
}
