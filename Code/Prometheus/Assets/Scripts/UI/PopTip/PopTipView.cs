using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopTipView : SingleGameObject<PopTipView> {

    [SerializeField]
    PopTipItem tipItem;

    string pname = "PTI";

    protected override void Init()
    {
        base.Init();
        ObjPool<PopTipItem>.Instance.InitOrRecyclePool(pname, tipItem);
    }

    public void Show(string text = null, float _duration = 0.8f)
    {
        ulong _id;
        var item = ObjPool<PopTipItem>.Instance.GetObjFromPoolWithID(out _id, pname);
        item.SetParentAndNormalize(this.transform);
        item.transform.localPosition = new Vector3(0f, 275f, 0);
        item.gameObject.SetActive(true);
        if (text != null)
        {
            text = TipsConfig.GetConfigDataByKey<TipsConfig>(text).text;
        }
        item.Set(_id, _duration, text); 
    }
}
