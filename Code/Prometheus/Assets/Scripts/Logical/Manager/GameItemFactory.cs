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

        var go = GameObject.Instantiate(monster_1, StageView.Instance.liveItemRoot) as GameObject;

        go.transform.localScale = Vector3.one;

        go.transform.position = bornBrick.transform.position;

        Monster monster = go.GetComponent<Monster>();

        monster.standBrick = bornBrick;

        MonsterConfig config = ConfigDataBase.GetConfigDataById<MonsterConfig>(id);

        SuperArray<float> propertys = config.propertys;

        monster.config = config;

        MonsterLevelDataConfig lv_Property = ConfigDataBase.GetConfigDataById<MonsterLevelDataConfig>((ulong)lv);

        monster.property.InitBaseProperty(
            lv_Property.mhp * propertys[pwr, 0],
            lv_Property.speed * propertys[pwr, 1],
            lv_Property.melee * propertys[pwr, 2],
            lv_Property.laser * propertys[pwr, 3],
            lv_Property.cartridge * propertys[pwr, 4]
            );

        monster.InitInfoUI();

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

        StageCore.Instance.RegisterItem(monster);

        monster.pwr = pwr;
        monster.cid = id;
        monster.lv = lv;

        if (bornBrick.brickExplored == BrickExplored.EXPLORED)
        {
            monster.OnDiscoverd();
        }

        return monster;
    }

    /// <summary>
    /// 创建也给玩家角色 并初始化他得基本属性和特殊属性
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="bornBrick"></param>
    /// <returns></returns>
    public Player CreatePlayer(ulong uid, Brick bornBrick)
    {
        if (player_1 == null) player_1 = Resources.Load("Prefab/Player") as GameObject;

        var go = GameObject.Instantiate(player_1, StageView.Instance.liveItemRoot) as GameObject;

        go.transform.localScale = Vector3.one;

        var player = go.GetComponent<Player>();

        player.standBrick = bornBrick;

        var config = ConfigDataBase.GetConfigDataById<PlayerInitConfig>(uid);
        
        player.config = config;

        player.property.InitBaseProperty(
            config.mhp,
            config.speed,
            config.melee,
            config.laser,
            config.cartridge
        );

        player.SetPlayerProperty(config.pmotorized, config.capacity, config.atkSpeed, config.reloadSpeed);

        //BrickCore.Instance.OpenNearbyBrick(bornBrick.pathNode.x, bornBrick.pathNode.z);

        StageCore.Instance.RegisterItem(player);

        player.InitInfoUI();

        return player;
    }

    public Supply CreateSupply(ulong uid, Brick bornBrick)
    {
        var go = GameObject.Instantiate(Resources.Load("Prefab/Supply"), bornBrick.transform) as GameObject;

        go.transform.localScale = Vector3.one;

        go.transform.SetSiblingIndex(2);

        var supply = go.GetComponent<Supply>();

        supply.standBrick = bornBrick;

        supply.config = ConfigDataBase.GetConfigDataById<SupplyConfig>(uid);

        StageCore.Instance.RegisterItem(supply);

        return supply;
    }

    public Maintenance CreateMaintenance(Brick bornBrick)
    {
        var go = GameObject.Instantiate(Resources.Load("Prefab/Maintenance"), bornBrick.transform) as GameObject;

        go.transform.localScale = Vector3.one;

        go.transform.SetSiblingIndex(2);

        var Maintence = go.GetComponent<Maintenance>();

        Maintence.standBrick = bornBrick;

        //supply.config = ConfigDataBase.GetConfigDataById<SupplyConfig>(uid);

        StageCore.Instance.RegisterItem(Maintence);

        return Maintence;
    }

    public Tablet CreateTablet(ulong uid, Brick bornBrick)
    {
        var go = GameObject.Instantiate(Resources.Load("Prefab/Tablet"), bornBrick.transform) as GameObject;

        go.transform.localScale = Vector3.one;

        go.transform.SetSiblingIndex(2);

        var tablet = go.GetComponent<Tablet>();

        tablet.standBrick = bornBrick;

        tablet.config = ConfigDataBase.GetConfigDataById<TotemConfig>(uid);

        StageCore.Instance.RegisterItem(tablet);

        return tablet;
    }

    public Treasure CreateTreasure(Brick bornBrick, ulong uid, int distance)
    {
        var go = GameObject.Instantiate(Resources.Load("Prefab/Treasure"), bornBrick.transform) as GameObject;

        go.transform.localScale = Vector3.one;

        go.transform.SetSiblingIndex(2);

        var treasure = go.GetComponent<Treasure>();

        treasure.standBrick = bornBrick;

        treasure.distance = distance;

        treasure.config = ConfigDataBase.GetConfigDataById<BoxConfig>(uid);

        ///宝箱比较复杂，需要去初始化一些东西
        treasure.Init();

        StageCore.Instance.RegisterItem(treasure);

        return treasure;
    }
}
