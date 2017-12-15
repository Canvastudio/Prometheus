using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class OrganBase : GameItemBase {

    public OperateConfig baseConfig;

    private void Awake()
    {
        brickBtn = transform.GetChild(0).GetComponent<Button>();
    }

    public abstract void Reactive();

    public void Clean()
    {
        standBrick.brickType = BrickType.EMPTY;
        standBrick.item = null;
        GameObject.Destroy(this.gameObject);
    }
}
