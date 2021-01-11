using UnityEngine;
using System;

public class Waypoint : ScriptableObject {

	private string wpName;
	private string wpType;
	private Vector2 wpPosition;
	private int wpAltitude;
	private DateTime wpTime;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void Construct (string wpNameString, string wpTypeString, Vector2 wpPositionVector, int wpAltitudeInt, DateTime wpTimeTime) {
		wpName = wpNameString;
		wpType = wpTypeString;
		wpPosition = wpPositionVector;
		wpAltitude = wpAltitudeInt;
		wpTime = wpTimeTime;
	}

	public string GetWpName () {
		return wpName;
	}

	public string GetWpType () {
		return wpType;
	}

	public Vector3 GetWpVector3Position () {
		return new Vector3 (wpPosition.x, 0, wpPosition.y);
	}

	public int GetWpAltitude () {
		return wpAltitude;
	}

	public DateTime GetWpTime () {
		return wpTime;
	}
}
