using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoleInfoView : MuiSingleBase<RoleInfoView>
{
    [SerializeField]
    Text chp;
    [SerializeField]
    Text mhp;

    [SerializeField]
    Text laser;
    [SerializeField]
    Text atk;
    [SerializeField]
    Text shield;
    [SerializeField]
    Text armor;
    [SerializeField]
    Text first;

    [Space(5)]
    [SerializeField]
    Button backButton;

    [SerializeField]
    Button weapon_add;
    [SerializeField]
    Button cloth_add;
    [SerializeField]
    Button other_add1;
    [SerializeField]
    Button other_add2;
    [SerializeField]
    Button other_add3;

    [SerializeField]
    Toggle stateToggle;
    [SerializeField]
    Toggle skillToggle;
    [SerializeField]
    Toggle weaponToggle;

    [Space(5)]
    [SerializeField]
    Transform root;

    [SerializeField]
    GameObject roleInfo;

    [SerializeField]
    StateItem stateItem;
    [SerializeField]
    SkillItem skillItem;
    [SerializeField]
    WeaponItem weaponItem;

    bool initState = false;
    bool initSkill = false;

    List<StateItem> stateList = new List<StateItem>();
    List<SkillItem> skillList = new List<SkillItem>();

    private string s3 = "weaponItem";
    private string s2 = "stateItem";
    private string s1 = "skillItem";

    public override IEnumerator Close(object param = null)
    {
        throw new System.NotImplementedException();
    }

    public override void Hide(object param = null)
    {
        gameObject.SetActive(false);
        ObjPool<StateItem>.Instance.RecyclePool(s2);
    }

    Player player;

    public override IEnumerator Init(object param = null)
    {
  
        ObjPool<StateItem>.Instance.InitOrRecyclePool(s2, stateItem, 4);
        ObjPool<SkillItem>.Instance.InitOrRecyclePool(s2, skillItem, 4);

        HudEvent.Get(backButton).onClick = Back;
        HudEvent.Get(cloth_add).onClick = OnClothAdd;
        HudEvent.Get(weapon_add).onClick = OnWeaponAdd;
        HudEvent.Get(other_add1).onClick = OnOtherAdd1;
        HudEvent.Get(other_add2).onClick = OnOtherAdd2;
        HudEvent.Get(other_add3).onClick = OnOtherAdd3;

        stateToggle.onValueChanged.AddListener(SetStateToggle);
        skillToggle.onValueChanged.AddListener(SetSkillToggle);
        weaponToggle.onValueChanged.AddListener(SetWeaponToggle);
         
        return null;
    }

    void Start()
    {
        stateToggle.onValueChanged.AddListener(SetStateToggle);
        skillToggle.onValueChanged.AddListener(SetSkillToggle);
        weaponToggle.onValueChanged.AddListener(SetWeaponToggle);
    }

    private void OnClothAdd()
    {
        
    }

    private void OnWeaponAdd()
    {

    }

    private void OnOtherAdd1()
    {

    }

    private void OnOtherAdd2()
    {

    }

    private void OnOtherAdd3()
    {

    }

    private void ShowWeaponList(EqpType type)
    {
        roleInfo.SetActive(false);

        weapon_add.gameObject.SetActive(false);
        cloth_add.gameObject.SetActive(false);
        other_add1.gameObject.SetActive(false);
        other_add2.gameObject.SetActive(false);
        other_add3.gameObject.SetActive(false);


        var player = StageCore.Instance.Player;
        foreach(var eqp in player.inventory.equipmentList)
        {
            if (eqp.config.eqpType == type)
            {

            }
        }
    }

    private void SetStateToggle(bool t)
    {
        if (t)
        {
            roleInfo.SetActive(true);

            if (!initState)
            {
                foreach (var state in player.state.state_list)
                {
                    if (state.stateConfig.iShow && state.active)
                    {
                        var item = ObjPool<StateItem>.Instance.GetObjFromPool(s2);
                        item.SetParentAndNormalize(root);
                        item.ShowStateInfo(state);
                        stateList.Add(item);
                    }
                }
            }
            else
            {
                foreach (var item in stateList)
                {
                    item.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var item in stateList)
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    private void SetSkillToggle(bool t)
    {
        if (t)
        {
            if (!initSkill)
            {
                roleInfo.SetActive(false);

                foreach (var ins in StageCore.Instance.Player.fightComponet.activeInsList)
                {
                    var item = ObjPool<SkillItem>.Instance.GetObjFromPool(s1);
                    item.SetParentAndNormalize(root);
                    item.gameObject.SetActive(true);
                    item.ShowSkillInfo(ins);
                    skillList.Add(item);
                }

                foreach (var ins in StageCore.Instance.Player.fightComponet.passiveInsList)
                {
                    var item = ObjPool<SkillItem>.Instance.GetObjFromPool(s1);
                    item.SetParentAndNormalize(root);
                    item.gameObject.SetActive(true);
                    item.ShowSkillInfo(ins);
                    skillList.Add(item);
                }
            }
            else
            {
                foreach (var item in skillList)
                {
                    item.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var item in skillList)
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    private void SetWeaponToggle(bool t)
    {
        if (t)
        {
            roleInfo.SetActive(true);

            weapon_add.gameObject.SetActive(true);
            cloth_add.gameObject.SetActive(true);
            other_add1.gameObject.SetActive(true);
            other_add2.gameObject.SetActive(true);
            other_add3.gameObject.SetActive(true);
        }
        else
        {
            weapon_add.gameObject.SetActive(false);
            cloth_add.gameObject.SetActive(false);
            other_add1.gameObject.SetActive(false);
            other_add2.gameObject.SetActive(false);
            other_add3.gameObject.SetActive(false);
        }
    }

    private void RefreshProperty()
    {
        laser.SetPropertyText(player, GameProperty.laser);
        atk.SetPropertyText(player, GameProperty.attack);
        shield.SetPropertyText(player, GameProperty.nshield);
        armor.SetPropertyText(player, GameProperty.guard);
        first.SetPropertyText(player, GameProperty.firstAtt);
        chp.SetPropertyText(player, GameProperty.nhp);
        mhp.SetPropertyText(player, GameProperty.mhp);
    }

    public void Back()
    {
        MuiCore.Instance.HideTop();
    }

    public override IEnumerator Open(object param = null)
    {
        player = StageCore.Instance.Player;

        RefreshProperty();

        SetStateToggle(true);

        gameObject.SetActive(true);

        return null;   
    }
}
