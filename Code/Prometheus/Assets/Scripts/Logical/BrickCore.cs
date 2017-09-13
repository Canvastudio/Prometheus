using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BrickCore : SingleGameObject<BrickCore> , IGetNode {

    [SerializeField]
    Transform _mapRoot;

    [SerializeField]
    Brick _brickPrefab;

	public Node GetNode(int row, int column)
	{
		return GetNode(row, 0 , column);
	}

	public Node GetNode(int x, int y ,int z)
	{
		return null;
	}

    public void CreateBrickGroup(ulong groupID)
    {
        MapConfig map = SuperConfig.Instance.GetConfigDataById<MapConfig>(groupID);
    }
}
