﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateItem : MonoBehaviour {

    [SerializeField]
    Text describe;
    [SerializeField]
    Text stateName;
    [SerializeField]
    Text stateDuration;
    [SerializeField]
    Image stateIcon;

    public void ShowStateInfo(StateIns ins)
    {
        describe.text = ins.stateConfig.describe;
        stateName.text = ins.stateConfig.name;
        stateIcon.SetStateIcon(ins.stateConfig.icon);

        if (ins.passive != null)
        {
            stateDuration.text = "∞";
        }
        else
        {
            stateDuration.text = ins.stateConfig.time.ToString();
        }
    }
}