using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ChipBoard : SingleGameObject<ChipBoard>
{


    const int maxRowNum = 23;
    const int maxColNum = 11;

    public int rowNum = 3;
    public int colNum = 3;

    [Space(5)]
    [SerializeField]
    ChipSquare chipSquare;
    [SerializeField]
    ChipListItem listItem;
    [SerializeField]
    ChipBoardInstance boardInstance;
    [SerializeField]
    BoardSupplyInstance supplyInstance;

    [Space(5)]
    [SerializeField]
    RectTransform chipBoardRoot;
    [SerializeField]
    RectTransform chipListRoot;
    [SerializeField]
    RectTransform chipInstanceRoot;

    [Space(5)]
    [SerializeField]
    GameObject backgroud;

    [SerializeField]
    public SpriteAtlas spriteAtlas;
    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField]
    Button closeBtn;

    float itemWidth = 50f;

    public Camera camera;

    private ChipDiskConfig config;
    private ChipSquare[,] chipSquareArray = new ChipSquare[maxRowNum, maxColNum ];
    private List<BoardSupplyInstance> powerSupplyList;
    [SerializeField]
    private List<BoardPowerGrid> powerGridArray = new List<BoardPowerGrid>(4);
    private List<BoardPowerGrid> powerGridSearchList = new List<BoardPowerGrid>(4);

    string itemName = "CI";
    string instanceName = "BI";

    /// <summary>
    /// 加一个简单的变量来区分是哪个电网
    /// </summary>
    public int powerGridUid = 0;

    [Space(5)]
    public Sprite normalSpirte;
    public Sprite positiveSprite;
    public Sprite negativeSprite;

    int midRowNum;
    int midColNum;

    /// <summary>
    /// 芯片范围的4个值(r1,c1) 左上,(r2,c2)右下
    /// </summary>
    int r1, r2, c1, c2;

    /// <summary>
    /// 
    /// </summary>
    public List<ChipBoardInstance> listInstance = new List<ChipBoardInstance>();
    public Dictionary<ulong, int> _temp_listInstance = new Dictionary<ulong, int>();
    private void Start()
    {
        HudEvent.Get(closeBtn.gameObject).onClick = CloseChipBoard;
        ObjPool<ChipListItem>.Instance.InitOrRecyclePool(itemName, listItem);
        ObjPool<ChipBoardInstance>.Instance.InitOrRecyclePool(instanceName, boardInstance);
    }



    public IEnumerator InitBoard(ulong play_id)
    {
        config = ConfigDataBase.GetConfigDataById<ChipDiskConfig>(play_id);

        var powerList = config.power.ToArray();
        powerSupplyList = new List<BoardSupplyInstance>(powerList.Length);
        int power_id = 0;
        int power = 0;
        int pi = 0;

        midRowNum = maxRowNum / 2;
        midColNum = maxColNum / 2;

        for (int i = 0; i < maxRowNum; ++i)
        {
            for (int m = 0; m < maxColNum; ++m)
            {
                ChipSquare chip = GameObject.Instantiate(chipSquare, chipBoardRoot);

                chip.name = "ChipSqarue: " + i + "," + m;

                var chip_grid = config.chipGridMatrix[i, m];
                chipSquareArray[i, m] = chip;
                chip.InitChipSquare(chip_grid);

                if (chip_grid == ChipGrid.Power)
                {
                    power = powerList[power_id];
                    var supply = GameObject.Instantiate(supplyInstance, chipInstanceRoot);
                    supply.Init(power);
                    supply.row = i;
                    supply.col = m;
                    supply.uid = 1 << power_id;
                    chip.state = ChipSquareState.Power;
                    chip.supplyInstance = supply;
                }

                chip.row = i;
                chip.col = m;

                chip.transform.localPosition = new Vector3(
                    itemWidth * (m - midColNum),
                    itemWidth * (midRowNum - i),
                    0
                    );
            }

            yield return 0;
        }




        CalculteChipBoardBound();
    }

    public void CalculteChipBoardBound()
    {
        powerSupplyList.Clear();

        r1 = Mathf.Max(0, midRowNum - rowNum);
        r2 = Mathf.Min(maxRowNum - 1, midRowNum + rowNum);
        c1 = Mathf.Max(0, midColNum - colNum);
        c2 = Mathf.Min(maxColNum - 1, midColNum + colNum);

        for (int m = 0; m < maxRowNum; ++m)
        {
            for (int n = 0; n < maxColNum; ++n)
            {
                bool active = (m >= r1 && m <= r2) && (n >= c1 && n <= c2);
                chipSquareArray[m, n].isActive = active;

                if (chipSquareArray[m, n].supplyInstance != null)
                {
                    if (!active)
                    {

                        chipSquareArray[m, n].supplyInstance.gameObject.SetActive(false);
                    }
                    else
                    {
                        powerSupplyList.Add(chipSquareArray[m, n].supplyInstance);
                    }
                }

            }
        }

        for (int i = 0; i < powerSupplyList.Count; ++i)
        {
            var supply = powerSupplyList[i];
            supply.transform.localPosition = chipSquareArray[supply.row, supply.col].transform.localPosition;
        }
    }

    public void OpenChipBoard()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        _temp_listInstance.Clear();

        foreach(var ins in listInstance)
        {
            _temp_listInstance.Add(ins.chipInventory.config.id, ins.Power);
        }

        InitChipList();
    }

    public void CloseChipBoard()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        CheckSkillPointAndProperty();
    }

    public void CheckSkillPointAndProperty()
    {
        for (int i = 0; i < listInstance.Count; ++i)
        {
            var ins = listInstance[i];

            int power;

            if(!_temp_listInstance.TryGetValue(ins.chipInventory.config.id, out power))
            {
                power = 0;
            }

            int change_power = ins.Power - power;

            if (change_power != 0)
            {
                var sp = ins.chipInventory.config.skillPoint;
                int c = sp.Count();
                {
                    for (int m = 0; m < c; ++m)
                    {
                        ulong skill_id = (ulong)sp[m, 0];
                        int count = sp[m, 1];
                        StageCore.Instance.Player.skillPointsComponet.ChangeSkillPointCount(skill_id, count * change_power);
                    }
                }

            }
        }
    }

    private void InitChipList()
    {
        ObjPool<ChipListItem>.Instance.RecyclePool(itemName);

       var chipList = StageCore.Instance.Player.inventory.GetUnusedChipList();

        for (int i = 0; i < chipList.Count; ++i)
        {
            var chip = ObjPool<ChipListItem>.Instance.GetObjFromPool(itemName);
            chip.transform.SetParent(chipListRoot);
            chip.transform.localScale = Vector3.one;
            chip.gameObject.SetActive(true);
            StartCoroutine(chip.InitItem(chipList[i]));
        }
    }

    public ChipBoardInstance CreateBoardInstance(ChipListItem item)
    {
        int id;

        var instance = ObjPool<ChipBoardInstance>.Instance.GetObjFromPoolWithID(out id, instanceName);
        instance.transform.SetParent(chipInstanceRoot);
        instance.transform.localScale = Vector3.one;
        instance.gameObject.SetActive(true);
        instance.uid = id;

        instance.Init(item);

        if (!AutoPutChipBoardInstance(instance))
        {
            ObjPool<ChipBoardInstance>.Instance.RecycleObj(instanceName, id);
            Debug.Log("自动寻找不到适合的的位置放置芯片");
            return null;
        }

        RefreshChipInstanceConnectInfo(instance);
        listInstance.Add(instance);

        ConstructPowerGrid();

        return instance;
    }

    private void RefreshChipInstanceConnectInfo(ChipBoardInstance instance)
    {
        instance.negativeConnectInstance.Clear();
        instance.positiveConnectInstance.Clear();

        //寻找他的正极相连的负极极和电源
        int r, c;
        instance.GetInPowerRC(out r, out c);
        var chipSquares = FindOutPowerSquareAround(r, c);
        if (chipSquares.Count > 0)
        {
            for (int i = 0; i < chipSquares.Count; ++i)
            {
                if (chipSquares[i].boardInstance != null)
                {
                    chipSquares[i].boardInstance.negativeConnectInstance.Add(instance);
                    instance.positiveConnectInstance.Add(chipSquares[i].boardInstance);
                }
                else
                {
                    instance.positiveConnectInstance.Add(chipSquares[i].supplyInstance);
                    chipSquares[i].supplyInstance.negativeConnectInstance.Add(instance);
                }
            }
        }

        //寻找他的负极相连的正极
        instance.GetOutPowerRC(out r, out c);
        chipSquares = FindInPowerSquareAround(r, c);
        if (chipSquares.Count > 0)
        {
            for (int i = 0; i < chipSquares.Count; ++i)
            {
                if (chipSquares[i].boardInstance != null)
                {
                    chipSquares[i].boardInstance.positiveConnectInstance.Add(instance);
                    instance.negativeConnectInstance.Add(chipSquares[i].boardInstance);
                }
                else
                {
                    instance.negativeConnectInstance.Add(chipSquares[i].supplyInstance);
                    chipSquares[i].supplyInstance.positiveConnectInstance.Add(instance);
                }
            }
        }
    }

    /// <summary>
    /// 构造电网
    /// </summary>
    private void ConstructPowerGrid()
    {
        Messenger.Invoke(ChipBoardEvent.CheckPowerState);

        Debug.Log("构建电网开始： " + Time.realtimeSinceStartup);

        powerGridSearchList.Clear();
        powerGridArray.Clear();

        List<BoardInstanceBase> connect_list = new List<BoardInstanceBase>(10);

        int depth = 0;

        BoardPowerGrid _boardPowerGrid = null;

        for (int i = 0; i < powerSupplyList.Count; ++i)
        {
            var supply = powerSupplyList[i];

                _boardPowerGrid = supply.GetOrAddComponet<BoardPowerGrid>();

                _boardPowerGrid.Clean();

                _boardPowerGrid.id = i << i;

                supply.powerGrid = _boardPowerGrid;

                powerGridArray.Add(_boardPowerGrid);

                _boardPowerGrid.AddSupply(supply);

            if (supply.negativeConnectInstance.Count > 0)
            {
                _boardPowerGrid.searchList.AddRange(supply.negativeConnectInstance);

                supply.depth = 0;

                supply.powerGrid = _boardPowerGrid;

                powerGridSearchList.Add(_boardPowerGrid);
            }
        }

        while (powerGridSearchList.Count > 0)
        {
            depth += 1;

            for (int i = powerGridSearchList.Count - 1; i >= 0; --i)
            {
                _boardPowerGrid = powerGridSearchList[i];

                connect_list.Clear();
                connect_list.AddRange(_boardPowerGrid.searchList);

                _boardPowerGrid.searchList.Clear();

                connect_list.Sort(InstanceComparison);

                _boardPowerGrid.unactiveDic.Add(depth, new List<BoardInstanceBase>());

                var unactive_list = _boardPowerGrid.unactiveDic[depth];

                for (int m = 0; m < connect_list.Count; ++m)
                {
                    var child_node = connect_list[m];

                    //子节点还没有被其他电网搜索到
                    if (child_node.depth == int.MaxValue)
                    {
                        child_node.depth = depth;
                        child_node.powerGrid = _boardPowerGrid;
                        unactive_list.Add(child_node);

                        _boardPowerGrid.searchList.AddRange(child_node.negativeConnectInstance);
                    }
                    else if (child_node.powerGrid.id == _boardPowerGrid.id)
                    {
                        if (child_node.depth <= depth)
                        {
                            continue;
                        }
                        else
                        {
                            Debug.Log("青鑫，出现了子节点得dpeht大于父节点的情况");
                        }
                    }
                    else
                    {
                        foreach (var snode in child_node.powerGrid.searchList)
                        {
                            snode.powerGrid = _boardPowerGrid;
                            connect_list.Add(snode);
                        }

                        foreach (var pair in child_node.powerGrid.unactiveDic)
                        {
                            _boardPowerGrid.unactiveDic[pair.Key].AddRange(pair.Value);
                        }

                        _boardPowerGrid.AddSupply(child_node.powerGrid.supplyList);

                        powerGridArray.Remove(child_node.powerGrid);

                        child_node.powerGrid.Clean();

                        child_node.powerGrid = _boardPowerGrid;
                    }
                }

                if (_boardPowerGrid.searchList.Count == 0)
                {
                    powerGridSearchList.Remove(_boardPowerGrid);
                }
            }
        }

        bool power_leak = false;

        for (int i = 0; i < powerGridArray.Count; ++i)
        {
            var power_grid = powerGridArray[i];

            foreach(var depth_list in power_grid.unactiveDic)
            {
                if (power_leak) break;

                var list = depth_list.Value;
                list.Sort(InstanceComparison);
                list.Reverse();
                for(int m = list.Count -1; m >=0;--m)
                {
                    var instance = list[m];
                    var pg = instance.powerGrid;
                    if (pg.powerGridTotalPower >= pg.powerGridCastPower + instance.castPower)
                    {
                        pg.powerGridCastPower += instance.castPower;
                        list.RemoveAt(m);
                        instance.Power = 1;
                        List<BoardInstanceBase> list1;
                        if(pg.activeDic.TryGetValue(depth_list.Key, out list1))
                        {
                            list.Add(instance);
                        }
                        else
                        {
                            list1 = new List<BoardInstanceBase>();
                            list1.Add(instance);
                            pg.activeDic.Add(depth_list.Key, list1);
                        }
                    }
                    else
                    {
                        power_leak = true;
                        break;
                    }
                }
                
            }
            
        }

        Debug.Log("构建电网结束： " + Time.realtimeSinceStartup);

    }
    private bool AutoPutChipBoardInstance(ChipBoardInstance instance)
    {
        var model = instance.chipInventory.model;
         
        int rn;
        int cn;
        int mr;
        int mc;

        var models = RemoveRedundant(model, out rn, out cn, out mr, out mc);

        //仔细想想这个和字符串匹配一样 可以做很多的算法优化啊...
        for (int r = r1; r <= r2; ++r)
        {
            for (int c = c1; c <= c2; ++c)
            {
                if (!MatrixPut(r, c, rn, cn, models, instance))
                {
                    continue;
                }
                else
                {
                    int mr9 = r + 1 - (1 - mr);
                    int mc9 = c + 1 - (1 - mc);

                    instance.transform.localPosition = chipSquareArray[mr9, mc9].transform.localPosition;
                    instance.row = mr9;
                    instance.col = mc9;
                    instance.lastLocalPos = instance.transform.localPosition;
                    return true;
                }
            }
        }

        return false;
    }




    /// <summary>
    /// 拖动结束的时候帮助判断
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public bool MatrixDragPut(ChipBoardInstance instance)
    {
        Vector2 first_localpos = instance.transform.localPosition + new Vector3(-itemWidth, itemWidth, 0);

        int r, c;
        ConvertVector2ToNodeRC(first_localpos, out r, out c);

        if (MatrixPut(r, c, 3, 3, new List<int>(instance.chipInventory.model), instance))
        {
            instance.transform.localPosition = chipSquareArray[r + 1, c + 1].transform.localPosition;
            instance.lastLocalPos = instance.transform.localPosition;
            instance.row = r + 1;
            instance.col = c + 1;


            for (int i = 0; i< instance.positiveConnectInstance.Count; ++i)
            {
                var ins = instance.positiveConnectInstance[i];

                ins.negativeConnectInstance.Remove(instance);
            }

            for (int i = 0; i < instance.negativeConnectInstance.Count; ++i)
            {
                var ins = instance.negativeConnectInstance[i];

                ins.positiveConnectInstance.Remove(instance);
            }

            RefreshChipInstanceConnectInfo(instance);

            ConstructPowerGrid();
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool ChipSquarePutable(int r, int c)
    {
        //保证目标sqaure的索引在正确的范围
        if (r < r1 || r > r2 || c < 0 || c > r2) return false;

        bool putable = chipSquareArray[r, c].state == ChipSquareState.Free
            && chipSquareArray[r, c].chipGrid != ChipGrid.None
            && chipSquareArray[r, c].chipGrid != ChipGrid.Power;

        return putable;
    }

    private void ConvertVector2ToNodeRC(Vector2 vec2, out int r, out int c)
    {
        float topBound = (midRowNum + 0.5f) * itemWidth;
        float rightBound = (midColNum + 0.5f) * -itemWidth;

        r = Mathf.FloorToInt((topBound - vec2.y) / itemWidth);
        c = Mathf.FloorToInt((vec2.x - rightBound) / itemWidth);
    }

    private List<int> RemoveRedundant(int[] models, out int rn, out int cn, out int mr, out int mc)
    {
        int temp = models[4];

        models[4] = -1;

        List<int> modelsList = new List<int>();

        rn = 0;
        cn = 3;

        for (int i = 0; i < 3; ++i)
        {
            int zero = 0;

            for (int m = 0; m < 3; ++m)
            {
                if (models[i * 3 + m] == 0)
                {
                    zero += 1;
                }
            }

            if (zero != 3)
            {
                modelsList.Add(models[i * 3]);
                modelsList.Add(models[i * 3 + 1]);
                modelsList.Add(models[i * 3 + 2]);
                rn += 1;
            }
        }

        for (int i = 2; i >= 0; --i)
        {
            int zero = 0;

            for (int m = 0; m < rn; ++m)
            {
                if (modelsList[i + m * cn] == 0)
                {
                    zero += 1;
                }
                else
                {
                    break;
                }

                if (zero == 3)
                {
                    zero -= 1;

                    while (zero >= 0)
                    {
                        modelsList.RemoveAt(i + cn * zero);
                        zero -= 1;
                    }

                    cn -= 1;
                }
            }
        }

        int index = modelsList.FindIndex((int a) => a == -1);

        mr = index / cn;
        mc = index % cn;

        modelsList[index] = temp;
        models[4] = temp;

        return modelsList;
    }

    public bool MatrixPut(int r, int c, int rn, int cn, List<int> modelsList, ChipBoardInstance instance)
    {
        if (instance.hasPut)
        {
            var m = instance.chipInventory.model;

            var r1 = (int)instance.row;
            var c1 = (int)instance.col;

            int i = 0;

            for (int q = -1; q <= 1; ++q)
            {
                for (int p = -1; p <= 1; ++p)
                {
                    if (m[i] > 0)
                    {
                        chipSquareArray[r1 + q, c1 + p].state = ChipSquareState.Free;
                    }

                    ++i;
                }
            }
        }

        bool b = true;

        for (int i = 0; i < rn; ++i)
        {
            if (b == false) break;

            for (int m = 0; m < cn; ++m)
            {
                if (r + i < r1 || r + i > r2)
                {
                    b = false;
                    break;
                }
                if (c + m < c1 || c + i > c2)
                {
                    b = false;
                    break;
                }

                if (modelsList[i * cn + m] > 0)
                {
                    if (!ChipSquarePutable(r + i, c + m))
                    {
                        b = false;
                        break;
                    }

                    if (m > 0 && i < rn - 1)
                    {
                        if (modelsList[(i + 1) * cn + (m - 1)] > 0)
                        {
                            if (!ChipSquarePutable(r + i, c + m - 1) && !ChipSquarePutable(r + i + 1, c + m))
                            {
                                b = false;
                                break;
                            }
                        }

                    }

                    if (m < cn - 1 && i < rn - 1)
                    {
                        if (modelsList[(i + 1) * cn + (m + 1)] > 0)
                        {
                            if (!ChipSquarePutable(r + i + 1, c + m) && !ChipSquarePutable(r + i, c + m + 1))
                            {
                                b = false;
                            }
                        }
                    }
                }
            }
        }

        if (instance.hasPut && !b)
        {
            var m = instance.chipInventory.model;

            var r1 = (int)instance.row;
            var c1 = (int)instance.col;

            int i = 0;

            for (int q = -1; q <= 1; ++q)
            {
                for (int p = -1; p <= 1; ++p)
                {
                    if (m[i] > 0)
                    {
                        int v = m[i];
                        ChipSquare chipSquare = chipSquareArray[r1 + q, c1 + p];
                        if (v == 1)
                        {
                            chipSquare.state = ChipSquareState.Use;
                        }
                        else if (v == 2)
                        {
                            chipSquare.state = ChipSquareState.Positive;
                        }
                        else
                        {
                            chipSquare.state = ChipSquareState.Negative;
                        }
                    }

                    ++i;
                }
            }
        }

        if (b == false)
        {
            return false;
        }

        ChipGrid require_square = instance.chipInventory.config.chipGridReqire;
        bool square_check = (require_square != ChipGrid.Normal);
        bool all_meet = true;

        for (int i = 0; i < rn; ++i)
        {
            for (int m = 0; m < cn; ++m)
            {
                int v = modelsList[i * cn + m];

                if (v > 0)
                {
                    ChipSquare chipSquare = chipSquareArray[r + i, c + m];
                    chipSquare.boardInstance = instance;
                    chipSquare.index = i * 3 + m;

                    if (v == 1)
                    {
                        chipSquare.state = ChipSquareState.Use;
                    }
                    else if (v == 2)
                    {
                        chipSquare.state = ChipSquareState.Positive;
                    }
                    else
                    {
                        chipSquare.state = ChipSquareState.Negative;
                    }

                    if (square_check)
                    {
                        all_meet = chipSquare.chipGrid == require_square;
                        square_check = all_meet;
                    }
                }
            }
        }

        instance.hasPut = true;
        instance.chipGridMeet = all_meet;

        return true;
    }

    public void CheckPowerState()
    {

    }


    public System.Comparison<ChipSquare> chipPowerComparison = new System.Comparison<ChipSquare>(
        (ChipSquare c1, ChipSquare c2) =>
        {
            float p1 = c1.boardInstance.castPower;
            float p2 = c2.boardInstance.castPower;
            if (p1 > p2) return 1;
            else if (p1 < p2) return -1;
            else return 0;
        });

    public System.Comparison<BoardInstanceBase> InstanceComparison = new System.Comparison<BoardInstanceBase>(
    (BoardInstanceBase c1, BoardInstanceBase c2) =>
    {
        float p1 = c1.castPower;
        float p2 = c2.castPower;
        if (p1 > p2) return 1;
        else if (p1 < p2) return -1;
        else return 0;
    });

    public List<ChipSquare> FindInPowerSquareAround(int r, int c)
    {
        var list = FindFourDirectionAround(r, c);

        for (int i = list.Count - 1; i >= 0; --i)
        {
            if (list[i].state != ChipSquareState.Positive)
            {
                list.RemoveAt(i);
            }
            else if (list[i].boardInstance != null 
                && chipSquareArray[r, c].boardInstance != null
                && list[i].boardInstance.uid == chipSquareArray[r,c].boardInstance.uid)
            {
                list.RemoveAt(i);
            }

        }

        list.Sort(chipPowerComparison);
        return list;
    }

    public List<ChipSquare> FindOutPowerSquareAround(int r, int c)
    {
        var list = FindFourDirectionAround(r, c);

        for (int i = list.Count - 1; i >= 0; --i)
        {
            if (list[i].state != ChipSquareState.Negative && list[i].state != ChipSquareState.Power)
            {
                list.RemoveAt(i);
            }
            else if (list[i].boardInstance != null
                && chipSquareArray[r, c].boardInstance != null
                && list[i].boardInstance.uid == chipSquareArray[r, c].boardInstance.uid)
            {
                list.RemoveAt(i);
            }

        }

        return list;
    }

    public List<ChipSquare> FindAround(int r, int c)
    {
        List<ChipSquare> list = new List<ChipSquare>(9);

        for (int q = -1; q <= 1; ++q)
        {
            for (int p = -1; p <= 1; ++p)
            {
                if (r + q >= r1 && r + q <= r2 && c + p >= c1 && c + p < c2)
                {
                    list.Add(chipSquareArray[r + q, c + p]);
                }
            }
        }

        return list;
    }

    public List<ChipSquare> FindFourDirectionAround(int r, int c)
    {
        List<ChipSquare> list = new List<ChipSquare>(4);

        for (int q = -1; q <= 1; ++q)
        {
            for (int p = -1; p <= 1; ++p)
            {
                if (q != 0 && p != 0) continue;
                if (q == 0 && p == 0) continue;

                if (r + q >= r1 && r + q <= r2 && c + p >= c1 && c + p <= c2)
                {
                    list.Add(chipSquareArray[r + q, c + p]);
                }
            }
        }

        return list;
    }
}

public class ChipBoardEvent
{
    public const string CheckPowerState = "CPS";
}

