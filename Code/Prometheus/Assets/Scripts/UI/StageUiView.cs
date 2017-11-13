using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageUiView : MuiSingleBase<StageUiView>
{
    [SerializeField]
    SkillListItem _skillListItemPrefab;
    public string skillListItemName = "SLIN";
    private List<SkillListItem> skillItemList = new List<SkillListItem>(10);
    public Transform skillListRoot;

    public override IEnumerator Close(object param)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator Hide(object param)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator Init(object param)
    {
        ObjPool<SkillListItem>.Instance.InitOrRecyclePool(skillListItemName, _skillListItemPrefab);

        return null;
    }

    public override IEnumerator Open(object param)
    {
        throw new System.NotImplementedException();
    }

    public void AddSkillIntoSkillList(ulong uid)
    {
#if UNITY_EDITOR
        foreach (var item in skillItemList)
        {
            if (item.skill_id == uid)
            {
                Debug.LogError("青鑫：尝试在技能列表里重复的添加技能: id: " + uid.ToString());
            }
        }
#endif

        if (uid > 0 && FightComponet.IdToSkillType(uid) == SkillType.Active)
        {
            int _id;
            var list_item = ObjPool<SkillListItem>.Instance.GetObjFromPoolWithID(out _id, skillListItemName);
            list_item.id = _id;
            list_item.SetInfo(uid);
            list_item.SetParentAndNormalize(skillListRoot);
            skillItemList.Add(list_item);
        }
    }

    public void RemoveSkillFromSkillList(ulong uid)
    {
        if (FightComponet.IdToSkillType(uid) == SkillType.Active)
        {
            for (int i = 0; i < skillItemList.Count; ++i)
            {
                if (skillItemList[i].skill_id == uid)
                {
                    ObjPool<SkillListItem>.Instance.RecycleObj(skillListItemName, skillItemList[i].id);
                    skillItemList.RemoveAt(i);
                    return;
                }
            }
        }
    }
}

