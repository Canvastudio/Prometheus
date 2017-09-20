using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class GameStateMachineWindow : EditorWindow {

	// Use this for initialization
	public static void OpenGSM(GameStateMachine gsm)
	{
		var window = EditorWindow.GetWindow<GameStateMachineWindow>("Game State Machine");


	}	

	void Initialze(GameStateMachine machine)
	{
		
	}
	// Update is called once per frame
}
