using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class AirplaneDots : MonoBehaviour {

	public GameObject airplaneDot;
	public GameObject airplaneMainDot;

	private List<GameObject> airplaneDots;
	private List<Vector3> positions;
	private Vector3 airplaneMainDotWorldPosition;
	private int positionsListIndex;
	private int positionsListSize;
	private int airplaneDotsListIndex;
	private int airplaneDotsListSize;
	private int offsetIndex;
	private bool active;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

	public void Construct () {
		airplaneMainDot = Instantiate (airplaneMainDot);
		airplaneMainDot.transform.SetParent (GetComponent<AirplaneMain> ().GetController ().GetComponent<ControllerMain> ().GetDIPanel ().transform);
		airplaneMainDot.transform.position = new Vector3 (Camera.main.WorldToScreenPoint (transform.position).x, Camera.main.WorldToScreenPoint (transform.position).y, 0);
		airplaneDotsListSize = 5;
		airplaneDotsListIndex = 0;
		offsetIndex = 2;
		positionsListSize = airplaneDotsListSize + offsetIndex;
		positionsListIndex = offsetIndex;
		offsetIndex = 0;
		positions = new List<Vector3> (positionsListSize);
		while (positions.Count < positionsListSize) {
			positions.Add (transform.position);
		}
		airplaneDots = new List<GameObject> (positionsListSize - 2);
		while (airplaneDots.Count < airplaneDotsListSize) {
			GameObject ad = Instantiate (airplaneDot);
			ad.transform.SetParent (GetComponent<AirplaneMain> ().GetController ().GetComponent<ControllerMain> ().GetDIPanel ().transform);
			ad.GetComponent<AirplaneDot> ().UpdateWorldPosition (transform.position);
			ad.GetComponent<AirplaneDot> ().UpdatePosition ();
			airplaneDots.Add (ad);
			ad.SetActive (active);
		}
		airplaneDot.SetActive (active);
	}

	public void UpdateWorldPosition () {
		airplaneMainDotWorldPosition = transform.position;
		UpdateDotsWorldPositions ();
	}

	public void UpdateUIPosition () {
		airplaneMainDot.transform.position = new Vector3 (Camera.main.WorldToScreenPoint (airplaneMainDotWorldPosition).x, Camera.main.WorldToScreenPoint (airplaneMainDotWorldPosition).y);
		if (airplaneDots != null) {
			foreach (GameObject go in airplaneDots) {
				go.GetComponent<AirplaneDot> ().UpdatePosition ();
			}
		}
	}

	private void UpdateDotsWorldPositions () {
		positions[positionsListIndex] = transform.position;
		airplaneDots[airplaneDotsListIndex].GetComponent<AirplaneDot> ().UpdateWorldPosition (positions[offsetIndex]);
		//RecolorDots (airplaneDots, airplaneDotsListIndex);
		offsetIndex = step (offsetIndex, positionsListSize);
		positionsListIndex = step (positionsListIndex, positionsListSize);
		airplaneDotsListIndex = step (airplaneDotsListIndex, airplaneDotsListSize);
	}

	private void RecolorDots (List<GameObject> apDots, int index) {
		float alpha = 0.4f;
		Color col = new Color (0, 255, 0, 255);
		for (int i = 0; i < apDots.Count; i++) {
			col.a = alpha;
			apDots[index].GetComponent<Image> ().color = col;
			alpha = alpha / 1.24f;
			index--;
			if (index <= -1) {
				index = apDots.Count - 1;
			}
		}
	}

	public void DestroyDots () {
		foreach (GameObject dot in airplaneDots) {
			Destroy (dot);
		}
		airplaneDots.Clear ();
		Destroy (airplaneMainDot);
	}

	int step (int i, int limit) {
		i++;
		if (i == limit) {
			i = 0;
		}
		return i;
	}

	//public gameobject getairplanemaindot () {
	//	return airplanemaindot;
	//}

	public Vector3 getAirplaneMainDotWorldPosition () {
		return airplaneMainDotWorldPosition;
	}

	public Vector3 getAirplaneMainDotPosition () {
		return airplaneMainDot.transform.position;
	}

	public void SetActive (bool a) {
		active = a;
		if (airplaneMainDot) {
			airplaneMainDot.SetActive (a);

		}
		if (airplaneDots != null) {
			if (airplaneDots.Count > 0) {
				foreach (GameObject go in airplaneDots) {
					go.SetActive (a);
				}
			}
		}
	}
}
