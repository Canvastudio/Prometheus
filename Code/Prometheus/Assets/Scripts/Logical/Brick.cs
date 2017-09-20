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


