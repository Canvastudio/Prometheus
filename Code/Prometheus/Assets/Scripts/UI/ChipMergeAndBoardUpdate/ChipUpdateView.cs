﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipUpdateView : MuiSingleBase<ChipUpdateView> {

    public ChipMerge chipMerge;
    public string optionName = "OPN";

    public override IEnumerator Close(object param)
    {
        throw new System.NotImplementedException();
    }

    public override void Hide(object param)
    {

    }

    public override IEnumerator Init(object param)
    {
        chipMerge.Init();

        gameObject.SetActive(false);

        return null;
    }

    public override IEnumerator Open(object param)
    {
        gameObject.SetActive(true);

        return null;
    }
}
