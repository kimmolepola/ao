using UnityEngine;
using System.Collections;

public class AirplaneDot : MonoBehaviour {

	Vector3 worldPosition;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void UpdateWorldPosition (Vector3 worldPos) {
		worldPosition = worldPos;
	}

	public void UpdatePosition () {
		transform.position = new Vector3 (Camera.main.WorldToScreenPoint (worldPosition).x, Camera.main.WorldToScreenPoint (worldPosition).y, 0);
	}
}
