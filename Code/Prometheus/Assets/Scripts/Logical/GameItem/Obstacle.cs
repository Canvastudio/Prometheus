using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : GameItemBase {

    /// <summary>
    /// 是否用被占用 现在设计为炮台可以占用
    /// </summary>
    public bool occupy = false;

    

    public override void Recycle()
    {
        base.Recycle();

        ObjPool<Obstacle>.Instance.RecycleObj(GameItemFactory.Instance.obstacle_pool, itemId);
    }

    Sprite oldSprite;

    public void SetAsFire()
    {
        string name = RuleBox.GetBlockLock(standBrick.row, standBrick.column);

        oldSprite = icon.sprite;

        icon.sprite = AtlasCore.Instance.GetSpriteFormAtlas("Stage", name);
    }

    public void SetAsNormal()
    {
        if (oldSprite != null)
        {
            icon.sprite = oldSprite;
        }
    }

}
