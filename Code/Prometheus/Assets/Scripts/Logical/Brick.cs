using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.EventSystems;
using System;

public class Brick : MonoBehaviour, IPointerClickHandler {

#region BrickInfo
    public BrickType brickType
    {
        get
        {
            return _brickType;
        }
    }

    private BrickType _brickType;

    public BrickExplored brickExplored
    {
        get
        {
            return _brickExplored;
        }
    }

    private BrickExplored _brickExplored = BrickExplored.UNEXPLORED;

    public BrickBlock brickBlock
    {
        get
        {
            return _brickBlock;
        }
    }

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

    private int _row;
    private int _column;

    #endregion


    public Node pathNode
	{
		get { return _pathNode; }
	}

	private Node _pathNode;

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public void Init(int row, int column, BrickType type)
    {
        _row = row;
        _column = column;
        _brickType = type;

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


