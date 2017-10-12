using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 一对一的互相映射，双字典简单实现
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="K"></typeparam>
public class Map<T, K> {

	public Dictionary<T, K> mapT = new Dictionary<T, K>();
    public Dictionary<K, T> mapK = new Dictionary<K, T>();

    public K this[T t]
    {
        get
        {
            return mapT[t];
        }
        set
        {
            mapT[t] = value;
        }
    }

    public T this[K k]
    {
        get
        {
            return mapK[k];
        }
        set
        {
            mapK[k] = value;
        }
    }

    public void Add(T t, K k)
    {
        mapT.Add(t, k);
        mapK.Add(k, t);
    }

    public void Remove(T t)
    {
        K k = mapT[t];
        mapK.Remove(k);
        mapT.Remove(t);
    }

    public bool ContainsKey(T t)
    {
        return mapT.ContainsKey(t);
    }

    public bool ContainsKey(K k)
    {
        return mapK.ContainsKey(k);
    }


}