using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoView : MuiSingleBase<SkillInfoView> {

    [SerializeField]
    Transform contentRoot;
    [SerializeField]
    SkillItem item;
    [SerializeField]
    Button asButton;
    [SerializeField]
    Button psButton;
    [SerializeField]
    Button chipButton;
    [SerializeField]
    Button closeButton;

    private string pname = "skillItem";

    /// <summary>
    /// 0显示主动 1 显示被动
    /// </summary>
    private int show_state = -1; 

    private void ShowAcitveList()
    {
        if (show_state != 0)
        {
            show_state = 0;

            ObjPool<SkillItem>.Instance.RecyclePool(pname);

            foreach (var ins in StageCore.Instance.Player.fightComponet.activeInsList)
            {
                var item = ObjPool<SkillItem>.Instance.GetObjFromPool(pname);
                item.SetParentAndNormalize(contentRoot);
                item.gameObject.SetActive(true);
                item.ShowSkillInfo(ins);
            }
        }
    }

    private void ShowPassiveList()
    {
        if (show_state != 1)
        {
            show_state = 1;
            ObjPool<SkillItem>.Instance.RecyclePool(pname);

            foreach (var ins in StageCore.Instance.Player.fightComponet.passiveInsList)
            {
                var item = ObjPool<SkillItem>.Instance.GetObjFromPool(pname);
                item.SetParentAndNormalize(contentRoot);
                item.gameObject.SetActive(true);
                //item.ShowSkillInfo(ins);
            }

        }
    }

    public override IEnumerator Init(object param)
    {
        ObjPool<SkillItem>.Instance.InitOrRecyclePool(pname, item);
        HudEvent.Get(asButton).onClick = ShowAcitveList;
        HudEvent.Get(psButton).onClick = ShowPassiveList;
        HudEvent.Get(chipButton).onClick = ShowChipView;
        HudEvent.Get(closeButton).onClick = HideSelf;
        return null;
    }

    private void HideSelf()
    {
        MuiCore.Instance.HideTop();
    }

    public void ShowChipView()
    {
        MuiCore.Instance.Open(UiName.strChipView);
    }

    public override IEnumerator Open(object param)
    {
        ObjPool<SkillDetailItem>.Instance.RecyclePool(pname);
        gameObject.SetActive(true);
        ShowAcitveList();
        return null;
    }

    public override IEnumerator Close(object param)
    {
        throw new System.NotImplementedException();
    }

    public override void Hide(object param)
    {
        show_state = -1;
        gameObject.SetActive(false);
    }
}
