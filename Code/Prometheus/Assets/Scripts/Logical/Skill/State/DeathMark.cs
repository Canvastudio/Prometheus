using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMark : StateEffectIns
{
    [UnityEngine.SerializeField]
    int count = 0;

    public DeathMark(LiveItem owner, StateConfig config, int index, PassiveSkillIns passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        count = Mathf.FloorToInt(stateConfig.stateArgs[index].f[0]);
        stateType = StateEffectType.OnDeadth;
    }

    protected override void Apply(object param)
    {
        if (active)
        {
            List<Monster> monsters = StageCore.Instance.tagMgr.GetEntity<Monster>(ETag.GetETag(ST.MONSTER, ST.UNDISCOVER));

            while (count > 0 && monsters.Count > 0)
            {
                int i = Random.Range(0, monsters.Count);

                var m = monsters[i];

                if (m.isDiscovered || m.itemId == owner.itemId)
                {
                    monsters.RemoveAt(i);
                    continue;
                }

                CoroCore.Instance.StartCoroutine(DeathKill(m));

                monsters.RemoveAt(i);

                --count;
            }
            
        }
    }

    IEnumerator DeathKill(Monster m)
    {
        yield return m.standBrick.OnDiscoverd();

        Damage damage = new Damage(m.cur_hp, owner, m, DamageType.Physical);

        yield return m.OnDead(damage);
    }

}
