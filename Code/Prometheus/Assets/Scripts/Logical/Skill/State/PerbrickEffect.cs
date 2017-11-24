using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerbrickEffect : Property
{
    public PerbrickEffect(LiveItem owner, StateConfig config, int index, bool passive, LiveItem source) : base(owner, config, index, passive, source)
    {

    }

    public override void Active()
    {
        Messenger.AddListener(SA.DiscoverBrickChange, OnDiscoverBrickCountChange);

        base.Active();
    }

    private void OnDiscoverBrickCountChange()
    {
        ResetChange();
        ApplyChange();
    }

    public override void Deactive()
    {
        base.Deactive();
        Messenger.RemoveListener(SA.DiscoverBrickChange, OnDiscoverBrickCountChange);
    }

    public override void Remove()
    {
        base.Remove();

        Messenger.RemoveListener(SA.DiscoverBrickChange, OnDiscoverBrickCountChange);
    }

}
