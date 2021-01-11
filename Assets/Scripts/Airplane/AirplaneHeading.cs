using UnityEngine;
using System;

public class AirplaneHeading : MonoBehaviour {

	public GameObject headingAssignedGameObject;
	public GameObject beaconRotator;

	private int[] headingAssigned;
	private Vector3[] targetPosition;
	private string[] targetName;
	private bool headingCommandCompleted;
	private bool newCommand;
	private bool once;
	private int heading;
	private int[] headingNormalOrLeftOrRight;
	// 0 = normal, 1 = left, 2 = right
	private int[] headingMode;
	// 0 = default, 1 = beacon mode, 2 = holding pattern mode
	private int beaconRotationSize;
	private float headingChangeRate;
	private float DelayedCommandTime;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		headingAssignedGameObject.transform.position = transform.position;
		if (newCommand) {
			if (Time.time > DelayedCommandTime) {
				targetPosition[0] = targetPosition[1];
				targetName[0] = targetName[1];
				headingMode[0] = headingMode[1];
				if (headingAssigned[1] == -1) {
					headingAssigned[0] = heading;
				} else {
					headingAssigned[0] = headingAssigned[1];
				}
				headingNormalOrLeftOrRight[0] = headingNormalOrLeftOrRight[1];
				headingCommandCompleted = false;
				newCommand = false;
				if (headingMode[0] == 2) {
					beaconRotator.SetActive (true);
					beaconRotator.transform.position = targetPosition[0] + headingAssignedGameObject.transform.TransformPoint (new Vector3 (0, 0, beaconRotationSize)) - headingAssignedGameObject.transform.position;
				} else {
					beaconRotator.SetActive (false);
				}
			}
		}

		if (headingCommandCompleted == false && headingMode[0] == 0) {
			CheckAndCorrectCommand ();
			UpdateRotation ();
			if (transform.rotation == headingAssignedGameObject.transform.rotation) {
				transform.eulerAngles = (new Vector3 (0, headingAssigned[0], 0));
				headingCommandCompleted = true;
			}
		}
		if (headingMode[0] == 1) {
			pointHeadingAssignedToBeacon ();
			if (Mathf.Abs (targetPosition[0].x - transform.position.x) < 10 && Mathf.Abs (targetPosition[0].z - transform.position.z) < 10) {
				headingMode[0] = 0;
			}
			RotateShortest ();
		}

		if (headingMode[0] == 2) {
			if (Vector3.Distance (targetPosition[0], transform.position) > beaconRotationSize * 2) {
				headingAssignedGameObject.transform.forward = targetPosition[0] - transform.position;
				beaconRotator.transform.position = targetPosition[0] + headingAssignedGameObject.transform.TransformPoint (new Vector3 (0, 0, beaconRotationSize)) - headingAssignedGameObject.transform.position;
			} else {
				if (!once) {
					GetComponent<AirplaneSpeed> ().CommandSpeed (200);
					once = true;
				}
				headingAssignedGameObject.transform.forward = beaconRotator.transform.position - transform.position;
				if (Vector3.Distance (beaconRotator.transform.position, transform.position) < Mathf.Max (1, 1 + Mathf.Abs (Vector3.Distance (targetPosition[0], transform.position) - beaconRotationSize))) {
					beaconRotator.transform.RotateAround (targetPosition[0], Vector3.up, 1);
				}
			}
			headingAssigned[0] = Mathf.RoundToInt (headingAssignedGameObject.transform.eulerAngles.y);
			RotateShortest ();
		}
		heading = (int)Math.Round (transform.eulerAngles.y);
	}

	public void Construct (int hdg) {
		targetName = new string[2];
		headingAssigned = new int[2];
		headingMode = new int[2];
		headingMode[0] = 0;
		headingNormalOrLeftOrRight = new int[2];
		headingNormalOrLeftOrRight[0] = 0;
		targetPosition = new Vector3[2];
		beaconRotationSize = 5;
		headingChangeRate = 2.7f;
		headingAssignedGameObject = Instantiate (headingAssignedGameObject);
		headingCommandCompleted = true;
		beaconRotator = Instantiate (beaconRotator);
		beaconRotator.SetActive (false);
		heading = hdg;
		CommandHeadingWithoutDelay (hdg);
	}

	void pointHeadingAssignedToBeacon () {
		headingAssignedGameObject.transform.forward = targetPosition[0] - transform.position;
		headingAssigned[0] = Mathf.RoundToInt (headingAssignedGameObject.transform.eulerAngles.y);
	}

	public void DestroyHeadingAssignedGameObject () {
		Destroy (headingAssignedGameObject.gameObject);
	}

	void CheckAndCorrectCommand () {
		if (headingAssigned[0] < 0) {
			while (headingAssigned[0] < 0) {
				headingAssigned[0] += 360;
			}
		} else if (headingAssigned[0] == 360) {
			headingAssigned[0] = 0;
		} else if (headingAssigned[0] > 360) {
			while (headingAssigned[0] > 360) {
				headingAssigned[0] -= 360;
			}
		}
	}

	void UpdateRotation () {
		headingAssignedGameObject.transform.eulerAngles = new Vector3 (0, headingAssigned[0], 0);
		if (headingNormalOrLeftOrRight[0] == 0) {
			RotateShortest ();
		} else {
			Vector3 cross = (Vector3.Cross (transform.forward, headingAssignedGameObject.transform.forward));
			if (headingNormalOrLeftOrRight[0] == 1) {
				if (cross.y < 0) {
					RotateShortest ();
				} else if (cross.y > 0) {
					transform.Rotate (Vector3.down * headingChangeRate * Time.deltaTime);
					if (cross.y <= 0) {
						transform.rotation = headingAssignedGameObject.transform.rotation;
					}
				}
			} else {
				if (cross.y > 0) {
					RotateShortest ();
				} else if (cross.y < 0) {
					transform.Rotate (Vector3.up * headingChangeRate * Time.deltaTime);
					if (cross.y >= 0) {
						transform.rotation = headingAssignedGameObject.transform.rotation;
					}
				}
			}
		}
	}

	void RotateShortest () {
		transform.rotation = Quaternion.RotateTowards (transform.rotation, headingAssignedGameObject.transform.rotation, headingChangeRate * Time.deltaTime);
	}

	public void SetHeading (int hdg) {
		heading = hdg;
	}

	public void CommandHeading (int hdg, int normalLeftOrRight) {
		headingMode[1] = 0;
		headingAssigned[1] = hdg;
		headingNormalOrLeftOrRight[1] = normalLeftOrRight;
		float delay = UnityEngine.Random.Range (1.5f, 7f);
		DelayedCommandTime = Time.time + delay;
		newCommand = true;
	}

	public int GetHeading () {
		return heading;
	}

	public int GetHeadingAssigned () {
		return headingAssigned[0];
	}

	public void CommandHeadingWithoutDelay (int hdg) {
		headingAssigned[0] = hdg;
		headingNormalOrLeftOrRight[0] = 0;
		headingMode[0] = 0;
		headingCommandCompleted = false;
	}

	public void CommandHoldingPattern (GameObject beaconGameObject) {
		once = false;
		targetPosition[1] = beaconGameObject.GetComponent<Beacon> ().GetWorldPosition ();
		targetName[1] = beaconGameObject.GetComponent<Beacon> ().GetId ();
		headingMode[1] = 2;
		float delay = UnityEngine.Random.Range (1.5f, 7f);
		DelayedCommandTime = Time.time + delay;
		newCommand = true;
	}

	public void CommandHeadingToPosition (Vector3 targetPositionVector, string targetNameString) {
		once = false;
		targetPosition[1] = targetPositionVector;
		targetName[1] = targetNameString;
		headingMode[1] = 1;
		float delay = UnityEngine.Random.Range (1.5f, 7f);
		DelayedCommandTime = Time.time + delay;
		newCommand = true;
	}

	public bool CheckCommand (int hdg) {
		if (hdg < 0) {
			return false;
		}
		if (hdg > 360) {
			return false;
		}
		return true;
	}

	public void Abort () {
		CommandHeading (-1, 0);
	}

	public string ReturnHeadingStatusString (Vector3 referenceForwardVector) {
		if (headingMode[0] == 2 && once) {
			return "holding at " + targetName[0];
		}
		float cross = (Vector3.Cross (transform.forward, referenceForwardVector)).y;
		if (cross > 0) {
			return "turning left " + ReturnTargetString (GetHeadingAssigned ());
		}
		if (cross < 0) {
			return "turning right " + ReturnTargetString (GetHeadingAssigned ()); ;
		}
		return "heading " + ReturnTargetString (GetHeadingAssigned ());
	}

	private string ReturnTargetString (int headingAssignedInt) {
		if (headingMode[0] == 1) {
			return "to " + targetName[0];
		}
		if (headingMode[0] == 2) {
			return "to " + targetName[0] + " for holding pattern";
		}
		return ReturnHeadingString (headingAssignedInt);
	}

	private string ReturnHeadingString (int headingInt) {
		if (headingInt >= 100) {
			return "" + headingInt;
		} else if (headingInt >= 10) {
			return "0" + headingInt;
		} else {
			return "00" + headingInt;
		}
	}
}
