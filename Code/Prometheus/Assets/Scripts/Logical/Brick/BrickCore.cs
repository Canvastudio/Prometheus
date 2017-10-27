using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;

/// <summary>
/// 创建brick和他上面对象的逻辑发起,管理brick对象
/// </summary>
public class BrickCore : SingleObject<BrickCore> , IGetNode {

    ulong curLevelId = 0;

    WeightSection _weightSection;

    /// <summary>
    /// 当前一共创建到多少row了
    /// </summary>
    int _row = 0;

    /// <summary>
    /// 还存在的最下一层
    /// </summary>
    int o_row = 0;
    /// <summary>
    /// 保存了砖块数据
    /// </summary>
    BrickData data = new BrickData();

    protected override void Init()
    {
        base.Init();
    }

    public IEnumerator CreatePrimitiveStage()
    {
        //初始生成的行数
        int max_Distance = StageView.Instance.viewBrickRow;

        _row = 0;

        while (_row < max_Distance)
        {
            yield return CreateBrickModuel(_row);
        }
    }

    public void CheckNeedCreawteMoudel()
    {
        if (_row - (-StageView.Instance.moveRoot.localPosition.y) / StageView.Instance.brickWidth < (StageView.Instance.viewBrickRow))
        {
            CreateBrickModuel(_row);
        }
    }

    public void CheckNeedRecycelBrick()
    {
        Brick brick = GetNode(o_row, 0).behavirour as Brick;

        if (brick.CheckIfRecycle())
        {
            RecycleRowBrick(o_row);
            o_row += 1;
        }
    }
   
    public void RecycleRowBrick(int row)
    {
        for (int i = 0; i < 6; ++i)
        {
            Brick brick = GetNode(o_row, i).behavirour as Brick;

            brick.Recycle();
        }
    }

    public void CreatePlayer(ulong uid)
    {
        data.GetFirstRowEmpty().CreatePlayer(uid);
    }

    public Node GetNode(int row, int column)
	{
		return GetNode(row, 0 , column);
	}

	public Node GetNode(int x, int y ,int z)
	{
        var brick = data.GetBrick(x, z);

        if (brick != null)
        {
            return brick.pathNode;
        }
        else
        {
            return null;
        }
	}

    public IEnumerator CreateBrickModuel(int distance)
    {
        var map_Data = MapConfig.GetConfigDataList<MapConfig>();

        MapConfig next_Map = null;

        ulong level_Id = 0;

        //for (int i = map_Data.Count - 1; i >= 0; ++i)
        for (int i = 0; i < map_Data.Count; ++i)
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

        ulong moduel_id = moduels[select_Moduel];

        var moduel = ModuleConfig.GetConfigDataById<ModuleConfig>(moduel_id);

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
                    _brick = StageView.Instance.CreateBrick(moduel_id, level_Id);
                }
                else
                {
                    string[] infos = brick_Desc.Split('_');

                    string prefix = infos[0];

                    if ("o" == prefix)
                    {
                        _brick = StageView.Instance.CreateBrick(moduel_id, level_Id).CreateObstacle();
                    }
                    else
                    {
                        _brick = StageView.Instance.CreateBrick(moduel_id, level_Id);

                        if ("x" == prefix)
                        {
                            _brick = _brick.CreateMaintence();
                        }
                        else if ("y" == prefix)
                        {
                            float prob = float.Parse(infos[2]);

                            if (prob > Random.Range(0f, 1f))
                            {
                                _brick = _brick.CreateTalbet(ulong.Parse(infos[1]));
                            }
                        }
                        else if ("b" == prefix)
                        {
                            float prob = float.Parse(infos[2]);

                            if (prob > Random.Range(0f, 1f))
                            {
                                _brick = _brick.CreateTreasure(ulong.Parse(infos[1]), real_Row);
                            }

                        }
                        else if ("g" == prefix)
                        {
                            float prob = float.Parse(infos[2]);

                            if (prob > Random.Range(0f, 1f))
                            {
                                _brick = _brick.CreateSupply(ulong.Parse(infos[1]));
                            }
                        }
                        else if ("r" == prefix)
                        {
                            var monster_Desc = brick_Desc.Split('_');

                            var probility = float.Parse(monster_Desc[2]);

                            if (Random.Range(0f, 1f) <= probility)
                            {
                                var enemys = next_Map.enemys.ToArray();
                                var enemy_Index = Random.Range(0, enemys.Length);
                                var enemy_Id = enemys[enemy_Index];

                                int lv_min = next_Map.enemy_level[0];
                                int lv_max = next_Map.enemy_level[1];
                                int lv = Random.Range(lv_min, lv_max + 1);

                                yield return _brick.CreateMonter(int.Parse(monster_Desc[1]), enemy_Id, lv);
                            }
                        }
                        else
                        {
                            Debug.LogError("出现了配置表中没有出现的brick前缀: " + brick_Desc);
                        }
                    }
                }
                    
                data.PushBrick( real_Row, col, _brick);
            }

        }

        _row += moduel_RowCount;
    }

    public void OpenNearbyBrick(int row, int column, bool explored_self = true)
    {
        if (explored_self)
            data.GetBrick(row, column).brickExplored = BrickExplored.EXPLORED;

        for (int n = -1; n <= 1; ++n)
        {
            for (int m = -1; m <= 1; ++m)
            {
                if (Mathf.Abs(n) == Mathf.Abs(m)) continue;

                var _brick = data.GetBrick(row + n, column + m);

                if (_brick != null && _brick.brickExplored == BrickExplored.UNEXPLORED)
                {
                    _brick.OnDiscoverd();

                    if (_brick.item != null)
                    {
                        _brick.item.OnDiscoverd();
                    }
                }
            }
        }
    }

    public IEnumerator OpenBrick(Brick _brick)
    {
        if (_brick != null && _brick.brickExplored == BrickExplored.UNEXPLORED)
        {
            yield return _brick.OnDiscoverd();

            if (_brick.item != null)
            {
               yield return _brick.item.OnDiscoverd();
            }
        }
    }

    public void BlockNearbyBrick(int row, int column)
    {
        for (int n = -1; n <= 1; ++n)
        {
            for (int m = -1; m <= 1; ++m)
            {
                if (Mathf.Abs(n) == Mathf.Abs(m)) continue;

                var _brick = data.GetBrick(row + n, column + m);

                if (_brick != null  
                    && (_brick.brickExplored == BrickExplored.UNEXPLORED 
                    || _brick.realBrickType == BrickType.SUPPLY || _brick.realBrickType == BrickType.MAINTENANCE))
                {
                    _brick.brickBlock += 1;
                }
            }
        }
    }

    public void CancelBlockNearbyBrick(int row, int column)
    {
        Debug.Log("取消block: " + row + " " + column);

        for (int n = -1; n <= 1; ++n)
        {
            for (int m = -1; m <= 1; ++m)
            {
                if (Mathf.Abs(n) == Mathf.Abs(m)) continue;

                var _brick = data.GetBrick(row + n, column + m);

                if (_brick != null &&_brick.brickBlock > 0)
                {
                    _brick.brickBlock -= 1;
                }
            }
        }
    }

    public void CleanNodeData()
    {
        data.CleanAllBrickPathNodeGH();
    }

    public List<Brick> GetStandableBricks()
    {
        var v_bricks = StageCore.Instance.tagMgr.GetEntity<Brick>(ETag.GetETag(ST.BRICK));

        for (int i = v_bricks.Count - 1; i >= 0; --i)
        {
            Brick brick1 = v_bricks[i];

            if (brick1.brickType != BrickType.EMPTY || brick1.brickBlock != 0)
            {
                v_bricks.RemoveAt(i);
            }
        }

        return v_bricks; ;
    }

    public Brick GetRandomStandableBrick()
    {
        Brick brick1 = null;

        var v_bricks = BrickCore.Instance.GetStandableBricks();

        if (v_bricks.Count > 0)
        {
            int i = Random.Range(0, v_bricks.Count);
            brick1 = v_bricks[i] as Brick;
        }

        return brick1;
    }

    public void CreateMonsterOnRandomStandableBrick(int pwr, int lv, ulong uid)
    {
        Brick brick1 = GetRandomStandableBrick();

        brick1.CreateMonter(pwr, uid, lv);
    }

    public IEnumerator CreateWhiteMonsterOnRandomStandableBrick()
    {
        Brick brick1 = GetRandomStandableBrick();

        yield return brick1.CreateMonter();
    }

    public List<Monster> GetVisableMonsters()
    {
        return StageCore.Instance.tagMgr.GetEntity<Monster>(ETag.GetETag(ST.ENEMY, ST.MONSTER));
    }

    public List<Node> GetNearbyNode(int r, int c, int distance)
    {
        List<Node> result = new List<Node>(distance * distance + (distance + 1) * (distance + 1));

        for (int i = -distance; i <= distance; ++i)
        {
            for (int m = -distance; m <= distance; ++i)
            {
                if (Mathf.Abs(i) + Mathf.Abs(m) <= distance)
                {
                    result.Add(GetNode(r + i, c + m));
                }
            }
        }

        return result;
    }

    public List<Brick> GetNearbyBrick(int r, int c, int distance)
    {
        List<Brick> result = new List<Brick>(distance * distance + (distance + 1) * (distance + 1));

        for (int i = -distance; i <= distance; ++i)
        {
            for (int m = -distance; m <= distance; ++i)
            {
                if (Mathf.Abs(i) + Mathf.Abs(m) <= distance)
                {
                    result.Add(GetNode(r + i, c + m).behavirour as Brick);
                }
            }
        }

        return result;
    }
}
