using System;
using System.Collections;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;


/// <summary>
/// SuperArray 存储普通类型（非类对象）
/// </summary>
public class SuperArrayValue<T> : SuperArrayBase<T>
{

    private readonly List<T> datas = new List<T>();

    public SuperArrayValue(string dataStr, string splitChar) : base(dataStr, splitChar)
    {
        foreach (var v in dataList)
        {
            datas.Add(InvalidValue(v));
        }
    }

    /// <summary>
    /// 将字符串转换成对应类型，不支持class类型
    /// </summary>
    private T InvalidValue(string str)
    {
        object returnObj = null;
        try
        {
            string dataStr = SuperTool.ConverSpace(str);
            if (!string.IsNullOrEmpty(dataStr))
            {
                //普通类型转换
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
                    //转换成枚举
                    returnObj = Enum.Parse(typeof(T), dataStr);
                }
            }
        }
        catch (Exception)
        {
            throw new ArgumentException("SuperArray赋值出错@ 把“" + str + "”转成<" + typeof(T) + ">??");
        }
        return (T)returnObj;
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

    public override T this[params int[] index]
    {
        get
        {
            return datas[DataIndex(index)];
        }
    }

    public override IEnumerator GetEnumerator()
    {
        return datas.GetEnumerator();
    }

    public override T[] ToArray(params int[] index)
    {
        if (index.Length == 0) return datas.ToArray();
        int[] temp = GetToArrIndexes(index);
        T[] res = new T[temp.Length];
        for (int i = 0; i < res.Length; i++) res[i] = datas[temp[i]];
        return res;
    }

    public override List<T> ToList(params int[] index)
    {
        return new List<T>(ToArray(index));
    }

}
