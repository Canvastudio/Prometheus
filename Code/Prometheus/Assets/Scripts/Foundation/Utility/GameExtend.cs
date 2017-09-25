using System.Collections;
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
}
