using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BrickCore : SingleGameObject<BrickCore> , IGetNode {

    [SerializeField]
    Transform _mapRoot;

    [SerializeField]
    Brick _brickPrefab;

    MapConfig currentLevel;
    
	public Node GetNode(int row, int column)
	{
		return GetNode(row, 0 , column);
	}

	public Node GetNode(int x, int y ,int z)
	{
		return null;
	}

    public void CreateBrickGroup(int distance)
    {
        var mapData = MapConfig.GetConfigDataList<MapConfig>();

        ulong levelId = 0;

        for (int i = mapData.Count - 1; i >= 0; ++i)
        {
            if (mapData[i].distance > distance)
            {
                levelId = mapData[i].id;
            }
        }

    }
}
