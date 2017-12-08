using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : GameItemBase {

    public Brick coverBrick;

    public override IEnumerator OnDiscoverd()
    {
        Debug.Log("去除Cover: " + gameObject.name);
        Recycle();
        return base.OnDiscoverd();
    }

    public override void Recycle()
    {
        base.Recycle();

        ObjPool<Cover>.Instance.RecycleObj(GameItemFactory.Instance.cover_pool, itemId);
    }
}
