using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class FmodSound : MonoBehaviour {

	public string path = "event:/Skill/";

	public string event_name = "";

	public string stop_parameter = "";

	public FMOD.Studio.EventInstance soundEvent;

	public void OnEnable() {

		//soundEvent = FMOD
		//SoundCore.PlayOneShot(event_name);

		soundEvent = FMODUnity.RuntimeManager.CreateInstance(event_name);

		soundEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
		soundEvent.start();
		soundEvent.release();
	
	}

	public void OnDisable() {

		if(stop_parameter.Length > 0)
			soundEvent.setParameterValue(stop_parameter, 0);
	
	}

	public void OnDrawGizmos() {

		event_name = path + this.gameObject.name;

	}

}
