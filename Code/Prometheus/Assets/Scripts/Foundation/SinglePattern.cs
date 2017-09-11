using System;
using UnityEngine;

/// <summary>
/// 继承该类以实现单例模式，使用Init代替new进行初始化
/// </summary>
public abstract class SingleObject<T> where T : new()
{
    private static bool alow;
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                alow = true;
                _instance = new T();
            }
            return _instance;
        }
    }

    protected SingleObject()
    {
        if (!alow) throw new ArgumentException("单利模式不能使用new，使用Instance访问唯一对象\nError@" + typeof(T));
        alow = false;
        Init();
    }
    /// <summary>
    /// 单利的初始化方法，代替new
    /// </summary>
    protected abstract void Init();
}



/// <summary>
/// MonoBehaviour的单例模式，使用Init替代Awake
/// </summary>
public abstract class SingleGameObject<T> : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            //if (_instance == null) throw new Exception("\n1. "+typeof(T) + "没有绑定GameObject   2. " + typeof(T)+"中使用了Awake");
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null) throw new Exception("出现多个对象\nError@" + typeof(T));
        _instance = GetComponent<T>();
        Init();
    }

    /// <summary>
    /// 单利的初始化方法，代替new
    /// </summary>
    protected abstract void Init();

}