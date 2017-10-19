using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using UnityEditor;
using System.IO;

using Random = UnityEngine.Random;

public class SuperTool
{

    private static List<Component> _components;
    public delegate void MoveDownCallback(object arg);

    #region 数组与list的操作

    /// <summary>
    /// 此方法将返回一个RandomSection对象，可以获得落点所在区间，或者归一化后的比例
    /// </summary>
    /// <param name="list">比例数组</param>
    /// <returns>调用该对象的Ran方法获得落点所在区间</returns>
    public static WeightSection CreateWeightSection(List<float> list)
    {
        return WeightSection.Create(list);
    }
    /// <summary>
    /// 此方法将返回一个RandomSection对象，可以获得落点所在区间，或者归一化后的比例
    /// </summary>
    /// <param name="list">比例数组</param>
    /// <returns>调用该对象的Ran方法获得落点所在区间</returns>
    public static WeightSection CreateWeightSection(List<double> list)
    {
        List<float> listTemp = list.Select(t => (float)t).ToList();
        return CreateWeightSection(listTemp);
    }
    /// <summary>
    /// 此方法将返回一个RandomSection对象，可以获得落点所在区间，或者归一化后的比例
    /// </summary>
    /// <param name="list">比例数组</param>
    /// <returns>调用该对象的Ran方法获得落点所在区间</returns>
    public static WeightSection CreateWeightSection(List<long> list)
    {
        List<float> listTemp = list.Select(t => (float)t).ToList();
        return CreateWeightSection(listTemp);
    }
    /// <summary>
    /// 此方法将返回一个RandomSection对象，可以获得落点所在区间，或者归一化后的比例
    /// </summary>
    /// <param name="list">比例数组</param>
    /// <returns>调用该对象的Ran方法获得落点所在区间</returns>
    public static WeightSection CreateWeightSection(List<int> list)
    {
        List<float> listTemp = list.Select(t => (float)t).ToList();
        return CreateWeightSection(listTemp);
    }

    public static WeightSection CreateWeightSection(List<string> list)
    {
        List<float> listTemp = list.Select(t => (float.Parse(t))).ToList();
        return CreateWeightSection(listTemp);
    }


    /// <summary>
    /// 将一个字符串转换成相应的数组
    /// </summary>
    /// <typeparam name="T">要转换的目标类型，只能是数字类型与string</typeparam>
    /// <param name="data">要转换的字符串</param>
    /// <param name="split">分隔符</param>
    public static List<T> StringConvert<T>(string data, char split)
    {
        if (string.IsNullOrEmpty(data)) return new List<T>();
        string[] temp = data.Split(split);
        Type type = typeof(T);
        object tempList;
        if (type == typeof(short))
        {
            tempList = new List<short>();
            foreach (var str in temp)
                ((List<short>)tempList).Add(short.Parse(str));
        }
        else if (type == typeof(int))
        {
            tempList = new List<int>();
            foreach (var str in temp)
                ((List<int>)tempList).Add(int.Parse(str));
        }
        else if (type == typeof(uint))
        {
            tempList = new List<uint>();
            foreach (var str in temp)
                ((List<uint>)tempList).Add(uint.Parse(str));
        }
        else if (type == typeof(long))
        {
            tempList = new List<long>();
            foreach (var str in temp)
                ((List<long>)tempList).Add(long.Parse(str));
        }
        else if (type == typeof(ulong))
        {
            tempList = new List<ulong>();
            foreach (var str in temp)
                ((List<ulong>)tempList).Add(ulong.Parse(str));
        }
        else if (type == typeof(float))
        {
            tempList = new List<float>();
            foreach (var str in temp)
                ((List<float>)tempList).Add(float.Parse(str));
        }
        else if (type == typeof(double))
        {
            tempList = new List<double>();
            foreach (var str in temp)
                ((List<double>)tempList).Add(double.Parse(str));
        }
        else if (type == typeof(string))
        {
            tempList = new List<string>();
            foreach (var str in temp)
                ((List<string>)tempList).Add(str);
        }
        else
        {
            Debug.Log("只能转换数字与string类型的字符串");
            throw new ArgumentException();
        }
        return tempList as List<T>;
    }



    /// <summary>
    /// 随机返回数组中的一个元素(抽取放回)
    /// </summary>
    public static T RandomElement<T>(T[] tar)
    {
        if (tar.Length == 0) return default(T);
        return tar[Random.Range(0, tar.Length)];
    }

    /// <summary>
    /// 随机弹出数组中的一个元素(抽取不放回)，传入数组的地址(ref)
    /// </summary>
    public static T RandomPopElement<T>(ref T[] tar)
    {
        if (tar.Length == 0) return default(T);
        List<T> tempList = new List<T>(tar);
        T temp = RandomPopElement(tempList);
        tar = tempList.ToArray();
        return temp;
    }

    /// <summary>
    /// 随机返回list中的一个元素(抽取放回)
    /// </summary>
    public static T RandomElement<T>(List<T> tar)
    {
        if (tar.Count == 0) return default(T);
        return tar[Random.Range(0, tar.Count)];
    }

    /// <summary>
    /// 随机弹出list中的一个元素(抽取不放回)
    /// </summary>
    public static T RandomPopElement<T>(List<T> tar)
    {
        if (tar.Count == 0) return default(T);
        int index = Random.Range(0, tar.Count);
        T temp = tar[index];
        tar.RemoveAt(index);
        return temp;
    }


    /// <summary>
    /// 弹出List的最后一个元素
    /// </summary>
    public static T Pop<T>(List<T> list)
    {
        if (list.Count == 0) return default(T);
        T temp = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        return temp;
    }

    /// <summary>
    /// 弹出指定元素
    /// </summary>
    public static T Pop<T>(List<T> list, int index)
    {
        if (list.Count == 0) return default(T);
        T temp = list[index];
        list.RemoveAt(index);
        return temp;
    }

    /// <summary>
    /// 弹出数组的最后一个元素，传入数组地址（ref）
    /// </summary>
    public static T Pop<T>(ref T[] arr)
    {
        if (arr.Length == 0) return default(T);
        List<T> tempList = new List<T>(arr);
        T temp = Pop(tempList);
        arr = tempList.ToArray();
        return temp;
    }

    /// <summary>
    /// 弹出指定元素，传入ref
    /// </summary>
    public static T Pop<T>(ref T[] arr, int index)
    {
        if (arr.Length == 0) return default(T);
        List<T> tempList = new List<T>(arr);
        T temp = Pop(tempList, index);
        arr = tempList.ToArray();
        return temp;
    }

    /// <summary>
    /// 判断一个数组是否有重复
    /// </summary>
    public static bool HasRepeat<T>(T[] array)
    {
        if (array == null || array.Length == 1 || array.Length == 0) return false;
        for (int i = 0; i < array.Length; i++)
            for (int j = 0; j < array.Length; j++)
            {
                if (i == j) continue;
                else if (array[i].Equals(array[j])) return true;
            }
        return false;
    }
    /// <summary>
    /// 判断一个List是否有重复
    /// </summary>
    public static bool HasRepeat<T>(List<T> list)
    {
        T[] arr = list.ToArray();
        return HasRepeat(arr);
    }

    /// <summary>
    /// 数组A是否被B包含
    /// </summary>
    public static bool Contains<T>(List<T> arr_a, List<T> arr_b)
    {
        if (arr_a == null || arr_b == null)
            throw new NullReferenceException("比对数组不能为null");
        return arr_a.All(arr_b.Contains);
    }

    /// <summary>
    /// 数组A是否被B包含
    /// </summary>
    public static bool Contains<T>(T[] arr_a, T[] arr_b)
    {
        if (arr_a == null || arr_b == null)
            throw new NullReferenceException("比对数组不能为null");
        return arr_a.All(arr_b.Contains);
    }


    /// <summary>
    /// 交换List中两个指定的元素
    /// </summary>
    public static void Swap<T>(List<T> list, int index_a, int index_b)
    {
        T temp = list[index_a];
        list[index_a] = list[index_b];
        list[index_b] = temp;
    }

    /// <summary>
    /// 交换数组中两个指定的元素
    /// </summary>
    public static void Swap<T>(T[] arr, int index_a, int index_b)
    {
        T temp = arr[index_a];
        arr[index_a] = arr[index_b];
        arr[index_b] = temp;
    }


    /// <summary>
    /// 判断一个数组是否属于另一个数组，并返回相应信息
    /// </summary>
    /// <param name="bigArr">资源数组</param>
    /// <param name="smallArr">比对数组</param>
    /// <returns>-2代表没有，-1代表存在但不连续，返回大于等于0的数代表连续存在，返回值代表其第一个索引位置</returns>
    public static int ArrayInclude<T>(T[] bigArr, T[] smallArr)
    {
        Dictionary<T, int> allSouldic = new Dictionary<T, int>();
        Dictionary<T, int> targetSouldic = new Dictionary<T, int>();
        foreach (var x in bigArr)
        {
            if (!allSouldic.ContainsKey(x)) allSouldic.Add(x, 1);
            else allSouldic[x]++;
        }
        foreach (var x in smallArr)
        {
            if (!targetSouldic.ContainsKey(x)) targetSouldic.Add(x, 1);
            else targetSouldic[x]++;
        }

        foreach (var x in targetSouldic.Keys)
        {
            if (!allSouldic.ContainsKey(x)) return -2;
            if (allSouldic[x] < targetSouldic[x]) return -2;
        }

        int checkIndex = 0;
        List<T> lifeList = new List<T>(smallArr);
        List<T> dethList = new List<T>();
        for (int i = 0; i < bigArr.Length; i++)
        {
            T temp = bigArr[i];
            if (lifeList.Contains(temp))
            {
                lifeList.Remove(temp);
                if (lifeList.Count == 0) return checkIndex;
                dethList.Add(temp);
            }
            else
            {
                if (dethList.Contains(temp))
                {
                    for (int j = checkIndex; j < bigArr.Length; j++)
                    {
                        if (bigArr[j].Equals(temp))
                        {
                            checkIndex = j + 1;
                            break;
                        }
                        lifeList.Add(bigArr[j]);
                    }
                }
                else
                {
                    i = checkIndex++;
                    dethList.Clear();
                    lifeList = new List<T>(smallArr);
                }
            }
        }
        return -1;
    }

    public static string Show(MatchCollection mc)
    {
        if (mc.Count == 0) return "0个元素";
        StringBuilder sb = new StringBuilder();
        foreach (var match in mc)
            sb.Append(match + ",");
        sb.Remove(sb.Length - 1, 1);
        return sb.ToString();
    }


    public static string Show<T>(T[] strs)
    {
        if (strs.Length == 0) return "0个元素";
        StringBuilder sb = new StringBuilder();
        foreach (var str in strs)
            sb.Append(str + ",");
        sb.Remove(sb.Length - 1, 1);
        return sb.ToString();
    }

    public static string Show<T>(List<T> strs)
    {
        if (strs.Count == 0) return "0个元素";
        StringBuilder sb = new StringBuilder();
        foreach (var str in strs)
            sb.Append(str + ",");
        sb.Remove(sb.Length - 1, 1);
        return sb.ToString();
    }

    public static string Show<T>(Stack<T> strs)
    {
        if (strs.Count == 0) return "0个元素";
        StringBuilder sb = new StringBuilder();
        foreach (var str in strs)
            sb.Append(str + ",");
        sb.Remove(sb.Length - 1, 1);
        return sb.ToString();
    }

    public static string Show(ArrayList strs)
    {
        if (strs.Count == 0) return "0个元素";
        StringBuilder sb = new StringBuilder();
        foreach (var str in strs)
            sb.Append(str + ",");
        sb.Remove(sb.Length - 1, 1);
        return sb.ToString();
    }


    #endregion

    /// <summary>
    /// 向上查找第一个找到的指定的组件
    /// </summary>
    public static T GetComponentUpward<T>(Component component) where T : Component
    {
        GameObject gameObject = component.gameObject;
        while (true)
        {
            Component cp = gameObject.GetComponent<T>();
            if (cp == null)
            {
                if (gameObject.transform.parent != null)
                    gameObject = gameObject.transform.parent.gameObject;
                else
                    return null;
            }
            else
            {
                return (T)cp;
            }
        }
    }

    /// <summary>
    /// 找物体及其所有子物体下面的Component
    /// </summary>
    public static List<T> GetComponentsInChildren<T>(Component component) where T : Component
    {
        return GetComponentsInChildren<T>(component.gameObject);
    }

    /// <summary>
    /// 找物体及其所有子物体下面的Component
    /// </summary>
    public static List<T> GetComponentsInChildren<T>(GameObject gameObject) where T : Component
    {
        _components = new List<Component>();
        FindChild<T>(gameObject.transform);
        if (_components.Count == 0)
            Debug.LogWarning(gameObject.name + "下面所有子物体中，均没有找到" + typeof(T).Name);

        List<T> temp = _components.Cast<T>().ToList();
        _components = null;
        return temp;
    }

    /// <summary>
    /// 寻找具体实现，这是一个递归寻找
    /// </summary>
    private static void FindChild<T>(Transform transform) where T : Component
    {

        T component = transform.GetComponent<T>();
        if (component != null) _components.Add(component);

        int num = transform.childCount;
        if (num != 0)
        {
            for (int i = 0; i < num; i++)
            {
                FindChild<T>(transform.GetChild(i));
            }
        }
    }

    /// <summary>
    /// 匹配一个字符串是否是一个组件以及其父级组件的名字
    /// </summary>
    /// <param name="cp">匹配的组件</param>
    /// <param name="matchName">匹配字符串</param>
    /// <returns>返回第一个匹配到的GameObject，如果没有匹配到，返回null</returns>
    public static GameObject IsMatchNameWithFather(Component cp, string matchName)
    {
        Transform transform = cp.gameObject.transform;
        while (transform != null)
        {
            string name = transform.gameObject.name.Replace("(Clone)", "");
            if (name == matchName) return transform.gameObject;
            else
                transform = transform.parent;
        }
        return null;
    }

    /// <summary>
    /// 根据tag和name找对象，注意对象必须处于激活才可以查找到
    /// </summary>
    public static GameObject FindWithTagAndName(string tag, string name)
    {
        name = name.Trim();
        tag = tag.Trim();
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        if (gameObjects == null || gameObjects.Length == 0)
        {
            Debug.LogWarning("不存在tag：" + tag);
            return null;
        }
        GameObject o = gameObjects.FirstOrDefault(go => go.name.Trim() == name);
        if (o == null) Debug.LogWarning("在tag:" + tag + "下，没有找到名为" + name + "的对象");
        return o;
    }

    /// <summary>
    /// 获取顶层父物体
    /// </summary>
    public static GameObject GetMasterFather(Component cp)
    {
        Transform ts = cp.transform;
        while (ts.parent != null)
        {
            ts = ts.parent;
        }
        return ts.gameObject;
    }

    /// <summary>
    /// 返回鼠标的世界坐标系坐标。
    /// </summary>
    public static Vector2 MouseWorldPosition(Camera cam = null)
    {
        if (cam == null) cam = Camera.main;
        Vector2 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        return pos;
    }

    /// <summary>
    /// 物体按速度移动到指定位置，可以指定物体到达后的事件，只能用于2D移动
    /// </summary>
    public static SuperToolMoveComponet MoveTo2D(MoveArgs arg)
    {
        arg.destroy = false;
        var t = arg.CurTransform.gameObject.GetComponent<SuperToolMoveComponet>();
        if (t != null) UnityEngine.Object.Destroy(t);
        var stc = arg.CurTransform.gameObject.AddComponent<SuperToolMoveComponet>();
        stc.SetPar(arg);
        return stc;
    }

    /// <summary>
    /// 物体按指定方式旋转到目标，可以指定物体到达后的事件，只能用于2D旋转
    /// </summary>
    public static SuperToolTowards Towards2D(TowardsArg arg)
    {
        arg.destroy = false;
        var t = arg.curTransform.gameObject.GetComponent<SuperToolTowards>();
        if (t != null) UnityEngine.Object.Destroy(t);
        var stt = arg.curTransform.gameObject.AddComponent<SuperToolTowards>();
        stt.SetArg(arg);
        return stt;
    }

    #region 简化设置父物体
    /// <summary>
    /// 设置一个物体的父物体，自动归零本地坐标，可以设置Z轴偏移，用于控制2D图片前后关系
    /// </summary>
    /// <param name="par">父物体</param>
    /// <param name="child">子物体</param>
    /// <param name="offsetZ">Z轴偏移</param>
    /// <param name="localZ">是否是本地坐标，如果为true，添加的child的Z坐标受到父物体影响，反之不受影响</param>
    public static void SetParentAndZero(Component par, Component child, float offsetZ = 0, bool localZ = true)
    {
        var tempPar = par as Transform;
        Transform parentTransform = tempPar ?? par.transform;
        var tempChild = child as Transform;
        Transform childTranfrom = tempChild ?? child.transform;
        Vector3 tempScale = childTranfrom.localScale;
        Quaternion routation = childTranfrom.localRotation;

        childTranfrom.SetParent(parentTransform);
        child.transform.localPosition = localZ ? new Vector3(0, 0, childTranfrom.position.z + offsetZ)
                                                                       : new Vector3(0, 0, childTranfrom.localPosition.z + offsetZ);
        childTranfrom.localScale = tempScale;
        childTranfrom.localRotation = routation;
    }

    /// <summary>
    /// 设置一个物体的父物体，自动归零本地坐标，可以设置Z轴偏移，用于控制2D图片前后关系
    /// </summary>
    /// <param name="par">父物体</param>
    /// <param name="child">子物体</param>
    /// <param name="offsetZ">Z轴偏移</param>
    /// <param name="localZ">是否是本地坐标，如果为true，添加的child的Z坐标受到父物体影响，反之不受影响</param>
    public static void SetParentAndZero(Component par, GameObject child, float offsetZ = 0, bool localZ = true)
    {
        SetParentAndZero(par, child.transform, offsetZ, localZ);
    }

    /// <summary>
    /// 设置一个物体的父物体，自动归零本地坐标，可以设置Z轴偏移，用于控制2D图片前后关系
    /// </summary>
    /// <param name="par">父物体</param>
    /// <param name="child">子物体</param>
    /// <param name="offsetZ">Z轴偏移</param>
    /// <param name="localZ">是否是本地坐标，如果为true，添加的child的Z坐标受到父物体影响，反之不受影响</param>
    public static void SetParentAndZero(GameObject par, GameObject child, float offsetZ = 0, bool localZ = true)
    {
        SetParentAndZero(par.transform, child.transform, offsetZ, localZ);
    }

    #endregion


    /// <summary>
    /// 将一个物体的坐标，按照局部坐标的方式，添加到父物体上。
    /// 方法存在意义：prefab设置好后，如果是手动拖动到父物体之下，采用prefab的xyz为局部坐标，
    /// 通过代码tranform.SetParent设置到父物体下，采用的perfab的xyz是世界坐标....
    /// 这个方法规避这个问题，统一将prefab的坐标视为局部坐标，与手动拖放的效果一致
    /// </summary>
    public static void SetParentWithLocal(Component par, Component child)
    {
        var tempPar = par as Transform;
        Transform parentTransform = tempPar ?? par.transform;
        var tempChild = child as Transform;
        Transform childTranfrom = tempChild ?? child.transform;

        Vector3 scaleTemp = childTranfrom.localScale;
        Vector3 posTemp = childTranfrom.position;
        Quaternion rotationTemp = childTranfrom.localRotation;
        childTranfrom.SetParent(parentTransform);
        childTranfrom.localScale = scaleTemp;
        childTranfrom.localPosition = posTemp;
        childTranfrom.localRotation = rotationTemp;
        if (childTranfrom is RectTransform)
            (childTranfrom as RectTransform).anchoredPosition = posTemp;

    }

    /// <summary>
    /// 将一个四则混合运算的字符串转换成逆波兰式字符串
    /// </summary>
    public static string ToRpn(string input)
    {
        string str = ConverSpace(input);
        if (string.IsNullOrEmpty(str)) return null;
        string left = null;
        Regex reg = new Regex(@"[=]");
        if (reg.IsMatch(str))
        {
            string[] tempstr = str.Split('=');
            left = tempstr[0];
            str = tempstr[1];
        }
        //把+-和--分别变换为-和+
        str = Regex.Replace(str, @"\+\-", "-");
        str = Regex.Replace(str, @"\-\-", "+");
        //把 -( 处理成-1*(
        str = Regex.Replace(str, @"\*\-\(", "*-1*(");
        str = Regex.Replace(str, @"\/\-\(", "/-1*(");
        str = Regex.Replace(str, @"\(\-\(", "(-1*(");
        //所有运算符前后加上逗号
        str = Regex.Replace(str, @"\+", ",+,");
        str = Regex.Replace(str, @"\-", ",-,");
        str = Regex.Replace(str, @"\*", ",*,");
        str = Regex.Replace(str, @"\/", ",/,");
        str = Regex.Replace(str, @"\(", ",(,");
        str = Regex.Replace(str, @"\)", ",),");
        //去掉开头和结尾的逗号，把重复的逗号变成1个
        str = Regex.Replace(str, @",+", ",");
        str = Regex.Replace(str, @"^,", "");
        str = Regex.Replace(str, @",$", "");
        //把负数的中间间隔逗号去掉
        str = Regex.Replace(str, @"^-,", "-");
        str = Regex.Replace(str, @"\*,-,", "*,-");
        str = Regex.Replace(str, @"\/,-,", "/,-");
        str = Regex.Replace(str, @"\(,-,", "(,-");
        string[] temp = str.Split(',');
        Stack<string> operatorBuffer = new Stack<string>();
        StringBuilder result = new StringBuilder();
        if (left != null) result.Append(left + '=');
        int check = 0;
        for (int i = 0; i < temp.Length; i++)
        {
            if (++check > 500) throw new Exception("逆波兰式转换发生死循环，公式不对？？  →" + input);
            if (IsOperator(temp[i]) == 0) result.Append(temp[i] + ',');
            else if (IsOperator(temp[i]) == 2)
            {
                if (temp[i] == "(") operatorBuffer.Push(temp[i]);
                else
                {
                    if (operatorBuffer.Peek() == "(") operatorBuffer.Pop();
                    else
                    {
                        result.Append(operatorBuffer.Pop() + ',');
                        i--;
                    }
                }
            }
            else if (IsOperator(temp[i]) == 1)
            {
                if (operatorBuffer.Count == 0 || operatorBuffer.Peek() == "(")
                    operatorBuffer.Push(temp[i]);
                else
                {
                    if (IsHighPriorityThanBefore(operatorBuffer.Peek(), temp[i]))
                        operatorBuffer.Push(temp[i]);
                    else
                    {
                        result.Append(operatorBuffer.Pop() + ',');
                        operatorBuffer.Push(temp[i]);
                    }
                }
            }
        }
        while (operatorBuffer.Count > 0) result.Append(operatorBuffer.Pop() + ",");
        result.Remove(result.Length - 1, 1);
        return result.ToString();
    }

    /// <summary>
    /// 运算符返回1，括号返回2，参数返回0
    /// </summary>
    private static byte IsOperator(string str)
    {
        Regex reg = new Regex(@"^[\+\-\*\/]$");
        Regex reg2 = new Regex(@"^[\(\)]$");
        if (reg.IsMatch(str))
            return 1;
        else if (reg2.IsMatch(str))
            return 2;
        else
            return 0;
    }

    /// <summary>
    /// 判断after计算优先级是否高于before
    /// </summary>
    private static bool IsHighPriorityThanBefore(string before, string after)
    {
        if (IsOperator(before) != 1 || IsOperator(after) != 1)
        {
            Debug.LogError("计算字符串有错误，传入的符号优先级对比必须是运算符：" + before + "," + after);
            throw new ArgumentException();
        }
        if ((before == "+" || before == "-") && (after == "*" || after == "/"))
            return true;
        else
            return false;
    }

    /// <summary>
    /// 构造一个FF00FF00（rgba）这种格式的颜色
    /// </summary>
    public static Color CreateColor(string hex)
    {
        hex = ConverSpace(hex);
        float r = (float)Convert.ToInt32(hex.Substring(0, 2), 16) / 255;
        float g = (float)Convert.ToInt32(hex.Substring(2, 2), 16) / 255;
        float b = (float)Convert.ToInt32(hex.Substring(4, 2), 16) / 255;
        float a = (float)Convert.ToInt32(hex.Substring(6, 2), 16) / 255;
        Color color = new Color(r, g, b, a);
        return color;
    }

    /// <summary>
    /// 用字符串来调用目标对象的方法
    /// </summary>
    /// <param name="target">目标对象</param>
    /// <param name="option">如果为RequireReceiver，找不到调用目标会报错，否则，找不到调用目标会无视</param>
    /// <param name="methodName">调用方法名</param>
    /// <param name="parameter ">参数</param>
    /// <returns></returns>
    public static object InvokeTarget(object target, SendMessageOptions option, string methodName, params object[] parameter)
    {
        var t = target.GetType().GetMethod(methodName);
        if (t == null)
        {
            if (option == SendMessageOptions.RequireReceiver) throw new NullReferenceException("目标对象<" + target + ">不存在" + methodName + "方法");
            return null;
        }
        return t.Invoke(target, parameter);
    }

    /// <summary>
    /// 用字符串来调用目标对象的方法
    /// </summary>
    /// <param name="target">目标对象</param>
    /// <param name="methodName">调用方法名</param>
    /// <param name="parameter ">参数</param>
    /// <returns></returns>
    public static object InvokeTarget(object target, string methodName,
        params object[] parameter)
    {
        return InvokeTarget(target, SendMessageOptions.RequireReceiver, methodName, parameter);
    }

    /// <summary>
    /// 检查Delegate是否可以调用
    /// </summary>
    public static bool CheckDelegate(Delegate d)
    {
        if (d.Method.IsStatic || !d.Target.Equals(null))
            return true;
        else
            return false;
    }

    /// <summary>
    /// 去除字符的任何空白部分
    /// </summary>
    public static string ConverSpace(string input)
    {
        if (input == null) return null;
        return Regex.Replace(input, @"\s+", "");
    }

    #region 工具菜单
#if UNITY_EDITOR
    [MenuItem("SuperTool/生成预制路径 &%0")]
    static void AddPrefab(MenuCommand cmd)
    {
        List<string> pathList = Selection.objects.Select(AssetDatabase.GetAssetPath).Where(path => !string.IsNullOrEmpty(path)).ToList();
        WriteConfigData("DefaultPath", "Prefab", CreatResFullPath(pathList.ToArray(), "prefab"));
    }

    [MenuItem("SuperTool/生成配置路径 &%9")]
    static void AddText(MenuCommand cmd)
    {
        List<string> pathList = Selection.objects.Select(AssetDatabase.GetAssetPath).Where(path => !string.IsNullOrEmpty(path)).ToList();
        WriteConfigData("DefaultPath", "Config", CreatResFullPath(pathList.ToArray(), "txt"));
    }

    //[MenuItem("SuperTool/清除PlayerPrefab &%8")]
    //static void ClearPlayerPrefab()
    //{
    //   ScriptableWizard.DisplayWizard<_WizardShow>("初始存储标记","确定"); 
    //}

    //private class _WizardShow : ScriptableWizard
    //{
    //    public string newMark;

    //    void OnWizardCreate()
    //    {
    //        PlayerPrefs.SetString("FirstRun", newMark);
    //    }
    //}
#endif




    /// <summary>
    /// 根据传入的路径，返回该路径与子路径下所有指定类型文件的路径
    /// </summary>
    /// <param name="rootPath"></param>
    /// <param name="fileType"></param>
    /// <returns></returns>
    private static string[] CreatResFullPath(string[] rootPath, string fileType)
    {
        string[] paths = GetHighter(rootPath);
        List<string> resPath = new List<string>();
        var reg = new Regex(@"^Assets/Resources");
        foreach (var v in paths)
        {
            if (!reg.IsMatch(v)) throw new ArgumentException("只能选择Resources内的文件或文件夹");
            var temp = ConvertPath(v, fileType);
            if (temp != null) resPath.AddRange(temp);
        }
        if (resPath.Count > 1)
        {
            for (int i = 0; i < resPath.Count - 1; i++)
            {
                if (resPath[i] == resPath[i + 1])
                {
                    resPath.RemoveAt(i + 1);
                    i--;
                }
            }
        }
        return resPath.ToArray();

    }


    /// <summary>
    /// 根据传入的路径（基于Resources），返回该路径与子路径下所有指定类型文件的路径
    /// </summary>
    public static string[] CreatResByPath(string[] rootPath, string fileType)
    {
        var t = rootPath;
        for (int i = 0; i < t.Length; i++) t[i] = "Assets/Resources/" + t[i];
        return CreatResFullPath(t, fileType);
    }

    /// <summary>
    /// 返回一个Resource的文件下指定的文件类型
    /// </summary>
    private static List<string> ConvertPath(string path, string fileType)
    {
        var reg = new Regex(@"." + fileType + "$");
        var reg2 = new Regex(@"^Assets/Resources[/\\]?");
        if (File.Exists(path))
        {
            string tPath = path;
            if (reg.IsMatch(tPath))
            {
                tPath = reg2.Replace(tPath, "").Replace('\\', '/');
                tPath = reg.Replace(tPath, "");
                return new List<string>(new[] { tPath });
            }
            return null;
        }
        List<string> tar = new List<string>();
        if (Directory.Exists(path))
        {
            string[] files = Directory.GetFiles(path);
            string[] dirs = Directory.GetDirectories(path);
            for (int i = 0; i < files.Length; i++)
            {
                if (reg.IsMatch(files[i]))
                {
                    files[i] = reg2.Replace(files[i], "").Replace('\\', '/');
                    files[i] = reg.Replace(files[i], "");
                    tar.Add(files[i]);
                }
            }
            foreach (string ts in dirs)
            {
                var t = ConvertPath(ts, fileType);
                if (t != null) tar.AddRange(t);
            }
        }
        return tar;
    }


    private static string[] GetHighter(params string[] paths)
    {
        if (paths.Length <= 1) return paths;
        List<string> pathList = new List<string>(paths);
        for (int i = 0; i < pathList.Count - 1; i++)
        {
            if (pathList[i] == pathList[i + 1])
            {
                pathList.RemoveAt(i + 1);
                i--;
            }
        }
        List<string> resList = new List<string>();
        List<string> remove = new List<string>();
        while (pathList.Count > 1)
        {
            string tar = pathList[0];
            for (int i = 1; i < pathList.Count; i++)
            {
                string res = HigherFileLevel(tar, pathList[i]);
                if (!string.IsNullOrEmpty(res))
                {
                    tar = res;
                    if (!remove.Contains(tar)) remove.Add(tar);
                    if (!remove.Contains(pathList[i])) remove.Add(pathList[i]);
                }
            }
            resList.Add(tar);
            pathList.RemoveAt(0);
            foreach (var v in remove) if (pathList.Contains(v)) pathList.Remove(v);
        }
        if (pathList.Count > 1) throw new Exception("错误？？");
        if (pathList.Count > 0) resList.Add(pathList[0]);
        return resList.ToArray();
    }


    private static string HigherFileLevel(string firstPath, string secondPath)
    {
        string[] first = firstPath.Split('/');
        string[] second = secondPath.Split('/');
        int fl = first.Length;
        int sl = second.Length;
        int count = fl < sl ? fl : sl;
        for (int i = 0; i < count; i++)
        {
            if (first[i] != second[i]) return null;
            if (i == fl - 1) return firstPath;
            if (i == sl - 1) return secondPath;
        }
        return null;
    }

    #region 
    /// <summary>
    /// 设置写入结果的文件名称，与搜索（多个）路径
    /// </summary>
    /// <param name="recordPath">记录文件路径（具体到某个文件）</param>
    /// <param name="resourcePath">数据目录，可以是文件夹</param>
    /// <param name="clear">true的话，会清除掉本次不存在的文件，否则只是追加描述路径</param>
    //    public static void SetResouceInform(string recordPath, string[] resourcePath, bool clear = true)
    //    {
    //#if UNITY_EDITOR
    //        var t = Resources.Load<PathInform>(recordPath);
    //        if (t == null)
    //        {
    //            t = ScriptableObject.CreateInstance<PathInform>();
    //            t.PathStrings.AddRange(resourcePath);
    //            AssetDatabase.CreateAsset(t, Path.Combine("Assets/Resources", recordPath + ".asset"));
    //        }
    //        else
    //        {
    //            if (clear)
    //            {
    //                t.PathStrings = new List<string>(resourcePath);
    //            }
    //            else
    //            {
    //                foreach (var v in resourcePath)
    //                {
    //                    if (!t.PathStrings.Contains(v)) t.PathStrings.Add(v);
    //                }
    //            }
    //        }
    //#endif
    //    }

    #endregion

    /// <summary>
    /// 设置写入结果的文件名称，与（多个）搜索路径
    /// </summary>
    /// <param name="recordPath">记录文件路径（具体到某个文件）</param>
    /// <param name="key">保存数据的key</param>
    /// <param name="resourcePath">数据目录，可以是文件夹</param>
    public static void WriteConfigData(string recordPath, string key, string[] resourcePath)
    {
        EasyConfig ec = EasyConfig.GetConfig(recordPath);
        ec.RemoveAt(key);
        ec.AddRange(key, resourcePath);
        ec.Write();
    }

    /// <summary>
    /// 获取指定路径下txt文件描述的的文件路径
    /// </summary>
    /// <returns></returns>
    public static string[] GetConfigData(string path, string key)
    {
        //PathInform pathsInform = Resources.Load<PathInform>(path);
        //if (pathsInform != null) return pathsInform.PathStrings.ToArray();
        EasyConfig ec = EasyConfig.GetConfig(path);
        return ec.GetDataList(key);
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
    #endregion

    /// <summary>
    /// 将一个从配置表读取的long翻译为指定意义。它返回一个数组，第一个元素是值（float）；
    /// 第二个元素如果为0代表这是一个数值，但如果为1代表这是一个对自己释放的属性枚举（其实是int），如果为2代表是对目标释放的属性
    /// </summary>
    public float[] GetValue(long v)
    {
        var t = BitConverter.GetBytes(v);
        float[] res = new float[2];//元素0是值; 元素1为0代表数值，为1代表是对自己的属性枚举，2代表是对目标的属性
        //表示这个是一个枚举(1)还是正数(2)或者负数(3)
        if (t[t.Length - 3] == 1)
        {
            res[0] = BitConverter.ToInt32(t, 0);
            res[1] = t[t.Length - 4] == 1 ? 1 : 2;
            return res;
        }
        int f = 1;
        if (t[t.Length - 3] == 3) f = -1;
        f = (int)(f * Math.Pow(10, t[t.Length - 4]));
        res[0] = (float)BitConverter.ToInt32(t, 0) / f;
        return res;
    }
}
