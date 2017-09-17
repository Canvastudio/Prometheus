using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickView : MonoBehaviour {

    private int lastRow;
    private int lastColumn;

    [SerializeField]
    Brick _brickPrefab;

    public int viewBrickRow = 9;

    public List<Brick[]> brickGrid = new List<Brick[]>();

    public void AddEmpty(int row = -1, int col = -1)
    {
        AddBrick(row, col, BrickType.EMPTY);
    }

    public void AddEnemy(int power, int row = -1, int col = -1)
    {
        AddBrick(row, col, BrickType.MONSTER);
    }

    public void Addobstacle(int row = -1, int col = -1)
    {
        AddBrick(row, col, BrickType.OBSTACLE);
    }

    private void AddBrick(int row, int col, BrickType type)
    {
        row = CheckRow(row);
        col = CheckCol(col);

        Brick _brick = GameObject.Instantiate<Brick>(_brickPrefab, this.transform);

        _brick.Init(row, col, type);

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
    }

    private int CheckRow(int row)
    {
        if (row == -1) return lastRow + 1;
        else return row;
    }

    private int CheckCol(int col)
    {
        if (col == -1) return col = lastColumn + 1 > 5 ? 1 : lastColumn + 1;
        else return col;
    }
}
