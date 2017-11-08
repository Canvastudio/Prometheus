﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class FmodSound : MonoBehaviour {

	public string path = "event:/Skill/";

	public string event_name = "";

	public void OnEnable() {

		FMODUnity.RuntimeManager.PlayOneShot(event_name);
	
	}

	public void OnDrawGizmos() {

		event_name = path + this.gameObject.name;

	}

}
