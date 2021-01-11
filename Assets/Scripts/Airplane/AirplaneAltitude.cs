using UnityEngine;

public class AirplaneAltitude : MonoBehaviour {

	private bool altitudeCommandCompleted;
	private bool newCommand;
	private int altitudeMin;
	private int altitudeMax;
	private float altitude;
	private float altitudeAssigned;
	private float unactivatedAltitudeAssigned;
	private float altitudeChangeRate;
	private float DelayedCommandTime;

	// Use this for initialization
	void Start () {
		altitudeMax = 60000;
		altitudeChangeRate = 153;
		altitudeCommandCompleted = true;
	}

	// Update is called once per frame
	void Update () {
		if (newCommand) {
			if (Time.time > DelayedCommandTime) {
				if (unactivatedAltitudeAssigned == -1) {
					altitudeAssigned = altitude;
				} else {
					altitudeAssigned = unactivatedAltitudeAssigned;
				}
				altitudeCommandCompleted = false;
				newCommand = false;
			}
		}

		if (altitudeCommandCompleted == false) {
			CheckAndCorrectCommand ();
			UpdateAltitude ();

			if (altitude == altitudeAssigned) {
				altitudeCommandCompleted = true;
			}
		}
	}

	public void Construct (float alt) {
		unactivatedAltitudeAssigned = alt;
		altitudeAssigned = alt;
		altitude = alt;
	}

	void CheckAndCorrectCommand () {
		if (altitudeAssigned < altitudeMin) {
			altitudeAssigned = altitudeMin;
		} else if (altitudeAssigned > altitudeMax) {
			altitudeAssigned = altitudeMax;
		}
	}

	void UpdateAltitude () {
		if (altitude < altitudeAssigned) {
			altitude += 1 * Time.deltaTime * altitudeChangeRate;
			if (altitude > altitudeAssigned) {
				altitude = altitudeAssigned;
			}
		} else if (altitude > altitudeAssigned) {
			altitude -= 1 * Time.deltaTime * altitudeChangeRate;
			if (altitude < altitudeAssigned) {
				altitude = altitudeAssigned;
			}
		}
		if (altitude < altitudeMin) {
			altitude = altitudeMin;
		} else if (altitude > altitudeMax) {
			altitude = altitudeMax;
		}
		transform.position = new Vector3 (transform.position.x, altitude * 0.0003048f, transform.position.z);
	}

	public bool CheckCommand (int alt) {
		if (alt < altitudeMin) {
			return false;
		}
		if (alt > altitudeMax) {
			return false;
		}
		return true;
	}

	public void SetAltitude (float alt) {
		altitude = alt;
	}

	public void CommandAltitude (float alt) {
		unactivatedAltitudeAssigned = alt;
		DelayedCommandTime = Time.time + UnityEngine.Random.Range (1.5f, 7f);
		newCommand = true;
	}

	public float GetAltitude () {
		return altitude;
	}

	public float GetAltitudeAssigned () {
		return altitudeAssigned;
	}

	public void SetAltitudeMin (int a) {
		altitudeMin = a;
	}

	public void CommandAltitudeWithoutDelay (float alt) {
		altitudeAssigned = alt;
		altitudeCommandCompleted = false;
	}

	public void ActivateGlideMode () {
		altitudeChangeRate = altitudeChangeRate / 5;
		altitudeMin = 0;
		altitudeAssigned = 0;
		altitudeCommandCompleted = false;
	}

	public void Abort (string modeString) {
		if (modeString == "takeoff" || modeString == "standby") {
			altitudeMin = 0;
			CommandAltitude (0);
		} else {
			altitudeMin = 1000;
			altitudeMax = 60000;
			altitudeChangeRate = 153;
			altitudeCommandCompleted = true;
			CommandAltitude (-1);
			newCommand = true;
		}
	}

	public string ReturnAltitudeStatusString () {
		if (RoundToNearestTen (altitude) > RoundToNearestTen (altitudeAssigned)) {
			return "altitude " + RoundToNearestTen (altitude) + " feet, descending to " + RoundToNearestTen (altitudeAssigned) + " feet";
		}
		if (RoundToNearestTen (altitude) < RoundToNearestTen (altitudeAssigned)) {
			return "altitude " + RoundToNearestTen (altitude) + " feet, climbing to " + RoundToNearestTen (altitudeAssigned) + " feet";
		}
		return "level at " + RoundToNearestTen (altitude) + " feet";
	}

	private int RoundToNearestTen (float number) {
		return ((int)(number / 10)) * 10;
	}
}
