using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //SuperConfig.Instance.Load(SuperTool.GetConfigData("DefaultPath.txt", "Config"));
	    SuperConfig.Instance.Load();
        Debug.Log(PassiveSkillsConfig.GetConfigDataById<PassiveSkillsConfig>(3000001).name);
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
