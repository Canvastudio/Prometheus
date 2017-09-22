using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class Brick : MonoBehaviour, IPointerClickHandler {

    [SerializeField]
    Image picture;

#region BrickInfo
    public BrickType brickType
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
    }

    [SerializeField]
    private BrickExplored _brickExplored = BrickExplored.UNEXPLORED;

    public BrickBlock brickBlock
    {
        get
        {
            return _brickBlock;
        }
    }

    [SerializeField]
    private BrickBlock _brickBlock = BrickBlock.NIL;

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
    public Monster monster;

    #endregion


    public Node pathNode
	{
		get { return _pathNode; }
	}

    [SerializeField]
	private Node _pathNode;

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }


    public void Init(int row, int column, BrickType type)
    {
       //Debug.Log("Add New brick, row: " + row + " column: " + column);

        _row = row;
        _column = column;
        _brickType = type;

        if (type == BrickType.OBSTACLE)
        {
            picture.sprite = BrickCore.Instance.brickView.brickAtlas.GetSprite(Predefine.BRICK_OBSTACLE_UNREACHABLE);
        }
        else if (type == BrickType.Normal)
        {
           picture.sprite = BrickCore.Instance.brickView.brickAtlas.GetSprite(Predefine.BRICK_NORMAL_UNREACHABLE);
        }

        _pathNode = new Node()
        {
            isWalkable = _brickExplored == BrickExplored.EXPLORED && _brickBlock == BrickBlock.NIL && _brickType == BrickType.EMPTY,
            nCost = 1f,
            worldObject = this.gameObject,
            nodeType = Node.NodeType.ground,
            x = row,
            z = column
        };
    }

    /// <summary>
    /// 在当前网格放置一个全新的怪物
    /// </summary>
    /// <param name="power"></param>
    /// <param name="id"></param>
    /// <param name="lv"></param>
    public void CreateMonter(int power, ulong id, int lv)
    {
        //创建数据
        monster = StageCore.Instance.CreateMonster(power, id, lv);

        //根据数据创建view
    }
}

public enum BrickType
{
    EMPTY,
    Normal,
    OBSTACLE,
    MONSTER,
    TRADER,
    OTHERS,
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
}


