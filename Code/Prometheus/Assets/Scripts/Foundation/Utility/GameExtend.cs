﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameExtend  {

    public static T GetOrAddComponet<T>(this Component component) where T: Component
    {
        T res;

        if ((res = component.GetComponent<T>()) == null)
        {
            return component.gameObject.AddComponent<T>();
        }
        else
        {
            return res;
        }
    }

    public static RectTransform Rt(this GameObject go)
    {
        return ((RectTransform)go.transform);
    }

    public static RectTransform Rt(this Transform transform)
    {
        return ((RectTransform)transform);
    }
    /// <summary>
    /// 不推荐, 由于Enum和enum的特殊关系，会产生GC
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public static int ToInt(this System.Enum e)
    {
        return System.Convert.ToInt32(e);
    }

    public static WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
}
