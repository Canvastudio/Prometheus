using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// 把TXT转成SO
/// </summary>
public class ConfigData : ScriptableObject
{

    public string className;
    public string[] types;
    public string[] splitMaks;
    public string[] names;
    public string[] datas;

    public Dictionary<string, object> GetDataDic()
    {
        Dictionary<string, object> resDic = new Dictionary<string, object>
        {
            {"ClassName", className},
            {"Types", types},
            {"SplitMaks", splitMaks},
            {"Names", names},
        };
        int col = names.Length;
        int row = datas.Length / col;
        int index = 0;
        List<string[]> tplist = new List<string[]>();
        for (int i = 0; i < row; i++)
        {
            tplist.Add(new string[col]);
            for (int j = 0; j < col; j++)
            {
                tplist[i][j] = datas[index++];
            }
        }
        resDic.Add("Datas", tplist);
        return resDic;
    }

    public void SetDataDic(Dictionary<string, object> resDic)
    {
        className = (string)resDic["ClassName"];
        types = (string[])resDic["Types"];
        splitMaks = (string[])resDic["SplitMaks"];
        names = (string[])resDic["Names"];
        var tempArr = (List<string[]>)resDic["Datas"];
        datas = tempArr.SelectMany(t => t).ToArray();
    }
}