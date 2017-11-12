﻿using System.Collections;
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
    Button brickBtn;
    [SerializeField]
    Image pathMask;
    [SerializeField]
    Image blockMask;

    public bool last_row;

    public int uid;
    public ulong moduel_id = 0;
    public ulong level_id = 0;

    #region BrickInfo
    public BrickType brickType
    {
        get
        {
            if (brickExplored == BrickExplored.UNEXPLORED && realBrickType != BrickType.OBSTACLE)
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
                icon.sprite  = StageView.Instance.itemAtlas.GetSprite(BrickNameCore.Instance.GetUnExploredBrickName());
            }
            else
            if (value == BrickExplored.EXPLORED)
            {
                if (column == 0)
                {
                    icon.sprite = StageView.Instance.itemAtlas.GetSprite(BrickNameCore.Instance.GetSideBrickName());
                }
                else if (column == 5)
                {
                    icon.sprite = StageView.Instance.itemAtlas.GetSprite(BrickNameCore.Instance.GetSideBrickName());
                    icon.transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    icon.sprite = StageView.Instance.itemAtlas.GetSprite(BrickNameCore.Instance.GetExploredBrickName());
                }
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

            if (_item == null)
            {
                if (value is LiveItem)
                {
                    LiveItem live = (value as LiveItem);

                    foreach (var halo in halo_dic.Keys)
                    {
                        if (live.Side == halo.Side)
                        {
                            StateIns state = new StateIns(halo.passive.stateConfig, live, true);
                            live.AddStateIns(state);

                            halo_dic[halo] = state;
                        }
                    }
                }
            }
            else if (value == null)
            {
                if (_item is LiveItem)
                {
                    LiveItem live = (_item as LiveItem);

                    foreach (var halo in halo_dic.Keys)
                    {
                        if (live.Side == halo.Side)
                        {
                            live.RemoveStateIns(halo_dic[halo]);
                            halo_dic[halo] = null;
                        }
                    }
                }
            }
            else if (_item is LiveItem && value is LiveItem)
            {
                LiveItem live = (_item as LiveItem);

                foreach (var halo in halo_dic.Keys)
                {
                    if (live.Side == halo.Side)
                    {
                        live.RemoveStateIns(halo_dic[halo]);
                        halo_dic[halo] = null;
                    }
                }

                live = value as LiveItem;

                foreach (var halo in halo_dic.Keys)
                {
                    if (live.Side == halo.Side)
                    {
                        StateIns state = new StateIns(halo.passive.stateConfig, live, true);
                        live.AddStateIns(state);

                        halo_dic[halo] = state;
                    }
                }

            }

            _item = value;
        }
    }

    /// <summary>
    /// 当前item对于每一个光环所拥有的状态实例
    /// </summary>
    public Dictionary<HaloInfo, StateIns> halo_dic = new Dictionary<HaloInfo, StateIns>();
    #endregion


    public Node pathNode
    {
        get { return _pathNode; }
    }

    [SerializeField]
    private Node _pathNode;


    protected override void OnEnable()
    {
        base.OnEnable();

        HudEvent.Get(brickBtn.gameObject).onClick = OnBrickClick;
        HudEvent.Get(brickBtn.gameObject).onLongPress = OnLongPress;
    }

    public override IEnumerator OnDiscoverd()
    {
        base.OnDiscoverd();

        brickExplored = BrickExplored.EXPLORED;

        isDiscovered = true;

        if (!item.isDiscovered)
        {
            yield return item.OnDiscoverd();
        }
    }

    public void RefreshWalkableAndBlockState()
    {
        blockMask.gameObject.SetActive(false);

        if (brickType == BrickType.EMPTY)
        {
            if (brickBlock > 0)
            {
                blockMask.gameObject.SetActive(true);
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
                    blockMask.gameObject.SetActive(false);
                }
                else
                {
                    blockMask.gameObject.SetActive(true);
                    pathNode.isWalkable = false;
                }
            }
            else
            {
                pathNode.isWalkable = false;
            }
        }
    }

    public void OnBrickClick()
    {
        Messenger<Brick>.Invoke(SA.PlayerClickBrick.ToString(), this);
    }

    public void OnLongPress()
    {
        Debug.Log(string.Format("显示砖块属性，{0}", gameObject.name));
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

        CancelAsPathNode();
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
    #endregion

    public void SetAsPathNode()
    {
        pathMask.gameObject.SetActive(true);
    }

    public void CancelAsPathNode()
    {
        pathMask.gameObject.SetActive(false);
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

        if (realBrickType != BrickType.EMPTY && item != null)
        {
            item.Recycle();
            item = null;
        }

        brickBlock = 0;
        blockMask.gameObject.SetActive(false);
        brickType = BrickType.EMPTY;
        brickExplored = BrickExplored.UNEXPLORED;
        ObjPool<Brick>.Instance.RecycleObj(StageView.Instance.brickName, itemId);

        //Debug.Log("回收砖块: row: " + row + " col: " + column);



        if (column == 0)
        {
            BrickCore.Instance.BrickRowRecycle(row);
            BrickCore.Instance.RemoveRowIndata(this);
        }
    }

    protected override void OnExitFromArea()
    {
        base.OnExitFromArea();

        Recycle();
    }


    /// <summary>
    /// 清除上面的item
    /// </summary>
    public void CleanItem()
    {
        item = null;
        brickType = BrickType.EMPTY;
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


