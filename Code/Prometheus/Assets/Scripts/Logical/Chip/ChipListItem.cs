using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChipListItem : DragableScrollItem {

    public ulong id;

    private Color color;
    public ChipConnectionItem connectionItem;
    [SerializeField]
    Text chipName;
    [SerializeField]
    Button button;
    [Space(5)]
    public ChipBoardInstance boardInstance;
    public ChipInventory chipInventory;


    private void Awake()
    {
        HudEvent.Get(button).onClick = OnClick;
        gameObject.SetActive(false);
    }

    private void OnLongPress()
    {

    }

    public void ShowChip(ChipInventory chip)
    {
        chipName.text = chip.config.name;
        gameObject.SetActive(true);
        chipInventory = chip;
        connectionItem.ShowChipConnection(chip, true);
    }

    private void OnDisable()
    {
        connectionItem.CleanConnectImage();
    }

    /// <summary>
    /// 点击的是偶需要生成一个实例
    /// </summary>
    private void OnClick()
    {
        boardInstance = ChipView.Instance.CreateBoardInstance(this.chipInventory);

        if (boardInstance != null)
        {
            ObjPool<ChipListItem>.Instance.RecycleObj(ChipView.Instance.itemName, id);
        }
    }
}
