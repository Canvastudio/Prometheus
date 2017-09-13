using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BrickCore : SingleObject<BrickCore> , IGetNode {

	public Node GetNode(int row, int column)
	{
		return GetNode(row, 0 , column);
	}

	public Node GetNode(int x, int y ,int z)
	{
		return null;
	}
}
