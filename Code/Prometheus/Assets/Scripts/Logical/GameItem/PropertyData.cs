using System;
using System.Collections.Generic;
using UnityEngine;

public class PropertyData
{
    protected Dictionary<GameProperty, float> data = new Dictionary<GameProperty, float>();

    public Callback<GameProperty> changeCallback;

    public float this[GameProperty gameProperty]
    {
        get
        {
            float result;

            if (data.TryGetValue(gameProperty, out result))
            {
                return result;
            }
            else
            {
                return 1;
            }
        }
    }

    public int GetIntProperty(GameProperty id)
    {
        float result;

        if (data.TryGetValue(id, out result))
        {
            return Mathf.CeilToInt(result);
        }
        else
        {
            return 0;
        }
    }

    public float GetFloatProperty(GameProperty id)
    {
        float result;

        if (data.TryGetValue(id, out result))
        {
            return result;
        }
        else
        {
            Debug.LogError("尝试获取不存在的属性： " + id.ToString());
            return 0;
        }
    }


    //public PropertyData SetIntProperty(GameProperty id, int value)
    //{
    //    data[id] = value;

    //    return this;
    //}

    public PropertyData SetFloatProperty(GameProperty id, float value, bool callback = true)
    {
        if (id == GameProperty.nhp)
        {
            value = Mathf.Min(data[GameProperty.mhp], value);
        }
        else if (id == GameProperty.mhp)
        {
            if (data.ContainsKey(GameProperty.nhp))
            {
                data[GameProperty.nhp] = Math.Min(data[GameProperty.nhp], value);
            }
        }

        data[id] = value;

        if (changeCallback != null && callback)
        {
            changeCallback.Invoke(id);
        }

        return this;
    }

}
