using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonstersInfoView : MuiSingleBase<MonstersInfoView>
{
    [SerializeField]
    Button backButton;

    [Space(5)]
    [SerializeField]
    Transform skillRoot;
    [SerializeField]
    Transform stateRoot;
    [SerializeField]
    SkillItem skillItem;
    [SerializeField]
    StateItem StateItem;

    [Space(5)]
    [SerializeField]
    Text hp;
    [SerializeField]
    Text moto;
    [SerializeField]
    Text melee;
    [SerializeField]
    Text laser;
    [SerializeField]
    Text cartridge;
    [SerializeField]
    Text monDescr;
    [SerializeField]
    Text monName;
    [SerializeField]
    Image monIcon;

    [SerializeField]
    GameObject ironType;
    [SerializeField]
    GameObject organicType;

    private string s1 = "skillItem";
    private string s2 = "stateItem";
    private string hpft = "{0}/{1}";

    public override IEnumerator Close(object param = null)
    {
        throw new System.NotImplementedException();
    }

    public override void Hide(object param = null)
    {
        ObjPool<SkillItem>.Instance.RecyclePool(s1);
        ObjPool<StateItem>.Instance.RecyclePool(s2);

        gameObject.SetActive(false);
    }

    public override IEnumerator Init(object param = null)
    {
        ObjPool<SkillItem>.Instance.InitOrRecyclePool(s1, skillItem, 3);
        ObjPool<StateItem>.Instance.InitOrRecyclePool(s2, StateItem, 3);
        HudEvent.Get(backButton).onClick = OnBackBtn;

        return null;
    }

    private void OnBackBtn()
    {
        MuiCore.Instance.HideTop();
    }

    public override IEnumerator Open(object param = null)
    {
        gameObject.SetActive(true);

        ObjPool<SkillItem>.Instance.InitOrRecyclePool(s1, skillItem, 3);
        ObjPool<StateItem>.Instance.InitOrRecyclePool(s2, StateItem, 3);

        Monster mon = param as Monster;

        var mf = mon.fightComponet as MonsterFightComponet;

        foreach(var ins in mf.monsterActiveInsList)
        {
            SkillItem item =  ObjPool<SkillItem>.Instance.GetObjFromPool(s1);
            item.SetParentAndNormalize(skillRoot);
            item.ShowSkillInfo(ins);
        }


        foreach (var ins in mf.passiveInsList)
        {
            SkillItem item = ObjPool<SkillItem>.Instance.GetObjFromPool(s1);
            item.SetParentAndNormalize(skillRoot);
            item.ShowSkillInfo(ins);
        }

        foreach (var ins in mon.state.state_list)
        {
            if (ins.stateConfig.iShow)
            {
                StateItem item = ObjPool<StateItem>.Instance.GetObjFromPool(s2);
                item.SetParentAndNormalize(stateRoot);
                item.ShowStateInfo(ins);
            }
        }

        hp.SetHpText(mon);
        moto.SetPropertyText(mon, GameProperty.speed);
        //melee.SetPropertyText(mon, GameProperty.melee);
        melee.SetPropertyText(mon, GameProperty.attack);
        laser.SetPropertyText(mon, GameProperty.laser);
        cartridge.SetPropertyText(mon, GameProperty.cartridge);
        monDescr.text = mon.config.describe;
        monName.text = mon.config.m_name;

        monIcon.SetItemIcon(mon.config.icon);

        ironType.SetActive(mon.config.monsterType == MonsterType.Iron);
        organicType.SetActive(mon.config.monsterType == MonsterType.Organisms);

        return null;
    }


}
