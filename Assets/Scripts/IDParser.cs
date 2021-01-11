using UnityEngine;
using System.Collections.Generic;

public class IDParser : MonoBehaviour {

	private GameObject dIPanel;
	private GameObject mainCamera;
	private List<List<GameObject>> targets;
	private GameObject target;
	private bool toggleWaypoint;
	private bool toggleFollow;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void Construct (GameObject mainCameraGO, List<List<GameObject>> targetsGO, GameObject dIPanelGO) {
		dIPanel = dIPanelGO;
		mainCamera = mainCameraGO;
		targets = targetsGO;
	}

	public void ToggleTargetWaypointDisplayOnOff (bool toggle) {
		toggleWaypoint = toggle;
		if (toggleWaypoint) {
			if (target) {
				if (target.GetComponent<AirplaneMain> ()) {
					target.GetComponent<AirplaneMain> ().ActivateWaypointGameObjects (true, dIPanel);
				}
			}
		} else {
			if (target) {
				if (target.GetComponent<AirplaneMain> ()) {
					target.GetComponent<AirplaneMain> ().ActivateWaypointGameObjects (false, dIPanel);
				}
			}
		}
	}

	public void ToggleTargetFollowCameraOnOff (bool toggle) {
		toggleFollow = toggle;
		if (toggleFollow) {
			mainCamera.GetComponent<MainCamera> ().SetTargetAndResetOffset (target);
		} else {
			mainCamera.GetComponent<MainCamera> ().SetTargetAndResetOffset (null);
		}
	}

	public bool SetIdIfValid (string targetId) {
		if (targetId == "") {
			target = null;
			ToggleTargetFollowCameraOnOff (toggleFollow);
			return false;
		} else {
			foreach (List<GameObject> golist in targets) {
				foreach (GameObject go in golist) {
					if (targetId == go.GetComponent<WorldPositionAndId> ().GetId ()) {
						target = go;
						ToggleTargetFollowCameraOnOff (toggleFollow);
						return true;
					}
				}
			}
			ToggleTargetFollowCameraOnOff (toggleFollow);
			return false;
		}
	}
}
