using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //SuperConfig.Instance.Load(SuperTool.GetConfigData("DefaultPath.txt", "Config"));
	    //SuperConfig.Instance.Load();
        //Debug.Log(ChipDiskConfig.GetConfigDataById<ChipDiskConfig>(1).chipGridMatrix[0,0]);

        SuperArrayValue<ChipGrid> tt=new SuperArrayValue<ChipGrid>("Normal;Normal|Normal;Normal", "|;");
	    Debug.Log(tt[0,0]);

	    //SuperTool.ToRpn()

	    //var t=SuperTool.CreateWeightSection(new List<int> { 1, 2, 3 });
	    //   Debug.Log(t.RanPoint());
	    //var t = new SuperArrayValue<int>("1:2|3",":|");
	    //   Debug.Log(t[0,0]);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
