using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SuperConfig.Instance.Load();
        Debug.Log(StateConfig.GetConfigDataById<StateConfig>(4000001).name);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
