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
            return Mathf.FloorToInt(result);
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
            return 0;
        }
    }

    //public PropertyData SetIntProperty(GameProperty id, int value)
    //{
    //    data[id] = value;

    //    return this;
    //}

    public PropertyData SetFloatProperty(GameProperty id, float value)
    {
        data[id] = value;

        if (changeCallback != null)
        {
            changeCallback.Invoke(id);
        }

        return this;
    }

}
