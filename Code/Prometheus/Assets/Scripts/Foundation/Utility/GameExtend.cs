using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

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

    public static T GetValueFromeDic<T>(this Dictionary<string, T> dic, string key)
    {
        T result;
        if (dic.TryGetValue(key, out result))
        {
            return result;
        }
        else return default(T);
        
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

 

    public static void SetParentAndNormalize(this Component com, Transform parent)
    {
        com.gameObject.SetActive(true);
        com.transform.SetParent(parent);
        com.transform.localScale = Vector3.one;
        com.transform.localPosition = Vector3.zero;
    }

    public static Coroutine ExStartCoroutine(this MonoBehaviour mono, IEnumerator routine)
    {
        if (routine != null)
        {
            return mono.StartCoroutine(routine);
        }
        else
        {
            return null;
        }
    }

    public static void Clean(this StringBuilder sb)
    {
        sb.Remove(0, sb.Length);
    }

    public static void FloatText(this Text text, float value)
    {
        if (GameTestData.Instance.infoDetail)
        {
            text.text = value.ToString();
        }
        else
        {
            text.text = Mathf.CeilToInt(value).ToString();
        }
    }
}

public class GameGlobalVariable 
{
    public static WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    public static WaitForSeconds wait0_3s = new WaitForSeconds(0.3f);
}

