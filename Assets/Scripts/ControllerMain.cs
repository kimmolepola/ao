using UnityEngine;
using System.Collections.Generic;

public class ControllerMain : MonoBehaviour {

	public GameObject chatText;
	public GameObject airplane;
	public GameObject ground;
	public GameObject mainCamera;
	public GameObject dIPanel;

	private Dictionary<int, GameObject> airplanesDictionary = new Dictionary<int, GameObject> ();
	private List<GameObject> airplanesList = new List<GameObject> ();
	private List<List<GameObject>> targets = new List<List<GameObject>> ();
	private Schedule scheduleScript;
	private ControllerServices controllerServicesScript;
	private BeaconsController beaconsControllerScript;
	private ApproachesController approachesControllerScript;

	private ScheduledFlight flight;
	private bool airplaneTextsOffset;
	private int counter;
	private string activeDisplayName;

	// Use this for initialization
	void Start () {
		SetupControllerScripts ();

		QualitySettings.antiAliasing = 0;
		QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
		counter = 0;
		switchDisplay ("radar");

		//CreateFirstFlight ();

	}

	// Update is called once per frame
	void Update () {
		if (Time.time < 1) {
			UpdateUIElementPositions ();
		}
		counter++;
		if (counter > 100) {
			CheckFlight ();
			counter = 0;
		}
	}

	public void UpdateUIElementPositions () {
		foreach (GameObject go in beaconsControllerScript.GetBeaconsList ()) {
			go.GetComponent<Beacon> ().UpdateBeaconUIPosition ();
		}
		foreach (GameObject go in airplanesList) {
			go.GetComponent<AirplaneMain> ().UpdateAirplaneUIElementUIPositions ();
			//tähän waypoint kontrollia
		}
		foreach (GameObject go in approachesControllerScript.GetApproachesList ()) {
			go.GetComponent<Approach> ().UpdateUIPosition ();
		}
	}

	public void switchDisplay (string displayName) {
		activeDisplayName = displayName;
		foreach (GameObject go in airplanesList) {
			go.GetComponent<AirplaneMain> ().ChangeDisplayName (displayName);
		}
		if (displayName == "radar") {
			ground.SetActive (false);
			foreach (GameObject go in approachesControllerScript.GetApproachesList ()) {
				go.GetComponent<SpriteRenderer> ().enabled = true;
				go.GetComponent<Approach> ().approachText.SetActive (true);
			}
			foreach (GameObject go in beaconsControllerScript.GetBeaconsList ()) {
				go.SetActive (true);
			}
			mainCamera.GetComponent<Camera> ().orthographic = true;
			UpdateUIElementPositions ();
		}
		if (displayName == "satellite") {
			ground.SetActive (true);
			foreach (GameObject go in approachesControllerScript.GetApproachesList ()) {
				go.GetComponent<SpriteRenderer> ().enabled = false;
				go.GetComponent<Approach> ().approachText.SetActive (false);
			}
			foreach (GameObject go in beaconsControllerScript.GetBeaconsList ()) {
				go.SetActive (false);
			}
			mainCamera.GetComponent<Camera> ().orthographic = false;
			UpdateUIElementPositions ();
		}
	}

	public void toggleAirplaneTextsOffset (bool offsetBool) {
		airplaneTextsOffset = offsetBool;
		foreach (GameObject ap in airplanesList) {
			ap.GetComponent<AirplaneMain> ().GetAirplaneText ().GetComponent<AirplaneText> ().RandomizeOffset (offsetBool, ap.GetComponent<AirplaneMain> ().GetMode ());
		}
	}

	public void ProcessOutOfFuel (int airplaneId) {
		controllerServicesScript.ProcessOutOfFuel (airplaneId, chatText.GetComponent<ChatText> ());
	}

	public void ProcessAirplanesTooClose (int airplaneId) {
		controllerServicesScript.ProcessAirplanesTooClose (airplaneId, chatText.GetComponent<ChatText> ());
	}

	public void addLanded () {
		controllerServicesScript.addLanded ();
	}

	public void RemoveAirplane (int i) {
		controllerServicesScript.RemoveAirplane (i, scheduleScript);
	}

	public bool GetAirplaneTextsOffset () {
		return airplaneTextsOffset;
	}

	public GameObject GetDIPanel () {
		return dIPanel;
	}

	public List<List<GameObject>> GetTargets () {
		return targets;
	}

	public string CreateThreeLetterId () {
		return controllerServicesScript.CreateThreeLetterId ();
	}

	public Dictionary<int, GameObject> GetAirplanesDictionary () {
		return airplanesDictionary;
	}

	public List<GameObject> GetAirplanesList () {
		return airplanesList;
	}

	private void CreateFirstFlight () {
		GameObject ap = Instantiate (airplane);
		ap.transform.position = new Vector3 (0, 0, 0);
		ap.GetComponent<AirplaneMain> ().Construct (1111, 2000, 240, UnityEngine.Random.Range (0, 360), "default", activeDisplayName, this.gameObject, chatText.GetComponent<ChatText> (), GetComponent<ColorsInterface> ().PickARandomColor (), dIPanel, airplaneTextsOffset);
		airplanesDictionary.Add (1111, ap);
		airplanesList.Add (ap);
		scheduleScript.AddToActiveIds (1111);
	}

	private void CheckFlight () {
		flight = scheduleScript.dequeueFlightIfReady ();
		if (flight) {
			GameObject ap = Instantiate (airplane);
			ap.transform.position = new Vector3 (flight.GetEntrypointPosition ().x, 0, flight.GetEntrypointPosition ().y);
			ap.GetComponent<AirplaneMain> ().Construct (flight.GetId (), flight.GetAltitude (), flight.GetSpeed (), flight.GetHeading (), flight.GetMode (), activeDisplayName, this.gameObject, chatText.GetComponent<ChatText> (), GetComponent<ColorsInterface> ().PickARandomColor (), dIPanel, airplaneTextsOffset);
			airplanesDictionary.Add (flight.GetId (), ap);
			airplanesList.Add (ap);
		}
	}

	private void AddToTargetsList (List<GameObject> gameObjectsList) {
		targets.Add (gameObjectsList);
	}

	private void SetupControllerScripts () {
		scheduleScript = GetComponent<Schedule> ();
		controllerServicesScript = GetComponent<ControllerServices> ();
		beaconsControllerScript = GetComponent<BeaconsController> ();
		approachesControllerScript = GetComponent<ApproachesController> ();

		beaconsControllerScript.Construct (dIPanel);
		approachesControllerScript.Construct (beaconsControllerScript.GetBeaconsList ());
		scheduleScript.Construct (approachesControllerScript.GetApproachesList ());
		AddToTargetsList (beaconsControllerScript.GetBeaconsList ());
		AddToTargetsList (airplanesList);
		GetComponent<ChatParser> ().Construct (chatText.GetComponent<ChatText> (), airplanesDictionary, airplanesList, beaconsControllerScript.GetBeaconsDictionary (), approachesControllerScript.GetApproachesDictionary ());
		GetComponent<KeyboardControls> ().Construct (mainCamera);
		GetComponent<IDParser> ().Construct (mainCamera, targets, dIPanel);
	}
}
