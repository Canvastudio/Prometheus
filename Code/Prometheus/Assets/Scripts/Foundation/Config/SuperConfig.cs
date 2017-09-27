using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using System;
using System.Linq;

public class SuperConfig : SingleObject<SuperConfig>
{
    private readonly Dictionary<string, List<ConfigDataBase>> dataDic = new Dictionary<string, List<ConfigDataBase>>();
    private readonly List<ResourceRequest> resDatas = new List<ResourceRequest>();
    private float resProgress;
    public float ResProgress
    {
        get
        {
            return resProgress;
        }
    }
    private bool isDone;
    /// <summary>
    /// 检查这个来判断读取是否完成
    /// </summary>
    public bool IsDone
    {
        get
        {
            return isDone;
        }
    }


    protected override void Init() { }


    /// <summary>
    /// 读取EasyConfig获取配置表路径数组
    /// </summary>
    public string[] GetPathByEasyConfig(string path = null)
    {
        if (path == null)
        {
            try
            {
                path = (string)Type.GetType("SuperConfigInform").GetField("SuperConfigDefaultPathOfEasyConfig").GetValue(null);
            }
            catch (Exception)
            {
                path = "DefaultPath";
            }
        }
        return SuperTool.GetConfigData(path, "Config");
    }

    /// <summary>
    /// 通过配置表路径记录EasyConfig读取配置表。
    /// TXT读取非常快，提供阻塞读取方法
    /// </summary>
    public void Load(string path = null)
    {
        var paths = GetPathByEasyConfig(path);
        Load(paths);
    }


    /// <summary>
    /// 通过配置表路径数组读取配置表。
    /// TXT读取非常快，提供阻塞读取方法
    /// </summary>
    public void Load(string[] names)
    {
        foreach (var v in names)
        {
            var res = Resources.Load<TextAsset>(v);
            DataConver(res);
        }
        isDone = true;
    }

    /// <summary>
    /// 通过配置表路径记录EasyConfig读取配置表。
    ///  异步读取，IsDone与resProgress分别表示读取是否完成与读取进度
    /// </summary>
    public System.Collections.IEnumerator LoadAsync(string path = null)
    {
        var paths = GetPathByEasyConfig(path);
        yield return LoadAsync(paths);
    }

    /// <summary>
    /// 通过配置表路径数组读取配置表。
    /// 异步读取，IsDone与resProgress分别表示读取是否完成与读取进度
    /// </summary>
    public System.Collections.IEnumerator LoadAsync(string[] names)
    {
        isDone = false;
        foreach (string t in names)
        {
            ResourceRequest data = Resources.LoadAsync<TextAsset>(t);
            resDatas.Add(data);
        }
        SuperTimer.Instance.RegisterFrameFunction(CheckRes);
        yield return new WaitUntil(() => IsDone);
        SuperTimer.Instance.LogoutFrameFunction(CheckRes);
    }

    /// <summary>
    /// 需要SuperTimer的Update支持
    /// </summary>
    private bool CheckRes(object obj)
    {
        isDone = true;
        resProgress = 0;
        foreach (ResourceRequest t in resDatas)
        {
            if (t.isDone)
            {
                if (!dataDic.ContainsKey(t.asset.name)) DataConver((TextAsset)t.asset);
            }
            else
            {
                isDone = false;
            }
            resProgress += t.progress;
        }
        resProgress = resProgress / resDatas.Count;
        if (isDone) return true;
        return false;
    }

    /// <summary>
    /// 将一个txt资源转换成相应内容
    /// </summary>
    private void DataConver(TextAsset textAsset)
    {
        string dataString = textAsset.text;
        string className = SuperTool.ConverSpace(textAsset.name);
        string[] dataLine = dataString.Split('\r');
        string[] typeAndArr = dataLine[1].Split('\t');//第二行是类型+数组标记
        string[] parType = new string[typeAndArr.Length];//类型
        string[] splitMak = new string[typeAndArr.Length];//切分标记
        Regex reg = new Regex(@"\[.+\]");
        for (int i = 0; i < typeAndArr.Length; i++)
        {
            typeAndArr[i] = SuperTool.ConverSpace(ReplaceQuote(typeAndArr[i]));

            if (reg.IsMatch(typeAndArr[i]))
            {
                splitMak[i] = reg.Match(typeAndArr[i]).Value;
                parType[i] = typeAndArr[i].Replace(splitMak[i], "");
                splitMak[i] = splitMak[i].Substring(1, splitMak[i].Length - 2);
            }
            else
            {
                splitMak[i] = null;
                parType[i] = typeAndArr[i];
            }
        }
        string[] parName = dataLine[2].Split('\t');//第三行是变量名
        for (int i = 0; i < parName.Length; i++) parName[i] = SuperTool.ConverSpace(ReplaceQuote(parName[i]));
        ulong inIndex = 1;
        List<ConfigDataBase> dataList = new List<ConfigDataBase>();
        for (int x = 3; x < dataLine.Length; x++)
        {
            if (string.IsNullOrEmpty(dataLine[x].Trim())) continue;
            ConfigDataBase dataBase = Activator.CreateInstance(Type.GetType(className)) as ConfigDataBase;
            string[] aData = dataLine[x].Split('\t');
            for (int j = 0; j < aData.Length; j++)
            {
                aData[j] = ReplaceQuote(aData[j]);
                if (!string.IsNullOrEmpty(aData[j]))
                {
                    //强制将id类型修正为ulong
                    if (parName[j].ToLower() == "id")
                    {
                        parName[j] = "id";
                        parType[j] = "ulong";
                        splitMak[j] = null;
                    }
                    try
                    {
                        PropertyInfo pi = dataBase.GetType().GetProperty(parName[j]);
                        if (string.IsNullOrEmpty(splitMak[j]))
                        {
                            switch (parType[j])
                            {
                                case "string":
                                    pi.SetValue(dataBase, aData[j], null);
                                    break;
                                case "float":
                                    pi.SetValue(dataBase, float.Parse(aData[j]), null);
                                    break;
                                case "int":
                                    pi.SetValue(dataBase, int.Parse(aData[j]), null);
                                    break;
                                case "ulong":
                                    pi.SetValue(dataBase, ulong.Parse(aData[j]), null);
                                    break;
                                case "bool":
                                    pi.SetValue(dataBase, ConverBool(aData[j]), null);
                                    break;
                                case "double":
                                    pi.SetValue(dataBase, double.Parse(aData[j]), null);
                                    break;
                                case "long":
                                    pi.SetValue(dataBase, long.Parse(aData[j]), null);
                                    break;
                                case "short":
                                    pi.SetValue(dataBase, short.Parse(aData[j]), null);
                                    break;
                                case "char":
                                    pi.SetValue(dataBase, char.Parse(aData[j]), null);
                                    break;
                                case "uint":
                                    pi.SetValue(dataBase, uint.Parse(aData[j]), null);
                                    break;
                                case "byte":
                                    pi.SetValue(dataBase, byte.Parse(aData[j]), null);
                                    break;
                                default:
                                    pi.SetValue(dataBase, Enum.Parse(Type.GetType(parType[j]), aData[j]), null);
                                    break;
                            }
                        }
                        else
                        {
                            switch (parType[j])
                            {
                                case "string":
                                    pi.SetValue(dataBase, new SuperArray<string>(aData[j], splitMak[j]), null);
                                    break;
                                case "float":
                                    pi.SetValue(dataBase, new SuperArray<float>(aData[j], splitMak[j]), null);
                                    break;
                                case "int":
                                    pi.SetValue(dataBase, new SuperArray<int>(aData[j], splitMak[j]), null);
                                    break;
                                case "ulong":
                                    pi.SetValue(dataBase, new SuperArray<ulong>(aData[j], splitMak[j]), null);
                                    break;
                                case "bool":
                                    pi.SetValue(dataBase, new SuperArray<bool>(aData[j], splitMak[j]), null);
                                    break;
                                case "double":
                                    pi.SetValue(dataBase, new SuperArray<double>(aData[j], splitMak[j]), null);
                                    break;
                                case "long":
                                    pi.SetValue(dataBase, new SuperArray<long>(aData[j], splitMak[j]), null);
                                    break;
                                case "short":
                                    pi.SetValue(dataBase, new SuperArray<short>(aData[j], splitMak[j]), null);
                                    break;
                                case "char":
                                    pi.SetValue(dataBase, new SuperArray<char>(aData[j], splitMak[j]), null);
                                    break;
                                case "uint":
                                    pi.SetValue(dataBase, new SuperArray<uint>(aData[j], splitMak[j]), null);
                                    break;
                                case "byte":
                                    pi.SetValue(dataBase, new SuperArray<byte>(aData[j], splitMak[j]), null);
                                    break;
                                default:
                                    //动态创建泛型对象
                                    Type t = typeof(SuperArray<>);
                                    t = t.MakeGenericType(Type.GetType(parType[j]));
                                    object o = Activator.CreateInstance(t, aData[j], splitMak[j], -1);
                                    pi.SetValue(dataBase, o, null);
                                    break;
                            }

                        }
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException("配置表动态赋值出错：" + className + "→ " + parName[j] + "@" + aData[j]);
                    }
                }
            }
            if (dataBase.id == 0) dataBase.id = inIndex++;
            dataList.Add(dataBase);
        }
        dataDic.Add(className, dataList);
    }

    /// <summary>
    /// 去掉开头和结尾的半角引号，顺便Trim
    /// </summary>
    private static string ReplaceQuote(string value)
    {
        if (value == null) return null;
        if (value == "") return "";
        Regex reg = new Regex(@"^"".*""$");
        if (reg.IsMatch(value)) value = value.Substring(1, value.Length - 2);
        return value.Trim();
    }

    /// <summary>
    /// 把字符串转换成BOOL
    /// </summary>
    private bool ConverBool(string value)
    {
        string temp = value.Trim().ToLower();
        if (temp == "false" || temp == "0" || temp == "")
            return false;
        return true;
    }

    /// <summary>
    /// 根据ID返回一个配置表对象
    /// </summary>
    public T GetConfigDataById<T>(string idStr) where T : ConfigDataBase
    {
        return GetConfigDataById<T>(ulong.Parse(idStr));
    }


    /// <summary>
    /// 根据ID返回一个配置表对象
    /// </summary>
    public T GetConfigDataById<T>(ulong id) where T : ConfigDataBase
    {
        string temp = typeof(T).ToString();
        if (!dataDic.ContainsKey(temp)) throw new ArgumentNullException("不存在的配置表：" + temp);
        List<ConfigDataBase> tempList = dataDic[temp];
        T t = tempList.Find(x => x.id == id) as T;
        if (t == null) Debug.LogWarning("配置表不存在id→ " + temp + ":" + id);
        return t;
    }

    /// <summary>
    /// 根据属性名称，查找配置表中第一个符合条件的对象
    /// </summary>
    /// <typeparam name="T">配置表类型</typeparam>
    /// <param name="pro">属性名</param>
    /// <param name="value">属性值</param>
    /// <returns></returns>
    public T GetConfigDataByProperty<T>(string pro, object value) where T : ConfigDataBase
    {
        string temp = typeof(T).ToString();
        if (!dataDic.ContainsKey(temp)) throw new ArgumentNullException("不存在的配置表@" + temp);
        var proType = typeof(T).GetProperty(pro).PropertyType;
        if (!(value is string) && value.GetType() != proType) throw new ArgumentNullException("查找类型不匹配@" + value);
        if (value == null || value.Equals(null) || string.IsNullOrEmpty(value.ToString())) throw new ArgumentNullException("查找值为空？");
        foreach (var v in from v in dataDic[temp] let compar = v.GetType().GetProperty(pro).GetValue(v, null) where compar != null && !compar.Equals(null) && !string.IsNullOrEmpty(compar.ToString()) where compar == value || compar.Equals(value) || compar.ToString() == value.ToString() select v)
        {
            return (T)v;
        }
        //foreach (var v in dataDic[temp])
        //{
        //    object compar = v.GetType().GetProperty(pro).GetValue(v, null);
        //    if (compar == null || compar.Equals(null) || string.IsNullOrEmpty(compar.ToString())) continue;
        //    if (compar == value || compar.Equals(value) || compar.ToString() == value.ToString()) return (T)v;
        //}
        Debug.LogWarning("配置表<" + temp + ">不存在[" + pro + "]为[" + value + "]的值");
        return null;
    }


    /// <summary>
    /// 根据类型返回配置表所有对象
    /// </summary>
    public List<T> GetConfigDataList<T>() where T : ConfigDataBase
    {
        string temp = typeof(T).ToString();
        if (!dataDic.ContainsKey(temp))
            throw new ArgumentNullException("不存在的配置表：" + temp);
        List<T> tempList = dataDic[temp].Cast<T>().ToList();
        return tempList;
    }


    /// <summary>
    /// 判断一个配置中是否存在该id的数据
    /// </summary>
    public bool Exists<T>(string idStr) where T : ConfigDataBase
    {
        return Exists<T>(ulong.Parse(idStr));
    }



    /// <summary>
    /// 判断一个配置中是否存在该id的数据
    /// </summary>
    public bool Exists<T>(ulong id) where T : ConfigDataBase
    {
        string temp = typeof(T).ToString();
        if (!dataDic.ContainsKey(temp))
            throw new ArgumentNullException("不存在的配置表：" + temp);
        var tempList = dataDic[temp];
        return tempList.Exists(x => x.id == id);
    }


}