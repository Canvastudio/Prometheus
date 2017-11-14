using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItemFactory : SingleObject<GameItemFactory>
{
    GameObject player_1;

    public string monster_pool = "MPI";
    public string obstacle_pool = "OBE";

    protected override void Init()
    {
        base.Init();
        var monster_go = Resources.Load("Prefab/Monster") as GameObject;
        var monster = monster_go.GetComponent<Monster>();

        var obstacle_go = Resources.Load("Prefab/Obstacle") as GameObject;
        var obstacle = obstacle_go.GetComponent<Obstacle>();

        ObjPool<Monster>.Instance.InitOrRecyclePool(monster_pool, monster, 6);
        ObjPool<Obstacle>.Instance.InitOrRecyclePool(obstacle_pool, obstacle, 6);
    }

    private void AddSkillToFightComponet(FightComponet fightComponet, SuperArrayValue<ulong> skill)
    {
        int skill_Count = 0;

        if (skill != null)
        {
            skill_Count = skill.Count();

            var skills = skill.ToArray();

            for (int i = 0; i < skill_Count; ++i)
            {
                var ulong_id = skills[i];

                fightComponet.AddSkill(ulong_id);
            }
        }
    }

    public void CreateMonster(int pwr, ulong id, int lv, Brick bornBrick)
    {
        int tid;

        var monster = ObjPool<Monster>.Instance.GetObjFromPoolWithID(out tid, monster_pool);

        monster.itemId = tid;

        var go = monster.gameObject;

        go.SetActive(true);

        go.transform.SetParent(StageView.Instance.liveItemRoot);

        go.transform.localScale = Vector3.one;

        go.transform.position = bornBrick.transform.position;

        monster.standBrick = bornBrick;

        MonsterConfig config = ConfigDataBase.GetConfigDataById<MonsterConfig>(id);

        SuperArrayValue<float> propertys = config.propertys;

        monster.config = config;

        MonsterLevelDataConfig lv_Property = ConfigDataBase.GetConfigDataById<MonsterLevelDataConfig>((ulong)lv);

        monster.Property.InitBaseProperty(
            lv_Property.mhp * propertys[pwr, 0],
            lv_Property.speed * propertys[pwr, 1],
            lv_Property.melee * propertys[pwr, 2],
            lv_Property.laser * propertys[pwr, 3],
            lv_Property.cartridge * propertys[pwr, 4]
            );

        monster.InitInfoUI();


        FightComponet fightComponet = monster.GetOrAddComponet<MonsterFightComponet>();

        AddSkillToFightComponet(fightComponet, config.skill_normal);

        if (pwr == 0)
        {
            monster.pwrFrame.sprite = StageView.Instance.itemAtlas.GetSprite("border_m_0");
        }
        else
        if (pwr == 1)
        {
            AddSkillToFightComponet(fightComponet, config.skill_rare);
            monster.pwrFrame.sprite = StageView.Instance.itemAtlas.GetSprite("border_m_1");
        }
        else
        if (pwr == 2)
        {
            AddSkillToFightComponet(fightComponet, config.skill_elite);
            monster.pwrFrame.sprite = StageView.Instance.itemAtlas.GetSprite("border_m_2");
        }
        else
        if (pwr == 3)
        {
            AddSkillToFightComponet(fightComponet, config.skill_boss);
            monster.pwrFrame.sprite = StageView.Instance.itemAtlas.GetSprite("border_m_3");
        }

        monster.fightComponet = fightComponet;

        monster.monsterType = config.monsterType;


        monster.pwr = pwr;
        monster.cid = id;
        monster.lv = lv;

        monster.icon.sprite = StageView.Instance.itemAtlas.GetSprite(config.icon);

        ulong AI_Id = config.ai[pwr];

        monster.AIConfig = ConfigDataBase.GetConfigDataById<AIConfig>(AI_Id);
        monster.dangerousLevels = monster.AIConfig.dangerous_levels;

        if (bornBrick.brickExplored == BrickExplored.EXPLORED)
        {
            CoroCore.Instance.StartCoroutine(monster.OnDiscoverd());
        }

        bornBrick.item = monster;

        monster.Init();
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
        player.transform.position = bornBrick.transform.position;

        var config = ConfigDataBase.GetConfigDataById<PlayerInitConfig>(uid);
        
        player.config = config;

        player.Property.InitBaseProperty(
            config.mhp,
            config.speed,
            config.melee,
            config.laser,
            config.cartridge
        );

        player.SetPlayerProperty(config.pmotorized, config.capacity, config.atkSpeed, config.reloadSpeed);

        //BrickCore.Instance.OpenNearbyBrick(bornBrick.pathNode.x, bornBrick.pathNode.z);

        FightComponet fightComponet = player.GetOrAddComponet<FightComponet>();
        player.fightComponet = fightComponet;

        SkillPointsComponet skillPointsComponet = player.GetOrAddComponet<SkillPointsComponet>();
        player.skillPointsComponet = skillPointsComponet;

        player.playerId = uid;
        player.InitInfoUI();

        StageCore.Instance.RegisterItem(player);

        fightComponet.ActivePassive();

        player.Side = LiveItemSide.SIDE1;

        player.isDiscovered = true;

        return player;
    }

    public Supply CreateSupply(ulong uid, Brick bornBrick)
    {
        var go = GameObject.Instantiate(Resources.Load("Prefab/Supply"), bornBrick.transform) as GameObject;

        go.transform.SetParentAndNormalize(bornBrick.transform);

        go.transform.SetSiblingIndex(2);

        var supply = go.GetComponent<Supply>();

        supply.standBrick = bornBrick;

        supply.config = ConfigDataBase.GetConfigDataById<SupplyConfig>(uid);

        return supply;
    }

    public Maintenance CreateMaintenance(Brick bornBrick)
    {
        var go = GameObject.Instantiate(Resources.Load("Prefab/Maintenance"), bornBrick.transform) as GameObject;

        go.transform.SetParentAndNormalize(bornBrick.transform);

        go.transform.SetSiblingIndex(2);

        var Maintence = go.GetComponent<Maintenance>();

        Maintence.standBrick = bornBrick;

        //supply.config = ConfigDataBase.GetConfigDataById<SupplyConfig>(uid);


        return Maintence;
    }

    public Tablet CreateTablet(ulong uid, Brick bornBrick)
    {
        var go = GameObject.Instantiate(Resources.Load("Prefab/Tablet"), bornBrick.transform) as GameObject;

        go.transform.SetParentAndNormalize(bornBrick.transform);

        go.transform.SetSiblingIndex(2);

        var tablet = go.GetComponent<Tablet>();

        tablet.standBrick = bornBrick;

        tablet.config = ConfigDataBase.GetConfigDataById<TotemConfig>(uid);

        return tablet;
    }

    public Treasure CreateTreasure(Brick bornBrick, ulong uid, int distance)
    {
        var go = GameObject.Instantiate(Resources.Load("Prefab/Treasure"), bornBrick.transform) as GameObject;

        go.transform.SetParentAndNormalize(bornBrick.transform);

        go.transform.SetSiblingIndex(2);

        var treasure = go.GetComponent<Treasure>();

        treasure.standBrick = bornBrick;

        treasure.distance = distance;

        treasure.config = ConfigDataBase.GetConfigDataById<BoxConfig>(uid);

        ///宝箱比较复杂，需要去初始化一些东西
        treasure.Init();

        return treasure;
    }

    public Obstacle CreateObstacle(Brick bornBrick)
    {
        int tid;

        var item = ObjPool<Obstacle>.Instance.GetObjFromPoolWithID(out tid, obstacle_pool);

        item.itemId = tid;

        item.transform.position = bornBrick.transform.position;

        item.transform.SetParent(StageView.Instance.NonliveItemRoot);
        item.transform.localScale = Vector3.one;
        item.standBrick = bornBrick;

        item.transform.position = bornBrick.transform.position;
        item.gameObject.SetActive(true);
        return item;
    }
}
