using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablet : GameItemBase {

    public TotemConfig config;

    [SerializeField]
    private float round = 0;

    public bool inViewArea = false;

#if UNITY_EDITOR
    [SerializeField]
    TotemType type;
#endif

    /// <summary>
    /// 起效的回合数
    /// </summary>
    [SerializeField]
    private float activeRound = int.MaxValue;

    /// <summary>
    /// 效果整形参数1
    /// </summary>
    int int_arg1 = 0;

    public void AddRound(float t)
    {
        if (inViewArea)
        {
            round += t;

            if (round > activeRound)
            {
                round -= activeRound;
                TakeEffect();
            }
        }
    }

    public void TakeEffect()
    {
        Debug.Log("石碑发动: " + gameObject.name + ",type: " + config.totemType.ToString());

        switch (config.totemType)
        {
            case TotemType.Protect:
                break;
        }
    }

    public override void OnDiscoverd()
    {
        base.OnDiscoverd();

        isDiscovered = true;

        Messenger<float>.AddListener(StageAction.StageTimeCast, AddRound);

        switch (config.totemType)
        {
            case TotemType.Protect:
                activeRound = int.Parse(config.arg);
                break;
            case TotemType.Summon:
                string[] args = config.arg.Split('_');
                activeRound = int.Parse(args[0]);
                int_arg1 = int.Parse(args[1]);
                break;
            case TotemType.Resurgence:
                activeRound = int.Parse(config.arg);
                break;
            case TotemType.Renew:
                activeRound = int.Parse(config.arg);
                break;
        }
    }
}

