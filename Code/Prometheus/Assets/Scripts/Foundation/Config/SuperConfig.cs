using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;


/// <summary>
/// 利用Unity自带Resources方式读取文本，并处理成相应对象类型。
/// 配置表中不能使用空格与引号。
/// </summary>
public class SuperConfig : SingleObject<SuperConfig>
{
    private readonly Dictionary<string, List<ConfigDataBase>> dataDic = new Dictionary<string, List<ConfigDataBase>>();
    private readonly List<ResourceRequest> resDatas = new List<ResourceRequest>();
    private readonly List<XYZW<ConfigDataBase, PropertyInfo, string, string>> postponeList = new List<XYZW<ConfigDataBase, PropertyInfo, string, string>>();

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
        private set
        {
            if (value) PostponeVoluation();
            isDone = value;
        }
    }


    protected override void Init() { }

#if UNITY_EDITOR
    [MenuItem("SuperTool/TXT转换成ScriptableObject &%9")]
    static void FormatConversion(MenuCommand cmd)
    {
        TxtToSo();
    }
#endif

    private static void TxtToSo()
    {
#if UNITY_EDITOR
        var paths = GetPathByEasyConfig();
        foreach (var path in paths)
        {
            //txt读取
            var res = Resources.Load<TextAsset>(path);
            if (res == null) continue;
            ConfigData cd = ScriptableObject.CreateInstance<ConfigData>();
            cd.SetDataDic(TxtAnalyze(res));
            AssetDatabase.CreateAsset(cd, Path.Combine("Assets/Resources", path + ".asset"));
            string txtFile = Path.Combine("Assets/Resources", path + ".txt");
            if (File.Exists(txtFile)) File.Delete(txtFile);
        }
        AssetDatabase.SaveAssets();
#endif
    }


    /// <summary>
    /// 读取EasyConfig获取配置表路径（对应Config行）数组
    /// </summary>
    public static string[] GetPathByEasyConfig(string path = null)
    {
        if (path == null) path = (string)GetSuperConfigInform("SuperConfigDefaultPathOfEasyConfig") ?? "DefaultPath";
        EasyConfig ec = EasyConfig.GetConfig(path);
        return ec.GetDataArray("Config");
    }

    /// <summary>
    /// 该方法尝试获取由VBA生成的代码变量，如果获取失败返回null
    /// </summary>
    public static object GetSuperConfigInform(string argName)
    {
        try
        {
            return Type.GetType("SuperConfigInform").GetField(argName).GetValue(null);
        }
        catch (Exception)
        {
            return null;
        }
    }


    /// <summary>
    /// 通过配置表路径记录EasyConfig读取配置表。
    /// 一般情况下，是不需要指定参数的（由外部VBA生成），除非有特殊需求读取其他位置的配置表。
    /// TXT读取非常快，提供阻塞读取方法
    /// </summary>
    public void Load(string path = null)
    {
        var paths = GetPathByEasyConfig(path);
        Load(paths);
    }


    private void Load(string[] names)
    {
        foreach (var v in names)
        {
            //如果存在TXT，就读取TXT，否则读取OS
            var res = Resources.Load<TextAsset>(v);
            var dic = res != null ? TxtAnalyze(Resources.Load<TextAsset>(v)) : Resources.Load<ConfigData>(v).GetDataDic();
            DataConver(dic);
        }
        IsDone = true;

    }


    /// <summary>
    /// 通过配置表路径记录EasyConfig读取配置表。
    /// 一般情况下，是不需要指定参数的（由外部VBA生成），除非有特殊需求读取其他位置的配置表。
    /// 异步读取，IsDone与resProgress分别表示读取是否完成与读取进度
    /// </summary>
    public System.Collections.IEnumerator LoadAsync(string path = null)
    {
        var paths = GetPathByEasyConfig(path);
        yield return LoadAsync(paths);
    }


    public System.Collections.IEnumerator LoadAsync(string[] names)
    {
        IsDone = false;
        foreach (string t in names)
        {
            var rr = Resources.LoadAsync<TextAsset>(t);
            if (rr.asset == null) rr = Resources.LoadAsync<ConfigData>(t);
            resDatas.Add(rr);
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
        IsDone = true;
        resProgress = 0;
        foreach (ResourceRequest t in resDatas)
        {
            if (t.asset == null) continue;
            if (t.isDone)
            {
                if (!dataDic.ContainsKey(t.asset.name))
                {
                    var dic = t.asset.GetType() == typeof(TextAsset) ? TxtAnalyze((TextAsset)t.asset) : ((ConfigData)t.asset).GetDataDic();
                    if (dic != null) DataConver(dic);
                }
            }
            else
            {
                IsDone = false;
            }
            resProgress += t.progress;
        }
        resProgress = resProgress / resDatas.Count;
        if (IsDone) return true;
        return false;
    }

    /// <summary>
    /// 解析一个TXT资源，分离表头与数据
    /// </summary>
    private static Dictionary<string, object> TxtAnalyze(TextAsset textAsset)
    {
        string dataString = textAsset.text;
        string className = SuperTool.ConverSpace(textAsset.name);
        string[] dataLine = dataString.Split('\r');
        string[] typeAndArr = dataLine[0].Split('\t');//第1行是类型+数组标记
        string[] parType = new string[typeAndArr.Length];//类型
        string[] splitMak = new string[typeAndArr.Length];//切分标记
        Regex reg = new Regex(@"\[.+\]$");
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
        string[] parName = dataLine[1].Split('\t');//第2行是变量名
        for (int i = 0; i < parName.Length; i++) parName[i] = SuperTool.ConverSpace(ReplaceQuote(parName[i]));

        List<string[]> datas = new List<string[]>();//除开前两行的数据
        for (int i = 2; i < dataLine.Length; i++)
        {
            if (dataLine[i].Trim() == "" || dataLine[i].Trim() == "\r") continue;
            var tempArr = dataLine[i].Split('\t');
            for (int j = 0; j < tempArr.Length; j++)
            {
                tempArr[j] = ReplaceQuote(tempArr[j]);
            }
            datas.Add(tempArr);
        }

        Dictionary<string, object> resDic = new Dictionary<string, object>
        {
            {"ClassName", className},
            {"Types", parType},
            {"SplitMaks", splitMak},
            {"Names", parName},
            {"Datas", datas},
        };
        return resDic;
    }


    /// <summary>
    /// 将一个内容转换成相应内容
    /// </summary>
    private void DataConver(Dictionary<string, object> resDic)
    {
        string className = (string)resDic["ClassName"];
        string[] parType = (string[])resDic["Types"];//类型
        string[] splitMak = (string[])resDic["SplitMaks"];//切分标记
        string[] parName = (string[])resDic["Names"];//参数名
        List<string[]> dataLine = (List<string[]>)resDic["Datas"];

        ulong inIndex = 1;
        List<ConfigDataBase> dataList = new List<ConfigDataBase>();
        foreach (string[] data in dataLine)
        {
            ConfigDataBase dataBase = Activator.CreateInstance(Type.GetType(className)) as ConfigDataBase;
            for (int j = 0; j < data.Length; j++)
            {
                //data[j] = ReplaceQuote(data[j]);
                if (!string.IsNullOrEmpty(data[j]))
                {
                    //强制将id类型修正为ulong
                    if (parName[j].ToLower() == "id")
                    {
                        parName[j] = "id";
                        parType[j] = "ulong";
                        splitMak[j] = null;
                    }
                    //强制将key类型修正为string
                    if (parName[j].ToLower() == "key")
                    {
                        parName[j] = "key";
                        parType[j] = "string";
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
                                pi.SetValue(dataBase, data[j], null);
                                break;
                                case "float":
                                pi.SetValue(dataBase, float.Parse(data[j]), null);
                                break;
                                case "int":
                                pi.SetValue(dataBase, int.Parse(data[j]), null);
                                break;
                                case "ulong":
                                pi.SetValue(dataBase, ulong.Parse(data[j]), null);
                                break;
                                case "bool":
                                pi.SetValue(dataBase, ConverBool(data[j]), null);
                                break;
                                case "double":
                                pi.SetValue(dataBase, double.Parse(data[j]), null);
                                break;
                                case "long":
                                pi.SetValue(dataBase, long.Parse(data[j]), null);
                                break;
                                case "short":
                                pi.SetValue(dataBase, short.Parse(data[j]), null);
                                break;
                                case "char":
                                pi.SetValue(dataBase, char.Parse(data[j]), null);
                                break;
                                case "uint":
                                pi.SetValue(dataBase, uint.Parse(data[j]), null);
                                break;
                                case "byte":
                                pi.SetValue(dataBase, byte.Parse(data[j]), null);
                                break;
                                default:
                                if (pi.PropertyType.IsEnum)
                                {
                                    pi.SetValue(dataBase, Enum.Parse(Type.GetType(parType[j]), data[j]), null);
                                }
                                else
                                {
                                    AddPostpone(dataBase, pi, parType[j], data[j]);
                                    //这是一个对泛型方法的调用，虽然没用上，但是还是留在这里以做参考
                                    //MethodInfo serviceMethod = this.GetType().GetMethod("TryAdd");
                                    //var t = serviceMethod.MakeGenericMethod(Type.GetType(parType[j])).Invoke(this, new object[] { ulong.Parse(aData[j]), parType[j], pi.GetValue(dataBase, null) });
                                }
                                break;
                            }
                        }
                        else
                        {
                            switch (parType[j])
                            {
                                case "string":
                                pi.SetValue(dataBase, new SuperArrayValue<string>(data[j], splitMak[j]), null);
                                break;
                                case "float":
                                pi.SetValue(dataBase, new SuperArrayValue<float>(data[j], splitMak[j]), null);
                                break;
                                case "int":
                                pi.SetValue(dataBase, new SuperArrayValue<int>(data[j], splitMak[j]), null);
                                break;
                                case "ulong":
                                pi.SetValue(dataBase, new SuperArrayValue<ulong>(data[j], splitMak[j]), null);
                                break;
                                case "bool":
                                pi.SetValue(dataBase, new SuperArrayValue<bool>(data[j], splitMak[j]), null);
                                break;
                                case "double":
                                pi.SetValue(dataBase, new SuperArrayValue<double>(data[j], splitMak[j]), null);
                                break;
                                case "long":
                                pi.SetValue(dataBase, new SuperArrayValue<long>(data[j], splitMak[j]), null);
                                break;
                                case "short":
                                pi.SetValue(dataBase, new SuperArrayValue<short>(data[j], splitMak[j]), null);
                                break;
                                case "char":
                                pi.SetValue(dataBase, new SuperArrayValue<char>(data[j], splitMak[j]), null);
                                break;
                                case "uint":
                                pi.SetValue(dataBase, new SuperArrayValue<uint>(data[j], splitMak[j]), null);
                                break;
                                case "byte":
                                pi.SetValue(dataBase, new SuperArrayValue<byte>(data[j], splitMak[j]), null);
                                break;
                                default:
                                //动态创建泛型对象
                                Type t = Type.GetType(parType[j]).IsEnum ? typeof(SuperArrayValue<>) : typeof(SuperArrayObj<>);
                                t = t.MakeGenericType(Type.GetType(parType[j]));
                                object o = Activator.CreateInstance(t, data[j], splitMak[j]);
                                pi.SetValue(dataBase, o, null);
                                break;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException("配置表动态赋值出错：<" + className + ">   " + parName[j] + "=" + data[j]);
                    }
                }
            }

            if (dataBase.id == 0) dataBase.id = inIndex++;
            if (string.IsNullOrEmpty(dataBase.key)) dataBase.key = dataBase.id.ToString();
            dataList.Add(dataBase);
            dataBase.OnLoadDone();
        }
        dataDic.Add(className, dataList);
    }


    /// <summary>
    /// 去掉开头和结尾的半角引号，与\r\n，顺便Trim
    /// </summary>
    private static string ReplaceQuote(string value)
    {
        if (value == null) return null;
        if (value == "") return "";
        Regex reg = new Regex(@"^"".*""$");
        if (reg.IsMatch(value)) value = value.Substring(1, value.Length - 2);
        Regex reg2 = new Regex(@"[\r\n]");
        value = reg2.Replace(value, "");
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
    public T GetConfigDataById<T>(ulong id) where T : ConfigDataBase
    {
        string temp = typeof(T).ToString();
        if (!dataDic.ContainsKey(temp)) throw new ArgumentNullException("不存在的配置表：" + temp);
        List<ConfigDataBase> tempList = dataDic[temp];
        T t = tempList.Find(x => x.id == id) as T;
#if UNITY_EDITOR
        if (t == null) Debug.LogWarning("配置表不存在id→ " + temp + ":" + id);
#endif
        return t;
    }

    /// <summary>
    /// 根据KEY返回一个配置表对象
    /// </summary>
    public T GetConfigDataBykey<T>(string key) where T : ConfigDataBase
    {
        string temp = typeof(T).ToString();
        if (!dataDic.ContainsKey(temp)) throw new ArgumentNullException("不存在的配置表：" + temp);
        List<ConfigDataBase> tempList = dataDic[temp];
        T t = tempList.Find(x => x.key == key) as T;
#if UNITY_EDITOR
        if (t == null) Debug.LogWarning("配置表不存在key→ " + temp + ":" + key);
#endif
        return t;
    }

    /// <summary>
    /// 判断一个配置中是否存在该key的数据
    /// </summary>
    public bool ExistsKey<T>(string key) where T : ConfigDataBase
    {
        string temp = typeof(T).ToString();
        if (!dataDic.ContainsKey(temp)) return false;
        var tempList = dataDic[temp];
        return tempList.Exists(x => x.key == key);
    }


    /// <summary>
    /// 判断一个配置中是否存在该id的数据
    /// </summary>
    public bool ExistsId<T>(ulong id) where T : ConfigDataBase
    {
        string temp = typeof(T).ToString();
        if (!dataDic.ContainsKey(temp)) return false;
        var tempList = dataDic[temp];
        return tempList.Exists(x => x.id == id);
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
    /// 配置表初始完成之前，某些对象是不能得到正确赋值的，所以把它们暂时存起来，在配置表初始完成后一并赋值，注意它是用key来索引的
    /// </summary>
    private void AddPostpone(ConfigDataBase cdb, PropertyInfo pi, string type, string key)
    {
        var arg = new XYZW<ConfigDataBase, PropertyInfo, string, string>(cdb, pi, type, key);
        postponeList.Add(arg);
    }

    /// <summary>
    /// 给延迟对象赋值
    /// </summary>
    private void PostponeVoluation()
    {
        foreach (var arg in postponeList)
        {
            if (!dataDic.ContainsKey(arg.z)) throw new ArgumentNullException("不存在的配置表：" + arg.z);
            List<ConfigDataBase> tempList = dataDic[arg.z];
            ConfigDataBase t = tempList.Find(x => x.key == arg.w) ?? tempList.Find(x => x.id == ulong.Parse(arg.w));
            arg.y.SetValue(arg.x, t, null);
        }
        postponeList.Clear();
    }
}

/// <summary>
/// 配置表类的基类
/// 该配置表第一行为参数说明，第二行为参数类型，第三行为参数名字
/// </summary>
public abstract class ConfigDataBase
{


    /// <summary>
    /// 主键，唯一
    /// </summary>
    public ulong id { get; set; }
    public string key { get; set; }

    /// <summary>
    /// 根据ID返回一个配置表对象
    /// </summary>
    public static T GetConfigDataById<T>(ulong id) where T : ConfigDataBase
    {
        return SuperConfig.Instance.GetConfigDataById<T>(id);
    }

    /// <summary>
    /// 根据ID返回一个配置表对象
    /// </summary>
    public static T GetConfigDataById<T>(string id) where T : ConfigDataBase
    {
        return SuperConfig.Instance.GetConfigDataById<T>(ulong.Parse(id));
    }


    /// <summary>
    /// 根据KEY返回一个配置表对象
    /// </summary>
    public static T GetConfigDataByKey<T>(string key) where T : ConfigDataBase
    {
        return SuperConfig.Instance.GetConfigDataBykey<T>(key);
    }

    /// <summary>
    /// 根据类型返回配置表所有对象
    /// </summary>
    public static List<T> GetConfigDataList<T>() where T : ConfigDataBase
    {
        return SuperConfig.Instance.GetConfigDataList<T>();
    }

    /// <summary>
    /// 根据属性名称，查找配置表中第一个符合条件的对象
    /// </summary>
    /// <typeparam name="T">配置表类型</typeparam>
    /// <param name="pro">属性名</param>
    /// <param name="value">属性值</param>
    /// <returns></returns>
    public static T GetConfigDataByProperty<T>(string pro, object value) where T : ConfigDataBase
    {
        return SuperConfig.Instance.GetConfigDataByProperty<T>(pro, value);
    }

    /// <summary>
    /// 判断一个配置中是否存在该id的数据
    /// </summary>
    public static bool ExistsId<T>(ulong id) where T : ConfigDataBase
    {
        return SuperConfig.Instance.ExistsId<T>(id);
    }

    /// <summary>
    /// 判断一个配置中是否存在该id的数据
    /// </summary>
    public static bool ExistsId<T>(string id) where T : ConfigDataBase
    {
        return SuperConfig.Instance.ExistsId<T>(ulong.Parse(id));
    }

    /// <summary>
    /// 判断一个配置中是否存在该id的数据
    /// </summary>
    public static bool ExistsKey<T>(string idStr) where T : ConfigDataBase
    {
        return ExistsKey<T>(idStr);
    }

    /// <summary>
    /// 每个配置表对象初始化完成时被调用
    /// </summary>
    public virtual void OnLoadDone() { }

}
