using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveBase  {

    public ulong uid;
    public PassiveSkillsConfig passiveConfig;
    public FightComponet fightComponet;
    public int index;

    public PassiveBase(PassiveSkillsConfig config, int index, FightComponet fightComponet)
    {
        uid = config.id;
        this.index = index;
        this.passiveConfig = config;
        this.fightComponet = fightComponet;
    }

    public abstract void Apply();
    public abstract void Remove();
}
