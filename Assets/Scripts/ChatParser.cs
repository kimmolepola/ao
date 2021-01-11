using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class ChatParser : MonoBehaviour {

	private ChatText chatTextScript;
	private AirplaneMain sApMain;
	private AirplaneChat sApChat;
	private Dictionary<int, GameObject> airplanesDictionary;
	private List<GameObject> airplanesList;
	private Dictionary<string, GameObject> beaconsDictionary;
	private Dictionary<string, GameObject> approachesDictionary;
	private Dictionary<char, string> ICAOPronounciations;
	private bool discardCommands;
	private int airplaneId;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

	public void Construct (ChatText chatTextScriptScript, Dictionary<int, GameObject> airplanesDictionaryDictionary, List<GameObject> airplanesListList, Dictionary<string, GameObject> beaconsDictionaryDictionary, Dictionary<string, GameObject> approachesDictionaryDictionary) {
		chatTextScript = chatTextScriptScript;
		airplanesDictionary = airplanesDictionaryDictionary;
		airplanesList = airplanesListList;
		beaconsDictionary = beaconsDictionaryDictionary;
		approachesDictionary = approachesDictionaryDictionary;
		SetupICAOPronounciations ();
	}

	public void SetAirplanesDictionary (Dictionary<int, GameObject> aPD) {
		airplanesDictionary = aPD;
	}

	public void SetAirplanesList (List<GameObject> aPL) {
		airplanesList = aPL;
	}

	public void SetBeaconsDictionary (Dictionary<string, GameObject> bD) {
		beaconsDictionary = bD;
	}

	public void SetApproachesDictionary (Dictionary<string, GameObject> approachesDic) {
		approachesDictionary = approachesDic;
	}

	public void Parse (string command) {
		if (command != "") {
			chatTextScript.StartNewLine ("<color=black>");
			string[] words = command.Split (' ');
			int deflt = 0;
			if (words[0] == "ground") {
				RequestStandbyCheckin ();
			} else if (int.TryParse (words[0], out deflt)) {
				int id = int.Parse (words[0]);
				chatTextScript.AddText (id.ToString ());
				if (airplanesDictionary.ContainsKey (id)) { // if there is an airplane with this id word[0]
					airplaneId = id;
					GameObject ap = airplanesDictionary[id];
					sApMain = ap.GetComponent<AirplaneMain> ();
					sApChat = ap.GetComponent<AirplaneChat> ();
				} else {
					airplaneId = 0;
					sApChat = null;
				}
				if (words.Length == 1) {
					if (airplaneId != 0) {
						sApChat.AddToChatList ("Tower");
					}
				} else {
					chatTextScript.AddComma ();
				}
				for (int i = 1; i < words.Length; i++) {
					if (words[i].Length > 0) {
						if (words[i][0] == 'a') { // if first character is a
							ParseAndAssignAltitude (sApChat, sApMain, words[i].Substring (1));
						} else if (words[i][0] == 's') { // if first character is s
							ParseAndAssignSpeed (sApChat, sApMain, words[i].Substring (1));
						} else if (words[i][0] == 'd') { // if first character is d
							ParseAndAssignHeading (sApChat, sApMain, words[i].Substring (1));
						} else if (words[i][0] == 'f') {
							ParseAndAssignHeadingToBeacon (sApChat, sApMain, words[i].Substring (1));
						} else if (words[i][0] == '+') {
							ParseAndAssignHeadingToBeaconAndHoldingPattern (sApChat, sApMain, words[i].Substring (1));
						} else if (words[i] == "-fuel") {
							RequestFuelInfo (sApChat, sApMain);
						} else if (words[i] == "-abort") {
							Abort (sApMain);
							sApChat.OverrideChatList ("aborting");
							discardCommands = true;
						} else if (words[i] == "-takeoff") {
							GrantTakeoffClearance (sApMain, sApChat);
						} else if (words[i] == "-waypoint") {
							CommandWaypointMode (sApMain, sApChat);
						} else if (words[i] == "-status") {
							RequestStatus (sApMain, sApChat);
						} else if (words[i].Length >= 5) {
							if (words[i].Substring (0, 5) == "-land") {
								GrantLandingClearance (sApMain, sApChat, words[i].Substring (5));
							} else {
								ProcessInvalidCommand (sApChat);
							}
						} else {
							ProcessInvalidCommand (sApChat);
						}
					}
				}
			}
			chatTextScript.AddDot ();
			chatTextScript.EndLine ();
			discardCommands = false;
		}
	}

	private void ProcessInvalidCommand (AirplaneChat apChatScript) {
		chatTextScript.AddText ("...");
		apChatScript.OverrideChatList ("say again");
		discardCommands = true;
	}

	private void RequestStandbyCheckin () {
		chatTextScript.AddText ("Flights ready for takeoff please check in");
		foreach (GameObject go in airplanesList) {
			if (go.GetComponent<AirplaneMain> ().GetMode () == "standby") {
				go.GetComponent<AirplaneChat> ().OverrideChatIDString ();
				go.GetComponent<AirplaneChat> ().AddToChatList (go.GetComponent<AirplaneMain> ().GetId () + " ready for takeoff");
			}
		}
	}

	void ParseAndAssignAltitude (AirplaneChat airplaneChatScript, AirplaneMain airplaneMainScript, string word) {
		chatTextScript.AddText ("altitude to " + word);
		if (!discardCommands) {
			int dflt = 0;
			if (int.TryParse (word, out dflt)) {
				int alt = int.Parse (word);
				if (airplaneMainScript) {
					if (airplaneMainScript.CheckAltitudeCommand (alt * 100)) {
						airplaneMainScript.CommandAltitude (alt * 100); // assign altitude				
						airplaneChatScript.AddToChatList ("altitude to " + alt.ToString ());
					} else {
						airplaneChatScript.OverrideChatList ("unable, invalid altitude");
						discardCommands = true;
					}
				}
			} else {
				airplaneChatScript.OverrideChatList ("say again");
				discardCommands = true;
			}
		}

	}

	void ParseAndAssignSpeed (AirplaneChat airplaneChatScript, AirplaneMain airplaneMainScript, string word) {
		chatTextScript.AddText ("speed to " + word);
		if (!discardCommands) {
			int dflt = 0;
			if (int.TryParse (word, out dflt)) {
				int spd = int.Parse (word);
				if (airplaneMainScript) {
					if (airplaneMainScript.CheckSpeedCommand (spd)) {
						airplaneMainScript.CommandSpeed (spd); // assign speed										
						airplaneChatScript.AddToChatList ("speed to " + spd.ToString ());
					} else {
						airplaneChatScript.OverrideChatList ("unable, invalid speed");
						discardCommands = true;
					}
				}
			} else {
				airplaneChatScript.OverrideChatList ("say again");
				discardCommands = true;
			}
		}
	}

	void ParseAndAssignHeading (AirplaneChat airplaneChatScript, AirplaneMain airplaneMainScript, string word) {
		if (word.Length > 0) {
			if (word[0] == 'l') {
				chatTextScript.AddText ("turn left heading");
				if (!discardCommands && airplaneChatScript) {
					airplaneChatScript.AddToChatList ("left to");
				}
				ParseAndAssignHeadingNormalOrLeftOrRight (sApChat, airplaneMainScript, word.Substring (1), 1);
			} else if (word[0] == 'r') {
				chatTextScript.AddText ("turn right heading ");
				if (!discardCommands && airplaneChatScript) {
					airplaneChatScript.AddToChatList ("right to");
				}
				ParseAndAssignHeadingNormalOrLeftOrRight (sApChat, airplaneMainScript, word.Substring (1), 2);
			} else {
				chatTextScript.AddText ("heading to");
				if (!discardCommands && airplaneChatScript) {
					airplaneChatScript.AddToChatList ("heading to");
				}
				ParseAndAssignHeadingNormalOrLeftOrRight (sApChat, airplaneMainScript, word, 0);
			}
		} else {
			chatTextScript.AddText ("...");
			airplaneChatScript.OverrideChatList ("say again");
			discardCommands = true;
		}
	}

	void ParseAndAssignHeadingNormalOrLeftOrRight (AirplaneChat airplaneChatScript, AirplaneMain airplaneMainScript, string word, int normalOrLeftOrRight) {
		chatTextScript.AddText (word);
		if (!discardCommands) {
			int dflt = 0;
			if (int.TryParse (word, out dflt)) {
				int hdg = int.Parse (word);
				if (airplaneMainScript) {
					if (airplaneMainScript.CheckHeadingCommand (hdg)) {
						airplaneMainScript.CommandHeading (hdg, normalOrLeftOrRight); // assign heading
						airplaneChatScript.AddToChatList (hdg.ToString ());
					} else {
						airplaneChatScript.AddToChatList ("unable, invalid heading");
						discardCommands = true;
					}
				}
			} else {
				airplaneChatScript.OverrideChatList ("say again");
				discardCommands = true;
			}
		}
	}

	void ParseAndAssignHeadingToBeacon (AirplaneChat airplaneChatScript, AirplaneMain airplaneMainScript, string word) {
		string beaconId = word.ToUpperInvariant ();
		chatTextScript.AddText ("heading to " + beaconId);
		if (!discardCommands) {
			if (airplaneMainScript) {
				if (beaconsDictionary.ContainsKey (beaconId)) {
					airplaneMainScript.CommandHeadingToPosition (beaconsDictionary[beaconId].GetComponent<Beacon> ().GetWorldPosition (), beaconsDictionary[beaconId].GetComponent<Beacon> ().GetId (), true);
					airplaneChatScript.AddToChatList ("heading to " + ConvertLettersToICAOPronounciation (beaconId));
				} else {
					airplaneChatScript.AddToChatList ("unable, invalid beacon id");
					discardCommands = true;
				}
			}
		}
	}

	void ParseAndAssignHeadingToBeaconAndHoldingPattern (AirplaneChat airplaneChatScript, AirplaneMain airplaneMainScript, string word) {
		string beaconId = word.ToUpperInvariant ();
		chatTextScript.AddText ("holding pattern at " + beaconId);
		if (!discardCommands) {
			if (airplaneMainScript) {
				if (beaconsDictionary.ContainsKey (beaconId)) {
					airplaneMainScript.CommandHoldingPattern (beaconsDictionary[beaconId]);
					airplaneChatScript.AddToChatList ("holding at " + ConvertLettersToICAOPronounciation (beaconId));
				} else {
					airplaneChatScript.AddToChatList ("unable, invalid beacon id");
					discardCommands = true;
				}
			}
		}
	}

	void GrantLandingClearance (AirplaneMain airplaneMainScript, AirplaneChat airplaneChatScript, string wrd) {
		string word = wrd.ToUpperInvariant ();
		chatTextScript.AddText ("cleared to land " + word);
		if (approachesDictionary.ContainsKey (word)) {
			airplaneMainScript.GrantLandingClearance (word);
			if (airplaneMainScript.CheckIfReadyToLand () == false) {
				airplaneChatScript.AddToChatList ("cleared to land " + word);
			}
		} else if (wrd == "") {
			airplaneChatScript.AddToChatList ("requesting approach id");
			discardCommands = true;
		} else {
			airplaneChatScript.AddToChatList ("unable, invalid approach id");
			discardCommands = true;
		}
	}

	void RequestFuelInfo (AirplaneChat airplaneChatScript, AirplaneMain airplaneMainScript) {
		chatTextScript.AddText ("report fuel level");
		if (!discardCommands) {
			airplaneChatScript.AddToChatList ("fuel level " + airplaneMainScript.RequestFuelInfo ());
		}
	}

	private void GrantTakeoffClearance (AirplaneMain airplaneMainScript, AirplaneChat airplaneChatScript) {
		chatTextScript.AddText ("cleared for takeoff");
		if (airplaneMainScript.GetMode () == "takeoff" || airplaneMainScript.GetMode () == "standby") {
			airplaneMainScript.GrantTakeoffClearance ("someRunway");
			if (airplaneMainScript.GetSpeedAssigned () < 140) {
				airplaneMainScript.CommandSpeed (240);
			}
			airplaneChatScript.AddToChatList ("cleared for takeoff");
		} else {
			airplaneChatScript.AddToChatList ("already airborne");
			discardCommands = true;
		}
	}

	private void CommandWaypointMode (AirplaneMain airplaneMainScript, AirplaneChat airplaneChatScript) {
		chatTextScript.AddText ("proceed to waypoints");
		airplaneMainScript.CommandWaypointMode ();
		airplaneChatScript.AddToChatList ("proceeding to waypoint " + airplaneMainScript.GetCurrentWaypointName ());
	}

	private void RequestStatus (AirplaneMain airplaneMainScript, AirplaneChat airplaneChatScript) {
		chatTextScript.AddText ("report status");
		airplaneMainScript.AddStatusReportToChatList ();
	}

	private void Abort (AirplaneMain airplaneMainScript) {
		chatTextScript.AddText ("abort");
		airplaneMainScript.Abort ();
	}

	private void SetupICAOPronounciations () {
		ICAOPronounciations = new Dictionary<char, string> ();
		//ICAOPronounciations.Add ('a', "Alpha");
		//ICAOPronounciations.Add ('b', "Bravo");
		//ICAOPronounciations.Add ('c', "Charlie");
		//ICAOPronounciations.Add ('d', "Delta");
		//ICAOPronounciations.Add ('e', "Echo");
		//ICAOPronounciations.Add ('f', "Foxtrot");
		//ICAOPronounciations.Add ('g', "Golf");
		//ICAOPronounciations.Add ('h', "Hotel");
		//ICAOPronounciations.Add ('i', "India");
		//ICAOPronounciations.Add ('j', "Juliet");
		//ICAOPronounciations.Add ('k', "Kilo");
		//ICAOPronounciations.Add ('l', "Lima");
		//ICAOPronounciations.Add ('m', "Mike");
		//ICAOPronounciations.Add ('n', "November");
		//ICAOPronounciations.Add ('o', "Oscar");
		//ICAOPronounciations.Add ('p', "Papa");
		//ICAOPronounciations.Add ('q', "Quebec");
		//ICAOPronounciations.Add ('r', "Romeo");
		//ICAOPronounciations.Add ('s', "Sierra");
		//ICAOPronounciations.Add ('t', "Tango");
		//ICAOPronounciations.Add ('u', "Uniform");
		//ICAOPronounciations.Add ('v', "Victor");
		//ICAOPronounciations.Add ('w', "Whiskey");
		//ICAOPronounciations.Add ('x', "Xray");
		//ICAOPronounciations.Add ('y', "Yankee");
		//ICAOPronounciations.Add ('z', "Zulu");
		ICAOPronounciations.Add ('a', "alpha");
		ICAOPronounciations.Add ('b', "bravo");
		ICAOPronounciations.Add ('c', "charlie");
		ICAOPronounciations.Add ('d', "delta");
		ICAOPronounciations.Add ('e', "echo");
		ICAOPronounciations.Add ('f', "foxtrot");
		ICAOPronounciations.Add ('g', "golf");
		ICAOPronounciations.Add ('h', "hotel");
		ICAOPronounciations.Add ('i', "india");
		ICAOPronounciations.Add ('j', "juliet");
		ICAOPronounciations.Add ('k', "kilo");
		ICAOPronounciations.Add ('l', "lima");
		ICAOPronounciations.Add ('m', "mike");
		ICAOPronounciations.Add ('n', "november");
		ICAOPronounciations.Add ('o', "oscar");
		ICAOPronounciations.Add ('p', "papa");
		ICAOPronounciations.Add ('q', "quebec");
		ICAOPronounciations.Add ('r', "romeo");
		ICAOPronounciations.Add ('s', "sierra");
		ICAOPronounciations.Add ('t', "tango");
		ICAOPronounciations.Add ('u', "uniform");
		ICAOPronounciations.Add ('v', "victor");
		ICAOPronounciations.Add ('w', "whiskey");
		ICAOPronounciations.Add ('x', "xray");
		ICAOPronounciations.Add ('y', "yankee");
		ICAOPronounciations.Add ('z', "zulu");
	}

	private string ConvertLettersToICAOPronounciation (string word) {
		StringBuilder sb = new StringBuilder ();
		for (int i = 0; i < word.Length; i++) {
			if (i > 0) {
				sb.Append (" ");
			}
			if (ICAOPronounciations.ContainsKey (char.ToLower (word[i]))) {
				sb.Append (ICAOPronounciations[char.ToLower (word[i])]);
			} else {
				sb.Append (char.ToUpper (word[i]));
			}
		}
		return sb.ToString ();
	}
}
