using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BeaconsController : MonoBehaviour {

	public GameObject beacon;

	Dictionary<string, GameObject> beaconsDictionary = new Dictionary<string, GameObject> ();
	List<GameObject> beaconsList = new List<GameObject> ();

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void Construct (GameObject dIPanelGO) {
		CreateBeacons (dIPanelGO);
	}

	public Dictionary<string, GameObject> GetBeaconsDictionary () {
		return beaconsDictionary;
	}

	public List<GameObject> GetBeaconsList () {
		return beaconsList;
	}

	private Vector3 CreateBeaconPosition (List<GameObject> beacons) {
		Vector3 beaconPosition = new Vector3 (0, 0, 0);
		while (beaconPosition == new Vector3 (0, 0, 0)) {
			beaconPosition = new Vector3 (UnityEngine.Random.Range (-100f, 100f), 0, UnityEngine.Random.Range (-100f, 100f));
			foreach (GameObject go in beacons) {
				if (Vector3.Distance (beaconPosition, go.GetComponent<Beacon> ().GetWorldPosition ()) < 10) {
					beaconPosition = new Vector3 (0, 0, 0);
				}
			}
		}
		return beaconPosition;
	}

	private void CreateBeacons (GameObject dIPanelGO) {
		int amount = 20;
		while (amount > 0) {
			amount--;
			while (true) {
				string beaconId = GetComponent<ControllerMain> ().CreateThreeLetterId ();
				if (!beaconsDictionary.ContainsKey (beaconId)) {
					Vector3 beaconPosition = CreateBeaconPosition (beaconsList);
					GameObject bcn = Instantiate (beacon);
					beaconsDictionary.Add (beaconId, bcn);
					beaconsList.Add (bcn);
					bcn.GetComponent<Text> ().text = beaconId;
					bcn.transform.SetParent (dIPanelGO.transform);
					bcn.GetComponent<Beacon> ().SetBeaconPosition (beaconPosition);
					bcn.GetComponent<Beacon> ().UpdateBeaconUIPosition ();
					bcn.GetComponent<Beacon> ().SetId (beaconId);
					break;
				}
			}
		}
	}
}
