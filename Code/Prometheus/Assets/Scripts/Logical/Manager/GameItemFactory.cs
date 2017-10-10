using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItemFactory : SingleObject<GameItemFactory>
{
    GameObject monster_1;
    GameObject player_1;

    public Monster CreateMonster(int pwr, ulong id, int lv, Brick bornBrick)
    {
        #region 基础属性

        if (monster_1 == null) monster_1 = Resources.Load("Prefab/Monster") as GameObject;

        var go = GameObject.Instantiate(monster_1, StageView.Instance.itemRoot) as GameObject;

        go.transform.localScale = Vector3.one;

        go.transform.position = bornBrick.transform.position;

        Monster monster = go.GetComponent<Monster>();

        monster.standBrick = bornBrick;

        MonsterConfig config = ConfigDataBase.GetConfigDataById<MonsterConfig>(id);

        SuperArray<float> propertys = config.propertys;

        monster.config = config;


        MonsterLevelDataConfig lv_Property = ConfigDataBase.GetConfigDataById<MonsterLevelDataConfig>((ulong)lv);

        monster.baseProperty.InitBaseProperty(
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

        if (config.skill_normal != null)
        {
            skill_Count = config.skill_normal.Count();


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
        }
        StageCore.Instance.RegisterMonster(monster);

        return monster;
    }

    public Player CreatePlayer(Brick bornBrick)
    {
        if (player_1 == null) player_1 = Resources.Load("Prefab/Player") as GameObject;

        var go = GameObject.Instantiate(player_1, StageView.Instance.itemRoot) as GameObject;

        go.transform.localScale = Vector3.one;

        go.transform.position = bornBrick.transform.position;

        var player = go.GetComponent<Player>();

        player.standBrick = bornBrick;

        BrickCore.Instance.OpenNearbyBrick(bornBrick.pathNode.x, bornBrick.pathNode.z);

        StageCore.Instance.RegisterPlayer(player);

        return player;
    }

    public Supply CreateSupply(ulong uid, Brick bornBrick)
    {
        var go = GameObject.Instantiate(Resources.Load("Prefab/Supply"), StageView.Instance.itemRoot) as GameObject;

        go.transform.localScale = Vector3.one;

        go.transform.position = bornBrick.transform.position;

        var supply = go.GetComponent<Supply>();

        supply.standBrick = bornBrick;

        supply.config = ConfigDataBase.GetConfigDataById<SupplyConfig>(uid);

        return supply;
    }

    public Tablet CreateTablet(ulong uid, Brick bornBrick)
    {
        var go = GameObject.Instantiate(Resources.Load("Prefab/Tablet"), StageView.Instance.itemRoot) as GameObject;

        go.transform.localScale = Vector3.one;

        go.transform.position = bornBrick.transform.position;

        var tablet = go.GetComponent<Tablet>();

        tablet.standBrick = bornBrick;

        tablet.config = ConfigDataBase.GetConfigDataById<TotemConfig>(uid);

        return tablet;
    }

    public Treasure CreateTreasure(Brick bornBrick)
    {
        var go = GameObject.Instantiate(Resources.Load("Prefab/Treasure"), StageView.Instance.itemRoot) as GameObject;

        go.transform.localScale = Vector3.one;

        go.transform.position = bornBrick.transform.position;

        var treasure = go.GetComponent<Treasure>();

        treasure.standBrick = bornBrick;

        return treasure;
    }
}
