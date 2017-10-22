using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ConfigTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //SuperConfig.Instance.Load(SuperTool.GetConfigData("DefaultPath.txt", "Config"));
        SuperTimer.Instance.CreatAndBound(this);
        //SuperTimer.Instance.CoroutineStart(Check(),this);
        //SuperConfig.Instance.Load();

	    //var t = new SuperArrayValue<long>("2199023255553,1103806595087,2199023255652", ",");
	    //Debug.Log(t[0]);

	    string t = "\"2199023255553,1103806595087,2199023255652,1103806595073,1103806595087,2199023255653,1103806595073,1103806595076,1103806595074,2199023255602,1103806595075\"";

	    Debug.Log(ReplaceQuote(t));
	    //ReplaceQuote("");



	    //Debug.Log(ActiveSkillsConfig.GetConfigDataById<ActiveSkillsConfig>(1000007).activeSkillArgs[0].u[0]);

	    //Debug.Log(ChipDiskConfig.GetConfigDataById<ChipDiskConfig>(1).chipGridMatrix[0,0]);

	    //Debug.Log((ChipConfig.GetConfigDataById<ChipConfig>(100226).propertyAddition.gamePropertys[0]).GetType() == typeof (GameProperty));

	    //var t = new SuperArrayValue<Stuff>("Coherer;Organics", ";");
	}

    // Update is called once per frame
    void Update () {
		
	}

    private IEnumerator Check()
    {
        yield return SuperConfig.Instance.LoadAsync();
        Debug.Log(ModuleConfig.GetConfigDataById<ModuleConfig>(1000001));
    }

    private  string ReplaceQuote(string value)
    {
        if (value == null) return null;
        if (value == "") return "";
        Regex reg = new Regex(@"^"".*""$");
        if (reg.IsMatch(value)) value = value.Substring(1, value.Length - 2);
        Regex reg2 = new Regex(@"[\r\n]");
        value = reg2.Replace(value, "");
        return value.Trim();
    }


}
