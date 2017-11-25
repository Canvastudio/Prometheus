using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : StateEffectIns
{
    public int range = 0;

    public Alarm(LiveItem owner, StateConfig config, int index, bool passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        range = Mathf.FloorToInt(stateConfig.stateArgs[index].f[0]);

        stateType = StateEffectType.Countdown;
    }

    protected override void Apply(object param)
    {
        Debug.LogError("Alarm 这个状态是自动触发.");
    }

    public override void OnOutData()
    {
        var bricks = BrickCore.Instance.GetNearbyBrick(owner.standBrick.row, owner.standBrick.column, range);

        for (int i = 0; i < bricks.Count; ++i)
        {
            if (bricks[i].item != null 
                && bricks[i].realBrickType == BrickType.MONSTER
                && (bricks[i].item as Monster).inViewArea)
            {
                var monster = bricks[i].item as Monster;
                monster.dangerousLevels = DangerousLevels.Hostility;
                monster.OnDiscoverd();
            }
        }
    }
}

 