using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPiece : OrganBase
{
    public override void Reactive()
    {
        BrickCore.Instance.UnLockFire();

        Recycle();
    }
}
