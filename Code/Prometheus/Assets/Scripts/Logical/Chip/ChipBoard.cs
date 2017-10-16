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
    RectTransform chipRoot;
    [SerializeField]
    GameObject backgroud;

    [Space(5)]
    [SerializeField]
    public SpriteAtlas spriteAtlas;

    private ChipDiskConfig config;
    private List<ChipGrid> gridList;
    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField]
    Button closeBtn;
    [SerializeField]
    RectTransform chipListRoot;
    [SerializeField]
    ChipListItem listItem;

    string itemName = "ChipListItem";

    private void Start()
    {
        HudEvent.Get(closeBtn.gameObject).onClick = CloseChipBoard;
        ObjPool<ChipListItem>.Instance.InitOrRecyclePool(itemName, listItem);
    }

    private ChipSquare[,] chipSquareArray = new ChipSquare[20, 11];

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

        ShowChipList();
    }

    public void CloseChipBoard()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    private void ShowChipList()
    {
        var chipList = StageCore.Instance.Player.inventory.GetChipList();

        for (int i = 0; i < chipList.Count; ++i)
        {
            var chip = ObjPool<ChipListItem>.Instance.GetObjFromPool(itemName);
            chip.transform.SetParent(chipListRoot);
            chip.transform.localScale = Vector3.one;
            chip.gameObject.SetActive(true);
            StartCoroutine(chip.InitItem(chipList[i]));
        }
    }
}
