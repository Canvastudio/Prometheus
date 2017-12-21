using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Debug = UnityEngine.Debug;

public abstract class SuperArrayBase<T> : IEnumerable
{

    private readonly int maxDepth;
    private readonly string saveDataStr;

    private readonly Dictionary<string, int> countDictionary = new Dictionary<string, int>();
    private readonly Dictionary<string, int> indexDictionary = new Dictionary<string, int>();

    //表示元素位置的字符串，把dataStr类似“data1_data2_data3_data4;data5;data6”转成“0_1_2_3;4;5”
    private readonly string indexStr;

    //切分字符数组
    private readonly char[] splitCharArr;

    //存放的数据字符串
    protected readonly string[] dataList;


    protected SuperArrayBase(string dataStr, string splitChar)
    {
        saveDataStr = dataStr;
        splitCharArr = splitChar.ToCharArray();
        maxDepth = splitCharArr.Length;


        //把这个数组里的值转换后得到真正的类型
        dataList = Regex.Split(dataStr, @"[" + splitChar + "]");
        //制造序号字符串
        Regex reg = new Regex(@"[^" + splitChar + "]+");
        indexStr = reg.Replace(dataStr, "");
        Regex reg2 = new Regex(@"(?=[" + splitChar + "])");
        int index = 0;
        indexStr = reg2.Replace(indexStr, match => "" + index++) + index;
    }


    /// <summary>
    /// 返回数据所在的序号
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    protected int DataIndex(params int[] index)
    {
        if (maxDepth != index.Length) throw new ArgumentOutOfRangeException("SuperArray搜索深度不合法 → " + saveDataStr);
        string key = Union(index);
        if (indexDictionary.ContainsKey(key)) return indexDictionary[key];
        string tempStr = indexStr;
        try
        {
            for (int i = 0; i < index.Length; i++)
            {
                var t = tempStr.Split(splitCharArr[i]);
                tempStr = t[index[i]];
            }
        }
        catch (IndexOutOfRangeException)
        {
            throw new IndexOutOfRangeException("SuperArray查询元素下标越界 → " + saveDataStr);
        }
        int reslut = int.Parse(tempStr);
        indexDictionary.Add(key, reslut);
        return reslut;
    }


    /// <summary>
    /// 对应位置数组长度，如果参数为空，获取首次分割长度。
    /// 如果Count(0)，返回第1次分割后，第0号元素的长度；
    /// 如果Count(1,2)，返回第1次分割后的第1号元素，该元素进行第2次分割后的第2号元素的长度；
    /// </summary>
    public int Count(params int[] index)
    {
        if (index.Length > maxDepth - 1) throw new ArgumentOutOfRangeException("SuperArray查询个数深度不合法 → " + saveDataStr);
        string key = Union(index);
        if (countDictionary.ContainsKey(key)) return countDictionary[key];
        string tempStr = indexStr;
        try
        {
            for (int i = 0; i < index.Length; i++)
                tempStr = tempStr.Split(splitCharArr[i])[index[i]];
        }
        catch (IndexOutOfRangeException)
        {
            throw new IndexOutOfRangeException("SuperArray查询元素下标越界 → " + saveDataStr);
        }
        int reslut = tempStr.Split(splitCharArr[index.Length]).Length;
        countDictionary.Add(key, reslut);
        return reslut;
    }

    protected int[] GetToArrIndexes(params int[] index)
    {
        if (index.Length != maxDepth - 1)
        {
            throw new ArgumentOutOfRangeException("SuperArray数组转换深度不合法 → " + saveDataStr);
        }
        string tempStr = indexStr;
        try
        {
            for (int i = 0; i < index.Length; i++)
                tempStr = tempStr.Split(splitCharArr[i])[index[i]];
        }
        catch (IndexOutOfRangeException)
        {
            throw new IndexOutOfRangeException("SuperArray查询元素下标越界 → " + saveDataStr);
        }

        string[] temp = tempStr.Split(splitCharArr[index.Length]);
        int[] result=new int[temp.Length];
        for (int i = 0; i < temp.Length; i++) result[i] = int.Parse(temp[i]);
        return result;
    }


    /// <summary>
    /// 将int数组转换成字符串，方便储存字典
    /// </summary>
    private static string Union(int[] arr)
    {
        StringBuilder sb = new StringBuilder("k");
        foreach (var v in arr)
        {
            sb.Append(v);
            sb.Append(',');
        }
        return sb.ToString();
    }
    public override string ToString()
    {
        return saveDataStr;
    }

    public abstract T this[params int[] index] { get; }
    public abstract IEnumerator GetEnumerator();
    public abstract T[] ToArray(params int[] index);
    public abstract List<T> ToList(params int[] index);

}

