using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ChipBoard : SingleGameObject<ChipBoard> {

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

    const int w = 11;
    const int h = 20;

    string itemName = "CI";
    string instanceName = "BI";

    [Space(5)]
    public Sprite normalSpirte;
    public Sprite positiveSprite;
    public Sprite negativeSprite;

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

    private ChipSquare[,] chipSquareArray = new ChipSquare[h,w];

    public IEnumerator InitBoard(ulong play_id)
    {
        config = ConfigDataBase.GetConfigDataById<ChipDiskConfig>(play_id);

        var powerList = config.power.ToArray();

        int power_id = 0;
        int power = 0;

        for (int i = 0; i < rowNum; ++i)
        {
            for (int m = 0; m < colNum; ++m)
            {
                var chip = GameObject.Instantiate(chipSquare, chipRoot);

                chip.name = "ChipSqarue: " + i + "," + m;

                var chip_grid = config.chipGridMatrix[i, m];

                if (chip_grid == ChipGrid.Power)
                {
                    power = powerList[power_id++];
                }

                chip.InitChipSquare(chip_grid, power);

                chipSquareArray[i, m] = chip;
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

                    return true;
                }
            }
        }

        return false;
    }

  
    private bool ChipSquarePutable(int r, int c)
    {
        //保证目标sqaure的索引在正确的范围
        if (r < 0 || r >= rowNum || c < 0 || c >= colNum) return false;

        bool putable = chipSquareArray[r, c].state == ChipSquareState.Free
            && chipSquareArray[r, c].chipGrid != ChipGrid.None
            && chipSquareArray[r, c].chipGrid != ChipGrid.Power;
        
        if (putable == false)
        {
            Debug.Log("1");
        }

        return putable;
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

        if(MatrixPut(r, c, 3, 3, new List<int>(instance.chipInventory.model), instance))
        {
            instance.transform.localPosition = chipSquareArray[r + 1, c + 1].transform.localPosition;
            instance.lastLocalPos = instance.transform.localPosition;
            instance.row_col = new Vector2(r + 1, c + 1);
            return true;
        }
        else
        {
            return false;
        }
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

                    if (m > 0 && i < rn -1)
                    {
                        if (modelsList[(i+1) * cn + (m-1)] > 0)
                        {
                            if (!ChipSquarePutable(r+i, c+m -1) && !ChipSquarePutable(r + i + 1, c + m))
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
                            chipSquare.state = ChipSquareState.Passitive;
                        }
                        else
                        {
                            chipSquare.state = ChipSquareState.Negative;
                        }
                    }
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
                        chipSquare.state = ChipSquareState.Passitive;
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
}
