using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

/// <summary>
/// 将一个字符串多次切分保存为ArrayList，
/// 使用多维下标方式取得数据，例如superArray[0,1,2]，只读，
/// 下标长度与切割字符串长度一致。
/// </summary>
public class SuperArrayValue<T> : IEnumerable
{
    private readonly ArrayList dataList;
    private readonly int maxDepth;
    private readonly bool isArr;
    private readonly Dictionary<string, int> countDictionary;
    private readonly Dictionary<string, T> valueDictionary;
    private readonly string saveDataStr;

    public SuperArrayValue(string dataStr, string splitChar, int depth = -1)
    {
        saveDataStr = dataStr;
        countDictionary = new Dictionary<string, int>();
        valueDictionary = new Dictionary<string, T>();
        maxDepth = splitChar.Length;
        object t = SuperSplit(dataStr, splitChar, depth);
        var list = t as ArrayList;
        if (list != null)
        {
            dataList = list;
            isArr = true;
        }
        else
        {
            dataList = new ArrayList { t };
            isArr = false;
        }
        FormatArray(dataList);
    }

    /// <summary>
    /// 对应位置数组长度，如果参数为空，获取首次分割长度。
    /// 如果Count(0)，返回第1次分割后，第0号元素的长度；
    /// 如果Count(1,2)，返回第1次分割后的第1号元素，该元素进行第2次分割后的第2号元素的长度；
    /// </summary>
    public int Count(params int[] index)
    {
        if (index.Length > maxDepth) throw new ArgumentOutOfRangeException("SuperArray搜索深度超过最大深度");
        string key = Union(index);
        if (countDictionary.ContainsKey(key)) return countDictionary[key];
        if (isArr)
        {
            object temp = dataList;
            for (int i = 0; i < index.Length; i++)
            {
                if (((ArrayList)temp)[index[i]] is ArrayList)
                {
                    temp = ((ArrayList)temp)[index[i]];
                }
                else
                {
                    //在temp[i]不是arraylist的情况下，搜索的不是最后一个下标，那么表示使用者认为该元素还在指向arraylist，与真实情况不符
                    if (i == index.Length - 1)
                    {
                        countDictionary.Add(key, 1);
                        return 1;
                    }
                    else throw new ArgumentOutOfRangeException("SuperArray元素不存在");
                }
            }
            int t = ((ArrayList)temp).Count;
            countDictionary.Add(key, t);
            return t;
        }
        else
        {
            countDictionary.Add(key, 1);
            return 1;
        }
    }

    /// <summary>
    /// 将int数组转换成字符串，方便储存字典
    /// </summary>
    private static string Union(int[] arr)
    {
        StringBuilder sb = new StringBuilder("k");
        foreach (var v in arr) sb.Append(v);
        return sb.ToString();
    }

    /// <summary>
    /// 通过下标返回相应List，如果参数为空，表示这是一个一维数组
    /// 如果ToList(0)，返回第1次分割后，第0号元素的数组；
    /// 如果Count(1,2)，返回第1次分割后的第1号元素，该元素进行第2次分割后的第2号元素的数组；
    /// </summary>
    public List<T> ToList(params int[] index)
    {
        return new List<T>(ToArray(index));
    }

    /// <summary>
    /// 通过下标返回相应数组，如果下标为空，表示这是一个一维数组
    /// 如果ToList(0)，返回第1次分割后，第0号元素的数组；
    /// 如果Count(1,2)，返回第1次分割后的第1号元素，该元素进行第2次分割后的第2号元素的数组；
    /// </summary>
    public T[] ToArray(params int[] index)
    {
        T[] t;
        if (isArr)
        {
            if (index.Length == 0)
            {
                if (maxDepth != 1) throw new ArgumentException("SuperArray搜索深度与内容不符");
                t = new T[dataList.Count];
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i] is ArrayList) throw new ArgumentException("SuperArray无法转换为数组");
                    t[i] = (T)dataList[i];
                }
                return t;
            }
            else
            {
                if (maxDepth <= index.Length) throw new ArgumentOutOfRangeException("SuperArray搜索深度超过最大深度");
                ArrayList temp = dataList;
                foreach (var v in index)
                {
                    var list = temp[v] as ArrayList;
                    if (list != null)
                        temp = list;
                    else
                    {
                        object o = temp[v];
                        temp = new ArrayList { o };
                        break;
                    }
                }

                t = new T[temp.Count];
                for (int i = 0; i < temp.Count; i++)
                {
                    if (temp[i] is ArrayList) throw new ArgumentException("SuperArray无法转换为数组");
                    t[i] = (T)temp[i];
                }
            }
        }
        else
        {
            if (!(index.Length == 0 || (index.Length == 1 && index[0] == 0))) throw new ArgumentException("SuperArray单元素");
            t = new[] { InvalidValue("" + dataList[0]) };

        }
        return t;
    }

    /// <summary>
    /// 通过下标获得值，下标长度与切割字符串长度一致
    /// </summary>
    public T this[params int[] index]
    {
        get
        {
            if (maxDepth < index.Length) throw new ArgumentOutOfRangeException("SuperArray查询深度超过最大深度");
            string key = Union(index);
            if (valueDictionary.ContainsKey(key)) return valueDictionary[key];
            ArrayList temp = dataList;
            foreach (var v in index)
            {
                if (v >= temp.Count) throw new ArgumentOutOfRangeException("SuperArray查询越界");
                var list = temp[v] as ArrayList;
                if (list != null)
                {
                    temp = list;
                }
                else
                {
                    T t = (T)temp[v];
                    valueDictionary.Add(key, t);
                    return t;
                }
            }
            throw new ArgumentException("SuperArray下标错误：查询深度与切割字符串长度通常是一致的（除非改变过默认depth）");
        }
    }

    /// <summary>
    /// 将字符串的ArrayList转换为对应数据类型
    /// </summary>
    private void FormatArray(ArrayList datas)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            var list = datas[i] as ArrayList;
            if (list != null)
                FormatArray(list);
            else
                datas[i] = InvalidValue((string)datas[i]);
        }
    }



    /// <summary>
    /// 将字符串转换成对应类型，不支持class类型
    /// </summary>
    private T InvalidValue(string val)
    {
        object returnObj = null;
        try
        {
            string dataStr = SuperTool.ConverSpace(val);
            if (!string.IsNullOrEmpty(dataStr))
            {
                if (typeof(T) == typeof(string))
                {
                    returnObj = dataStr;
                }
                else if (typeof(T) == typeof(float))
                {
                    returnObj = float.Parse(dataStr);
                }
                else if (typeof(T) == typeof(int))
                {
                    returnObj = int.Parse(dataStr);
                }
                else if (typeof(T) == typeof(ulong))
                {
                    returnObj = ulong.Parse(dataStr);
                }
                else if (typeof(T) == typeof(bool))
                {
                    returnObj = ConverBool(dataStr);
                }
                else if (typeof(T) == typeof(double))
                {
                    returnObj = double.Parse(dataStr);
                }
                else if (typeof(T) == typeof(long))
                {
                    returnObj = long.Parse(dataStr);
                }
                else if (typeof(T) == typeof(short))
                {
                    returnObj = short.Parse(dataStr);
                }
                else if (typeof(T) == typeof(char))
                {
                    returnObj = char.Parse(dataStr);
                }
                else if (typeof(T) == typeof(uint))
                {
                    returnObj = uint.Parse(dataStr);
                }
                else if (typeof(T) == typeof(byte))
                {
                    returnObj = byte.Parse(dataStr);
                }
                else
                {
                    returnObj = Enum.Parse(typeof(T), dataStr);
                }
            }
        }
        catch (Exception)
        {
            throw new ArgumentException("SuperArray赋值出错@" + val);
        }
        return (T)returnObj;
    }

    /// <summary>
    /// 递归将字符串切分，存入ArrayList
    /// 如果是可以继续切割的内容，存入的ArrayList每个元素代表一个ArrayList，否则存入值，
    /// </summary>
    /// <param name="dataStr">数据字符串</param>
    /// <param name="splitChar">切割字符串</param>
    /// <param name="depth">最大深度，不指定的话，按照splitChar挨个切</param>
    /// <returns></returns>
    private object SuperSplit(string dataStr, string splitChar, int depth = -1)
    {
        if (splitChar.Length == 0 || depth == 0) return dataStr;
        var resultList = dataStr.Contains(splitChar[0]) ? new ArrayList(dataStr.Split(splitChar[0])) : new ArrayList { dataStr };
        string newSplit = splitChar.Remove(0, 1);
        for (int i = 0; i < resultList.Count; i++)
            resultList[i] = SuperSplit((string)resultList[i], newSplit, depth - 1);
        return resultList;
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

    public override string ToString()
    {
        return saveDataStr;
    }

    private SuperArrayIEnum sai;
    IEnumerator IEnumerable.GetEnumerator()
    {
        if (sai == null) sai = new SuperArrayIEnum(dataList);
        else sai.Reset();
        return sai;
    }

    /// <summary>
    /// 这是一个内部类：为了实现foreach必须现实该接口
    /// </summary>
    public class SuperArrayIEnum : IEnumerator
    {
        private int checkIndex;
        private readonly List<T> usefulDatas;


        public SuperArrayIEnum(ArrayList data)
        {
            usefulDatas = new List<T>();
            InitData(data);
        }

        private void InitData(ArrayList datas)
        {
            foreach (var v in datas)
            {
                var list = v as ArrayList;
                if (list != null) InitData(list);
                else usefulDatas.Add((T)v);
            }
        }


        public bool MoveNext()
        {
            if (checkIndex < usefulDatas.Count)
            {
                Current = usefulDatas[checkIndex++];
                return true;
            }
            return false;
        }

        public void Reset()
        {
            checkIndex = 0;
        }

        public object Current { get; private set; }
    }

}
