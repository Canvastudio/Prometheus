using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ClickLimit : MonoBehaviour
{

    public static bool AlowClick
    {
        get
        {
            if (objectLocks.Count == 0)
            {
                return true;
            }
            else
            {
                List<object> deleteList = (from t in objectLocks where t.Value <= 0 select t.Key).ToList();
                foreach (var v in deleteList) objectLocks.Remove(v);
                return objectLocks.Count == 0;
            }
        }
    }

    private static readonly Dictionary<object, int> objectLocks = new Dictionary<object, int>();

    public static string Check
    {
        get
        {
            var t = AlowClick;
            StringBuilder sb = new StringBuilder();
            sb.Append(objectLocks.Count + "个限制：\n");
            foreach (var v in objectLocks)
            {
                sb.Append(v.Key + "," + v.Value + "\n");
            }
            return sb.ToString();
        }


    }

    public static void AddLock(object o, int count = 1)
    {
        if (count <= 0) return;
        if (o == null) throw new ArgumentException("AddLock对象为空");
        if (objectLocks.ContainsKey(o))
            objectLocks[o] += count;
        else
            objectLocks.Add(o, count);
    }

    /// <summary>
    /// 在通过绑定脚本达到锁定目的时，可以设置加锁和解锁的时机
    /// </summary>
    public LockMode lockMode;



    /// <summary>
    /// 如果是严格的解锁，那么必须解锁次数大于等级上锁次数才能解锁，否则只需要解锁一次
    /// </summary>
    /// <param name="o"></param>
    /// <param name="strict">如果是严格的解锁，那么必须解锁次数大于等级上锁次数才能解锁，否则只需要解锁一次</param>
    public static void UnLock(object o, bool strict = false)
    {
        if (o == null) throw new ArgumentException("UnLock对象为空");
        if (strict)
        {
            if (objectLocks.ContainsKey(o))
            {
                objectLocks[o]--;
            }
            else
                objectLocks.Add(o, -1);
        }
        else
        {
            if (objectLocks.ContainsKey(o))
            {
                objectLocks.Remove(o);
            }
        }
    }


    void Start()
    {
        if (lockMode == LockMode.StartToDestroy)
            AddLock(this);
    }

    void OnDestroy()
    {
        if (lockMode == LockMode.StartToDestroy)
            UnLock(this);
    }

    void OnEnable()
    {
        if (lockMode == LockMode.EnableToDisable)
            AddLock(this);
    }



    void OnDisable()
    {
        if (lockMode == LockMode.EnableToDisable)
            UnLock(this);
    }
}

public enum LockMode
{
    StartToDestroy,
    EnableToDisable,
}