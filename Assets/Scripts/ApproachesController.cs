using UnityEngine;
using System.Collections.Generic;

public class ApproachesController : MonoBehaviour {

	public GameObject approach;

	private Dictionary<string, GameObject> approachesDictionary = new Dictionary<string, GameObject> ();
	private List<GameObject> approachesList = new List<GameObject> ();

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void Construct (List<GameObject> beaconsListList) {
		CreateApproaches (beaconsListList);
	}

	public Dictionary<string, GameObject> GetApproachesDictionary () {
		return approachesDictionary;
	}

	public List<GameObject> GetApproachesList () {
		return approachesList;
	}

	private void CreateApproaches (List<GameObject> beaconsListList) {
		int approachesToBeCreated = 2;
		foreach (GameObject bcn in beaconsListList) {
			GameObject aprch = Instantiate (approach);
			int direction = UnityEngine.Random.Range (0, 36);
			string approachId;
			if (direction < 10) {
				approachId = bcn.GetComponent<Beacon> ().GetId () + "0" + direction;
			} else {
				approachId = bcn.GetComponent<Beacon> ().GetId () + direction;
			}
			aprch.GetComponent<Approach> ().SetupDirection (direction * 10);
			aprch.GetComponent<Approach> ().SetId (approachId);
			aprch.GetComponent<Approach> ().SetupPosition (bcn.GetComponent<Beacon> ().GetWorldPosition ());
			aprch.GetComponent<Approach> ().RunSecondaryStart (this.gameObject);
			aprch.SetActive (true);
			approachesDictionary.Add (approachId, aprch);
			approachesList.Add (aprch);
			approachesToBeCreated--;
			if (approachesToBeCreated == 0) {
				return;
			}
		}
	}
}
