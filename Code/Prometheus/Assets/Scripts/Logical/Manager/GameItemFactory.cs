using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItemFactory : SingleObject<GameItemFactory>
{
    GameObject player_1;

    public string monster_pool = "MPI";
    public string obstacle_pool = "OBE";

    public string treasure_pool1 = "TRP1";
    public string treasure_pool2 = "TRP2";
    public string treasure_pool3 = "TRP3";
    public string treasure_pool4 = "TRP4";

    public string tablet_pool1 = "TAT1";
    public string tablet_pool2 = "TAT2";
    public string tablet_pool3 = "TAT3";
    public string tablet_pool4 = "TAT4";
    public string maintenance_pool = "MTE";
    public string supply1_pool = "SUY1";
    public string supply2_pool = "SUY2";
    public string supply3_pool = "SUY3";

    public string cover_pool = "CER";

    protected override void Init()
    {
        base.Init();

        var monster_go = Resources.Load("Prefab/Monster") as GameObject;
        var monster = monster_go.GetComponent<Monster>();

        var obstacle_go = Resources.Load("Prefab/Obstacle") as GameObject;
        var obstacle = obstacle_go.GetComponent<Obstacle>();

        var treasure_go1 = Resources.Load("Prefab/Treasure1") as GameObject;
        var treasure1 = treasure_go1.GetComponent<Treasure>();
        var treasure_go2 = Resources.Load("Prefab/Treasure2") as GameObject;
        var treasure2 = treasure_go2.GetComponent<Treasure>();
        var treasure_go3 = Resources.Load("Prefab/Treasure3") as GameObject;
        var treasure3 = treasure_go3.GetComponent<Treasure>();
        var treasure_go4 = Resources.Load("Prefab/Treasure4") as GameObject;
        var treasure4 = treasure_go4.GetComponent<Treasure>();

        ObjPool<Treasure>.Instance.InitOrRecyclePool(treasure_pool1, treasure1, 2);
        ObjPool<Treasure>.Instance.InitOrRecyclePool(treasure_pool2, treasure2, 2);
        ObjPool<Treasure>.Instance.InitOrRecyclePool(treasure_pool3, treasure3, 2);
        ObjPool<Treasure>.Instance.InitOrRecyclePool(treasure_pool4, treasure4, 2);

        var tablet_go1 = Resources.Load("Prefab/Tablet1") as GameObject;
        var tablet1 = tablet_go1.GetComponent<Tablet>();
        var tablet_go2 = Resources.Load("Prefab/Tablet2") as GameObject;
        var tablet2 = tablet_go2.GetComponent<Tablet>();
        var tablet_go3 = Resources.Load("Prefab/Tablet3") as GameObject;
        var tablet3 = tablet_go3.GetComponent<Tablet>();
        var tablet_go4 = Resources.Load("Prefab/Tablet4") as GameObject;
        var tablet4 = tablet_go4.GetComponent<Tablet>();
        ObjPool<Tablet>.Instance.InitOrRecyclePool(tablet_pool1, tablet1, 2);
        ObjPool<Tablet>.Instance.InitOrRecyclePool(tablet_pool2, tablet2, 2);
        ObjPool<Tablet>.Instance.InitOrRecyclePool(tablet_pool3, tablet3, 2);
        ObjPool<Tablet>.Instance.InitOrRecyclePool(tablet_pool4, tablet4, 2);

        var maintenance_go = Resources.Load("Prefab/Maintenance") as GameObject;
        var maintenance = maintenance_go.GetComponent<Maintenance>();

        var supply1_go = Resources.Load("Prefab/Supply1") as GameObject;
        var supply1 = supply1_go.GetComponent<Supply>();
        var supply2_go = Resources.Load("Prefab/Supply2") as GameObject;
        var supply2 = supply2_go.GetComponent<Supply>();
        var supply3_go = Resources.Load("Prefab/Supply3") as GameObject;
        var supply3 = supply3_go.GetComponent<Supply>();

        ObjPool<Supply>.Instance.InitOrRecyclePool(supply1_pool, supply1, 2);
        ObjPool<Supply>.Instance.InitOrRecyclePool(supply2_pool, supply2, 2);
        ObjPool<Supply>.Instance.InitOrRecyclePool(supply3_pool, supply3, 2);

        ObjPool<Monster>.Instance.InitOrRecyclePool(monster_pool, monster, 6);
        ObjPool<Obstacle>.Instance.InitOrRecyclePool(obstacle_pool, obstacle, 6);
        ObjPool<Maintenance>.Instance.InitOrRecyclePool(maintenance_pool, maintenance, 3);

        var cover_go = Resources.Load("Prefab/BrickCover") as GameObject;
        var cover = cover_go.GetComponent<Cover>();

        ObjPool<Cover>.Instance.InitOrRecyclePool(cover_pool, cover, 50);
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

    public Monster CreateMonster(int pwr, ulong id, int lv, Brick bornBrick)
    {
        if (GameTestData.Instance.SuperMonster)
        {
            pwr = 3;
        }

        ulong tid;

        var item = ObjPool<Monster>.Instance.GetObjFromPoolWithID(out tid, monster_pool);

        item.itemId = tid;

        var go = item.gameObject;

        go.SetActive(true);

        go.transform.SetParent(StageView.Instance.uper);
        go.transform.SetAsFirstSibling();

        go.transform.localScale = Vector3.one;

        go.transform.position = bornBrick.transform.position;

        item.standBrick = bornBrick;

        if (item.state == null)
            item.state = item.GetOrAddComponet<StateComponent>();

        MonsterConfig config = ConfigDataBase.GetConfigDataById<MonsterConfig>(id);

        SuperArrayValue<float> propertys = config.propertys;

        item.config = config;

        MonsterLevelDataConfig lv_Property = ConfigDataBase.GetConfigDataById<MonsterLevelDataConfig>((ulong)lv);

        item.Property.InitBaseProperty(
            lv_Property.mhp * propertys[pwr, 0],
            lv_Property.speed * propertys[pwr, 1],
            lv_Property.melee * propertys[pwr, 2],
            lv_Property.laser * propertys[pwr, 3],
            lv_Property.cartridge * propertys[pwr, 4],
            //lv_Property.attack * propertys[pwr, 5]
            20
            );

        item.InitInfoUI();


        MonsterFightComponet fightComponet = item.GetOrAddComponet<MonsterFightComponet>();

        fightComponet.activeInsList.Clear();
        fightComponet.passiveInsList.Clear();
        fightComponet.summonSkillConfigs.Clear();
        fightComponet.monsterActiveInsList.Clear();

        if (pwr == 0)
        {
            item.pwrFrame.sprite = StageView.Instance.itemAtlas.GetSprite("border_m_0");
            AddSkillToFightComponet(fightComponet, config.skill_normal);
        }
        else
        if (pwr == 1)
        {
            AddSkillToFightComponet(fightComponet, config.skill_rare);
            item.pwrFrame.sprite = StageView.Instance.itemAtlas.GetSprite("border_m_1");
        }
        else
        if (pwr == 2)
        {
            AddSkillToFightComponet(fightComponet, config.skill_elite);
            item.pwrFrame.sprite = StageView.Instance.itemAtlas.GetSprite("border_m_2");
        }
        else
        if (pwr == 3)
        {
            AddSkillToFightComponet(fightComponet, config.skill_boss);
            item.pwrFrame.sprite = StageView.Instance.itemAtlas.GetSprite("border_m_3");
        }

        item.fightComponet = fightComponet;

        item.monsterType = config.monsterType;


        item.pwr = pwr;
        item.cid = id;
        item.lv = lv;
        item.isAlive = true;
        item.icon.SetStageItemIcon(config.icon);

        ulong AI_Id = config.ai[pwr];

        item.AIConfig = ConfigDataBase.GetConfigDataById<AIConfig>(AI_Id);
        item.dangerousLevels = item.AIConfig.dangerous_levels;

        bornBrick.item = item;

        item.monsterName.text = config.m_name;

        item.Side = LiveItemSide.SIDE0;

        item.ListenInit();

        item.standBrick.item = item;
        item.standBrick.brickType = BrickType.MONSTER;

#if UNITY_EDITOR
        item.name = config.m_name + "_" + tid;
#endif
        if (bornBrick.brickExplored == BrickExplored.EXPLORED)
        {
            CoroCore.Instance.StartCoroutine(item.OnDiscoverd());
        }

        return item;
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

        var go = GameObject.Instantiate(player_1, StageView.Instance.top) as GameObject;

        go.transform.localScale = Vector3.one;

        var player = go.GetComponent<Player>();

        player.isAlive = true;

        player.standBrick = bornBrick;

        player.transform.position = bornBrick.transform.position;

        player.state = player.GetOrAddComponet<StateComponent>();

        player.itemId = GlobalUid.Instance.GetUid();

        var config = ConfigDataBase.GetConfigDataById<PlayerInitConfig>(uid);
        
        player.config = config;
        player.monsterType = config.playerType;

        if (GameTestData.Instance.SuperPlayer)
        {
            player.Property.InitBaseProperty(
                9999 ,
                2,
                999,
                999,
                999,
               999
            );
        }
        else
        {
            player.Property.InitBaseProperty(
                config.mhp,
                config.speed,
                config.melee,
                config.laser,
                config.cartridge,
                config.attack
            );
        }

        player.SetPlayerProperty(config.pmotorized, config.capacity, config.atkSpeed, config.reloadSpeed);

        FightComponet fightComponet = player.GetOrAddComponet<FightComponet>();
        player.fightComponet = fightComponet;

        SkillPointsComponet skillPointsComponet = player.GetOrAddComponet<SkillPointsComponet>();
        player.skillPointsComponet = skillPointsComponet;

        player.playerId = uid;
        player.InitInfoUI();

        StageCore.Instance.RegisterItem(player);

        fightComponet.ActivePassive();

        player.Side = LiveItemSide.SIDE1;

        player.OnDiscoverd();

        return player;
    }

    public Supply CreateSupply(ulong uid, Brick bornBrick)
    {
        ulong tid;

        Supply item;

        if (uid == 101)
        {
            item = ObjPool<Supply>.Instance.GetObjFromPoolWithID(out tid, supply1_pool);
            item.pool_name = supply1_pool;
        }
        else if (uid == 102)
        {
            item = ObjPool<Supply>.Instance.GetObjFromPoolWithID(out tid, supply2_pool);
            item.pool_name = supply2_pool;
        }
        else
        {
            item = ObjPool<Supply>.Instance.GetObjFromPoolWithID(out tid, supply3_pool);
            item.pool_name = supply3_pool;
        }

        item.itemId = tid;
        item.transform.SetParentAndNormalize(StageView.Instance.uper);
        item.transform.SetAsFirstSibling();
        item.transform.position = bornBrick.transform.position;
        item.standBrick = bornBrick;
        item.config = ConfigDataBase.GetConfigDataById<SupplyConfig>(uid);
        item.ListenInit();
        item.icon.SetNativeSize();
        //CoroCore.Instance.StartCoroutine(item.standBrick.OnDiscoverd());
        return item;
    }

    public Maintenance CreateMaintenance(Brick bornBrick)
    {
        ulong tid;

        var item = ObjPool<Maintenance>.Instance.GetObjFromPoolWithID(out tid, maintenance_pool);

        item.itemId = tid;
        item.transform.SetParentAndNormalize(StageView.Instance.uper);
        item.transform.SetAsFirstSibling();
        item.transform.position = bornBrick.transform.position;
        item.standBrick = bornBrick;

        if (item.canvasGroup == null)
        {
            item.canvasGroup = item.GetOrAddComponet<CanvasGroup>();
        }

        item.ListenInit();
        item.icon.SetNativeSize();
        return item;
    }

    public Tablet CreateTablet(ulong uid, Brick bornBrick)
    {
        ulong tid;
        Tablet item = null;

        if (uid == 1)
        {
            item = ObjPool<Tablet>.Instance.GetObjFromPoolWithID(out tid, tablet_pool1);
        }
        else if (uid == 2)
        {
            item = ObjPool<Tablet>.Instance.GetObjFromPoolWithID(out tid, tablet_pool2);
        }
        else if (uid == 3)
        {
            item = ObjPool<Tablet>.Instance.GetObjFromPoolWithID(out tid, tablet_pool3);
        }
        else 
        {
            item = ObjPool<Tablet>.Instance.GetObjFromPoolWithID(out tid, tablet_pool4);
        }

        item.itemId = tid;
        item.transform.SetParentAndNormalize(StageView.Instance.uper);
        item.transform.SetAsFirstSibling();
        item.transform.position = bornBrick.transform.position;
        item.standBrick = bornBrick;

        item.config = ConfigDataBase.GetConfigDataById<TotemConfig>(uid);

        if (item.canvasGroup == null)
        {
            item.canvasGroup = item.GetOrAddComponet<CanvasGroup>();
        }

        item.ListenInit();
        item.icon.SetNativeSize();
        return item;
    }

    public Treasure CreateTreasure(Brick bornBrick, ulong uid, int distance)
    {
        ulong tid;

        Treasure item;

        if (uid == 201)
        {
            item = ObjPool<Treasure>.Instance.GetObjFromPoolWithID(out tid, treasure_pool1);
            item.pool_name = treasure_pool1;
        }
        else if (uid == 202)
        {
            item = ObjPool<Treasure>.Instance.GetObjFromPoolWithID(out tid, treasure_pool2);
            item.pool_name = treasure_pool2;
        }
        else if (uid == 203)
        {
            item = ObjPool<Treasure>.Instance.GetObjFromPoolWithID(out tid, treasure_pool3);
            item.pool_name = treasure_pool3;
        }
        else 
        {
            item = ObjPool<Treasure>.Instance.GetObjFromPoolWithID(out tid, treasure_pool4);
            item.pool_name = treasure_pool4;
        }

        item.itemId = tid;
        item.transform.SetParentAndNormalize(StageView.Instance.uper);
        item.transform.SetAsFirstSibling();
        item.transform.position = bornBrick.transform.position;
        item.standBrick = bornBrick;

        item.distance = distance;

        item.config = ConfigDataBase.GetConfigDataById<BoxConfig>(uid);

        ///宝箱比较复杂，需要去初始化一些东西
        item.Init();

        //CoroCore.Instance.StartCoroutine(item.standBrick.OnDiscoverd());

        item.ListenInit();
        item.icon.SetNativeSize();
        return item;
    }

    public Obstacle CreateObstacle(Brick bornBrick)
    {
        ulong tid;

        var item = ObjPool<Obstacle>.Instance.GetObjFromPoolWithID(out tid, obstacle_pool);

        item.itemId = tid;

        item.transform.position = bornBrick.transform.position;
        item.itemId = tid;
        item.transform.SetParentAndNormalize(StageView.Instance.cover);
        item.transform.SetAsFirstSibling();
        item.transform.position = bornBrick.transform.position;
        item.standBrick = bornBrick;

        item.ListenInit();
        item.icon.SetBlockIcon(bornBrick.row, bornBrick.column);

        return item;
    }

    public Cover CreateCover(Brick bornBrick)
    {
        ulong tid;

        var item = ObjPool<Cover>.Instance.GetObjFromPoolWithID(out tid, cover_pool);

        item.itemId = tid;

        item.transform.SetParentAndNormalize(StageView.Instance.cover);
        item.transform.SetAsFirstSibling();
        item.transform.localScale = Vector3.one;

        if (item.standBrick != null)
        {
            Debug.LogError("Cover已经被使用了吗？");
        }

        item.standBrick = bornBrick;
        item.transform.position = bornBrick.transform.position;
        item.gameObject.SetActive(true);
        item.icon.SetCoverIcon(bornBrick.row, bornBrick.column);
        bornBrick.cover = item;

        item.ListenInit();

        if (GameTestData.Instance.alwaysShow)
        {
            item.icon.color = new Color(1, 1, 1, 0.5f);
        }

        return item;
    }

    public void CreateOrgan(ulong uid, Brick bornBrick)
    {
        OperateConfig config = ConfigDataBase.GetConfigDataById<OperateConfig>(uid);

        string prefab = config.prefab;

        var go = GameObject.Instantiate((Resources.Load("ground/" + prefab) as GameObject), StageView.Instance.uper);

#if UNITY_EDITOR
        go.name = "Organ_" + config.operate.ToString();
#endif
        switch(config.operate)
        {
            case Operate.ActiveSkills:
                var organ = go.AddComponent<ActiveSkillOrgan>();
                organ.config = config.arg1;
                organ.standBrick = bornBrick;
                bornBrick.brickType = BrickType.Organ;
                bornBrick.item = organ;
                organ.baseConfig = config;
                break;
            case Operate.Radar:
                var rader = go.AddComponent<RadarOrgan>();
                int min = config.arg5[0];
                int max = config.arg5[1];
                rader.openCount = Random.Range(min, max);
                rader.standBrick = bornBrick;
                bornBrick.brickType = BrickType.Organ;
                bornBrick.item = rader;
                rader.baseConfig = config;
                break;
            case Operate.Property:
                var property = go.AddComponent<PropertyOrgan>();
                property.config = config;
                property.standBrick = bornBrick;
                bornBrick.brickType = BrickType.OrganProperty;
                bornBrick.item = property;
                property.baseConfig = config;
                break;
            case Operate.AddSkill:
                var add = go.AddComponent<AddSkillOrgan>();
                add.config = config.arg1;
                min = config.arg5[0];
                max = config.arg5[1];
                add.count = Random.Range(min, max);
                add.standBrick = bornBrick;
                bornBrick.brickType = BrickType.Organ;
                bornBrick.item = add;
                add.baseConfig = config;
                break;
            case Operate.SummonSkill:
                var summon = go.AddComponent<SummonOrgan>();
                summon.summonConfig = config.arg2;
                summon.standBrick = bornBrick;
                bornBrick.brickType = BrickType.Organ;
                bornBrick.item = summon;
                summon.baseConfig = config;
                break;
            case Operate.Kelid:

                break;
        }

        go.transform.position = bornBrick.transform.position;
        
    }


}
