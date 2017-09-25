using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class BrickView : MonoBehaviour {

    private int lastRow = 0;
    private int lastColumn = 0;

    [SerializeField]
    Brick _brickPrefab;
    [SerializeField]
    public SpriteAtlas brickAtlas;

    public int viewBrickRow = 9;

    public List<Brick[]> brickGrid = new List<Brick[]>();

    public Brick AddEmpty(int row = -1, int col = -1)
    {
        return AddBrick(row, col, BrickType.EMPTY);
    }

    public Brick AddEnemy(int power, ulong id, int lv, int row = -1, int col = -1)
    {
        var brick = AddBrick(row, col, BrickType.MONSTER);
        brick.CreateMonter(power, id, lv);
        return brick;
    }

    public Brick Addobstacle(int row = -1, int col = -1)
    {
        var brick = AddBrick(row, col, BrickType.OBSTACLE);
        return brick;
    }

    private Brick AddBrick(int row, int col, BrickType type)
    {
        if (col == -1) 
        {
            if (lastColumn + 1 > Predefine.BRICK_VIEW_WIDTH - 1)
            {
                col = lastColumn = 0;
                row = ++lastRow;
            }
            else
            {
                col = ++lastColumn;
                row = lastRow;
            }
        }

        Brick _brick = GameObject.Instantiate<Brick>(_brickPrefab, this.transform);

        _brick.Init(row, col, type);

        #if UNITY_EDITOR
        _brick.name = row.ToString() + " : " + col.ToString() + " : " + _brick.brickType.ToString();
        #endif

        switch(type)
        {
            case BrickType.EMPTY:
                break;
            case BrickType.MONSTER:
                break;
            case BrickType.OBSTACLE:
                break;
            default:
                //未处理
                break;
        }

        return _brick;
    }
}
