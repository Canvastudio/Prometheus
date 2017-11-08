using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCore {

	public static void PlayOneShot(string name) {
	
		FMODUnity.RuntimeManager.PlayOneShot(name);
	
	}

}
