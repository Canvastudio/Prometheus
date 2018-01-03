using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class Brick : GameItemBase, IEquatable<Brick> {

    [SerializeField]
    Image picture;

    [SerializeField]
    public Cover cover;

    public int rowInModuel;

    public bool last_row;
    public Transform blockMask;
    public int uid;
    public ulong moduel_id = 0;
    public ulong level_id = 0;

    public bool isFire;

    public BrickType brickType
    {
        get
        {
            if (brickExplored == BrickExplored.UNEXPLORED && _brickType != BrickType.OBSTACLE)
                return BrickType.UNKNOWN;
            else
                return _brickType;
        }
        set
        {
            _brickType = value;

            RefreshWalkableAndBlockState();
        }
    }

    public BrickType realBrickType
    {
        get
        {
            return _brickType;
        }
    }

    [SerializeField]
    private BrickType _brickType;

    public BrickExplored brickExplored
    {
        get
        {
            return _brickExplored;
        }
        set
        {
            if (value == BrickExplored.UNEXPLORED)
            {
                //icon.sprite  = StageView.Instance.itemAtlas.GetSprite(BrickNameCore.Instance.GetUnExploredBrickName());
            }
            else
            if (value == BrickExplored.EXPLORED)
            {
                //if (column == 0)
                //{
                //    icon.sprite = StageView.Instance.itemAtlas.GetSprite(BrickNameCore.Instance.GetSideBrickName());
                //}
                //else if (column == 5)
                //{
                //    icon.sprite = StageView.Instance.itemAtlas.GetSprite(BrickNameCore.Instance.GetSideBrickName());
                //    icon.transform.localScale = new Vector3(-1, 1, 1);
                //}
                //else
                //{
                //    icon.sprite = StageView.Instance.itemAtlas.GetSprite(BrickNameCore.Instance.GetExploredBrickName());
                //}
            }

            _brickExplored = value;

            RefreshWalkableAndBlockState();
        }
    }

    [SerializeField]
    private BrickExplored _brickExplored = BrickExplored.UNEXPLORED;

    public int brickBlock
    {
        get
        {
            return _brickBlock;
        }
        set
        {
            _brickBlock = value;

            RefreshWalkableAndBlockState();
        }
    }

    [SerializeField]
    private int _brickBlock = 0;

    public int row
    {
        get
        {
            return _row;
        }
    }

    public int column
    {
        get
        {
            return _column;
        }
    }
    [SerializeField]
    private int _row;
    [SerializeField]
    private int _column;

    public HaloComponent haloComponent;

    /// <summary>
    /// 当前格子得怪物，为空表示没有怪物
    /// </summary>
    [SerializeField]
    private GameItemBase _item;
    public GameItemBase item
    {
        get
        {
            return _item;
        }
        set
        {
            if (_item == value) return;

            _item = value;
        }
    }

    public Node pathNode
    {
        get { return _pathNode; }
    }

    [SerializeField]
    private Node _pathNode;


    string fxName;
    public ArtFxBase loopFx;

    public void ShowTipDanger()
    {
        if (loopFx != null)
        {
            Debug.LogError("一个格子上面有2个提示特效?");
        }

        fxName = "格子怪物提示";
        string name = SpecialEffectConfig.GetConfigDataByKey<SpecialEffectConfig>(fxName).effectName;
        //loopFx = StageView.Instance.ShowFxLoop(this, fxName);

        loopFx = ArtSkill.Show(name, transform.position);
    }

    public void ShowTipRich()
    {
        if (loopFx != null)
        {
            Debug.LogError("一个格子上面有2个提示特效?");
        }

        fxName = "格子宝物提示";
        //loopFx = StageView.Instance.ShowFxLoop(this, fxName);

        string name = SpecialEffectConfig.GetConfigDataByKey<SpecialEffectConfig>(fxName).effectName;
        //loopFx = StageView.Instance.ShowFxLoop(this, fxName);

        loopFx = ArtSkill.Show(name, transform.position);
    }

    protected override void OnBrickClick()
    {
        Messenger<Brick>.Invoke(SA.PlayerClickBrick.ToString(), this);
    }

    protected override void OnLongPress()
    {
        Debug.Log(string.Format("显示砖块属性，{0}", gameObject.name));

        if (GameTestData.Instance.alwaysShow || isDiscovered)
        {
            if (realBrickType == BrickType.MONSTER)
            {
                var monster = item as Monster;

                if (GameTestData.Instance.alwaysShow || monster.isDiscovered)
                {
                    StartCoroutine(MuiCore.Instance.AddOpen(UiName.strMonsterInfoView, monster));
                }
            }
            else
            {
                StageUIView.Instance.ShowItemInfo(this);
            }
        }
    }

    protected override void LongPressRelease()
    {
        StageUIView.Instance.HideItemInfo();
    }

    public override IEnumerator OnDiscoverd()
    {
        base.OnDiscoverd();

        brickExplored = BrickExplored.EXPLORED;

        isDiscovered = true;

        if (loopFx != null)
        {
            loopFx.OnEnd();
            loopFx = null;
        }

        //StartCoroutine(StageView.Instance.ShowFx(this, "格子翻开"));
        if (realBrickType != BrickType.MONSTER)
        {
            string name = SpecialEffectConfig.GetConfigDataByKey<SpecialEffectConfig>("格子翻开").effectName;
            ArtSkill.Show(name, transform.position);
        }

        yield return new WaitForSeconds(0.1f);

        if (cover != null)
        {
            IEnumerator ie = cover.OnDiscoverd();
        }
        else
        {
            Debug.Log("砖块OnDiscover的时候,cover不存在？");
        }

        if (item != null)
        {
            if (!item.isDiscovered)
            {
                if (realBrickType == BrickType.MONSTER)
                {
                    //StartCoroutine(StageView.Instance.ShowFx(standBrick, "怪物翻开"));
                    string name = SpecialEffectConfig.GetConfigDataByKey<SpecialEffectConfig>("怪物翻开").effectName;
                    ArtSkill.Show(name, transform.position);
                }

                yield return item.OnDiscoverd();
            }
        }



        if (realBrickType != BrickType.OBSTACLE)
        {
            GContext.Instance.discover_brick += 1;
        }

        GContext.Instance.dark_brick -= 1;
    }


    ulong bid;

    private void ShowBlock()
    {
        if (blockMask == null)
        {
            bid = StageView.Instance.AddBlockMask(this, ref blockMask);
        }

        blockMask.gameObject.SetActive(true);
    }

    private void HideBlock()
    {
        if (blockMask != null)
        {
            blockMask.gameObject.SetActive(false);
        }
    }

    public void RecycBlock()
    {
        if (blockMask != null)
        {
            StageView.Instance.RemoveBlockMask(bid);
        }

        blockMask = null;
    }


    public void RefreshWalkableAndBlockState()
    {
        if (brickType == BrickType.EMPTY)
        {
            if (brickBlock > 0)
            {
                ShowBlock();
                pathNode.isWalkable = false;
            }
            else
            {
                pathNode.isWalkable = true;
            }
        }
        else
        {
            if (brickType == BrickType.UNKNOWN)
            {
                if (brickBlock == 0)
                {
                    pathNode.isWalkable = true;
                    HideBlock();
                }
                else
                {
                    ShowBlock();
                    pathNode.isWalkable = false;
                }
            }
            else
            {
                pathNode.isWalkable = false;
                HideBlock();
            }
        }

        if (isFire)
        {
            pathNode.isWalkable = false;
        }

        //if (!inViewArea) pathNode.isWalkable = false;
    }

    

    public void Init(int row, int column)
    {
        _row = row;
        _column = column;

        //if (type == BrickType.OBSTACLE)
        //{
        //    picture.sprite = StageView.Instance.brickAtlas.GetSprite(Predefine.BRICK_OBSTACLE_UNREACHABLE);
        //}
        //else
        {
            //picture.sprite = StageView.Instance.brickAtlas.GetSprite(Predefine.BRICK_NORMAL_REACHABLE);
        }

        brickExplored = BrickExplored.UNEXPLORED;

        _pathNode = new Node()
        {
            //isWalkable = _brickExplored == BrickExplored.EXPLORED && _brickBlock == BrickBlock.NIL && _brickType == BrickType.EMPTY,
            isWalkable = true,
            nCost = 1f,
            behavirour = this,
            nodeType = Node.NodeType.ground,
            x = row,
            z = column
        };

        brickType = BrickType.EMPTY;
    }

    #region Add Item
    public void CreateMonster(int power, ulong uid, int lv)
    {
        //创建数据
        GameItemFactory.Instance.CreateMonster(power, uid, lv, this);

        brickType = BrickType.MONSTER;
    }

    public Brick CreateMonter()
    {
        //创建数据
        var map_Config = ConfigDataBase.GetConfigDataById<MapConfig>(level_id);

        int lv_min = map_Config.enemy_level[0];
        int lv_max = map_Config.enemy_level[1];
        int lv = UnityEngine.Random.Range(lv_min, lv_max + 1);

        var enemys = map_Config.enemys.ToArray();
        var enemy_Index = UnityEngine.Random.Range(0, enemys.Length);
        var enemy_Id = enemys[enemy_Index];

        CreateMonster(1, enemy_Id, lv);

        return this;
    }

    public Brick CreateSupply(ulong uid)
    {
        //创建数据
        item = GameItemFactory.Instance.CreateSupply(uid, this);
        brickType = BrickType.SUPPLY;
        return this;
    }

    public Brick CreateMaintence()
    {
        item = GameItemFactory.Instance.CreateMaintenance(this);
        brickType = BrickType.MAINTENANCE;
        return this;
    }

    public Brick CreateTalbet(ulong uid)
    {
        //创建数据
        item = GameItemFactory.Instance.CreateTablet(uid, this);
        brickType = BrickType.TABLET;
        return this;
    }

    public Brick CreateTreasure(ulong uid, int distance)
    {
        //创建数据
        item = GameItemFactory.Instance.CreateTreasure(this, uid, distance);
        brickType = BrickType.TREASURE;
        return this;
    }

    public Brick CreatePlayer(ulong uid)
    {
        //创建数据
        GameItemFactory.Instance.CreatePlayer(uid, this);
        brickType = BrickType.PLAYER;
        return this;
    }

    public Brick CreateObstacle()
    {
        GameItemFactory.Instance.CreateObstacle(this);
        brickType = BrickType.OBSTACLE;
        return this;
    }

    public Brick CreateOrgan(ulong uid)
    {
        GameItemFactory.Instance.CreateOrgan(uid, this);

        return this;
    }

    public Brick CreateCover()
    {
        this.cover = GameItemFactory.Instance.CreateCover(this);
        return this;
    }
    #endregion

    public bool IsLiveItemBrick()
    {
        return (item != null && item is LiveItem);
    }

    public bool Equals(Brick other)
    {
        return pathNode == other.pathNode;
    }

    public bool CheckIfRecycle()
    {
        var screen_Pos = RectTransformUtility.WorldToScreenPoint(StageView.Instance.show_camera, transform.position);

        if (screen_Pos.y < StageView.Instance.brickWidth)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Recycle()
    {
        base.Recycle();

        if (loopFx != null)
        {
            loopFx.OnEnd();
        }

        if (realBrickType != BrickType.EMPTY && item != null)
        {
            item.Recycle();
            item = null;
        }

        if (cover != null)
        {
            cover.Recycle();
            cover = null;
        }

        if (loopFx != null)
        {
            loopFx.OnEnd();
            loopFx = null;
        }

        brickBlock = 0;
        brickType = BrickType.EMPTY;
        brickExplored = BrickExplored.UNEXPLORED;
        ObjPool<Brick>.Instance.RecycleObj(StageView.Instance.brickName, itemId);

        haloComponent.clean();
        //Debug.Log("回收砖块: row: " + row + " col: " + column);

        if (isDiscovered)
        {
            GContext.Instance.discover_brick -= 1;
        }
        else
        {
            GContext.Instance.dark_brick -= 1;
        }

        //RecycBlock();

        //if (column == 0)
        //{
        //    BrickCore.Instance.CreateBrickRow();

        //    Debug.Log("回收砖块行: row: " + row);
        //}
    }


    public override void OnEnterIntoArea()
    {
        base.OnEnterIntoArea();

        if (item != null)
        {
            item.OnEnterIntoArea();
        }

        if (!isDiscovered)
        {
            GContext.Instance.dark_brick += 1;
        }

        RefreshWalkableAndBlockState();
    }

    public override void OnExitFromArea()
    {
        base.OnExitFromArea();

        if (item != null)
        {
            item.OnExitFromArea();
        }

        //Recycle();

        RefreshWalkableAndBlockState();
    }

    public override void ViewArea()
    {
        base.ViewArea();

        //if (GCamera.Instance.transform.position.y / 1.2f -1 > row)
        //{
        //    Recycle();
        //}
    }

    public override void RefreshPosistion()
    {
        base.RefreshPosistion();

        RefreshWalkableAndBlockState();
    }

    /// <summary>
    /// 清除上面的item
    /// </summary>
    public void CleanItem()
    {
        item = null;
        brickType = BrickType.EMPTY;
    }

    public void SetAsFire()
    {
        isFire = true;
        RefreshWalkableAndBlockState();
    }

    public void SetAsNormal()
    {
        isFire = false;
        RefreshWalkableAndBlockState();
    }
}

public enum BrickType
{
    EMPTY,
    TABLET,
    OBSTACLE,
    MONSTER,
    MAINTENANCE,
    SUPPLY,
    TREASURE,
    OTHERS,
    UNKNOWN,
    PLAYER,
    Organ,
    OrganProperty,
}

public enum BrickExplored
{
    UNEXPLORED = 0,
    EXPLORED = 1,
}

public enum BrickBlock
{
    NIL = 0,
    BLOCK_OTHER = 1,
    BLOCKED_BY_OTHER = 2,
    NO_BLOCK = 3,
}


