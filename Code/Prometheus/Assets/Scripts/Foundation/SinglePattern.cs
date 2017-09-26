using System;
using UnityEngine;

public interface ISingleHandler
{
    void CleanData();
    void ResetData();
}

/// <summary>
/// 继承该类以实现单例模式，使用Init代替new进行初始化
/// </summary>
public class SingleObject<T> : ISingleHandler where T : new() 
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();

                SingleManager.RegisterSingle(_instance as ISingleHandler);
            }
            return _instance;
        }
    }

    protected SingleObject()
    {
        Init();
    }

    /// <summary>
    /// 单利的初始化方法，代替new
    /// </summary>
    protected virtual void Init() { }
    public virtual void CleanData() { }
    public virtual void ResetData() { CleanData(); Init(); }
}



/// <summary>
/// MonoBehaviour的单例模式，使用Init替代Awake
/// </summary>
public abstract class SingleGameObject<T> : MonoBehaviour, ISingleHandler where T: Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                //if (_instance == null) throw new Exception("\n1. "+typeof(T) + "没有绑定GameObject   2. " + typeof(T)+"中使用了Awake");
                var go = (GameObject.FindObjectOfType(typeof(T)) as GameObject);
                
                if (go != null)
                {
                    _instance = go.GetComponent<T>();
                }
                else
                {
                    _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                }

                DontDestroyOnLoad(go);

                SingleManager.RegisterSingle(_instance as ISingleHandler);
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = GetComponent<T>();

            SingleManager.RegisterSingle(_instance as ISingleHandler);

            Init();
        }
    }

    /// <summary>
    /// 单利的初始化方法，代替new
    /// </summary>
    protected virtual void Init() { }
    public virtual void CleanData() { }
    public virtual void ResetData() { CleanData();Init(); }

}