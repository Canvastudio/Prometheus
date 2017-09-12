using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;

/// <summary>
/// 简易配置格式(Unicode格式限定)，可以用任意空白字符进行分割，只能记录string，支持单个\多维元素：
/// 如果是单数据，可以用foreach，count获取所有单数据；
/// 如果多维数据，首位值为索引，之后值为多维数据成员;
/// 使用//表示本行为注释
/// 如果路径没有文本配置，表示建立一个新配置，使用write可以将设置好的配置写入该路径
/// </summary>
public class EasyConfig : IEnumerable
{

    private static Dictionary<string, EasyConfig> _totleDic;
    private readonly Dictionary<string, List<string>> contentDictionary = new Dictionary<string, List<string>>();
    private readonly List<string> contentList = new List<string>();
    private string path;
    private string oldData;

    /// <summary>
    /// 单元素个数
    /// </summary>
    public int Count
    {
        get { return contentList.Count; }
    }

    public static EasyConfig GetConfig(string path)
    {
        if (_totleDic == null) _totleDic = new Dictionary<string, EasyConfig>();
        if (_totleDic.ContainsKey(path)) return _totleDic[path];
        var res = Resources.Load<TextAsset>(path);
        var ec = new EasyConfig { path = path };
        if (res != null) ec.BuildingContent(res.text);
        _totleDic.Add(path, ec);
        return ec;
    }

    private EasyConfig() { }


    private void BuildingContent(string data)
    {
        oldData = data;
        string[] temp = data.Split('\r');
        for (int i = 0; i < temp.Length; i++)
        {
            string line = temp[i];
            if (string.IsNullOrEmpty(line.Trim()) || Comments(line.Trim())) continue;
            line = ConvertSpace(line);
            string[] t2 = line.Split('\t');
            if (t2.Length > 1)
            {
                List<string> stringList = new List<string>();
                contentDictionary.Add(t2[0], stringList);
                for (int j = 1; j < t2.Length; j++)
                    stringList.Add(ReplaceQuote(t2[j]));
            }
            else
            {
                contentList.Add(ReplaceQuote(t2[0]));
            }
        }
    }

    /// <summary>
    /// 根据序号获取单元素
    /// </summary>
    public string this[int index]
    {
        get
        {
            return contentList[index];
        }
    }


    /// <summary>
    /// 根据字符获取元素
    /// </summary>
    public string this[string key]
    {
        get
        {
            return contentDictionary[SuperTool.ConverSpace(key)].ToArray()[0];
        }
    }

    /// <summary>
    /// 根据字符获取列表元素
    /// </summary>
    public string[] GetDataList(string key)
    {
        return contentDictionary[SuperTool.ConverSpace(key)].ToArray();
    }

    /// <summary>
    /// 添加一个元素
    /// </summary>
    public string Add(string data, bool repeatCheck = true)
    {
        data = SuperTool.ConverSpace(data);
        if (repeatCheck)
        {
            if (contentList.Contains(data))
                return null;
        }
        contentList.Add(data);
        return data;
    }

    /// <summary>
    /// 添加一堆元素
    /// </summary>
    public void AddRange(string[] data, bool repeatCheck = true)
    {
        for (int i = 0; i < data.Length; i++) data[i] = SuperTool.ConverSpace(data[i]);
        if (repeatCheck)
        {
            foreach (var d in data)
            {
                if (contentList.Contains(d)) continue;
                contentList.Add(d);
            }
        }
        else contentList.AddRange(data);

    }

    /// <summary>
    /// 添加一个列表元素
    /// </summary>
    public string Add(string key, string data, bool repeatCheck = true)
    {
        key = SuperTool.ConverSpace(key);
        data = SuperTool.ConverSpace(data);
        if (!contentDictionary.ContainsKey(key))
        {
            var t = new List<string> { data };
            contentDictionary.Add(key, t);
        }
        else
        {
            var t = contentDictionary[key];
            if (repeatCheck && t.Contains(data))
                return null;
            t.Add(data);
        }
        return data;
    }


    /// <summary>
    /// 添加一个组列表元素
    /// </summary>
    public void AddRange(string key, string[] data, bool repeatCheck = true)
    {
        key = SuperTool.ConverSpace(key);
        for (int i = 0; i < data.Length; i++) data[i] = SuperTool.ConverSpace(data[i]);
        if (!contentDictionary.ContainsKey(key))
        {
            var t = new List<string>(data);
            contentDictionary.Add(key, t);
        }
        else
        {
            var t = contentDictionary[key];
            if (repeatCheck)
            {
                foreach (var d in data)
                {
                    if (!t.Contains(d)) t.Add(d);
                }
            }
            else
            {
                t.AddRange(data);
            }
        }
    }


    /// <summary>
    /// 清除掉所有内容
    /// </summary>
    public void Clear()
    {
        contentDictionary.Clear();
        contentList.Clear();
    }

    /// <summary>
    /// 移除一个指定的非索引元素
    /// </summary>
    public void Remove(string value)
    {
        contentList.Remove(SuperTool.ConverSpace(value));
    }

    /// <summary>
    /// 按编号移除一个非索引元素
    /// </summary>
    public void RemoveAt(int index)
    {
        contentList.RemoveAt(index);
    }

    /// <summary>
    /// 移除一个索引中的某个元素
    /// </summary>
    public void Remove(string key, string value)
    {
        key = SuperTool.ConverSpace(key);
        value = SuperTool.ConverSpace(value);
        if (!contentDictionary.ContainsKey(key)) return;
        contentDictionary[key].Remove(value);
    }

    /// <summary>
    /// 移除一个索引的全部元素
    /// </summary>
    public void RemoveAt(string key)
    {
        key = SuperTool.ConverSpace(key);
        if (!contentDictionary.ContainsKey(key)) return;
        contentDictionary.Remove(key);
    }

    /// <summary>
    /// 处理多于空格与去掉首尾空字符
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private static string ConvertSpace(string data)
    {
        string temp = Regex.Replace(data, @"\s+", "\t");
        return temp.Trim();
    }

    /// <summary>
    /// 去掉开头和结尾的半角引号，与空白字符
    /// </summary>
    private static string ReplaceQuote(string value)
    {
        if (value == null) return null;
        if (value == "") return "";
        value = SuperTool.ConverSpace(value);
        value = Regex.Replace(value, @"^""+", "");
        value = Regex.Replace(value, @"""+$", "");
        return value;
    }

    public IEnumerator GetEnumerator()
    {
        return contentList.GetEnumerator();
    }

    private bool Comments(string line)
    {
        Regex t = new Regex(@"^\s*(\\{2}|/{2}).*");
        return t.IsMatch(line);
    }

    /// <summary>
    /// 将数据写入指定的文本，只能在Unity编辑器环境下生效
    /// 注意写入数据会清除掉原有的注释
    /// </summary>
    public void Write()
    {
#if UNITY_EDITOR
        StringBuilder sb1 = new StringBuilder();
        foreach (var v in contentList) sb1.Append(v + "\r\n");
        foreach (var k in contentDictionary.Keys)
        {
            sb1.Append(k + "\t");
            var templist = contentDictionary[k];
            for (int i = 0; i < templist.Count; i++)
            {
                sb1.Append(templist[i]);
                if (i < templist.Count - 1) sb1.Append("\t");
            }
            sb1.Append("\r\n");
        }
        string res = sb1.ToString().Trim();
        if (oldData == res) return;
        FileStream aFile = new FileStream(Application.dataPath + "/Resources/" + path + ".txt", FileMode.Create);
        StreamWriter sw = new StreamWriter(aFile);
        sw.Write(res);
        sw.Close();
        sw.Dispose();
#endif
    }
}
