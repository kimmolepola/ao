using UnityEngine;

public class Beacon : MonoBehaviour {

	private Vector3 beaconPosition;
	private string id;

	// Use this for initialization
	void Start () {
		UpdateBeaconUIPosition ();
	}

	// Update is called once per frame
	void Update () {

	}

	public void SetBeaconPosition (Vector3 bp) {
		beaconPosition = bp;
	}

	public void UpdateBeaconUIPosition () {
		transform.position = new Vector3 (Camera.main.WorldToScreenPoint (beaconPosition).x, Camera.main.WorldToScreenPoint (beaconPosition).y, 0);
	}

	public Vector3 GetWorldPosition () {
		return beaconPosition;
	}

	public void SetId (string beaconId) {
		id = beaconId;
	}

	public string GetId () {
		return id;
	}
}
