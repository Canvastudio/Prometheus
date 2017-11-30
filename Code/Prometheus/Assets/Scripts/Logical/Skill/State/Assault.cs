﻿public class Assault : StateEffectIns
{
    [UnityEngine.SerializeField]
    EffectCondition condition;
    [UnityEngine.SerializeField]
    float extra = 0;
    [UnityEngine.SerializeField]
    float total_extra = 0;

    public Assault(LiveItem owner, StateConfig config, int index, PassiveSkillIns passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        condition = stateConfig.stateArgs[index].ec[0];
        extra = stateConfig.stateArgs[index].f[0];

        stateType = StateEffectType.OnGenerateDamage;
    }

    public override void Active()
    {
        base.Active();

        Messenger<Brick>.AddListener(SA.PlayerMoveStep, OnPlayerMove);
    }

    public override void Deactive()
    {
        base.Deactive();

        Messenger<Brick>.RemoveListener(SA.PlayerMoveStep, OnPlayerMove);
    }


    public override void Remove()
    {
        base.Remove();

        Messenger<Brick>.RemoveListener(SA.PlayerMoveStep, OnPlayerMove);
    }

    private void OnPlayerMove(Brick brick)
    {
        if (active)
        {
            total_extra += extra;
        }
    }

    protected override void Apply(object param)
    {
        Damage damage = param as Damage;

        if (active && FightComponet.CheckEffectCondition(condition, null, damage.damageType))
        {
            damage.damage = damage.damage * (1 + total_extra);

            total_extra = 0;
        }
    }
}
