using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BrickCore : SingleObject<BrickCore> , IGetNode {

    [SerializeField]
    Transform _mapRoot;

    [SerializeField]
    Brick _brickPrefab;

    /// <summary>
    /// brick界面管理对象
    /// </summary>
    public BrickView brickView;

    ulong curLevelId = 0;
    WeightSection _weightSection;


    public Node GetNode(int row, int column)
	{
		return GetNode(row, 0 , column);
	}

	public Node GetNode(int x, int y ,int z)
	{
		return null;
	}

    public void CreateBrickModuel(int distance)
    {
        //如果brickView还未创建则找到它
        if (brickView == null)
        {
            
        }

        var mapData = MapConfig.GetConfigDataList<MapConfig>();

        MapConfig nextMap = null;

        ulong levelId = 0;

        for (int i = mapData.Count - 1; i >= 0; ++i)
        {
            if (mapData[i].distance > distance)
            {
                levelId = mapData[i].id;
                nextMap = mapData[i];
                break;
            }
        }

        if (nextMap == null)
        {
            nextMap = mapData[mapData.Count - 1];
            levelId = nextMap.id;
        }

        var moduels = nextMap.exit_models.ToList(0);

        if (curLevelId != levelId)
        {
            _weightSection = WeightSection.CreatePrimitive(moduels.Count);
        }

        //随机到了模块ID
        int selectModuel = _weightSection.RanPoint();

        _weightSection.ScaleWeightExOne(selectModuel).CheckBound();
    }
}
