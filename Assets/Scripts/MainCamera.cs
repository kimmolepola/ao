using UnityEngine;

public class MainCamera : MonoBehaviour {

	public GameObject controller;
	Vector2 offset;

	private float size;
	private float fieldOfView;
	private float delay;
	private GameObject target;
	private Vector3 nonOffsetPosition;

	// Use this for initialization
	void Start () {
		GetComponent<Camera> ().fieldOfView = 50;
		GetComponent<Camera> ().orthographicSize = 50;
		delay = 0;
	}

	// Update is called once per frame
	void Update () {
		if (target) {
			nonOffsetPosition = target.transform.position;
			transform.position = new Vector3 (nonOffsetPosition.x + offset.x, transform.position.y, nonOffsetPosition.z + offset.y);
			controller.GetComponent<ControllerMain> ().UpdateUIElementPositions ();
		}
	}

	public void SetTargetAndResetOffset (GameObject trgt) {
		target = trgt;
		offset = new Vector2 (0, 0);
	}

	public void ZoomIn () {
		if (Time.time > delay) {
			if (Camera.main.orthographic) {
				size = GetComponent<Camera> ().orthographicSize;
				size--;
				if (size >= 1) {
					GetComponent<Camera> ().orthographicSize--;
				}
			} else {
				fieldOfView = GetComponent<Camera> ().fieldOfView;
				if (fieldOfView > 0.0005) {
					GetComponent<Camera> ().fieldOfView = fieldOfView / ((180 - fieldOfView) / 2000 + 1);
				}
			}
			delay = Time.time + 0.03f;
			controller.GetComponent<ControllerMain> ().UpdateUIElementPositions ();
		}
	}

	public void ZoomOut () {
		if (Time.time > delay) {
			if (Camera.main.orthographic) {
				size = GetComponent<Camera> ().orthographicSize;
				size++;
				if (size <= 200) {
					GetComponent<Camera> ().orthographicSize++;
				}
			} else {
				fieldOfView = GetComponent<Camera> ().fieldOfView;
				if (fieldOfView < 175) {
					GetComponent<Camera> ().fieldOfView = fieldOfView * ((180 - fieldOfView) / 2000 + 1);
				}
			}
			delay = Time.time + 0.03f;
			controller.GetComponent<ControllerMain> ().UpdateUIElementPositions ();
		}
	}
	public void AddToOffset (Vector2 os) {
		offset = offset + os;
		if (!target) {
			transform.position = new Vector3 (nonOffsetPosition.x + offset.x, transform.position.y, nonOffsetPosition.z + offset.y);
			nonOffsetPosition = transform.position;
			offset = new Vector2 (0, 0);
		}
	}
}