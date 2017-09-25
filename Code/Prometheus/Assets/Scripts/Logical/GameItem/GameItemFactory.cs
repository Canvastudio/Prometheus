using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItemFactory : SingleObject<GameItemFactory> {

    public Monster CreateMonster(int pwr, ulong id, int lv, Vector3 worldPostion)
    {
        #region 基础属性
        //TODO:替换成正式的生成一个游戏中的对象
        var go = GameObject.Instantiate(Resources.Load("Prefab/Monster"), StageView.Instance.monsterRoot) as GameObject;

        go.transform.localScale = Vector3.one;
        go.transform.position = worldPostion;

        Monster monster = go.GetComponent<Monster>();

        MonsterConfig config = ConfigDataBase.GetConfigDataById<MonsterConfig>(id);
        
        SuperArray<float> propertys = config.propertys;

        monster.config = config;

        monster.property = new MonsterProperty();

        MonsterLevelDataConfig lv_Property = ConfigDataBase.GetConfigDataById<MonsterLevelDataConfig>((ulong)lv);
        
        monster.property.InitBaseProperty(
            lv_Property.m_mhp * propertys[pwr, 0],
            lv_Property.m_speed * propertys[pwr, 1],
            lv_Property.m_melee * propertys[pwr, 2],
            lv_Property.m_laser * propertys[pwr, 3],
            lv_Property.m_cartridge * propertys[pwr, 4]
            );
        #endregion
        //添加战斗组件
        #region 战斗组件
        FightComponet fightComponet = monster.GetOrAddComponet<FightComponet>();

        int skill_Count = 0;

        try
        {
            skill_Count = config.skill_normal.Count();
        }
        catch 
        {
            Debug.Log("");
        }

        for (int i = 0; i < skill_Count; ++i)
        {
            string type = config.skill_normal[i, 0];
            string skill_Id = config.skill_normal[i, 1];

            switch (type)
            {
                case "p":
                    PassiveSkillsConfig pconfig = ConfigDataBase.GetConfigDataById<PassiveSkillsConfig>(skill_Id);
                    fightComponet.passiveSkillConfigs.Add(pconfig);
                    break;
                case "a":
                    ActiveSkillsConfig aconfig = ConfigDataBase.GetConfigDataById<ActiveSkillsConfig>(skill_Id);
                    fightComponet.activeSkillConfigs.Add(aconfig);
                    break;
                case "s":
                    SummonSkillsConfig sconfig = ConfigDataBase.GetConfigDataById<SummonSkillsConfig>(skill_Id);
                    fightComponet.summonSkillConfigs.Add(sconfig);
                    break;
            }
        }

        fightComponet.SortAcitveSkill();
        #endregion

        StageCore.Instance.RegisterMonster(monster);

        return monster;
    }

    public Player CreatePlayer(Vector3 worldPostion)
    {
        var go = GameObject.Instantiate(Resources.Load("Prefab/Player"), StageView.Instance.monsterRoot) as GameObject;

        go.transform.localScale = Vector3.one;
        go.transform.position = worldPostion;

        return go.GetComponent<Player>();
    }


}
