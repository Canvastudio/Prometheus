using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maintenance : GameItemBase
{
    public int upTime = 0;

    public override void Recycle()
    {
        base.Recycle();
        upTime = 0;
        ObjPool<Maintenance>.Instance.RecycleObj(GameItemFactory.Instance.maintenance_pool, itemId);
    }


    public void Reactive()
    {
        MuiCore.Instance.Open(UiName.strChipUpdateView, this);
    }
}
