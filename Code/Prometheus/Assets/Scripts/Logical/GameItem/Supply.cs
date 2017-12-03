using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supply : GameItemBase, IReactive {

    public SupplyConfig config;

    public void Reactive()
    {
        Debug.Log(string.Format("TODO: 回复血量: {0}！", config.arg));

        StageCore.Instance.Player.AddHpPercent(float.Parse(config.arg));

        standBrick.brickType = BrickType.EMPTY;

        Recycle();
    }

    public override void Recycle()
    {
        base.Recycle();

        ObjPool<Supply>.Instance.RecycleObj(GameItemFactory.Instance.supply_pool, itemId);
    }
}
