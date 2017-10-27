using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroCore : SingleGameObject<CoroCore>{

    public WaitForSeconds waitForOneSecone = new WaitForSeconds(1f);
	public WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
}
