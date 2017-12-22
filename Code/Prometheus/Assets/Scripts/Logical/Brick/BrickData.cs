using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BrickData {

    [SerializeField]
	List<Brick[]> bricks = new List<Brick[]>();

    [SerializeField]
	int _lowestRow = 0;
    [SerializeField]
	int _highestRow = 0;

	public Brick GetBrick(int row ,int column)
	{
        if (row < 0) return null;

        if (row < _lowestRow) return null; // throw new System.ArgumentException("row 不能低于最低的row: " + _lowestRow);	

        //if (row > _lowestRow + bricks.Count - 1)
        //{
        //    throw new System.ArgumentOutOfRangeException(
        //        string.Format("row 超出的限制: row: {0}, _lowRow: {1}, row_count: {2}",
        //        row, _lowestRow, bricks.Count));
        //}

        if (column < 0 || column > bricks[row - _lowestRow].Length - 1) return null;
        return bricks[row - _lowestRow][column];
	}

    public List<Brick> GetRow(int row)
    {
        if (row < _lowestRow) return null;
        if (row > _lowestRow + bricks.Count - 1)
        {
            return null;
        }

        return new List<Brick>(bricks[row - _lowestRow]);
    }

    public List<Brick> GetCol(int col)
    {
        List<Brick> brickList = new List<Brick>();

        for (int i = 0; i < bricks.Count; ++i)
        {
            brickList.Add(bricks[i][col]);
        }

        return brickList;
    }


    public void Remove(Brick brick)
    {
        bricks.RemoveAt(0);
        _lowestRow++;
    }

    public void CleanAllBrickPathNodeGH()
    {
        foreach(var brick_row in bricks)
        {
            foreach(var brick in brick_row)
            {
                brick.pathNode.gCost = 0;
                brick.pathNode.hCost = 0;
            }
        }
    }

	public void PushBrick(int row, int column, Brick brick)
	{
		if (row < _lowestRow) throw new System.ArgumentException("row 不能低于最低的rowzhi");
		if (row > _lowestRow + bricks.Count) throw new System.ArgumentOutOfRangeException("row 超出的限制");
		if (column > Predefine.BRICK_VIEW_WIDTH - 1) throw new System.ArgumentOutOfRangeException("column 不能超过预定义的宽度");

		int index = row - _lowestRow;

		if (bricks.Count > index && bricks[index] != null)
		{
			bricks[index][column] = brick;
		}
		else
		{
			bricks.Add(new Brick[Predefine.BRICK_VIEW_WIDTH]);
			bricks[index][column] = brick;
		}
	}	

	public void Clean()
	{
		bricks.Clear();
	}

    /// <summary>
    /// 从存储数据中的第一排中找到一个未被占用的格子
    /// </summary>
    /// <returns></returns>
    public Brick GetFirstRowEmpty()
    {
        int w = bricks[0].Length;
        int[] emptyIndex = new int[w];

        var row_Bricks = bricks[0];

        int m = 0;

        for (int i = 0; i< w; ++i)
        {
            if (row_Bricks[i].realBrickType == BrickType.EMPTY && row_Bricks[i].brickBlock == 0)
            {
                emptyIndex[m++] = i;
            }
        }

        if (m > 0)
        {
            int v = Random.Range(0, m);

            return row_Bricks[emptyIndex[v]];
        }
        else
        {
            return null;
        }
    }

}
