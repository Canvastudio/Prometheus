﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;

/// <summary>
/// 创建brick和他上面对象的逻辑发起
/// </summary>
public class BrickCore : SingleObject<BrickCore> , IGetNode {

    ulong curLevelId = 0;

    WeightSection _weightSection;

    /// <summary>
    /// 保存了砖块数据
    /// </summary>
    BrickData data = new BrickData();

    protected override void Init()
    {
        base.Init();
    }

    public void CreatePrimitiveStage()
    {
        //初始生成的行数
        int max_Distance = StageView.Instance.viewBrickRow;

        int _row = 0;

        while (_row < max_Distance)
        {
            _row += CreateBrickModuel(_row);
        }
    }

    public void CreatePlayer()
    {
        data.GetFirstRowEmpty().CreatePlayer();
    }

    public Node GetNode(int row, int column)
	{
		return GetNode(row, 0 , column);
	}

	public Node GetNode(int x, int y ,int z)
	{
		return data.GetBrick(x,z).pathNode;
	}

    public int CreateBrickModuel(int distance)
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

        //新的level 重置概率
        if (curLevelId != level_Id)
        {
            _weightSection = WeightSection.CreatePrimitive(moduels.Count);
            curLevelId = level_Id;
        }

        //随机到了模块ID
        int select_Moduel = _weightSection.RanPoint();

        //调整和检查权重
        _weightSection.ScaleWeightExOne(select_Moduel).CheckBound();

        var moduel = ModuleConfig.GetConfigDataById<ModuleConfig>(moduels[select_Moduel]);

        var moduel_RowCount = moduel.contents.Count();

        int real_Row;

        for (int row = 0; row < moduel_RowCount; ++row)
        {
            for(int col = 0; col < 6; ++col)
            {
                var brick_Desc = moduel.GetBrickInfo(row, col);

                Brick _brick = null;

                real_Row = row + distance;

                if (string.IsNullOrEmpty(brick_Desc))
                {
                    _brick = StageView.Instance.AddEmpty();
                }
                else
                {
                    string[] infos = brick_Desc.Split('_');

                    string prefix = infos[0];

                    if ("o" == prefix)
                    {
                        _brick = StageView.Instance.Addobstacle();
                    }
                    else if ("x" == prefix)
                    {
                        _brick = StageView.Instance.AddMaintenance();
                    }
                    else if ("y" == prefix)
                    {
                        float prob = float.Parse(infos[2]);

                        if (prob > Random.Range(0f, 1f))
                        {
                            _brick = StageView.Instance.AddTablet(int.Parse(infos[1]));
                        }
                        else
                        {
                            _brick = StageView.Instance.AddEmpty();
                        }
                    }
                    else if ("b" == prefix)
                    {
                        float prob = float.Parse(infos[2]);

                        if (prob > Random.Range(0f, 1f))
                        {
                            _brick = StageView.Instance.AddTablet(int.Parse(infos[1]));
                        }
                        else
                        {
                            _brick = StageView.Instance.AddEmpty();
                        }
                    }
                    else if ("g" == prefix)
                    {
                        float prob = float.Parse(infos[2]);

                        if (prob > Random.Range(0f, 1f))
                        {
                            _brick = StageView.Instance.AddSupply(int.Parse(infos[1]));
                        }
                        else
                        {
                            _brick = StageView.Instance.AddEmpty();
                        }
                    }
                    else if ("r" == prefix)
                    {
                        var monster_Desc = brick_Desc.Split('_');

                        var probility = float.Parse(monster_Desc[2]);

                        if (Random.Range(0, 1) <= probility)
                        {
                            var enemys = next_Map.enemys.ToArray();
                            var enemy_Index = Random.Range(0, enemys.Length);
                            var enemy_Id = enemys[enemy_Index];

                            int lv_min = next_Map.enemy_level[0];
                            int lv_max = next_Map.enemy_level[1];
                            int lv = Random.Range(lv_min, lv_max + 1);

                            _brick = StageView.Instance.AddEnemy(int.Parse(monster_Desc[1]), enemy_Id, lv);
                        }
                        else
                        {
                            _brick = StageView.Instance.AddEmpty();
                        }
                    }
                    else
                    {
                        Debug.LogError("出现了配置表中没有出现的brick前缀: " + brick_Desc);
                    }
                }
                    
                data.PushBrick( real_Row, col, _brick);
            }

        }

        return moduel_RowCount;
    }
}