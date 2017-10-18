using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ChipBoard : SingleGameObject<ChipBoard>
{

    [SerializeField]
    int rowNum = 20;
    [SerializeField]
    int colNum = 11;

    [Space(5)]
    [SerializeField]
    ChipSquare chipSquare;
    [SerializeField]
    ChipListItem listItem;
    [SerializeField]
    ChipBoardInstance boardInstance;

    [Space(5)]
    [SerializeField]
    RectTransform chipRoot;
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
    [SerializeField]
    float itemWidth = 60f;

    public Camera camera;

    private ChipDiskConfig config;
    private List<ChipSquare> powerInstances = new List<ChipSquare>(10);

    private ChipSquare[,] chipSquareArray = new ChipSquare[h, w];
    private List<ChipSquare> powerSquareList;

    const int w = 11;
    const int h = 20;

    string itemName = "CI";
    string instanceName = "BI";

    [Space(5)]
    public Sprite normalSpirte;
    public Sprite positiveSprite;
    public Sprite negativeSprite;

    private uint instance_id = 0;

    /// <summary>
    /// 
    /// </summary>
    public List<ChipBoardInstance> listInstance = new List<ChipBoardInstance>();

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
        powerSquareList = new List<ChipSquare>(powerList.Length);
        int power_id = 0;
        int power = 0;
        int pi = 0;

        for (int i = 0; i < rowNum; ++i)
        {
            for (int m = 0; m < colNum; ++m)
            {
                ChipSquare chip = GameObject.Instantiate(chipSquare, chipRoot);

                chip.name = "ChipSqarue: " + i + "," + m;

                var chip_grid = config.chipGridMatrix[i, m];

                if (chip_grid == ChipGrid.Power)
                {
                    power = powerList[power_id++];
                    powerSquareList.Add(chip);
                }

                chip.InitChipSquare(chip_grid, power);

                chipSquareArray[i, m] = chip;

                chip.row = i;
                chip.col = m;
            }

            yield return 0;
        }
    }

    public void OpenChipBoard()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        InitChipList();
    }

    public void CloseChipBoard()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    private void InitChipList()
    {
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
        var instance = ObjPool<ChipBoardInstance>.Instance.GetObjFromPool(instanceName);
        instance.transform.SetParent(chipInstanceRoot);
        instance.transform.localScale = Vector3.one;
        instance.gameObject.SetActive(true);

        instance.Init(item);
        instance.uid = instance_id++;

        if (!AutoPutChipBoardInstance(instance))
        {
            Debug.Log("自动寻找不到适合的的位置放置芯片");
        }

        return instance;
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
        for (int r = 0; r <= h - 3; ++r)
        {
            for (int c = 0; c <= w - 3; ++c)
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
                    instance.row_col = new Vector2(mr9, mc9);
                    instance.lastLocalPos = instance.transform.localPosition;
                    CheckPowerState();
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
            instance.row_col = new Vector2(r + 1, c + 1);
            CheckPowerState();
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
        if (r < 0 || r >= rowNum || c < 0 || c >= colNum) return false;

        bool putable = chipSquareArray[r, c].state == ChipSquareState.Free
            && chipSquareArray[r, c].chipGrid != ChipGrid.None
            && chipSquareArray[r, c].chipGrid != ChipGrid.Power;

        return putable;
    }

    private void ConvertVector2ToNodeRC(Vector2 vec2, out int r, out int c)
    {
        float topBound = rowNum / 2f * itemWidth;
        float rightBound = colNum / 2f * -itemWidth;

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
                        modelsList.RemoveAt(i + 3 * zero);
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

            var r1 = (int)instance.row_col.x;
            var c1 = (int)instance.row_col.y;

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
            for (int m = 0; m < cn; ++m)
            {
                if (modelsList[i * cn + m] > 0)
                {
                    if (!ChipSquarePutable(r + i, c + m))
                    {
                        b = false;
                    }

                    if (m > 0 && i < rn - 1)
                    {
                        if (modelsList[(i + 1) * cn + (m - 1)] > 0)
                        {
                            if (!ChipSquarePutable(r + i, c + m - 1) && !ChipSquarePutable(r + i + 1, c + m))
                            {
                                b = false;
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

            var r1 = (int)instance.row_col.x;
            var c1 = (int)instance.row_col.y;

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
                }
            }
        }

        instance.hasPut = true;

        return true;
    }

    public void CheckPowerState()
    {
        Messenger.Invoke(ChipBoardEvent.CheckPowerState);
        powerInstances.Clear();
        powerInstances.AddRange(powerSquareList);
        CheckPowerInstance(powerInstances);

        //for (int i = 0; i < powerSquareList.Length; ++i)
        //{
        //    CheckPowerInstance(FindPositiveSquareAround(powerSquareList[i].row, powerSquareList[i].col));

            //List<ChipSquare> positiveList = FindPositiveSquareAround(powerSquareList[i].row, powerSquareList[i].col);

            //int left_power = powerSquareList[i].power;

            //for (int m = 0; m < positiveList.Count; ++m)
            //{
            //    int power_cast = positiveList[i].boardInstance.chipInventory.power;

            //    if (left_power > power_cast)
            //    {
            //        positiveList[i].boardInstance.powerSquare = powerSquareList[i];
            //        left_power -= power_cast;
            //        positiveList[i].leftPower = left_power;
            //        int r, c;
            //        positiveList[i].boardInstance.GetNegativeRC(out r, out c);
            //        powerInstances.Add(chipSquareArray[r, c]);
            //    }
            //}
        //}
    }

    private void CheckPowerInstance(List<ChipSquare> powerInstances)
    {
        while (powerInstances.Count > 0)
        {
            for (int i = powerInstances.Count -1 ; i >= 0; --i)
            {
                List<ChipSquare> positiveList = FindPositiveSquareAround(powerInstances[i].row, powerInstances[i].col);

                int left_power = powerInstances[i].power;

                for (int m = 0; m < positiveList.Count; ++m)
                {
                    int power_cast = positiveList[m].boardInstance.chipInventory.power;

                    if (left_power >= power_cast)
                    {
                        positiveList[m].boardInstance.powerSquare = powerInstances[i];
                        left_power -= power_cast;
                        powerInstances[i].leftPower = left_power;
                        int r, c;
                        positiveList[m].boardInstance.GetNegativeRC(out r, out c);
                        powerInstances.Add(chipSquareArray[r, c]);
                    }
                }

                powerInstances.RemoveAt(i);
            }
        }
    }

    System.Comparison<ChipSquare> chipPowerComparison = new System.Comparison<ChipSquare>(
        (ChipSquare c1, ChipSquare c2) =>
        {
            int p1 = c1.boardInstance.chipInventory.power;
            int p2 = c2.boardInstance.chipInventory.power;
            if (p1 > p2) return 1;
            else if (p1 < p2) return -1;
            else return 0;
        });

    public List<ChipSquare> FindPositiveSquareAround(int r, int c)
    {
        var list = FindAround(r, c);

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

    public List<ChipSquare> FindAround(int r, int c)
    {
        List<ChipSquare> list = new List<ChipSquare>(9);

        for (int q = -1; q <= 1; ++q)
        {
            for (int p = -1; p <= 1; ++p)
            {
                if (r + q >= 0 && r + q < rowNum && c + p >= 0 && c + p < colNum)
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

