using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfo  {

    public ActiveSkillsConfig config;
    public LiveItem source;
    public List<GameItemBase> targets;

    public SkillInfo(ActiveSkillsConfig config, LiveItem source, List<GameItemBase> target)
    {
        this.config = config;
        this.source = source;
        this.targets = target;
    }
}
