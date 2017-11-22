using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// 资源预读，一个游戏只能有一个该对象
/// </summary>
public class SuperResource : SingleObject<SuperResource>
{
    private readonly Dictionary<string, Object> resDic = new Dictionary<string, Object>();//构建的对象池
    private readonly List<ResourceRequest> resDatas = new List<ResourceRequest>();
    private int resProgress;
    public int ResProgress
    {
        get
        {
            return resProgress;
        }
    }
    private bool isDone;
    /// <summary>
    /// 检查这个来判断读取是否完成
    /// </summary>
    public bool IsDone
    {
        get
        {
            return isDone;
        }
    }


    protected override void Init() { }

    /// <summary>
    /// 读取传入路径的prefab，如果不传递参数，自动读取Resources/LoadPath/prefabPath.asset
    /// 异步读取，IsDone与resProgress分别表示读取是否完成与读取进度
    /// </summary>
    public void LoadAsync(string[] names = null)
    {
        isDone = false;
        string[] check;
        if (names == null)
        {
            EasyConfig ec = EasyConfig.GetConfig("DefaultPath");
            check = ec.GetDataArray("Prefab");
        }
        else
        {
            check = names;
        }
        foreach (string t in check)
        {
            ResourceRequest data = Resources.LoadAsync<Object>(t);
            resDatas.Add(data);
        }
        SuperTimer.Instance.RegisterFrameFunction(CheckRes);
    }

    private bool CheckRes(object obj)
    {
        isDone = true;
        resProgress = 0;
        foreach (ResourceRequest t in resDatas)
        {
            if (t.isDone)
            {
                if (!resDic.ContainsKey(t.asset.name)) resDic.Add(t.asset.name, t.asset);
            }
            else
            {
                isDone = false;
            }
            resProgress += (int)(t.progress * 100);
        }
        resProgress = resProgress / resDatas.Count;

        if (isDone)
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// 获取并创建一个GameObject舞台对象(已经实例化)
    /// </summary>
    public GameObject GetInstance(string name)
    {
        try
        {
            return Object.Instantiate(resDic[name] as GameObject);
        }
        catch (Exception)
        {
            Debug.LogError("资源获取出错，是不是路径有错？  @ " + name);
            throw;
        }
    }


    /// <summary>
    /// 获取一个GameObject对象（未实例化）
    /// </summary>
    public GameObject GetObject(string name)
    {
        try
        {
            GameObject gb = resDic[name] as GameObject;
            return gb;
        }
        catch (Exception)
        {
            Debug.LogError("资源获取出错，是不是路径有错？  @ " + name);
            throw;
        }
    }


    /// <summary>
    /// 移除所有资源
    /// </summary>
    public void Clear()
    {
        isDone = false;
        resDic.Clear();
    }

}
