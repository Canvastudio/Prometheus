using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaloPassive : PassiveBase
{
    int range = 0;

    List<LiveItem> affected_item = new List<LiveItem>(10);
    StateConfig StateConfig = null;

    bool enemy = false;

    public HaloPassive(PassiveSkillsConfig config, int index, FightComponet fightComponet) : base(config, index, fightComponet)
    {
        range = Mathf.FloorToInt(config.passiveSkillArgs[index].f[0]);
        enemy = config.passiveSkillArgs[index].b[0];
        StateConfig = ConfigDataBase.GetConfigDataById<StateConfig>(config.passiveSkillArgs[index].u[0]);
    }

    private void CheckScopeAndAddState(float a)
    {
        List<Brick> bricks = BrickCore.Instance.GetNearbyBrick(fightComponet.ownerObject.standBrick.row, fightComponet.ownerObject.standBrick.column, range);
        List<LiveItem> list = new List<LiveItem>(10);

        for (int i = bricks.Count - 1; i >= 0; --i)
        {
            var item = bricks[i].item;

            if (item)
            {
                if (enemy)
                {
                    if (item is Monster && !((item as Monster).enslave))
                    {
                        list.Add(item as LiveItem);
                    }
                }
                else
                {
                    if (item is Player || (item is Monster && ((item as Monster).enslave)))
                    {
                        list.Add(item as LiveItem);
                    }
                }
            }
        }

        for (int i = affected_item.Count - 1; i >= 0; --i)
        {
            if (!list.Contains(affected_item[i]))
            {
                affected_item[i].RemoveHaloBuff(StateConfig);
                affected_item.RemoveAt(i);
            }
            else
            {
                list.Remove(affected_item[i]);
            }
        }

        for (int i = 0; i < list.Count; ++i)
        {
            affected_item.Add(list[i]);
            list[i].AddHaloBuff(StateConfig);
        }
    }

    public override void Apply()
    {
        Messenger<float>.AddListener(SA.StageTimeCast, CheckScopeAndAddState);

        CheckScopeAndAddState(999);
    }

    public override void Remove()
    {
        Messenger<float>.RemoveListener(SA.StageTimeCast, CheckScopeAndAddState);

        for (int i = 0; i < affected_item.Count; ++i)
        {
            affected_item[i].RemoveHaloBuff(StateConfig);
        }
    }
}
