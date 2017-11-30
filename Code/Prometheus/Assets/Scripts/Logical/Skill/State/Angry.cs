using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angry : StateEffectIns
{
    public EffectCondition condition;

    public float extra0 = 0;
    public float extra1 = 0;
    public float extra2 = 0;

    public float total_extra0 = 0;
    public float total_extra1 = 0;
    public float total_extra2 = 0;

    public Angry(LiveItem owner, StateConfig config, int index, PassiveSkillIns passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        condition = stateConfig.stateArgs[index].ec[0];
        extra1 = stateConfig.stateArgs[index].f[1];
        extra0 = stateConfig.stateArgs[index].f[0];
        extra2 = stateConfig.stateArgs[index].f[2];

        stateType = StateEffectType.OnGenerateDamage;
    }

    public override void Active()
    {
        base.Active();

        Messenger<Damage>.AddListener(SA.ItemTakeDamage, OnDamage);
    }

    public override void Deactive()
    {
        base.Deactive();

        Messenger<Damage>.RemoveListener(SA.ItemTakeDamage, OnDamage);
    }

    public override void Remove()
    {
        base.Remove();

        Messenger<Damage>.RemoveListener(SA.ItemTakeDamage, OnDamage);
    }

    private void OnDamage(Damage damage)
    {
        if (active && damage.damageTarget.itemId == owner.itemId && FightComponet.CheckEffectCondition(condition, null, damage.damageType))
        {
            total_extra0 += extra0;
            total_extra1 += extra1;
            total_extra2 += extra2;
        }
    }

    protected override void Apply(object param)
    {
        Damage damage = param as Damage;

        if (active)
        {
            if (damage.damageType == DamageType.Physical)
            {
                damage.damage = damage.damage * (1 + total_extra0);
            }
            else if(damage.damageType == DamageType.Cartridge)
            {
                damage.damage = damage.damage * (1 + total_extra1);
            }
            else if (damage.damageType == DamageType.Laser)
            {
                damage.damage = damage.damage * (1 + total_extra2);
            }
        }
    }
}
