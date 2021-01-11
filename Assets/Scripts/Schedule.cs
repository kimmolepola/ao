using UnityEngine;
using System.Collections.Generic;
using System;

public class Schedule : MonoBehaviour {

	public GameObject scheduleText;

	private List<GameObject> approaches;
	private DateTime previousEntryTime;
	private List<int> activeIds;
	private Queue<ScheduledFlight> scheduledFlights;
	private int scheduleSize;
	private List<FlightEntrypoint> entrypoints;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		while (scheduledFlights.Count < scheduleSize) {
			ScheduledFlight flight = ScriptableObject.CreateInstance<ScheduledFlight> ();
			flight.SetUpValues (activeIds, previousEntryTime, entrypoints);
			activeIds.Add (flight.GetId ());
			previousEntryTime = flight.GetEntryTime ();
			scheduledFlights.Enqueue (flight);
			UpdateScheduleText ();
		}
	}

	public void Construct (List<GameObject> approachesList) {
		approaches = approachesList;
		CreateEntrypoints ();
		if (activeIds == null) {
			activeIds = new List<int> ();
		}
		scheduleSize = 20;
		previousEntryTime = DateTime.Now;
		scheduledFlights = new Queue<ScheduledFlight> ();
	}

	private void CreateEntrypoints () {
		entrypoints = new List<FlightEntrypoint> ();
		FlightEntrypoint entrypoint = ScriptableObject.CreateInstance<FlightEntrypoint> ();
		foreach (GameObject go in approaches) {
			entrypoint = ScriptableObject.CreateInstance<FlightEntrypoint> ();
			entrypoint.SetId (go.GetComponent<Approach> ().GetId ());
			entrypoint.SetDirection (go.GetComponent<Approach> ().GetDirection ());
			entrypoint.SetPosition (go.GetComponent<Approach> ().GetPositionWorldVector2 ());
			entrypoint.SetTakeoff (true);
			entrypoints.Add (entrypoint);
		}
		entrypoint = ScriptableObject.CreateInstance<FlightEntrypoint> ();
		entrypoint.SetId ("A");
		entrypoint.SetDirection (0);
		entrypoint.SetPosition (new Vector2 (40, -40));
		entrypoint.SetTakeoff (false);
		entrypoints.Add (entrypoint);
		entrypoint = ScriptableObject.CreateInstance<FlightEntrypoint> ();
		entrypoint.SetId ("B");
		entrypoint.SetDirection (90);
		entrypoint.SetPosition (new Vector2 (-40, -40));
		entrypoint.SetTakeoff (false);
		entrypoints.Add (entrypoint);
		entrypoint = ScriptableObject.CreateInstance<FlightEntrypoint> ();
		entrypoint.SetId ("C");
		entrypoint.SetDirection (180);
		entrypoint.SetPosition (new Vector2 (-40, 40));
		entrypoint.SetTakeoff (false);
		entrypoints.Add (entrypoint);
		entrypoint = ScriptableObject.CreateInstance<FlightEntrypoint> ();
		entrypoint.SetId ("D");
		entrypoint.SetDirection (270);
		entrypoint.SetPosition (new Vector2 (40, 40));
		entrypoint.SetTakeoff (false);
		entrypoints.Add (entrypoint);
	}

	void UpdateScheduleText () {
		scheduleText.GetComponent<ScheduleText> ().UpdateInformationForScheduleText (scheduledFlights);
	}

	public List<int> getActiveIds () {
		return activeIds;
	}

	public ScheduledFlight dequeueFlightIfReady () {
		if (DateTime.Compare (DateTime.Now, scheduledFlights.Peek ().GetEntryTime ()) > 0) {
			return scheduledFlights.Dequeue ();
		}
		return null;
	}

	public void AddToActiveIds (int id) {
		if (activeIds == null) {
			activeIds = new List<int> ();
		}
		activeIds.Add (id);
	}

	public void RemoveIdFromActiveIds (int id) {
		activeIds.Remove (id);
	}
}
