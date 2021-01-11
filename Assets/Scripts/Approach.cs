using UnityEngine;
using UnityEngine.UI;

public class Approach : MonoBehaviour {

	//	public GameObject approachImage;
	public GameObject approachText;

	private string approachId;

	// Use this for initialization
	void Start () {

	}

	public void RunSecondaryStart (GameObject controller) {
		approachText = Instantiate (approachText);
		approachText.transform.SetParent (controller.GetComponent<ControllerMain> ().GetDIPanel ().transform);
		approachText.GetComponent<Text> ().text = approachId;
		UpdateUIPosition ();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.GetComponent<AirplaneMain> ()) {
			collider.gameObject.GetComponent<AirplaneMain> ().ControlApproachAreaEnterAndExit (true, transform, approachId);
		}
	}

	void OnTriggerExit (Collider collider) {
		if (collider.gameObject.GetComponent<AirplaneMain> ()) {
			collider.gameObject.GetComponent<AirplaneMain> ().ControlApproachAreaEnterAndExit (false, transform, approachId);
		}
	}

	public void UpdateUIPosition () {
		approachText.transform.position = new Vector3 (Camera.main.WorldToScreenPoint (transform.TransformPoint (2, -16.2f, 0)).x, Camera.main.WorldToScreenPoint (transform.TransformPoint (2, -16.2f, 0)).y);
	}

	public void SetId (string id) {
		this.approachId = id;
	}

	public void SetupDirection (int dir) {
		transform.eulerAngles = new Vector3 (90, dir, 0);
	}

	public void SetupPosition (Vector3 pos) {
		transform.position = pos;
	}

	public string GetId () {
		return approachId;
	}

	public int GetDirection () {
		return (int)transform.eulerAngles.y;
	}

	public Vector2 GetPositionWorldVector2 () {
		return new Vector2 (transform.position.x, transform.position.z);
	}
}
