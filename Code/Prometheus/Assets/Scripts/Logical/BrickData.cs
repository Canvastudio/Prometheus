using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickData {

	List<Brick[]> bricks = new List<Brick[]>();

	int _lowestRow = 0;
	int _highestRow = 0;

	public Brick GetBrick(int row ,int column)
	{
		if (row < _lowestRow) throw new System.ArgumentException("row 不能低于最低的rowzhi");	
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
}
