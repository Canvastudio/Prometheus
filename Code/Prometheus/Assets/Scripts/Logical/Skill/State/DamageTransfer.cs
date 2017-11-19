 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTransfer : StateEffectIns
{
    /// <summary>
    /// 能转移多少次
    /// </summary>
    public int times = 0;
    EffectCondition condition;
    int range = 0;
    List<Monster> list = new List<Monster>(8);

    public DamageTransfer(LiveItem owner, StateConfig config, int index, bool passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        times = passive ? -1 : Mathf.FloorToInt(config.stateArgs[index].f[0]);
        condition = config.stateArgs[index].ec[0];
        range = Mathf.FloorToInt(config.stateArgs[index].f[1]);
        stateType = StateEffectType.OnTakenDamage;
    }

    protected override void Apply(object _damageInfo)
    {
        var damageInfo = _damageInfo as Damage;
        if (!damageInfo.isTransfer && FightComponet.CheckEffectCondition(condition, owner, damageInfo.damageType))
        {
            if (!passive) { times -= 1; }

            StageCore.Instance.GetMonsterInRange(owner.standBrick, range, ref list);

            if (list.Count > 0)
            {
                for (int i = list.Count - 1; i >= 0; --i)
                {
                    if (list[i].itemId == damageInfo.damageSource.itemId)
                    {
                        list.RemoveAt(i);
                    }
                }
            }

            if (list.Count > 0)
            {
                int index = Random.Range(0, list.Count);

                Damage di = new Damage(damageInfo.damage, owner, list[index], damageInfo.damageType, true);

                list[index].TakeDamage(di);

                damageInfo.damage = 0;
            }


            if (times == 0)
            {
                out_data = true;
            }
        }
    }
}
