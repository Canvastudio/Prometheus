using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonItem : LiveItem {

    SummonSkillsConfig config;

    public void InitSummonItem(SummonSkillsConfig config, LiveItem owner)
    {
        Side = owner.Side;

        if (config.specialAction == SpecialAction.NormalAttack)
        {
            int skillCount = config.arg.u.Count();
            for (int i = 0; i < skillCount; ++i)
            {
                fightComponet.AddSkill(config.arg.u[i]);
            }
        }
    }

    /// <summary>
    /// 被动技能被沉默之后，相应的召唤物也应该停止工作
    /// </summary>
    public void Deactive()
    {
        fightComponet.DeactiveSkill();
    }

    public void Active()
    {
        fightComponet.ActiveSkill();
    }
}
