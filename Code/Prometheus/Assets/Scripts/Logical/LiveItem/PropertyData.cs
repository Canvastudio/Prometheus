using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyData
{
    protected Dictionary<string, float> data = new Dictionary<string, float>();

    public int GetIntProperty(string propertyName)
    {
        float result;

        if (data.TryGetValue(propertyName, out result))
        {
            return Mathf.FloorToInt(result);
        }
        else
        {
            return 0;
        }
    }

    public float GetFloatProperty(string propertyName)
    {
        float result;

        if (data.TryGetValue(propertyName, out result))
        {
            return result;
        }
        else
        {
            return 0;
        }
    }

    public PropertyData SetIntProperty(string propertyName, int value)
    {
        data[propertyName] = value;

        return this;
    }

    public PropertyData SetFloatProperty(string propertyName, float value)
    {
        data[propertyName] = value;

        return this;
    }
}
