using UnityEngine;

public class UICanvas : MonoBehaviour {

	public GameObject uIPanel;
	public GameObject clockField;
	public GameObject scheduleField;
	public GameObject offsetButton;
	public GameObject iDWaypointButton;
	public GameObject iDCameraButton;
	public GameObject iDInputField;
	public GameObject radarButton;
	public GameObject satelliteButton;
	public GameObject plusButton;
	public GameObject minusButton;
	public GameObject chatField;
	public GameObject chatInputField;

	// Use this for initialization
	void Start () {
		SetupAnchors ();
		SetupPivot ();
		SetupSize ();
		SetupPosition ();
	}

	// Update is called once per frame
	void Update () {

	}

	private void SetupPosition () {
		float xPos = 5;
		float yPos = -5;
		clockField.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (xPos, yPos);
		yPos = yPos - clockField.GetComponent<RectTransform> ().sizeDelta.y - 5;
		scheduleField.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (xPos, yPos);
		yPos = yPos - scheduleField.GetComponent<RectTransform> ().sizeDelta.y - 5;
		offsetButton.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (xPos, yPos);
		yPos = yPos - offsetButton.GetComponent<RectTransform> ().sizeDelta.y - 5;
		iDWaypointButton.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (xPos, yPos);
		xPos = xPos + iDWaypointButton.GetComponent<RectTransform> ().sizeDelta.x + 5;
		iDCameraButton.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (xPos, yPos);
		xPos = xPos + iDCameraButton.GetComponent<RectTransform> ().sizeDelta.x + 5;
		iDInputField.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (xPos, yPos);
		xPos = 5;
		yPos = yPos - offsetButton.GetComponent<RectTransform> ().sizeDelta.y - 5;
		radarButton.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (xPos, yPos);
		xPos = xPos + radarButton.GetComponent<RectTransform> ().sizeDelta.x + 5;
		satelliteButton.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (xPos, yPos);
		xPos = xPos + satelliteButton.GetComponent<RectTransform> ().sizeDelta.x + 5;
		plusButton.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (xPos, yPos);
		xPos = xPos + plusButton.GetComponent<RectTransform> ().sizeDelta.x + 5;
		minusButton.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (xPos, yPos);
		xPos = 5;
		yPos = yPos - plusButton.GetComponent<RectTransform> ().sizeDelta.y - 5;
		chatField.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (xPos, yPos);
		yPos = yPos - chatField.GetComponent<RectTransform> ().sizeDelta.y - 5;
		chatInputField.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (xPos, yPos);
	}

	private void SetupSize () {
		clockField.GetComponent<RectTransform> ().sizeDelta = new Vector2 (uIPanel.GetComponent<RectTransform> ().sizeDelta.x - 10, 40);
		offsetButton.GetComponent<RectTransform> ().sizeDelta = new Vector2 (30, 30);
		iDWaypointButton.GetComponent<RectTransform> ().sizeDelta = new Vector2 (30, 30);
		iDCameraButton.GetComponent<RectTransform> ().sizeDelta = new Vector2 (30, 30);
		iDInputField.GetComponent<RectTransform> ().sizeDelta = new Vector2 (65, 30);
		radarButton.GetComponent<RectTransform> ().sizeDelta = new Vector2 (30, 30);
		satelliteButton.GetComponent<RectTransform> ().sizeDelta = new Vector2 (30, 30);
		plusButton.GetComponent<RectTransform> ().sizeDelta = new Vector2 (30, 30);
		minusButton.GetComponent<RectTransform> ().sizeDelta = new Vector2 (30, 30);
		chatInputField.GetComponent<RectTransform> ().sizeDelta = new Vector2 (uIPanel.GetComponent<RectTransform> ().sizeDelta.x - 10, 30);
		SetupScheduleAndChatFieldSizes ();
	}

	private void SetupScheduleAndChatFieldSizes () {
		scheduleField.GetComponent<RectTransform> ().sizeDelta = new Vector2 (uIPanel.GetComponent<RectTransform> ().sizeDelta.x - 10, CountHeightForScheduleAndChatFieldEach ());
		chatField.GetComponent<RectTransform> ().sizeDelta = new Vector2 (uIPanel.GetComponent<RectTransform> ().sizeDelta.x - 10, CountHeightForScheduleAndChatFieldEach ());
	}

	private float CountHeightForScheduleAndChatFieldEach () {
		return (GetComponent<RectTransform> ().sizeDelta.y + uIPanel.GetComponent<RectTransform> ().sizeDelta.y - CountTotalHeightUsed ()) / 2;
	}

	private float CountTotalHeightUsed () {
		return 5
			+ clockField.GetComponent<RectTransform> ().sizeDelta.y
			+ 5
			+ 5
			+ offsetButton.GetComponent<RectTransform> ().sizeDelta.y
			+ 5
			+ iDWaypointButton.GetComponent<RectTransform> ().sizeDelta.y
			+ 5
			+ plusButton.GetComponent<RectTransform> ().sizeDelta.y
			+ 5
			+ 5
			+ chatInputField.GetComponent<RectTransform> ().sizeDelta.y
			+ 5;
	}

	private void SetupPivot () {
		clockField.GetComponent<RectTransform> ().pivot = new Vector2 (0, 1);
		scheduleField.GetComponent<RectTransform> ().pivot = new Vector2 (0, 1);
		offsetButton.GetComponent<RectTransform> ().pivot = new Vector2 (0, 1);
		iDWaypointButton.GetComponent<RectTransform> ().pivot = new Vector2 (0, 1);
		iDCameraButton.GetComponent<RectTransform> ().pivot = new Vector2 (0, 1);
		iDInputField.GetComponent<RectTransform> ().pivot = new Vector2 (0, 1);
		radarButton.GetComponent<RectTransform> ().pivot = new Vector2 (0, 1);
		satelliteButton.GetComponent<RectTransform> ().pivot = new Vector2 (0, 1);
		plusButton.GetComponent<RectTransform> ().pivot = new Vector2 (0, 1);
		minusButton.GetComponent<RectTransform> ().pivot = new Vector2 (0, 1);
		chatField.GetComponent<RectTransform> ().pivot = new Vector2 (0, 1);
		chatInputField.GetComponent<RectTransform> ().pivot = new Vector2 (0, 1);
	}

	private void SetupAnchors () {
		clockField.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 1);
		clockField.GetComponent<RectTransform> ().anchorMax = new Vector2 (0, 1);
		scheduleField.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 1);
		scheduleField.GetComponent<RectTransform> ().anchorMax = new Vector2 (0, 1);
		offsetButton.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 1);
		offsetButton.GetComponent<RectTransform> ().anchorMax = new Vector2 (0, 1);
		iDWaypointButton.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 1);
		iDWaypointButton.GetComponent<RectTransform> ().anchorMax = new Vector2 (0, 1);
		iDCameraButton.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 1);
		iDCameraButton.GetComponent<RectTransform> ().anchorMax = new Vector2 (0, 1);
		iDInputField.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 1);
		iDInputField.GetComponent<RectTransform> ().anchorMax = new Vector2 (0, 1);
		radarButton.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 1);
		radarButton.GetComponent<RectTransform> ().anchorMax = new Vector2 (0, 1);
		satelliteButton.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 1);
		satelliteButton.GetComponent<RectTransform> ().anchorMax = new Vector2 (0, 1);
		plusButton.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 1);
		plusButton.GetComponent<RectTransform> ().anchorMax = new Vector2 (0, 1);
		minusButton.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 1);
		minusButton.GetComponent<RectTransform> ().anchorMax = new Vector2 (0, 1);
		chatField.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 1);
		chatField.GetComponent<RectTransform> ().anchorMax = new Vector2 (0, 1);
		chatInputField.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 1);
		chatInputField.GetComponent<RectTransform> ().anchorMax = new Vector2 (0, 1);
	}
}
