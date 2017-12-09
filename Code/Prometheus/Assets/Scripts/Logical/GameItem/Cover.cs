using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : GameItemBase {

    public override IEnumerator OnDiscoverd()
    {
        Debug.Log("去除Cover: " + gameObject.name);
        Recycle();
        return base.OnDiscoverd();
    }

    public override void Recycle()
    {
        base.Recycle();
        standBrick.cover = null;
        standBrick = null;
        ObjPool<Cover>.Instance.RecycleObj(GameItemFactory.Instance.cover_pool, itemId);
    }
}
