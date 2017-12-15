using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillOrgan : OrganBase {

    public ActiveSkillsConfig config;

    public override void Reactive()
    {
        var player = StageCore.Instance.Player;
        player.StartCoroutine(player.fightComponet.DoActiveSkill(null, config));

        Clean();
    }
}
