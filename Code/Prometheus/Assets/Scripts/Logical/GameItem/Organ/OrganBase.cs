using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrganBase : GameItemBase {

    public OperateConfig baseConfig;

    private void Awake()
    {
        brickBtn = transform.GetChild(0).GetOrAddComponet<Button>();
    }

    public virtual void Reactive()
    {
        Debug.Log("激活机关: " + baseConfig.name);
    }

    public override void Recycle()
    {
        base.Recycle();

        ResetValues();
        standBrick.brickType = BrickType.EMPTY;
        standBrick.item = null;
        GameObject.Destroy(this.gameObject);
    }
}
