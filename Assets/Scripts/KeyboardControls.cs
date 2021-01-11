using UnityEngine;

public class KeyboardControls : MonoBehaviour {

	private GameObject mainCamera;
	private float mainCameraMoveSpeed;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		UpdateMainCamera ();
	}

	public void Construct (GameObject mainCameraGO) {
		mainCamera = mainCameraGO;
		mainCameraMoveSpeed = 30;
	}

	void UpdateMainCamera () {
		if (Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl)) {
			if (mainCamera.GetComponent<Camera> ().orthographic) {
				mainCameraMoveSpeed = mainCamera.GetComponent<Camera> ().orthographicSize;
			} else {
				mainCameraMoveSpeed = mainCamera.GetComponent<Camera> ().fieldOfView * 3;
			}
			if (Input.GetKey (KeyCode.UpArrow)) {
				mainCamera.GetComponent<MainCamera> ().AddToOffset (new Vector2 (0, mainCameraMoveSpeed * Time.deltaTime));
			}
			if (Input.GetKey (KeyCode.DownArrow)) {
				mainCamera.GetComponent<MainCamera> ().AddToOffset (new Vector2 (0, -(mainCameraMoveSpeed * Time.deltaTime)));
			}
			if (Input.GetKey (KeyCode.LeftArrow)) {
				mainCamera.GetComponent<MainCamera> ().AddToOffset (new Vector2 (-(mainCameraMoveSpeed * Time.deltaTime), 0));
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				mainCamera.GetComponent<MainCamera> ().AddToOffset (new Vector2 (mainCameraMoveSpeed * Time.deltaTime, 0));
			}
			GetComponent<ControllerMain> ().UpdateUIElementPositions ();
		}
	}
}
