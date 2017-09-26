using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickData {

	List<Brick[]> bricks = new List<Brick[]>();

	int _lowestRow = 0;
	int _highestRow = 0;

	public Brick GetBrick(int row ,int column)
	{
        if (row < _lowestRow) return null; // throw new System.ArgumentException("row 不能低于最低的row: " + _lowestRow);	
        if (column < 0 || column > 5) return null;
		if (row > _lowestRow + bricks.Count - 1) throw new System.ArgumentOutOfRangeException("row 超出的限制");

		return bricks[row - _lowestRow][column];
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
        //:-0 娱乐写法
        //int v = Random.Range(1, 61); 

        //int tmp = v;

        //var row_Bricks = bricks[0];

        //int m = 0;

        //while (true)
        //{ 
        //    if (row_Bricks[m % 6].brickType == BrickType.EMPTY)
        //    {
        //        v -= 1;

        //        if (v == 0)
        //            break;
        //    }


        //    ++m;
        //    if (m > 6 && tmp =)
        //}

        //return row_Bricks[m];

        int[] emptyIndex = new int[6];

        var row_Bricks = bricks[0];

        int m = 0;

        for (int i = 0; i< 6; ++i)
        {
            if (row_Bricks[i].brickType == BrickType.EMPTY)
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
