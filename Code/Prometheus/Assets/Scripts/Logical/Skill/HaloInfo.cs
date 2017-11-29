using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 光环信息
/// </summary>
[System.Serializable]
public class HaloInfo  {

    static int _id;

    int range = 0;
    public PassiveSkillIns passive;
    public int id;
    public LiveItem owner;
    public LiveItemSide Side;

    public List<Brick> effectBricks = new List<Brick>(10);

    public bool active = false;

    public HaloInfo(int _range, LiveItemSide _side, LiveItem _owner, PassiveSkillIns _passive)
    {
        passive = _passive;
        Side = _side;
        range = _range;
        owner = _owner;

        id = _id++;

    }

    public void Active()
    {
        RefreshEffectItem();
    }

    public void Remove()
    {
        foreach(var brick in effectBricks)
        {
            brick.haloComponent.RemoveHalo(this);
        }
    }

    public void RefreshEffectItem()
    {
        var bricks = BrickCore.Instance.GetNearbyBrick(owner.standBrick, range);

        bricks.Add(owner.standBrick);

        foreach(var brick in bricks)
        {
            if (!effectBricks.Contains(brick))
            {
                brick.haloComponent.AddHalo(this);
                effectBricks.Add(brick);
            }
        }

        for (int i = effectBricks.Count -1; i>= 0; --i)
        {
            var brick = effectBricks[i];

            if (!bricks.Contains(brick))
            {
                brick.haloComponent.RemoveHalo(this);
                effectBricks.RemoveAt(i);
            }
        }
        
    }

    public override bool Equals(object obj)
    {
        return id == (obj as HaloInfo).id;
    }

}
