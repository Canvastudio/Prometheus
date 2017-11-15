using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopTipItem : MonoBehaviour {

    public int uid;

    [SerializeField]
    Text tipText;
    string pname = "PTI";
    float duration;
    bool show;

    public void Set(int id, float _duration = 0.8f, string text = null)
    {
        if (text == null)
        {
            tipText.text = "没有配置...";
        }
        else
        {
            tipText.text = text;
        }

        uid = id;
        duration = _duration;
        show = true;
    }

    private void Update()
    {
        if (show)
        {
            duration -= Time.deltaTime;

            if (duration < 0)
            {
                show = false;
                gameObject.SetActive(false);
                ObjPool<PopTipItem>.Instance.RecycleObj(pname, uid);

            }
        }
    }
}
