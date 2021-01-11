using UnityEngine;
using System.Collections.Generic;
using System;

public class ScheduledFlight : ScriptableObject {

	private DateTime entryTime;
	private int id;
	private int altitude;
	private int speed;
	private string entrypointId;
	private Vector2 entrypointPosition;
	private string mode;
	private int heading;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

	public void SetUpValues (List<int> activeIds, DateTime previousEntryTime, List<FlightEntrypoint> entrypoints) {
		entryTime = SelectArbitraryEntrytime (previousEntryTime);
		id = CreateId (activeIds);
		SelectArbitraryEntrypoint (entrypoints);
		if (mode == "standby") {
			altitude = 0;
			speed = 0;
		} else {
			altitude = SelectArbitraryAltitude ();
			speed = SelectArbitrarySpeed ();
		}
	}

	DateTime SelectArbitraryEntrytime (DateTime previousEntryTime) {
		int maxEntryTimeSeparationInSeconds = 60;
		return previousEntryTime.AddSeconds (UnityEngine.Random.Range (0, maxEntryTimeSeparationInSeconds));
	}

	int CreateId (List<int> activeIds) {
		while (true) {
			int id = UnityEngine.Random.Range (1, 10000);
			if (!activeIds.Contains (id)) {
				return id;
			}
		}
	}

	int SelectArbitraryAltitude () {
		return UnityEngine.Random.Range (10000, 30000);
	}

	int SelectArbitrarySpeed () {
		return UnityEngine.Random.Range (250, 500);
	}

	private void SelectArbitraryEntrypoint (List<FlightEntrypoint> entrypoints) {
		FlightEntrypoint entrypoint = entrypoints[UnityEngine.Random.Range (0, entrypoints.Count)];
		entrypointId = entrypoint.GetId ();
		entrypointPosition = entrypoint.GetPosition ();
		heading = entrypoint.GetDirection ();
		if (entrypoint.GetTakeoff ()) {
			mode = "standby";
		} else {
			mode = "default";
		}
	}

	override public string ToString () {
		if (mode == "standby") {
			return "ETA " + HourOrMinuteToTwoDigitString (entryTime.Hour) + ":" + HourOrMinuteToTwoDigitString (entryTime.Minute) + " - ID " + IdToFourDigitString (id) + " - " + entrypointId;
		}
		return "ETA " + HourOrMinuteToTwoDigitString (entryTime.Hour) + ":" + HourOrMinuteToTwoDigitString (entryTime.Minute) + " - ID " + IdToFourDigitString (id) + " - " + entrypointId + " - FL" + AltitudeToRoundedFlightlevelString (altitude);
	}

	string HourOrMinuteToTwoDigitString (int integ) {
		if (integ > 9) {
			return integ.ToString ();
		}
		return "0" + integ;
	}

	string IdToFourDigitString (int idInt) {
		if (id > 999) {
			return "" + idInt;
		}
		if (id > 99) {
			return "0" + idInt;
		}
		if (id > 9) {
			return "00" + idInt;
		}
		return "000" + idInt;
	}

	string AltitudeToRoundedFlightlevelString (int alt) {
		int altid = 10 * (int)Math.Round (alt * 0.001f);
		return altid.ToString ();
	}

	public DateTime GetEntryTime () {
		return entryTime;
	}

	public int GetId () {
		return id;
	}

	public int GetAltitude () {
		return altitude;
	}

	public int GetSpeed () {
		return speed;
	}

	public string GetEntryointId () {
		return entrypointId;
	}

	public Vector2 GetEntrypointPosition () {
		return entrypointPosition;
	}

	public string GetMode () {
		return mode;
	}

	public int GetHeading () {
		return heading;
	}
}
