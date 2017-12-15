using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUIView : MuiSingleBase<StageUIView>
{
    [SerializeField]
    SkillListItem _skillListItemPrefab;
    [SerializeField]
    Button skillInfoButton;
    [SerializeField]
    Button chipButton;
    [SerializeField]
    public UpUIView upUIView;
    [SerializeField]
    public Transform playerCeiling;
    [SerializeField]
    GameItemInfo itemInfo;
    [SerializeField]
    Transform bottomInfoPos;
    [SerializeField]
    Transform topInfoPos;
    [SerializeField]
    SkillItem skillItem;
    [SerializeField]
    public MaterialsInfo mat;

    public RectTransform viewArea;

    public void ShowItemInfo(Brick brick)
    {
        if (RectTransformUtility.WorldToScreenPoint(GameManager.Instance.GCamera, brick.transform.position).y > Screen.height / 2)
        {
            itemInfo.transform.position = bottomInfoPos.transform.position;
        }
        else
        {
            itemInfo.transform.position = topInfoPos.transform.position;
        }

        if (brick.realBrickType == BrickType.SUPPLY)
        {
            itemInfo.ShowSupplyInfo(brick.item as Supply);
            itemInfo.gameObject.SetActive(true);
        }
        else if (brick.realBrickType == BrickType.TABLET)
        {
            itemInfo.ShowTabletInfo(brick.item as Tablet);
            itemInfo.gameObject.SetActive(true);
        }
        else if (brick.realBrickType == BrickType.TREASURE)
        {
            itemInfo.ShowTreasureInfo(brick.item as Treasure);
            itemInfo.gameObject.SetActive(true);
        }
        else if (brick.realBrickType == BrickType.Organ || brick.realBrickType == BrickType.OrganProperty)
        {
            itemInfo.ShowOrganInfo(brick.item as OrganBase);
            itemInfo.gameObject.SetActive(true);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            HideItemInfo();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            HideItemInfo();
        }
    }

    public void HideItemInfo()
    {
        itemInfo.gameObject.SetActive(false);
    }

    public void ShowSkillInfo(ActiveSkillsConfig config)
    {
        skillItem.ShowSkillInfo(config);
    }

    public void HideSkillInfo()
    {
        skillItem.gameObject.SetActive(false);
    }

    public override IEnumerator Close(object param = null)
    {
        throw new System.NotImplementedException();
    }

    public override void Hide(object param = null)
    {
        gameObject.SetActive(false);
    }

    public override IEnumerator Init(object param = null)
    {
        ObjPool<SkillListItem>.Instance.InitOrRecyclePool(skillListItemName, _skillListItemPrefab);

        HudEvent.Get(skillInfoButton).onClick = ShowSkillInfo;
        HudEvent.Get(chipButton).onClick = OnChipButton;

        upUIView.Init();

        return null;
    }

    public void IniMat()
    {
        mat.CleanCost();
        mat.RefreshOwned();
    }

    private void OnChipButton()
    {
        MuiCore.Instance.Open(UiName.strChipView);
    }

    public override IEnumerator Open(object param = null)
    {
        gameObject.SetActive(true);

        yield return 0;
    }

    public string skillListItemName = "SLIN";

    public Transform skillListRoot;

    private List<SkillListItem> skillItemList = new List<SkillListItem>(10);

    private void ShowSkillInfo()
    {
        StartCoroutine(MuiCore.Instance.AddOpen(UiName.strSkillInfoView));
    }

    public void AddChipSkillIntoSkillList(ActiveSkillIns ins)
    {
        foreach (var item in skillItemList)
        {
            if (item.ins.config.id == ins.config.id)
            {
                Debug.LogError("青鑫：尝试在技能列表里重复的添加技能: id: " + ins.config.id.ToString());
            }
        }


        if (ins.config.id > 0 && FightComponet.IdToSkillType(ins.config.id) == SkillType.Active)
        {
            ulong _id;
            var list_item = ObjPool<SkillListItem>.Instance.GetObjFromPoolWithID(out _id, skillListItemName);
            list_item.id = _id;
            list_item.SetInfo(ins);
            list_item.SetParentAndNormalize(skillListRoot);
            skillItemList.Add(list_item);
        }
    }

    public void AddNewOrganSkillIntoSkillList(ActiveSkillIns ins)
    {

        foreach (var item in skillItemList)
        {
            if (item.ins.config.id == ins.config.id)
            {
                Debug.LogError("青鑫：尝试在技能列表里重复的添加技能: id: " + ins.config.id.ToString());
            }
        }

        if (ins.config.id > 0 && FightComponet.IdToSkillType(ins.config.id) == SkillType.Active)
        {
            ulong _id;
            var list_item = ObjPool<SkillListItem>.Instance.GetObjFromPoolWithID(out _id, skillListItemName);
            list_item.id = _id;
            list_item.SetInfo(ins);
            list_item.SetParentAndNormalize(skillListRoot);
            skillItemList.Add(list_item);
        }
    }

    public void AddOrganSkillCount(ulong id, int count)
    {
        foreach (var item in skillItemList)
        {
            if (item.ins.count > 0)
            {
                item.ins.count += count;
                if (item.ins.count == 0)
                {
                    RemoveOrganSkill(id);
                    StageCore.Instance.Player.fightComponet.RemoveOrganSkill(id);
                }
                else
                {
                    item.RefreshCount();
                }
                return;
            }
        }

        Debug.LogError("无法找到对应的organ skill");
    }


    public void RemoveChipSkill(ulong uid)
    {
        if (FightComponet.IdToSkillType(uid) == SkillType.Active)
        {
            for (int i = 0; i < skillItemList.Count; ++i)
            {
                if (skillItemList[i].ins.config.id == uid && skillItemList[i].ins.count == -1)
                {
                    ObjPool<SkillListItem>.Instance.RecycleObj(skillListItemName, skillItemList[i].id);
                    skillItemList.RemoveAt(i);
                    return;
                }
            }
        }
    }

    public void RemoveOrganSkill(ulong uid)
    {
        if (FightComponet.IdToSkillType(uid) == SkillType.Active)
        {
            for (int i = 0; i < skillItemList.Count; ++i)
            {
                if (skillItemList[i].ins.config.id == uid && skillItemList[i].ins.count == 0)
                {
                    ObjPool<SkillListItem>.Instance.RecycleObj(skillListItemName, skillItemList[i].id);
                    skillItemList.RemoveAt(i);
                    return;
                }
            }
        }
    }
}
