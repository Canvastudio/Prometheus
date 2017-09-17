using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;

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


    protected override void Init()
    {
        base.Init();

        brickView = GameObject.Find("BrickView").GetComponent<BrickView>();
    }

    public void CreatePrimitiveStage()
    {
        //初始生成的行数
        int max_Distance = brickView.viewBrickRow;

        int _row = 0;

        while (_row < max_Distance)
        {
            CreateBrickModuel(_row);

            _row += 1;
        }
    }

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
        var map_Data = MapConfig.GetConfigDataList<MapConfig>();

        MapConfig next_Map = null;

        ulong level_Id = 0;

        for (int i = map_Data.Count - 1; i >= 0; ++i)
        {
            if (map_Data[i].distance > distance)
            {
                level_Id = map_Data[i].id;
                next_Map = map_Data[i];
                break;
            }
        }

        if (next_Map == null)
        {
            next_Map = map_Data[map_Data.Count - 1];
            level_Id = next_Map.id;
        }

        var moduels = next_Map.map_models.ToList();

        if (curLevelId != level_Id)
        {
            _weightSection = WeightSection.CreatePrimitive(moduels.Count);
        }

        //随机到了模块ID
        int select_Moduel = _weightSection.RanPoint();

        //调整和检查权重
        _weightSection.ScaleWeightExOne(select_Moduel).CheckBound();

        var moduel = ModuleConfig.GetConfigDataById<ModuleConfig>(moduels[select_Moduel]);

        var moduel_RowCount = moduel.contents.Count();

        for (int row = 0; row < moduel_RowCount; ++row)
        {
            for(int col = 0; col < 6; ++col)
            {
                var brick_Desc = moduel.GetBrickInfo(row, col);

                if (string.IsNullOrEmpty(brick_Desc))
                {
                    brickView.AddEmpty();
                }
                else if ("o" == brick_Desc)
                {
                    brickView.Addobstacle();
                }
                else if (brick_Desc.Contains('r'))
                {
                    var monster_Desc = brick_Desc.Split('_');

                    var probility = float.Parse(monster_Desc[2]);

                    if (Random.Range(0, 1) <= probility)
                    {
                        brickView.AddEnemy(int.Parse(monster_Desc[1]));
                    }
                }
                    
            }

        }
    }
}
