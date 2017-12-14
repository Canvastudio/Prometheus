using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGrid : Property
{
    public OpenGrid(LiveItem owner, StateConfig config, int index, PassiveSkillIns passive, LiveItem source) : base(owner, config, index, passive, source)
    {

    }

    public override void Active()
    {
        active = true;

        Messenger.AddListener(SA.OpenBrick, OnOpenBrick);
    }

    private void OnOpenBrick()
    {
        ApplyChange();
    }
}
