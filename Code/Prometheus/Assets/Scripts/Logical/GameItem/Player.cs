using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LiveItem {

    public ulong typeId = 1;
    public PlayerInitConfig config;
    public Inventory inventory = new Inventory();
    public FightComponet fightComponet;
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
                ulong new_skillId = skill_point.GetNewSkillId();

                if (new_skillId != skill_point.skillId)
                {
                    if (skill_point.skillId > 0)
                    {
                        fightComponet.RemoveSkill(skill_point.skillId);
                        StageView.Instance.RemoveSkillFromSkillList(skill_point.skillId);
                    }

                    if (new_skillId > 0)
                    {
                        fightComponet.AddSkill(new_skillId);
                        StageView.Instance.AddSkillIntoSkillList(skill_point.GetNewSkillId());
                    }
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

}
