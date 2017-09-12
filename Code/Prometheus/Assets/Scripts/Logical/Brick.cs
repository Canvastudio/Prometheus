using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {

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
#endregion


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


