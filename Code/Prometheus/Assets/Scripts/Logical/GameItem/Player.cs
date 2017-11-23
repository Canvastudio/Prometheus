using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LiveItem {

    public ulong playerId = 1;
    public PlayerInitConfig config;
    public Inventory inventory = new Inventory();
    public SkillPointsComponet skillPointsComponet;

    public Player SetPlayerProperty(float motorized, float capacity, float atkSpeed, float reloadSpeed)
    {
        Property.SetFloatProperty(GameProperty.motorized, motorized)
            .SetFloatProperty(GameProperty.atkSpeed, atkSpeed)
            .SetFloatProperty(GameProperty.reloadSpeed, reloadSpeed);

        return this;
    }

    public Player RefreshSkillPointStateToSkill()
    {
        for (int i =0; i < skillPointsComponet.pointList.Count; ++i)
        {
            var skill_point = skillPointsComponet.pointList[i];

            if (skill_point.last_count != skill_point.count)
            {
                if (skill_point.skillId > 0)
                {
                    fightComponet.RemoveSkill(skill_point.skillId);
                    StageView.Instance.RemoveSkillFromSkillList(skill_point.skillId);
                }

                ulong new_skillId = skill_point.GetNewSkillId();

                if (new_skillId > 0)
                {
                    fightComponet.AddSkill(new_skillId, skill_point);
                    StageView.Instance.AddSkillIntoSkillList(skill_point.GetNewSkillId());
                }
            }
        }

        return this;
    }

    public override IEnumerator MeleeAttackTarget<T>(T target)
    {
        var e = base.MeleeAttackTarget(target);

        var config = ConfigDataBase.GetConfigDataById<GlobalParameterConfig>(1);
        var atk_Speed = Property.GetFloatProperty(GameProperty.atkSpeed);
        var timeSpend = (1 - ((atk_Speed + 100) / (atk_Speed + 101))) * 15; 
        
        StageCore.Instance.TimeCast(timeSpend);

        return e;
    }

    public override float TakeDamage(Damage damageInfo)
    {
        Messenger.Invoke(SA.PlayHpChange);

        return base.TakeDamage(damageInfo);
    }

    protected override void OnSetStandBrick(Brick brick)
    {
        base.OnSetStandBrick(brick);

        brick.brickType = BrickType.PLAYER;
        brick.brickExplored = BrickExplored.EXPLORED;
    }

    public override void Recycle()
    {
        base.Recycle();
    }

    public override void AddStateIns(StateIns ins)
    {
        base.AddStateIns(ins);

        Messenger<StateIns>.Invoke(SA.PlayerAddState, ins);
    }

    public override void RemoveStateIns(StateIns ins)
    {
        base.RemoveStateIns(ins);

        Messenger<StateIns>.Invoke(SA.PlayerRemoveState, ins);
    }

}
