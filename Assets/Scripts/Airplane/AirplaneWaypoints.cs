using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine.UI;

public class AirplaneWaypoints : MonoBehaviour {

	public GameObject waypointDotImage;
	public GameObject waypointText;
	public GameObject waypointLineImage;

	private AirplaneMain airplaneMainScript;
	private List<Waypoint> waypointsList;
	private List<GameObject[]> waypointImagesList;
	private bool active;
	private int waypointsIndex;

	// Use this for initialization
	void Start () {
		waypointsIndex = 0;
		airplaneMainScript = GetComponent<AirplaneMain> ();
		waypointsList = new List<Waypoint> ();
		Waypoint waypoint;
		DateTime wptime = DateTime.Now;
		waypoint = ScriptableObject.CreateInstance<Waypoint> ();
		wptime = wptime.AddMinutes (5);
		waypoint.Construct ("Alpha", "waypoint", new Vector3 (20, 20), 10000, wptime);
		waypointsList.Add (waypoint);
		waypoint = ScriptableObject.CreateInstance<Waypoint> ();
		wptime = wptime.AddMinutes (5);
		waypoint.Construct ("Bravo", "waypoint", new Vector3 (0, 40), 10000, wptime);
		waypointsList.Add (waypoint);
		waypoint = ScriptableObject.CreateInstance<Waypoint> ();
		wptime = wptime.AddMinutes (5);
		waypoint.Construct ("Charlie", "waypoint", new Vector3 (-15, 30), 10000, wptime);
		waypointsList.Add (waypoint);
	}

	// Update is called once per frame
	void Update () {
		if (Mathf.Abs (waypointsList[waypointsIndex].GetWpVector3Position ().x - transform.position.x) < 1 && Mathf.Abs (waypointsList[waypointsIndex].GetWpVector3Position ().z - transform.position.z) < 1) {
			if (waypointsIndex + 1 < waypointsList.Count) {
				waypointsIndex++;
			}
			if (airplaneMainScript.GetMode () == "waypoint") {
				Activate (true);
			}
		}
	}

	public bool WaypointDisplayIsActive () {
		if (waypointImagesList != null) {
			return true;
		}
		return false;
	}

	public void UpdateWaypointImagesUIPositionsAndDrawLines () {
		Vector3 positionA;
		Vector3 positionB = new Vector3 ();
		foreach (GameObject[] go in waypointImagesList) {
			positionA = positionB;
			positionB = UpdateWaypointImageUIPosition (go[0]);
			if (go[1] != null) {
				DrawLine (positionA, positionB, go[1]);
			}
		}
	}

	public void ActivateWaypointGameObjects (bool b, GameObject dIPanel) {
		if (b) {
			DestroyWaypointGameObjects ();
			CreateWaypointGameObjects (dIPanel);
			UpdateWaypointImagesUIPositionsAndDrawLines ();
		} else {
			DestroyWaypointGameObjects ();
		}
	}

	public void Activate (bool activate) {
		airplaneMainScript.CommandHeadingToPosition (waypointsList[waypointsIndex].GetWpVector3Position (), waypointsList[waypointsIndex].GetWpName (), false);
	}

	public string GetCurrentWaypointName () {
		return waypointsList[waypointsIndex].GetWpName ();
	}

	public string ReturnWaypointStatusString () {
		DateTime wpTime = waypointsList[waypointsIndex].GetWpTime ();
		int hours = wpTime.Hour - DateTime.Now.Hour;
		int minutes = wpTime.Minute - DateTime.Now.Minute;
		int seconds = wpTime.Second - DateTime.Now.Second;
		StringBuilder sb = new StringBuilder ();
		AppendHoursMinutesAndSeconds (sb, hours, minutes, seconds);
		return sb.ToString () + ", role " + waypointsList[waypointsIndex].GetWpType ();
	}

	private Vector3 UpdateWaypointImageUIPosition (GameObject waypointImageGO) {
		Vector3 conversionToScreenPoint = Camera.main.WorldToScreenPoint (waypointImageGO.GetComponent<WaypointImage> ().GetWorldPosition ());
		Vector3 uIPosition = new Vector3 (conversionToScreenPoint.x, conversionToScreenPoint.y, 0);
		waypointImageGO.transform.position = uIPosition;
		return uIPosition;
	}

	private void DrawLine (Vector3 pointA, Vector3 pointB, GameObject lineImageGO) { // http://answers.unity3d.com/questions/865927/draw-a-2d-line-in-the-new-ui.html
		int lineWidth = 2;
		Vector3 differenceVector = pointB - pointA;

		lineImageGO.GetComponent<RectTransform> ().sizeDelta = new Vector2 (differenceVector.magnitude, lineWidth);
		lineImageGO.GetComponent<RectTransform> ().pivot = new Vector2 (0, 0.5f);
		lineImageGO.GetComponent<RectTransform> ().position = pointA;
		float angle = Mathf.Atan2 (differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
		lineImageGO.GetComponent<RectTransform> ().rotation = Quaternion.Euler (0, 0, angle);
	}

	private void CreateWaypointGameObjects (GameObject dIPanel) {
		waypointImagesList = new List<GameObject[]> ();
		foreach (Waypoint wp in waypointsList) {
			GameObject wpImage = Instantiate (waypointDotImage);
			GameObject wpText = Instantiate (waypointText);
			wpText.transform.SetParent (wpImage.transform);
			wpImage.transform.SetParent (dIPanel.transform);
			wpImage.GetComponent<WaypointImage> ().SetWorldPosition (wp.GetWpVector3Position ());
			wpText.transform.localPosition = new Vector3 (0, -42, 0);
			wpText.GetComponent<Text> ().text = ""
				+ GetComponent<AirplaneMain> ().GetId () + "\n"
				+ wp.GetWpName ().ToUpper () + "\n"
				+ wp.GetWpType ().ToUpper () + "\n"
				+ "FL" + wp.GetWpAltitude () / 100 + "\n"
				+ wp.GetWpTime ().ToString ("hh:mm:ss");
			GameObject[] dotImageAndLineImagePair = new GameObject[2];
			dotImageAndLineImagePair[0] = wpImage;
			if (waypointImagesList.Count != 0) {
				dotImageAndLineImagePair[1] = Instantiate (waypointLineImage);
				dotImageAndLineImagePair[1].GetComponent<Image> ().color = new Color32 (255, 255, 0, 127);
				dotImageAndLineImagePair[1].SetActive (true);
				dotImageAndLineImagePair[1].transform.SetParent (dIPanel.transform);
			}
			waypointImagesList.Add (dotImageAndLineImagePair);
		}
	}

	private void DestroyWaypointGameObjects () {
		if (waypointImagesList != null) {
			foreach (GameObject[] go in waypointImagesList) {
				Destroy (go[0]);
				if (go[1] != null) {
					Destroy (go[1]);
				}
			}
			waypointImagesList = null;
		}
	}

	private void AppendHoursMinutesAndSeconds (StringBuilder sb, int hours, int minutes, int seconds) {
		if (hours != 0 || minutes != 0 || seconds >= 10) {
			sb.Append ("ETA ");
			if (hours > 1) {
				sb.Append (hours + " hours");
			} else if (hours == 1) {
				sb.Append ("1 hour");
				AppendMinutesRoundedToTen (sb, minutes);
			} else {
				AppendMinutesAndSeconds (sb, minutes, seconds);
			}
		}
	}

	private void AppendMinutesRoundedToTen (StringBuilder sb, int minutes) {
		if (minutes >= 10) {
			minutes = minutes / 10;
			sb.Append (" " + minutes + "0 minutes");
		}
	}

	private void AppendMinutesAndSeconds (StringBuilder sb, int minutes, int seconds) {
		if (minutes > 30) {
			int fives = minutes % 10;
			if (fives > 4) {
				fives = 5;
			} else {
				fives = 0;
			}
			sb.Append (minutes / 10 + "" + fives + " minutes");
		} else if (minutes > 1) {
			sb.Append (minutes + " minutes");
		} else if (minutes == 1) {
			sb.Append ("1 minute");
			AppendSecondsRoundedToTen (sb, seconds);
		} else {
			AppendSeconds (sb, seconds);
		}
	}

	private void AppendSecondsRoundedToTen (StringBuilder sb, int seconds) {
		if (seconds >= 10) {
			seconds = seconds / 10;
			sb.Append (" " + seconds + "0 seconds");
		}
	}

	private void AppendSeconds (StringBuilder sb, int seconds) {
		if (seconds >= 10) {
			seconds = seconds / 10;
			sb.Append (seconds + "0 seconds");
		}
	}
}
