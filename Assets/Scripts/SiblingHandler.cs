using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SiblingHandler : object {

	public static void SetOnFront (Transform t) {
		t.SetAsLastSibling ();
		t = t.parent;
		while (t) {
			t.SetAsLastSibling ();
			t = t.parent;
		}
	}
}
