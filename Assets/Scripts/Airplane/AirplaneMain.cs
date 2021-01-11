using UnityEngine;
using System.Collections;
using System;

public class AirplaneMain : MonoBehaviour {

	public GameObject airplaneText;
	public GameObject airplaneSprite;

	private GameObject controller;
	private AirplaneAltitude airplaneAltitudeScript;
	private AirplaneSpeed airplaneSpeedScript;
	private AirplaneHeading airplaneHeadingScript;
	private AirplaneWaypoints airplaneWaypointsScript;
	private AirplaneLanding airplaneLandingScript;
	private AirplaneDots airplaneDotsScript;
	private AirplaneChat airplaneChatScript;


	private string mode;
	private bool readyForDestroy;
	private bool abort;
	private int id;
	private int timeCounterForUIElementsUpdate;
	private float speedMapScaleFactor;
	private float timeCounterForDelayedCommands;
	private int takeoffHeading;
	private bool clearedToTakeoff;
	private string displayName;
	private string runwayName;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (mode == "takeoff" || mode == "standby") {
			ProcessTakeoff ();
		}

		if (readyForDestroy) {
			if (Time.time > timeCounterForDelayedCommands) {
				DestroyThisEntity ();
			}
		}

		if (abort) {
			if (Time.time > timeCounterForDelayedCommands) {
				ProcessAbort ();
			}
		}
		UpdatePosition ();
	}

	void OnTriggerEnter (Collider collider) {
		AirplaneMain otherAirplaneMainScript = collider.gameObject.GetComponent<AirplaneMain> ();
		if (otherAirplaneMainScript) {
			if (otherAirplaneMainScript.GetAltitude () < GetAltitude () + 900 && otherAirplaneMainScript.GetAltitude () > GetAltitude () - 900) {
				controller.GetComponent<ControllerMain> ().ProcessAirplanesTooClose (id);
			}
		}
	}

	public void Construct (int iDInt, int altitudeInt, int speedInt, int headingInt, string modeString, string dispNameString, GameObject controllerGO, ChatText chatTextScript, string iDColorString, GameObject dIPanelGO, bool airplaneTextOffsetBool) {
		airplaneSprite = Instantiate (airplaneSprite);
		speedMapScaleFactor = 0.000514444444f; // 1 kts = 0.000514444444 km / s
		SetupTimeCounterForUIElementsUpdate ();
		SetupAirplaneScripts ();
		SetupVariablesOfThisScripts (dispNameString, iDInt, modeString, headingInt, controllerGO);
		ConstructOtherScriptsOfThisGameObject (altitudeInt, speedInt, headingInt, iDColorString, chatTextScript);
		SetupAirplaneTextGameObject (iDInt, dIPanelGO, airplaneTextOffsetBool, modeString);
		transform.eulerAngles = new Vector3 (0, headingInt, 0);
		if (modeString == "standby") {
			RunIfConstructTakeoffTrue (headingInt);
		} else {
			RunIfConstructTakeoffFalse (dispNameString);
		}
		ChangeDisplayName (dispNameString);
	}

	public void SetMode (string modeString) {
		mode = modeString;
	}

	public void ChangeDisplayName (string dn) {
		displayName = dn;
		if (dn == "radar") {
			airplaneSprite.SetActive (false);
			if (mode == "standby") {
				airplaneDotsScript.SetActive (false);
				airplaneText.SetActive (false);
			} else {
				airplaneDotsScript.SetActive (true);
				airplaneText.SetActive (true);
			}

		} else if (dn == "satellite") {
			airplaneDotsScript.SetActive (false);
			airplaneText.SetActive (false);
			if (mode == "standby") {
				airplaneSprite.SetActive (false);
			} else {
				airplaneSprite.SetActive (true);
			}
		}
	}

	public void UpdateAirplaneUIElementUIPositions () {
		if (airplaneDotsScript) {
			airplaneDotsScript.UpdateUIPosition ();
		}
		airplaneText.GetComponent<AirplaneText> ().UpdateUIPosition (airplaneDotsScript.getAirplaneMainDotPosition ());
		if (airplaneWaypointsScript.WaypointDisplayIsActive ()) {
			airplaneWaypointsScript.UpdateWaypointImagesUIPositionsAndDrawLines ();
		}
	}

	public void Abort () {
		abort = true;
		timeCounterForDelayedCommands = Time.time + UnityEngine.Random.Range (1.5f, 3f);
	}

	public void GrantLandingClearance (string clearedApprchId) {
		runwayName = clearedApprchId;
		airplaneLandingScript.GrantLandingClearance (clearedApprchId);
	}

	public void ControlApproachAreaEnterAndExit (bool enter, Transform approachTransform, string id) {
		airplaneLandingScript.ControlApproachAreaEnterAndExit (enter, approachTransform, id);
	}

	public bool CheckIfReadyToLand () {
		return airplaneLandingScript.CheckIfReadyToLand ();
	}

	public void ActivateOutOfFuelMode () {
		controller.GetComponent<ControllerMain> ().ProcessOutOfFuel (id);
		airplaneAltitudeScript.ActivateGlideMode ();
	}

	public void GrantTakeoffClearance (string runwayNameString) {
		runwayName = runwayNameString;
		timeCounterForDelayedCommands = Time.time + UnityEngine.Random.Range (8, 16);
		clearedToTakeoff = true;
	}

	public void ExecuteLandingMessage () {
		airplaneChatScript.ExecuteLandingMessage ();
	}

	public void ExecuteLandingCompletedMessage () {
		airplaneChatScript.ExecuteLandingCompletedMessage ();
	}

	public void AddLanded () {
		controller.GetComponent<ControllerMain> ().addLanded ();
	}

	public void ActivateReadyForDestroy () {
		readyForDestroy = true;
		timeCounterForDelayedCommands = Time.time + 3;
	}

	public void ActivateBrakingMode () {
		airplaneSpeedScript.ActivateBrakingMode ();
	}

	public void CommandHeading (int headingInt, int normalLeftOrRightInt) {
		airplaneHeadingScript.CommandHeading (headingInt, normalLeftOrRightInt);
		mode = "default";
	}

	public void CommandAltitude (float altitudeFloat) {
		airplaneAltitudeScript.CommandAltitude (altitudeFloat);
	}

	public void CommandSpeed (int speedInt) {
		airplaneSpeedScript.CommandSpeed (speedInt);
	}

	public void CommandHeadingToPosition (Vector3 targetPositionVector, string targetName, bool defaultMode) {
		airplaneHeadingScript.CommandHeadingToPosition (targetPositionVector, targetName);
		if (defaultMode) {
			mode = "default";
		}
	}

	public void CommandHoldingPattern (GameObject beaconGameObject) {
		airplaneHeadingScript.CommandHoldingPattern (beaconGameObject);
		mode = "holding pattern";
	}

	public void CommandWaypointMode () {
		airplaneWaypointsScript.Activate (true);
		mode = "waypoint";
	}

	public bool CheckHeadingCommand (int headingInt) {
		return airplaneHeadingScript.CheckCommand (headingInt);
	}

	public bool CheckAltitudeCommand (int altitudeInt) {
		return airplaneAltitudeScript.CheckCommand (altitudeInt);
	}

	public bool CheckSpeedCommand (int speedInt) {
		return airplaneSpeedScript.CheckCommand (speedInt);
	}

	public void CommandHeadingWithoutDelay (int hdg) {
		airplaneHeadingScript.CommandHeadingWithoutDelay (hdg);
	}

	public void CommandAltitudeWithoutDelay (float alt) {
		airplaneAltitudeScript.CommandAltitudeWithoutDelay (alt);
	}

	public void CommandSpeedWithoutDelay (int spd) {
		airplaneSpeedScript.CommandSpeedWithoutDelay (spd);
	}

	public int RequestFuelInfo () {
		return airplaneSpeedScript.RequestFuelInfo ();
	}

	public string GetId () {
		return id.ToString ();
	}

	public GameObject GetAirplaneText () {
		return airplaneText;
	}

	public GameObject GetController () {
		return controller;
	}

	public string GetMode () {
		return mode;
	}

	public float GetAltitude () {
		return airplaneAltitudeScript.GetAltitude ();
	}

	public float GetSpeed () {
		return airplaneSpeedScript.GetSpeed ();
	}

	public int GetSpeedAssigned () {
		return airplaneSpeedScript.GetSpeedAssigned ();
	}

	public string GetCurrentWaypointName () {
		return airplaneWaypointsScript.GetCurrentWaypointName ();
	}

	public string ReturnStatusString () {
		return airplaneAltitudeScript.ReturnAltitudeStatusString () + ", ";
	}

	public void SetAltitudeMin (int alt) {
		airplaneAltitudeScript.SetAltitudeMin (alt);
	}

	public void SetSpeedMin (int spd) {
		airplaneSpeedScript.SetSpeedMin (spd);
	}

	public void AddStatusReportToChatList () {
		StartCoroutine (ProcessStatusReportRequest ());
	}

	public void ActivateWaypointGameObjects (bool b, GameObject dIPanel) {
		airplaneWaypointsScript.ActivateWaypointGameObjects (b, dIPanel);
	}

	private IEnumerator ProcessStatusReportRequest () {
		Vector3 referenceForward = transform.forward;
		yield return null;
		if (mode == "standby") {
			airplaneChatScript.AddToChatList ("ready for takeoff runway " + runwayName);
		} else if (mode == "takeoff") {
			airplaneChatScript.AddToChatList ("executing takeoff runway " + runwayName);
		} else if (mode == "landing") {
			airplaneChatScript.AddToChatList ("landing to runway " + runwayName);
		} else if (mode == "waypoint") {
			airplaneChatScript.AddToChatList (airplaneAltitudeScript.ReturnAltitudeStatusString ()
				+ ", " + airplaneHeadingScript.ReturnHeadingStatusString (referenceForward)
				+ ", " + airplaneSpeedScript.ReturnSpeedStatusString ()
				+ ", " + airplaneWaypointsScript.ReturnWaypointStatusString ());
		} else {
			airplaneChatScript.AddToChatList (airplaneAltitudeScript.ReturnAltitudeStatusString ()
				+ ", " + airplaneHeadingScript.ReturnHeadingStatusString (referenceForward)
				+ ", " + airplaneSpeedScript.ReturnSpeedStatusString ());
		}
	}

	private void ProcessTakeoff () {
		if (clearedToTakeoff && Time.time > timeCounterForDelayedCommands) {
			mode = "takeoff";
		}
		if (mode == "standby") {
			airplaneSpeedScript.SetSpeed (0);
			airplaneAltitudeScript.SetAltitude (0);
			airplaneHeadingScript.SetHeading (takeoffHeading);
		} else {
			GetComponent<Collider> ().enabled = true;
			if (displayName == "radar") {
				airplaneDotsScript.SetActive (true);
				airplaneText.SetActive (true);
				if (controller.GetComponent<ControllerMain> ().GetAirplaneTextsOffset ()) {
					airplaneText.GetComponent<AirplaneText> ().SetLineImageActive (true);
				}
				airplaneText.GetComponent<AirplaneText> ().UpdateUIPosition (airplaneDotsScript.getAirplaneMainDotPosition ());
			}
			if (airplaneSpeedScript.GetSpeed () < 145) {
				airplaneAltitudeScript.SetAltitude (0);
				airplaneHeadingScript.SetHeading (takeoffHeading);
			} else {
				airplaneAltitudeScript.SetAltitudeMin (1000);
				airplaneAltitudeScript.CommandAltitudeWithoutDelay (airplaneAltitudeScript.GetAltitudeAssigned ());
				airplaneSpeedScript.SetSpeedMin (140);
				mode = "default";
			}
		}
	}

	private void ProcessAbort () {
		airplaneLandingScript.Abort ();
		airplaneAltitudeScript.Abort (mode);
		airplaneSpeedScript.Abort (mode);
		airplaneHeadingScript.Abort ();
		mode = "abort";
		abort = false;
	}

	private void UpdatePosition () {
		UpdateWorldPosition ();
		if (Time.time > timeCounterForUIElementsUpdate) {
			UpdateAirplaneUIElements ();
			timeCounterForUIElementsUpdate += 3;
		}
		airplaneSprite.transform.position = transform.position;
		airplaneSprite.transform.eulerAngles = new Vector3 (90, 0, transform.eulerAngles.y * -1);
	}

	private void UpdateWorldPosition () {
		transform.position += transform.forward * Time.deltaTime * airplaneSpeedScript.GetSpeed () * speedMapScaleFactor;
	}

	private void UpdateAirplaneUIElements () {
		UpdateAirplaneText ();
		airplaneDotsScript.UpdateWorldPosition ();
		UpdateAirplaneUIElementUIPositions ();
	}

	private void UpdateAirplaneText () {
		airplaneText.GetComponent<AirplaneText> ().UpdateAirplaneText (AltitudeToFlightlevel (airplaneAltitudeScript.GetAltitude ()), AltitudeToFlightlevel (airplaneAltitudeScript.GetAltitudeAssigned ()), airplaneHeadingScript.GetHeading (), (int)airplaneSpeedScript.GetSpeed ());
	}

	private void DestroyThisEntity () {
		controller.GetComponent<ControllerMain> ().RemoveAirplane (id);
		airplaneText.GetComponent<AirplaneText> ().DestoryLineImage ();
		Destroy (airplaneText.gameObject);
		airplaneDotsScript.DestroyDots ();
		airplaneHeadingScript.DestroyHeadingAssignedGameObject ();
		Destroy (airplaneSprite.gameObject);
		Destroy (this.gameObject);
	}

	private int AltitudeToFlightlevel (float altitude) {
		return (int)Math.Round (altitude * 0.01f);
	}

	private void SetupAirplaneTextGameObject (int idInt, GameObject dIPanelGO, bool airplaneTextOffsetBool, string modeString) {
		airplaneText = Instantiate (airplaneText);
		airplaneText.GetComponent<AirplaneText> ().Construct (idInt, dIPanelGO, airplaneTextOffsetBool, modeString);
	}

	private void ConstructOtherScriptsOfThisGameObject (float airplaneAltitude, int airplaneSpeed, int airplaneHeading, string idColr, ChatText chatTextScript) {
		airplaneAltitudeScript.Construct (airplaneAltitude);
		airplaneSpeedScript.Construct (airplaneSpeed);
		airplaneHeadingScript.Construct (airplaneHeading);
		airplaneChatScript.Construct (idColr, chatTextScript);
		airplaneDotsScript.Construct ();
	}

	private void SetupVariablesOfThisScripts (string dispName, int airplaneId, string modeString, int airplaneHeading, GameObject controllerGameObject) {
		displayName = dispName;
		id = airplaneId;
		mode = modeString;
		takeoffHeading = airplaneHeading;
		controller = controllerGameObject;
	}

	private void RunIfConstructTakeoffTrue (int airplaneHeading) {
		airplaneDotsScript.SetActive (false);
		airplaneAltitudeScript.SetAltitudeMin (0);
		airplaneSpeedScript.SetSpeedMin (0);
		GetComponent<Collider> ().enabled = false;
	}

	private void RunIfConstructTakeoffFalse (string dispName) {
		airplaneAltitudeScript.SetAltitudeMin (1000);
		airplaneSpeedScript.SetSpeedMin (140);
		GetComponent<Collider> ().enabled = true;
	}

	private void SetupTimeCounterForUIElementsUpdate () {
		while (timeCounterForUIElementsUpdate < Time.time) {
			timeCounterForUIElementsUpdate += 3;
		}
	}

	private void SetupAirplaneScripts () {
		airplaneAltitudeScript = GetComponent<AirplaneAltitude> ();
		airplaneSpeedScript = GetComponent<AirplaneSpeed> ();
		airplaneHeadingScript = GetComponent<AirplaneHeading> ();
		airplaneWaypointsScript = GetComponent<AirplaneWaypoints> ();
		airplaneLandingScript = GetComponent<AirplaneLanding> ();
		airplaneDotsScript = GetComponent<AirplaneDots> ();
		airplaneChatScript = GetComponent<AirplaneChat> ();
	}
}