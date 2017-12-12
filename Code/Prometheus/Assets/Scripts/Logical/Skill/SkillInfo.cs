using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfo  {

    public ActiveSkillsConfig config;
    public LiveItem source;
    //public LiveItem target;

    public SkillInfo(ActiveSkillsConfig config, LiveItem source)
    {
        this.config = config;
        this.source = source;
       // this.target = target;
    }
}
