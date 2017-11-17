using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipDetailView : MuiSingleBase<ChipDetailView> {

    [SerializeField]
    SkillPointInfo pointItem;
    [SerializeField]
    Transform pointRoot;
    [SerializeField]
    MergeOption option;
    [SerializeField]
    Transform chipRoot;
    [SerializeField]
    Button backButton;

    string pname = "cspi";
    string cname = "OPN";

    public override IEnumerator Init(object param = null)
    {
        ObjPool<SkillPointInfo>.Instance.InitOrRecyclePool(pname, pointItem);
        ObjPool<MergeOption>.Instance.InitOrRecyclePool(cname, option);
        HudEvent.Get(backButton).onClick = OnClose;
        return null;
    }

    private void OnClose()
    {
        MuiCore.Instance.Open(UiName.strChipView);
    }

    public override IEnumerator Open(object param = null)
    {
        ObjPool<SkillPointInfo>.Instance.RecyclePool(pname);
        ObjPool<SkillPointInfo>.Instance.RecyclePool(cname);

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
        for (int i = 0; i < ci.Count; ++i)
        {
            if (ci[i].boardInstance == null)
            {
                var chip = ObjPool<MergeOption>.Instance.GetObjFromPoolWithID(out id, cname);
                chip.SetParentAndNormalize(chipRoot);
                chip.Init(ci[i], id, OnChipClick);
                chip.gameObject.SetActive(true);
            }
        }

        yield return 0;

        gameObject.SetActive(true);
    }

    private void OnChipClick(MergeOption op)
    {
        if (ChipView.Instance.CreateBoardInstance(op.chip) != null)
        {
            ObjPool<MergeOption>.Instance.RecycleObj(cname, op.id);
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
