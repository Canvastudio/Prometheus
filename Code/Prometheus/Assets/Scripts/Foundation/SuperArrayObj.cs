using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// SuperArray 存储ConfigDataBase对象
/// </summary>
public class SuperArrayObj<T> : SuperArrayBase<T> where T : ConfigDataBase, new()
{

    private List<T> _datas;

    private List<T> Datas
    {
        get
        {
            if (_datas == null)
            {
                _datas = new List<T>();
                foreach (var key in dataList) _datas.Add(SuperConfig.Instance.GetConfigDataBykey<T>(key));
            }
            return _datas;
        }
    }

    public SuperArrayObj(string dataStr, string splitChar) : base(dataStr, splitChar) { }




    public override T this[params int[] index]
    {
        get
        {
            return Datas[DataIndex(index)];
        }
    }

    public override IEnumerator GetEnumerator()
    {
        return Datas.GetEnumerator();
    }

    public override T[] ToArray(params int[] index)
    {
        if (index.Length == 0) return Datas.ToArray();
        int[] temp = GetToArrIndexes(index);
        T[] res = new T[temp.Length];
        for (int i = 0; i < res.Length; i++) res[i] = Datas[temp[i]];
        return res;
    }

    public override List<T> ToList(params int[] index)
    {
        return new List<T>(ToArray(index));
    }

}
