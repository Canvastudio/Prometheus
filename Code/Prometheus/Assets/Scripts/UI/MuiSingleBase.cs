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
            return _instance;
        }
    }

    public override void Awake()
    {
        base.Awake();

        if (_instance == null)
        {
            _instance = GetComponent<T>();
        }
    }
}
