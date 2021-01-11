using UnityEngine;

public class WaypointImage : MonoBehaviour {

	private Vector3 worldPosition;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void SetWorldPosition (Vector3 worldPositionVector3) {
		worldPosition = worldPositionVector3;
	}

	public Vector3 GetWorldPosition () {
		return worldPosition;
	}
}
