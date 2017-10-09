using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : GameItemBase, IReactive {

    public BoxConfig config;

    public void Reactive()
    {
        Debug.Log("TODO: 开宝箱获得材料！");

        standBrick.brickType = BrickType.EMPTY;

        GameObject.Destroy(this.gameObject);
    }
}
