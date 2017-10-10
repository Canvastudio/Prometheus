using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablet : GameItemBase {

    public TotemConfig config;

    private float round = 0;

    private void OnEnable()
    {
        Messenger<float>.AddListener(StageAction.StageTimeCast, AddRound);
    }

    public void Open()
    {
        switch (config.totemType)
        {
            case TotemType.Protect:
                break;
            case TotemType.Renew:
                break;
            case TotemType.Resurgence:
                break;
            case TotemType.Summon:
                break;
        }

    }

    public void Close()
    {

    }

    public void AddRound(float t)
    {
        round += t;
    }


}

