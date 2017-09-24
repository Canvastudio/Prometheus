using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCore : SingleObject<StageCore> {

    Dictionary<ulong, Monster> monsterDic = new Dictionary<ulong, Monster>();

    public Monster CreateMonster(int pwr, ulong id, int lv)
    {
        Monster monster = new Monster();

        MonsterConfig config = ConfigDataBase.GetConfigDataById<MonsterConfig>(id);

        SuperArray<float> propertys = config.propertys;

        monster.config = config;
       
        monster.property = new MonsterProperty();

        MonsterLevelDataConfig lv_Property = ConfigDataBase.GetConfigDataById<MonsterLevelDataConfig>((ulong)lv);

        monster.property.InitBaseProperty(
            lv_Property.m_mhp * propertys[pwr ,0],
            lv_Property.m_speed * propertys[pwr, 1],
            lv_Property.m_melee * propertys[pwr, 2],
            lv_Property.m_laser * propertys[pwr, 3],
            lv_Property.m_cartridge * propertys[pwr, 4]
            );

        int skill_Count = config.skill_normal.Count();

        for(int i = 0; i < skill_Count; ++i)
        {
            string type = config.skill_normal[i, 0];
            string skill_Id = config.skill_normal[i, 1];

            switch (type)
            {
                case "p":
                    PassiveSkillsConfig pconfig = ConfigDataBase.GetConfigDataById<PassiveSkillsConfig>(skill_Id);
                    monster.fightComponet.passiveSkillConfigs.Add(pconfig);
                    break;
                case "a":
                    ActiveSkillsConfig aconfig = ConfigDataBase.GetConfigDataById<ActiveSkillsConfig>(skill_Id);
                    monster.fightComponet.activeSkillConfigs.Add(aconfig);
                    break;
                case "s":
                    SummonSkillsConfig sconfig = ConfigDataBase.GetConfigDataById<SummonSkillsConfig>(skill_Id);
                    monster.fightComponet.summonSkillConfigs.Add(sconfig);
                    break;
            }
        }

        RegisterMonster(monster);

        return monster;
    }

    private void RegisterMonster(Monster newMonster)
    {
        monsterDic.Add(newMonster.uid, newMonster);
    }
}
