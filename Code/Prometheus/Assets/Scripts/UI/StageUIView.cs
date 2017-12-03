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
    public UpUIView upUIView;

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

        upUIView.Init();

        return null;
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
