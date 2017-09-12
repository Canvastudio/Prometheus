using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 将一个比例数组转换并封装，内部会把它转换成float，所以更高的精度是无效的
/// </summary>
public class WeightSection
{
    private float[] rateList;
    private float totle;
    private WeightSection() { }

    public int Count
    {
        get { return rateList.Length; }
    }

    public static WeightSection Create(List<float> list)
    {
        WeightSection arg = new WeightSection { rateList = new float[list.Count] };
        float totle = 0;
        for (int i = 0; i < arg.rateList.Length; i++)
        {
            totle += list[i];
            arg.rateList[i] = totle;
        }
        arg.totle = totle;
        return arg;
    }


    /// <summary>
    /// 根据事先封装好的比例数组，它将按比例返回一个落点所在区间，
    /// 比如传入数组是{2.5,2.5,5.0}，区间0与1的概率是25%，区间3的概率是50%
    /// </summary>
    /// <returns>落点所在区间，从0开始</returns>
    public int RanPoint()
    {
        float rad = Random.Range(0, totle);
        for (int j = 0; j < rateList.Length; j++)
            if (rad < rateList[j]) return j;
        Debug.LogError("区间随机异常");
        throw new Exception();
    }

    /// <summary>
    /// 根据事先封装好的比例数组，它将按序列返回一个归一化的值，
    /// 比如传入数组是{1,2,3}，normalizeNum(0)将返回1/6
    /// </summary>
    public float NormalizeNum(int index)
    {
        if (index < 0 || index >= rateList.Length)
            throw new ArgumentOutOfRangeException("WeightSection数组越界");
        return rateList[index] / totle;
    }
}
