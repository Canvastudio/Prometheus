using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HudEditor : EditorWindow {

	[MenuItem ("Game Assistant/HudTools")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		HudEditor window = (HudEditor)EditorWindow.GetWindow (typeof (HudEditor));

	}

	void OnGUI () {
	
		if(GUILayout.Button("Create Hud Code")) {

			HudHelper.CreateHud();

		}

		if(GUILayout.Button("Create all hud code")) {
		
			HudHelper.CreateHuds();
		
		}

		if (GUILayout.Button("Create net port")) {

		
		}
	
	}

}
