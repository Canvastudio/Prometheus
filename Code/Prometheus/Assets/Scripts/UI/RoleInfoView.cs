using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoleInfoView : MuiSingleBase<RoleInfoView>
{
    [SerializeField]
    Text hp;
    [SerializeField]
    Text speed;
    [SerializeField]
    Text moto;
    [SerializeField]
    Text power;
    [SerializeField]
    Text reloadSpeed;
    [SerializeField]
    Text atkSpeed;
    [SerializeField]
    Text melee;
    [SerializeField]
    Text laser;
    [SerializeField]
    Text cartridge;

    [Space(5)]
    [SerializeField]
    Button backButton;

    [Space(5)]
    [SerializeField]
    Transform stateRoot;

    [SerializeField]
    StateItem item;

    private string s2 = "stateItem";

    public override IEnumerator Close(object param = null)
    {
        throw new System.NotImplementedException();
    }

    public override void Hide(object param = null)
    {
        gameObject.SetActive(false);
        ObjPool<StateItem>.Instance.RecyclePool(s2);
    }

    public override IEnumerator Init(object param = null)
    {
        ObjPool<StateItem>.Instance.InitOrRecyclePool(s2, item, 4);
        HudEvent.Get(backButton).onClick = Back;
        return null;
    }

    public void Back()
    {
        MuiCore.Instance.HideTop();
    }

    public override IEnumerator Open(object param = null)
    {
        Player player = StageCore.Instance.Player;
        hp.SetHpText(player);
        speed.SetPropertyText(player, GameProperty.speed);
        moto.SetPropertyText(player, GameProperty.motorized);
        power.SetPropertyText(player, GameProperty.capacity);
        reloadSpeed.SetPropertyText(player, GameProperty.reloadSpeed);
        atkSpeed.SetPropertyText(player, GameProperty.atkSpeed);
        melee.SetPropertyText(player, GameProperty.melee);
        laser.SetPropertyText(player, GameProperty.laser);
        cartridge.SetPropertyText(player, GameProperty.cartridge);

        foreach(var state in player.state.state_list)
        {
            if (state.stateConfig.iShow && state.active)
            {
                var item = ObjPool<StateItem>.Instance.GetObjFromPool(s2);
                item.SetParentAndNormalize(stateRoot);
                item.ShowStateInfo(state);
            }
        }

        gameObject.SetActive(true);

        return null;   
    }
}
