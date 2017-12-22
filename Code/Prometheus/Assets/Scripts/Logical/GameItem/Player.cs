using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LiveItem {

    public ulong playerId = 1;
    public PlayerInitConfig config;
    public Inventory inventory = new Inventory();
    public SkillPointsComponet skillPointsComponet;

    public Player InitPlayerProperty(float motorized, float capacity, float atkSpeed, float reloadSpeed)
    {
        Property.
            SetFloatProperty(GameProperty.motorized, motorized)
            .SetFloatProperty(GameProperty.capacity, capacity)
            .SetFloatProperty(GameProperty.atkSpeed, atkSpeed)
            .SetFloatProperty(GameProperty.reloadSpeed, reloadSpeed);

        OriginProperty.
            SetFloatProperty(GameProperty.motorized, motorized)
            .SetFloatProperty(GameProperty.capacity, capacity)
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
                    StageUIView.Instance.RemoveChipSkill(skill_point.skillId);
                }

                ulong new_skillId = skill_point.GetNewSkillId();

                if (new_skillId > 0)
                {
                    fightComponet.AddSkill(new_skillId, skill_point);
                }
            }
        }

        return this;
    }

    //public override IEnumerator MeleeAttackTarget<T>(T target)
    //{
    //    Debug.Log("玩家攻击：" + target.gameObject.name);



    //    yield return base.MeleeAttackTarget(target);
    //}

    public override float TakeDamage(Damage damageInfo)
    {
        return base.TakeDamage(damageInfo);
    }

    protected override void OnSetStandBrick(Brick brick)
    {
        base.OnSetStandBrick(brick);

        brick.brickType = BrickType.PLAYER;

        //if (!brick.isDiscovered)
        //{
        //    StartCoroutine(brick.OnDiscoverd());
        //}

        if (brick.row > StageCore.Instance.playerDistance)
        {
            StageCore.Instance.playerDistance = brick.row;
            StageUIView.Instance.upUIView.distance.text = brick.row.ToString();
        }
    }

    public override void Recycle()
    {
        base.Recycle();
    }

    public override void AddStateUI(StateIns ins)
    {
        base.AddStateUI(ins);

        StageUIView.Instance.upUIView.OnStateAdd(ins);
    }

    public override void RemoveStateUI(StateIns ins)
    {
        base.RemoveStateUI(ins);

        StageUIView.Instance.upUIView.OnStateRemove(ins);
    }
}
