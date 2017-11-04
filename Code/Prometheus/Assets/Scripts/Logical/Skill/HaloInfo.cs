using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 光环信息
/// </summary>
public class HaloInfo  {

    static int _id;
    /// <summary>
    /// 影响到的方块
    /// </summary>
    List<Brick> effect_bricks;

    int range = 0;
    bool enemy = false;
    public PassiveSkillIns passive;
    public int id;
    public LiveItem owner;
    int side;

    public HaloInfo(int _range, int _side, LiveItem _owner, PassiveSkillIns _passive)
    {
        passive = _passive;
        side = _side;
        range = _range;
        owner = _owner;

        effect_bricks = BrickCore.Instance.GetNearbyBrick(owner.standBrick, range);

        foreach(var brick in effect_bricks)
        {
            brick.halo_list.Add(this);

            if (brick.item != null)
            {
                if (brick.item is LiveItem)
                {
                    LiveItem live = brick.item as LiveItem;
                    
                    if (live.side == side)
                    {
                        StateIns state = new StateIns(passive.stateConfig, live, true);

                        live.AddStateIns(state);
                    }
                }
            }
        }

        owner.halo_list.Add(this);

        id = _id++;
    }

    public void ApplyStateToBrick()
    {
        var new_bricks = BrickCore.Instance.GetNearbyBrick(owner.standBrick, range);

        foreach(var brick in effect_bricks)
        {
            if (!new_bricks.Contains(brick))
            {
                brick.halo_list.Remove(this);

                if (brick.item is LiveItem)
                {
                    (brick.item as LiveItem).RemoveStateIns(passive.stateIns);
                }
            }
        }

        foreach(var brick in new_bricks)
        {
            if (!effect_bricks.Contains(brick))
            {
                brick.halo_list.Add(this);

                if (brick.item != null)
                {
                    if (brick.item is LiveItem)
                    {
                        LiveItem live = brick.item as LiveItem;

                        if (live.side == side)
                        {
                            StateIns state = new StateIns(passive.stateConfig, live, true);

                            live.AddStateIns(state);
                        }
                    }
                }
            }
        }

    }

    public override bool Equals(object obj)
    {
        return id == (obj as HaloInfo).id;
    }

}
