using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class ConfigTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //SuperConfig.Instance.Load(SuperTool.GetConfigData("DefaultPath.txt", "Config"));
        SuperTimer.Instance.CreatAndBound(this);
        //SuperTimer.Instance.CoroutineStart(Check(),this);
        SuperConfig.Instance.Load();
        //RpnTestShow("1103806595091,2203318222859,1103806595076,1103806595073,1103806595072");
        //Debug.Log(SkillArg.GetConfigDataByKey<SkillArg>("主动技能效果参数2").rpn.ToArray(0)[1]);
        var t = ActiveSkillsConfig.GetConfigDataById<ActiveSkillsConfig>(1000003);
        Debug.Log(t.stuffCost.values);



    }

    private void RpnTestShow(string str)
    {
        string[] datas = str.Split(',');
        foreach (var data in datas)
        {
            StringBuilder sb=new StringBuilder();
            float[] res = new float[2];
            SuperTool.GetValue(long.Parse(data), ref res);
            if (res[1] == 0)
            {
                sb.Append(res[0]);
            }
            else if (res[1] == 1)
            {
                sb.Append("s~"+(GameProperty)res[0]);
            }
            else if (res[1] == 2)
            {
                sb.Append("t~"+(GameProperty)res[0]);
            }
            Debug.Log(sb);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Check()
    {
        yield return SuperConfig.Instance.LoadAsync();
        Debug.Log(ModuleConfig.GetConfigDataById<ModuleConfig>(1000001));
    }

    private string ReplaceQuote(string value)
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
