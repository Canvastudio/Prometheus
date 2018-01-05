using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPiece : OrganBase
{
    public int c;

    public override void Reactive()
    {
        BrickCore.Instance.UnLockFire(c);

        Recycle();
    }
}
