using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MuiSingleBase<T> : MuiBase where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            //if (_instance == null)
            //{
            //    var go = (GameObject.FindObjectOfType(typeof(T)) as GameObject);

            //    if (go != null)
            //    {
            //        _instance = go.GetComponent<T>();
            //    }
            //    else
            //    {
            //        _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
            //    }
            //}

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = GetComponent<T>();
        }
    }
}
