using UnityEngine;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

public class ScheduleText : MonoBehaviour {

	private Queue<ScheduledFlight> scheduledFlights;
	private int counter;

	// Use this for initialization
	void Start () {
		counter = 750;

	}

	// Update is called once per frame
	void Update () {
		counter++;
		if (counter > 1000) {
			UpdateScheduleText ();
			counter = 0;
		}
	}

	public void UpdateInformationForScheduleText (Queue<ScheduledFlight> flights) {
		scheduledFlights = flights;
	}

	void UpdateScheduleText () {
		Queue<ScheduledFlight> scheduledFlightsClone = new Queue<ScheduledFlight> (scheduledFlights);
		StringBuilder sb = new StringBuilder ();
		while (scheduledFlightsClone.Count > 0) {
			sb.AppendLine (scheduledFlightsClone.Dequeue ().ToString ());
		}
		GetComponent<Text> ().text = sb.ToString ();

	}
}
