using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSkillOrgan : OrganBase
{
    public ActiveSkillsConfig config;
    public int count;
    public override void Reactive()
    {
        StageCore.Instance.Player.inventory.AddLimitedSkill(config.id, count);
        Recycle();
    }
}
