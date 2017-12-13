using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonItem : LiveItem {

    SummonSkillsConfig config;
    LiveItem owner;
    public void InitSummonItem(SummonSkillsConfig config, LiveItem owner)
    {
        Side = owner.Side;
        this.owner = owner;
        Property = owner.Property;
        state = owner.state;
        fightComponet.ownerObject = this;

        if (config.specialAction == SpecialAction.NormalAttack)
        {
            int skillCount = config.arg.u.Count();
            for (int i = 0; i < skillCount; ++i)
            {
                fightComponet.AddSkill(config.arg.u[i]);
            }
        }
    }

    private void OnPlayerUseSkill(SkillInfo info)
    {
        if (owner.itemId == info.source.itemId)
        {
            Debug.Log(gameObject.name + " 复制拥有着技能: " + info.config.name); 
            StartCoroutine(fightComponet.CopySkill(info.config, info.targets));
        }
    }

    /// <summary>
    /// 被动技能被沉默之后，相应的召唤物也应该停止工作
    /// </summary>
    public void Deactive()
    {
        fightComponet.DeactiveSkill();

        Messenger<SkillInfo>.RemoveListener(SA.LivePreuseSkill, OnPlayerUseSkill);
    }

    public void Active()
    {
        fightComponet.ActiveSkill();

        Messenger<SkillInfo>.AddListener(SA.LivePreuseSkill, OnPlayerUseSkill);
    }
}
