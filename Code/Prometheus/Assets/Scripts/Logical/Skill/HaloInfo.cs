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
    public LiveItemSide Side;

    public bool active = false;

    public HaloInfo(int _range, LiveItemSide _side, LiveItem _owner, PassiveSkillIns _passive)
    {
        passive = _passive;
        Side = _side;
        range = _range;
        owner = _owner;

        owner.halo_list.Add(this);

        id = _id++;
    }

    /// <summary>
    /// 光环的所有者移动之后需要刷新光环的影响范围
    /// </summary>
    public void ApplyStateToBrick()
    {
        var new_bricks = BrickCore.Instance.GetNearbyBrick(owner.standBrick, range);

        if (effect_bricks != null)
        {
            foreach (var brick in effect_bricks)
            {
                if (!new_bricks.Contains(brick))
                {
                    if (brick.item is LiveItem)
                    {
                        (brick.item as LiveItem).RemoveStateIns(brick.halo_dic[this]);
                    }

                    brick.halo_dic.Remove(this);
                }
            }
        }

        foreach(var brick in new_bricks)
        {
            if (effect_bricks == null || !effect_bricks.Contains(brick))
            {
                StateIns state = null;

                if (brick.item != null)
                {
                    if (brick.item is LiveItem)
                    {
                        LiveItem live = brick.item as LiveItem;

                        if (live.Side == Side)
                        {
                            state = new StateIns(passive.stateConfig, live, true);

                            live.AddStateIns(state);
                        }
                    }
                }

                brick.halo_dic.Add(this, state);
            }
        }

        effect_bricks = new_bricks;
    }

    public void Deactive()
    {
        if (active)
        {
            foreach (var brick in effect_bricks)
            {
                if (brick.item is LiveItem)
                {
                    (brick.item as LiveItem).RemoveStateIns(brick.halo_dic[this]);
                }

                brick.halo_dic.Remove(this);
            }
        }
    }

    public void Active()
    {
        if (effect_bricks == null)
        {
            ApplyStateToBrick();
        }
        else
        if (!active)
        {
            foreach (var brick in effect_bricks)
            {
                StateIns state = null;

                if (brick.item != null)
                {
                    if (brick.item is LiveItem)
                    {
                        LiveItem live = brick.item as LiveItem;

                        if (live.Side == Side)
                        {
                            state = new StateIns(passive.stateConfig, live, true);

                            live.AddStateIns(state);
                        }
                    }
                }

                brick.halo_dic.Add(this, state);
            }
        }
    }

    public override bool Equals(object obj)
    {
        return id == (obj as HaloInfo).id;
    }

}
