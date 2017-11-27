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

    public List<GameItemBase> effectItems = new List<GameItemBase>(10);

    public bool active = false;

    public HaloInfo(int _range, LiveItemSide _side, LiveItem _owner, PassiveSkillIns _passive)
    {
        passive = _passive;
        Side = _side;
        range = _range;
        owner = _owner;

        id = _id++;
    }

    public void Deactive()
    {

    }

    public void Active()
    {

    }

    private void RefreshEffectItem(float f)
    {
        var items = BrickCore.Instance.GetNearbyLiveItem(owner.standBrick, range);
    }

    public override bool Equals(object obj)
    {
        return id == (obj as HaloInfo).id;
    }

}
