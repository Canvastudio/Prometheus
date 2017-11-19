using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maintenance : GameItemBase{

    public override void Recycle()
    {
        base.Recycle();

        ObjPool<Maintenance>.Instance.RecycleObj(GameItemFactory.Instance.maintenance_pool, itemId);
    }


}
