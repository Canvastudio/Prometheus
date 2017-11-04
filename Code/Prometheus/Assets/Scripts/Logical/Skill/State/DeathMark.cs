using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMark : StateEffectIns
{
    int count = 0;

    public DeathMark(LiveItem owner, StateConfig config, int index, bool passive) : base(owner, config, index, passive)
    {
        count = Mathf.FloorToInt(stateConfig.stateArgs[index].f[0]);
    }

    public override void Active()
    {
        base.Active();

        Messenger<Monster>.AddListener(SA.MonsterDead, OnMonsterDead);
    }

    public override void Deactive()
    {
        base.Deactive();

        Messenger<Monster>.RemoveListener(SA.MonsterDead, OnMonsterDead);
    }

    private void OnMonsterDead(Monster monster)
    {
        if (active)
        {
            if (monster.itemId == owner.itemId)
            {
                List<Monster> monsters = StageCore.Instance.tagMgr.GetEntity<Monster>(ETag.GetETag(ST.MONSTER, ST.UNDISCOVER));

                foreach(var m in monsters)
                {
                    if (count <= 0) return;

                    m.standBrick.OnDiscoverd();

                    Damage damage = new Damage(m.cur_hp, owner, m, DamageType.Physical);

                    m.TakeDamage(damage);

                    --count;
                }
            }
        }
    }

    protected override void Apply(object param)
    {

    }
}
