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
    private float[] weightList;
    private float total;
    private WeightSection() { }

    public int Count
    {
        get { return rateList.Length; }
    }

    public static WeightSection CreatePrimitive(int count)
    {
        WeightSection arg = new WeightSection() { rateList = new float[count] };

        float totle = 0;

        for (int i = 0; i < arg.rateList.Length; i++)
        {
            totle += 1f;
            arg.weightList[i] = 1f;
            arg.rateList[i] = totle;
        }

        arg.total = totle;

        return arg;
    }

    public static WeightSection Create(List<float> list)
    {
        WeightSection arg = new WeightSection { weightList = new float[list.Count] };
        arg.CalculateTotalAndRate();
        return arg;
    }

    private void CalculateTotalAndRate()
    {
        float totle = 0;

        for (int i = 0; i < rateList.Length; i++)
        {
            totle += weightList[i];
            rateList[i] = totle;
        }

        total = totle;
    }

    /// <summary>
    /// 根据事先封装好的比例数组，它将按比例返回一个落点所在区间，
    /// 比如传入数组是{2.5,2.5,5.0}，区间0与1的概率是25%，区间3的概率是50%
    /// </summary>
    /// <returns>落点所在区间，从0开始</returns>
    public int RanPoint()
    {
        float rad = Random.Range(0, total);
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
        return rateList[index] / total;
    }

    

    /// <summary>
    /// 检查权重是否超越边界值，如果超过则按照scale进行缩放
    /// </summary>
    /// <param name="bound"></param>
    /// <param name="sacle"></param>
    public WeightSection CheckBound(float bound = 1000f, float scale = 0.1f, float minWeight = 1f)
    {
        bool _outBound = false;

        for (int i = 0; i < rateList.Length - 1; ++i)
        {
            if (!_outBound)
            {
                if (rateList[i + 1] - rateList[i] > bound)
                {
                    _outBound = true;
                    i = 0;
                }

                if (rateList[i] < minWeight) rateList[i] = minWeight;
            }
            else
            {
                var temp = weightList[i] * scale;
                weightList[i] = temp < minWeight ? minWeight : temp;
            }
        }

        if (_outBound) CalculateTotalAndRate();

        return this;
    }

    /// <summary>
    /// 对除去制定的index之外的所有权重进行缩放
    /// </summary>
    /// <param name="exclude"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public WeightSection ScaleWeightExOne(int exclude, float scale = 2f)
    {
        for (int i = 0; i < weightList.Length; ++i)
        {
            if (i != exclude)
            {
                weightList[i] *= scale;
            }
        }

        CalculateTotalAndRate();

        return this;
    }
}
