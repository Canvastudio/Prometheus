using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class HelpFunction  {
    
    public static Sprite GetSkillIcon(ulong skillId, SpriteAtlas atlas)
    {
        string icon_name = null;

        if (skillId < 2000000)
        {
            icon_name = ConfigDataBase.GetConfigDataById<ActiveSkillsConfig>(skillId).icon;
        }
        else if (skillId < 3000000)
        {
            icon_name = ConfigDataBase.GetConfigDataById<SummonSkillsConfig>(skillId).icon;
        }
        else 
        {
            icon_name = ConfigDataBase.GetConfigDataById<PassiveSkillsConfig>(skillId).icon;
        }

        return atlas.GetSprite(icon_name);
    }
}
