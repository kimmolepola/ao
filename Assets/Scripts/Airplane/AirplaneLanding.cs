using UnityEngine;

public class AirplaneLanding : MonoBehaviour {


	private AirplaneMain AirplaneMainScript;
	private Transform otherTransform;

	private bool once;
	private bool landing;
	private bool clearedToLand;
	private int landingHeadingCorrectionFactor;
	private int runwayDirection;
	private string approachId;
	private string clearedApproachId;
	private bool abort;
	private float delayedCommandTime;
	private bool isInsideApproachArea;
	private int runwayLength;
	private int glideSlopeLength;

	void Awake () {
		landingHeadingCorrectionFactor = 10;
		runwayLength = 1;
		glideSlopeLength = 9;
	}

	// Use this for initialization
	void Start () {
		AirplaneMainScript = GetComponent<AirplaneMain> ();
	}

	// Update is called once per frame
	void Update () {
		if (landing) {
			Land ();
		} else if (clearedToLand && Time.time > delayedCommandTime && CheckIfReadyToLand ()) {
			StartLanding ();
		}
	}

	public void Abort () {
		clearedToLand = false;
		landing = false;
	}

	public bool CheckIfReadyToLand () {
		if (isInsideApproachArea
			&& clearedApproachId == approachId
			&& AirplaneMainScript.GetAltitude () <= 2000
			&& AirplaneMainScript.GetSpeed () <= 240
			&& (transform.eulerAngles.y > runwayDirection - 11 || transform.eulerAngles.y > -11 + 360)
			&& (transform.eulerAngles.y < runwayDirection + 11 || transform.eulerAngles.y < +11 - 360)) {
			return true;
		}
		return false;
	}

	public void GrantLandingClearance (string clearedApprchId) {
		clearedApproachId = clearedApprchId;
		delayedCommandTime = Time.time + UnityEngine.Random.Range (1.5f, 4f);
		clearedToLand = true;
	}

	public void ControlApproachAreaEnterAndExit (bool b, Transform approachTransform, string aprchId) {
		isInsideApproachArea = b;
		approachId = aprchId;
		runwayDirection = (int)approachTransform.eulerAngles.y;
		otherTransform = approachTransform;
	}

	private void StartLanding () {
		AirplaneMainScript.SetAltitudeMin (0);
		AirplaneMainScript.SetSpeedMin (0);
		AirplaneMainScript.ExecuteLandingMessage ();
		AirplaneMainScript.SetMode ("landing");
		landing = true;
	}

	private void Land () {
		CorrectHeading ();
		if (AirplaneMainScript.GetSpeed () <= 0 && AirplaneMainScript.GetAltitude () == 0 && otherTransform.InverseTransformPoint (transform.position).y > -runwayLength) {
			CompleteLanding ();
		} else if (AirplaneMainScript.GetAltitude () == 0 && otherTransform.InverseTransformPoint (transform.position).y > -runwayLength) {
			BrakeOnGround ();
		} else if (otherTransform.InverseTransformPoint (transform.position).y > -(runwayLength + glideSlopeLength)) {
			FollowGlideSlope ();
		} else {
			ApproachGlideSlopeWithCorrectSpeedAssigned ();
		}
	}

	private void CompleteLanding () {
		if (!once) {
			AirplaneMainScript.ExecuteLandingCompletedMessage ();
			AirplaneMainScript.AddLanded ();
			AirplaneMainScript.ActivateReadyForDestroy ();
			once = true;
		}
	}

	private void BrakeOnGround () {
		AirplaneMainScript.ActivateBrakingMode ();
	}

	private void FollowGlideSlope () {
		float distanceFromRunway = -(otherTransform.InverseTransformPoint (transform.position).y + runwayLength);
		float glideSlopeRatio = 2000 / glideSlopeLength;
		if (AirplaneMainScript.GetAltitude () > distanceFromRunway * glideSlopeRatio) {
			AirplaneMainScript.CommandAltitudeWithoutDelay (distanceFromRunway * glideSlopeRatio);
		}
		AirplaneMainScript.CommandSpeedWithoutDelay (160);
	}

	private void ApproachGlideSlopeWithCorrectSpeedAssigned () {
		AirplaneMainScript.CommandSpeedWithoutDelay (160);
	}

	private void CorrectHeading () {
		AirplaneMainScript.CommandHeadingWithoutDelay (runwayDirection - Mathf.RoundToInt (otherTransform.InverseTransformPoint (transform.position).x * landingHeadingCorrectionFactor));
	}
}
