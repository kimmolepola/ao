using UnityEngine;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

public class ChatText : MonoBehaviour {

	private StringBuilder oneLineBuilder;
	private Queue<string> chatStrings;
	private bool refresh;
	private bool newLine;

	// Use this for initialization
	void Start () {
		refresh = false;
		chatStrings = new Queue<string> ();
		oneLineBuilder = new StringBuilder ();
	}

	// Update is called once per frame
	void Update () {
		if (refresh) {
			GetComponent<Text> ().text = QueueOfStringsToString (chatStrings);
			float textBoxHeight = GetComponent<RectTransform> ().rect.height;
			float textHeight = LayoutUtility.GetPreferredHeight (GetComponent<RectTransform> ());
			if (textHeight > textBoxHeight) {
				chatStrings.Dequeue ();
				GetComponent<Text> ().text = QueueOfStringsToString (chatStrings);
			} else {
				refresh = false;
			}
		}
	}

	string QueueOfStringsToString (Queue<string> queueOfStrings) {
		StringBuilder multiLineBuilder = new StringBuilder ();
		Queue<string> queueOfStringsClone = new Queue<string> (queueOfStrings);
		while (queueOfStringsClone.Count > 0) {
			multiLineBuilder.AppendLine (queueOfStringsClone.Dequeue ());
		}
		return multiLineBuilder.ToString ();
	}

	void AddNewLineToContainer (string newLine) {
		chatStrings.Enqueue (newLine);
	}

	public void StartNewLine (string color) {
		newLine = true;
		oneLineBuilder = new StringBuilder ();
		oneLineBuilder.Append (color);
	}

	public void EndLine () {
		oneLineBuilder.Append ("</color>");
		AddNewLineToContainer (oneLineBuilder.ToString ());
		refresh = true;
	}

	public void AddText (string text) {
		if (newLine) {
			text = char.ToUpper (text[0]) + text.Substring (1);
			oneLineBuilder.Append (text);
			newLine = false;
		} else {
			oneLineBuilder.Append (" " + text);
		}
	}

	public void AddComma () {
		oneLineBuilder.Append (",");
	}

	public void AddDot () {
		oneLineBuilder.Append (".");
	}

	public void EnableBold () {
		oneLineBuilder.Append ("<b>");
	}

	public void DisableBold () {
		oneLineBuilder.Append ("</b>");
	}
}
