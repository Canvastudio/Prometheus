﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class StageView : SingleGameObject<StageView> {


    private int lastRow = 0;
    private int lastColumn = 0;

    [SerializeField]
    Brick _brickPrefab;

    public Transform monsterRoot;
    public Transform brickRoot;

    [SerializeField]
    public SpriteAtlas brickAtlas;

    public int viewBrickRow = 9;

    public List<Brick[]> brickGrid = new List<Brick[]>();

#region Add Brick
    public Brick AddEmpty(int row = -1, int col = -1)
    {
        return CreateBrick(row, col, BrickType.EMPTY);
    }

    public Brick AddTREASURE(int quality, int row = -1, int col = -1)
    {
        return CreateBrick(row, col, BrickType.TREASURE).CreateTreasure();
    }

    public Brick AddSupply(int lv, int row = -1, int col = -1)
    {
        return CreateBrick(row, col, BrickType.SUPPLY).CreateSupply();
    }

    public Brick AddTablet(int id, int row = -1, int col = -1)
    {
        return CreateBrick(row, col, BrickType.TABLET).CreateTalbet();
    }

    public Brick AddMaintenance(int row = -1, int col = -1)
    {
        return CreateBrick(row, col, BrickType.MAINTENANCE);
    }

    public Brick AddEnemy(int power, ulong id, int lv, int row = -1, int col = -1)
    {
        var brick = CreateBrick(row, col, BrickType.MONSTER).CreateMonter(power, id, lv);

        return brick;
    }

    public Brick Addobstacle(int row = -1, int col = -1)
    {
        var brick = CreateBrick(row, col, BrickType.OBSTACLE);
        return brick;
    }

    private Brick CreateBrick(int row, int col, BrickType type)
    {
        if (col == -1)
        {
            if (lastColumn > Predefine.BRICK_VIEW_WIDTH - 1)
            {
                col = 0;
                lastColumn = 1;
                row = ++lastRow;
            }
            else
            {
                col = lastColumn++;
                row = lastRow;
            }
        }

        Brick _brick = GameObject.Instantiate<Brick>(_brickPrefab, brickRoot);

        _brick.Init(row, col, type);

#if UNITY_EDITOR
        _brick.name = row.ToString() + " : " + col.ToString() + " : " + _brick.brickType.ToString();
#endif

        return _brick;
    }
    #endregion

    List<Brick> pathBrick = new List<Brick>(20);

    public void SetNodeAsPath(List<Pathfinding.Node> list)
    {
        pathBrick.Clear();

        foreach(var node in list)
        {
            var brick = node.behavirour as Brick;

            brick.SetAsPathNode();

            pathBrick.Add(brick);
        }
    }

    public void CancelPahtNode()
    {
        for (int i = 0; i < pathBrick.Count; ++i)
        {
            pathBrick[i].CancelAsPathNode();
        }

        pathBrick.Clear();
    }
}
