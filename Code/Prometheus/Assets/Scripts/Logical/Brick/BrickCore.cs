using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;

/// <summary>
/// 创建brick和他上面对象的逻辑发起,管理brick对象
/// </summary>
public class BrickCore : SingleGameObject<BrickCore> , IGetNode {

    WeightSection _weightSection;

    public int maxRowCount;

    /// <summary>
    /// 当前一共创建到多少row了
    /// </summary>
    [SerializeField]
    public int topRow = 0;
    [SerializeField]
    int totalRow = 0;
    [SerializeField]
    int topFireRow = 0;
    [SerializeField]
    int bottomFireRowCount = 0;

    /// <summary>
    /// 保存了砖块数据
    /// </summary>
    [SerializeField]
    public BrickData data = new BrickData();

    public void RemoveLowestRowIndata()
    {
        data.Remove();
    }

    public void RemoveLowestRow()
    {
        var bricks = data.GetRow(lowestRow);

        RemoveLowestRowIndata();

        for (int i = 0; i < bricks.Count; ++i)
        {
            bricks[i].Recycle();
        }

        lowestRow++;
        totalRow--;
    }

    public void SetLowestRowFire()
    {
        var bricks = data.GetRow(topFireRow++);

        for (int i = 0; i < bricks.Count; ++i)
        {
            if (bricks[i].item != null)
            {
                bricks[i].item.Recycle();
            }

            if (bricks[i].cover != null)
            {
                bricks[i].cover.Recycle();
            }

            bricks[i].SetAsFire();
        }

        ++bottomFireRowCount;

        if (bottomFireRowCount > 8)
        {
            RemoveLowestRow();
        }
    }

    protected override void Init()
    {
        base.Init();

        map_Data = MapConfig.GetConfigDataList<MapConfig>();
        var gc = GlobalParameterConfig.GetConfigDataById<GlobalParameterConfig>(1);
        Predefine.BRICK_VIEW_WIDTH =gc.MapWidth;
        maxRowCount = gc.MaxRow;
    }

    int max_Distance = 0;

    public void CreatePrimitiveStage()
    {
        //初始生成的行数
        max_Distance = Mathf.FloorToInt(StageView.Instance.transform.Rt().sizeDelta.y / StageView.Instance.brickWidth) + 4;

        topRow = 0;

        while (topRow < max_Distance)
        {
            CreateBrickRow();
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

    List<MapConfig> map_Data;
    MapConfig curMapData;
    ModuleConfig curModule;
    ulong curLevelId = 0;
    ulong moduel_id = 0;
    int distance = 0;
    int curRowInModule = 0;
    int moduelRowCount = 0;
    public int lowestRow = 0;
    List<ulong> enemys;

    /// <summary>
    /// 创造一行无法通过的行
    /// </summary>
    public void CreateFireRow()
    {

    }

    public void CreateBrickRow()
    {
        if (StageCore.Instance.Player != null)
        {
            if (totalRow == maxRowCount)
            {
                if (StageCore.Instance.Player.standBrick.row > 0.5f * (maxRowCount + lowestRow))
                {
                    RemoveLowestRow();
                }
                else
                {
                    return;
                }
            }
        }

        ///如果
        if (curModule == null)
        {
            ulong level_Id = 0;

            for (int i = 0; i < map_Data.Count; ++i)
            {
                if (map_Data[i].distance > topRow)
                {
                    level_Id = map_Data[i].id;
                    curMapData = map_Data[i];
                    enemys = null;
                    break;
                }
            }

            if (level_Id == 0)
            {
                level_Id = map_Data[map_Data.Count - 1].id;
                curMapData = map_Data[map_Data.Count - 1];
                enemys = null;
            }

            var moduels = curMapData.map_models.ToList();

            if (curLevelId != level_Id)
            {
                _weightSection = WeightSection.CreatePrimitive(moduels.Count);
                curLevelId = level_Id;
            }

            //随机到了模块ID
            int select_Moduel = _weightSection.RanPoint();

            //调整和检查权重
            _weightSection.ScaleWeightExOne(select_Moduel).CheckBound();

            moduel_id = moduels[select_Moduel];

            curModule = ModuleConfig.GetConfigDataById<ModuleConfig>(moduel_id);

            curRowInModule = 0;
           
            moduelRowCount = curModule.contents.Count();
        }

        int w = curModule.contents.Count(curRowInModule);

        for (int col = 0; col < w; ++col)
        {
            var brick_Desc = curModule.GetBrickInfo(curRowInModule, col);

            Brick _brick = null;

            _brick = StageView.Instance.CreateBrick(moduel_id, curLevelId, topRow, col);
            _brick.rowInModuel = curRowInModule;
            if (string.IsNullOrEmpty(brick_Desc))
            {
                _brick = _brick.CreateCover();
            }
            else
            {
                string[] infos = brick_Desc.Split('_');

                string prefix = infos[0];

                if ("o" == prefix)
                {
                    _brick = _brick.CreateObstacle();
                }
                else
                {
                    _brick = _brick.CreateCover();

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

                            float tip = float.Parse(infos[3]);

                            if (Random.Range(0f, 1f) <= tip)
                            {
                                _brick.ShowTipRich();
                            }
                        }

                    }
                    else if ("b" == prefix)
                    {
                        float prob = float.Parse(infos[2]);

                        if (prob > Random.Range(0f, 1f))
                        {
                            _brick = _brick.CreateTreasure(ulong.Parse(infos[1]), topRow);

                            float tip = float.Parse(infos[3]);

                            if (Random.Range(0f, 1f) <= tip)
                            {
                                _brick.ShowTipRich();
                            }
                        }

                    }
                    else if ("g" == prefix)
                    {
                        float prob = float.Parse(infos[2]);

                        if (prob > Random.Range(0f, 1f))
                        {
                            _brick = _brick.CreateSupply(ulong.Parse(infos[1]));

                            float tip = float.Parse(infos[3]);

                            if (Random.Range(0f, 1f) <= tip)
                            {
                                _brick.ShowTipRich();
                            }
                        }
                    }
                    else if ("r" == prefix)
                    {
                        var monster_Desc = brick_Desc.Split('_');

                        var probility = float.Parse(monster_Desc[2]);

                        if (Random.Range(0f, 1f) <= probility)
                        {
                            if (enemys == null || enemys.Count == 0)
                            {
                                enemys = curMapData.enemys.ToList();
                            }

                            var enemy_Index = Random.Range(0, enemys.Count);
                            var enemy_Id = enemys[enemy_Index];
                            enemys.RemoveAt(enemy_Index);

                            int lv_min = curMapData.enemy_level[0];
                            int lv_max = curMapData.enemy_level[1];
                            int lv = Random.Range(lv_min, lv_max + 1);

                            if (!GameTestData.Instance.noMonster)
                            {
                                _brick.CreateMonster(int.Parse(monster_Desc[1]), enemy_Id, lv);

                                float tip = float.Parse(monster_Desc[3]);

                                if (Random.Range(0f, 1f) <= tip)
                                {
                                    _brick.ShowTipDanger();
                                }
                            }

                        }
                    }
                    else if ("j" == prefix)
                    {
                        ulong id = ulong.Parse(infos[1]);
                        var probility = float.Parse(infos[2]);
                        if (Random.Range(0f,1f) <= probility)
                        {
                            _brick.CreateOrgan(id);

                            float tip = float.Parse(infos[3]);

                            if (Random.Range(0f, 1f) <= tip)
                            {
                                _brick.ShowTipRich();
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("出现了配置表中没有出现的brick前缀: " + brick_Desc);
                    }


                }
            }

            data.PushBrick(topRow, col, _brick);
        }

        //下一行
        curRowInModule += 1;
        topRow += 1;
        totalRow++;

        if (curRowInModule == moduelRowCount)
        {
            curModule = null;
        }
    }

    //public int CreateBrickModuel()
    //{
    //    int distance = total_row;

    //    MapConfig next_Map = null;

    //    ulong level_Id = 0;

    //    for (int i = 0; i < map_Data.Count; ++i)
    //    {
    //        if (map_Data[i].distance > distance)
    //        {
    //            level_Id = map_Data[i].id;
    //            next_Map = map_Data[i];
    //            break;
    //        }
    //    }

    //    if (next_Map == null)
    //    {
    //        next_Map = map_Data[map_Data.Count - 1];
    //        level_Id = next_Map.id;
    //    }

    //    var moduels = next_Map.map_models.ToList();

    //    //新的level 重置概率
    //    if (curLevelId != level_Id)
    //    {
    //        _weightSection = WeightSection.CreatePrimitive(moduels.Count);
    //        curLevelId = level_Id;
    //    }

    //    //随机到了模块ID
    //    int select_Moduel = _weightSection.RanPoint();

    //    //调整和检查权重
    //    _weightSection.ScaleWeightExOne(select_Moduel).CheckBound();

    //    ulong moduel_id = moduels[select_Moduel];

    //    var moduel = ModuleConfig.GetConfigDataById<ModuleConfig>(moduel_id);

    //    var moduel_RowCount = moduel.contents.Count();

    //    int real_Row;

    //    for (int row = 0; row < moduel_RowCount; ++row)
    //    {
    //        for (int col = 0; col < Predefine.BRICK_VIEW_WIDTH; ++col)
    //        {
    //            var brick_Desc = moduel.GetBrickInfo(row, col);

    //            Brick _brick = null;

    //            real_Row = row + distance;

    //            _brick = StageView.Instance.CreateBrick(moduel_id, level_Id).CreateCover();

    //            if (string.IsNullOrEmpty(brick_Desc))
    //            {

    //            }
    //            else
    //            {
    //                string[] infos = brick_Desc.Split('_');

    //                string prefix = infos[0];

    //                if ("o" == prefix)
    //                {
    //                    _brick = StageView.Instance.CreateBrick(moduel_id, level_Id).CreateObstacle();
    //                }
    //                else
    //                {
    //                    _brick = StageView.Instance.CreateBrick(moduel_id, level_Id);

    //                    if ("x" == prefix)
    //                    {
    //                        _brick = _brick.CreateMaintence();
    //                    }
    //                    else if ("y" == prefix)
    //                    {
    //                        float prob = float.Parse(infos[2]);

    //                        if (prob > Random.Range(0f, 1f))
    //                        {
    //                            _brick = _brick.CreateTalbet(ulong.Parse(infos[1]));
    //                        }

    //                        float tip = float.Parse(infos[3]);

    //                        if (Random.Range(0f, 1f) <= tip)
    //                        {
    //                            _brick.ShowTipRich();
    //                        }
    //                    }
    //                    else if ("b" == prefix)
    //                    {
    //                        float prob = float.Parse(infos[2]);

    //                        if (prob > Random.Range(0f, 1f))
    //                        {
    //                            _brick = _brick.CreateTreasure(ulong.Parse(infos[1]), real_Row);
    //                        }

    //                        float tip = float.Parse(infos[3]);

    //                        if (Random.Range(0f, 1f) <= tip)
    //                        {
    //                            _brick.ShowTipRich();
    //                        }

    //                    }
    //                    else if ("g" == prefix)
    //                    {
    //                        float prob = float.Parse(infos[2]);

    //                        if (prob > Random.Range(0f, 1f))
    //                        {
    //                            _brick = _brick.CreateSupply(ulong.Parse(infos[1]));
    //                        }

    //                        float tip = float.Parse(infos[3]);

    //                        if (Random.Range(0f, 1f) <= tip)
    //                        {
    //                            _brick.ShowTipRich();
    //                        }
    //                    }
    //                    else if ("r" == prefix)
    //                    {
    //                        var monster_Desc = brick_Desc.Split('_');

    //                        var probility = float.Parse(monster_Desc[2]);

    //                        if (Random.Range(0f, 1f) <= probility)
    //                        {
    //                            var enemys = next_Map.enemys.ToArray();
    //                            var enemy_Index = Random.Range(0, enemys.Length);
    //                            var enemy_Id = enemys[enemy_Index];

    //                            int lv_min = next_Map.enemy_level[0];
    //                            int lv_max = next_Map.enemy_level[1];
    //                            int lv = Random.Range(lv_min, lv_max + 1);

    //                            if (!GameTestData.Instance.noMonster)
    //                            {
    //                                _brick.CreateMonster(int.Parse(monster_Desc[1]), enemy_Id, lv);

    //                                float tip = float.Parse(monster_Desc[3]);

    //                                if (Random.Range(0f, 1f) <= tip)
    //                                {
    //                                    _brick.ShowTipDanger();
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            Debug.LogError("出现了配置表中没有出现的brick前缀: " + brick_Desc);
    //                        }


    //                    }
    //                }

    //                if (row == moduel_RowCount - 1)
    //                {
    //                    _brick.last_row = true;
    //                }
    //                else
    //                {
    //                    _brick.last_row = false;
    //                }

    //                data.PushBrick(real_Row, col, _brick);
    //            }

    //        }
    //    }

    //    total_row += moduel_RowCount;
    

    //    return moduel_RowCount;
    //}
    
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
        }
    }

    public void BlockNearbyBrick(int row, int column)
    {
        Debug.Log("Block nearby: " + row + " : " + column);

        for (int n = -1; n <= 1; ++n)
        {
            for (int m = -1; m <= 1; ++m)
            {
                //if (Mathf.Abs(n) == Mathf.Abs(m)) continue;

                var _brick = data.GetBrick(row + n, column + m);

                if (_brick != null && _brick.realBrickType != BrickType.PLAYER  && !_brick.isDiscovered
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
        //Debug.Log("取消block: " + row + " " + column);

        for (int n = -1; n <= 1; ++n)
        {
            for (int m = -1; m <= 1; ++m)
            {
                //if (Mathf.Abs(n) == Mathf.Abs(m)) continue;

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

        brick1.CreateMonster(pwr, uid, lv);
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
            for (int m = -distance; m <= distance; ++m)
            {
                if (Mathf.Abs(i) + Mathf.Abs(m) <= distance)
                {
                    result.Add(GetNode(r + i, c + m));
                }
            }
        }

        return result;
    }

    public List<Brick> GetNearbyBrick(int r, int c, int distance, bool diagonal = false)
    {
        List<Brick> result = new List<Brick>(distance * distance + (distance + 1) * (distance + 1));

        for (int i = -distance; i <= distance; ++i)
        {
            for (int m = -distance; m <= distance; ++m)
            {
                if (diagonal)
                {
                    if ((Mathf.Abs(i) <= distance || Mathf.Abs(m)<= distance) && !(i == 0 && m == 0))
                    {
                        if (GetNode(r + i, c + m) != null)
                        {
                            result.Add(GetNode(r + i, c + m).behavirour as Brick);
                        }
                    }
                }
                else
                {
                    if (Mathf.Abs(i) + Mathf.Abs(m) <= distance && !(m == 0 && i == 0))
                    {
                        if (GetNode(r + i, c + m) != null)
                        {
                            result.Add(GetNode(r + i, c + m).behavirour as Brick);
                        }
                    }
                }
            }
        }

        return result;
    }

    public List<Brick> GetBrickInDistance(int r, int c, int distance)
    {
        List<Brick> result = new List<Brick>(24);

        for (int i = -distance; i <= distance; ++i)
        {
            for (int m = -distance; m <= distance; ++m)
            {
                if (Mathf.Abs(i) + Mathf.Abs(m) == distance)
                {
                    if (GetNode(r + i, c + m) != null)
                    {
                        result.Add(GetNode(r + i, c + m).behavirour as Brick);
                    }
                }
            }
        }

        return result;
    }

    public List<LiveItem> GetNearbyLiveItem(int r, int c, int distance, bool diagonal = false)
    {
        var bricks = GetNearbyBrick(r, c, distance, diagonal);

        var res = new List<LiveItem>();

        foreach(var brick in bricks)
        {
            if (brick.item != null && brick.item is LiveItem)
            {
                res.Add(brick.item as LiveItem);
            }
        }

        return res;
    }

    public List<LiveItem> GetNearbyLiveItem(Brick brick, int distance, bool diagonal = false)
    {
        return GetNearbyLiveItem(brick.row, brick.column, distance, diagonal);
    }

    public List<Brick> GetBrickOnRow(int row)
    {
        return data.GetRow(row);
    }

    public List<Brick> GetBrickOnColumn(int col)
    {
        return data.GetCol(col);
    }

    public List<Brick> GetNearbyBrick(Brick brick, int distance)
    {
        return GetNearbyBrick(brick.row, brick.column, distance);
    }

    public void SetNearbyCoverLight(Brick brick)
    {
        var bricks = GetNearbyBrick(brick, 1);
        foreach(var b in bricks)
        {

            if (b.cover != null)
            {
                b.cover.SetLight();
            }
        }
    }

    public void SetNearbyCoverDark(Brick brick)
    {
        var bricks = GetNearbyBrick(brick, 1);
        foreach (var b in bricks)
        {

            if (b.cover != null)
            {
                b.cover.SetDark();
            }
        }
    }
}
