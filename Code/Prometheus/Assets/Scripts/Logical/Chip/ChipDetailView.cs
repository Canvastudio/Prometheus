using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipDetailView : MuiSingleBase<ChipDetailView> {

    [SerializeField]
    SkillPointInfo pointItem;
    [SerializeField]
    Transform pointRoot;
    //[SerializeField]
    public ChipItem chipItem;
    [SerializeField]
    Transform chipRoot;
    [SerializeField]
    Button backButton;

    string pname = "SkillPointInfo";
    string cname = "ChipItem";

    public override IEnumerator Init(object param = null)
    {
        ObjPool<SkillPointInfo>.Instance.InitOrRecyclePool(pname, pointItem);
        ObjPool<ChipItem>.Instance.InitOrRecyclePool(cname, chipItem);
        pointItem.gameObject.SetActive(false);
        chipItem.gameObject.SetActive(false);
        HudEvent.Get(backButton).onClick = OnClose;
        return null;
    }

    private void OnClose()
    {
        //MuiCore.Instance.Open(UiName.strChipView);
        MuiCore.Instance.HideTop();
    }

    public override IEnumerator Open(object param = null)
    {
        ObjPool<SkillPointInfo>.Instance.RecyclePool(pname);
        ObjPool<ChipItem>.Instance.RecyclePool(cname);

        var ai = StageCore.Instance.Player.fightComponet.activeInsList;
        var pi = StageCore.Instance.Player.fightComponet.passiveInsList;

        int id;

        for (int i = 0;  i < ai.Count; ++i)
        {
            var info = ObjPool<SkillPointInfo>.Instance.GetObjFromPoolWithID(out id, pname);
            info.uid = id;
            info.SetParentAndNormalize(pointRoot);
            info.Set(ai[i]);
            info.gameObject.SetActive(true);
        }

        for (int i = 0; i < pi.Count; ++i)
        {
            var info = ObjPool<SkillPointInfo>.Instance.GetObjFromPoolWithID(out id, pname);
            info.uid = id;
            info.SetParentAndNormalize(pointRoot);
            info.Set(pi[i]);
            info.gameObject.SetActive(true);
        }

        var ci = StageCore.Instance.Player.inventory.GetChipList();
        Debug.Log("芯片数量: " + ci.Count);
        for (int i = 0; i < ci.Count; ++i)
        {
 
            var chip = ObjPool<ChipItem>.Instance.GetObjFromPoolWithID(out id, cname);
            chip.SetParentAndNormalize(chipRoot);
            chip.ShowChipInfo(ci[i], id, OnChipClick);
            chip.gameObject.SetActive(true);
        }

        yield return 0;

        gameObject.SetActive(true);
    }

    public void OnChipClick(ChipItem item)
    {
        if (item.chip.boardInstance == null)
        {
            if (ChipView.Instance.CreateBoardInstance(item.chip) != null)
            {
                ObjPool<MergeOption>.Instance.RecycleObj(cname, item.id);
                ChipView.Instance.RefreshChipList();
            }

            OnClose();
        }
    }


    public override IEnumerator Close(object param = null)
    {
        throw new System.NotImplementedException();
    }

    public override void Hide(object param = null)
    {
        gameObject.SetActive(false);
    }
}
