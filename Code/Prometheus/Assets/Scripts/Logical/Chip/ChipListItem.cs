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

    bool isChipDraging = false;
    Vector2 dragPath;
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

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (!isChipDraging)
        {
            base.OnDrag(eventData);
        }

        dragPath += eventData.delta;

        if (boardInstance == null)
        {
            if (dragPath.y > 100)
            {
                isChipDraging = true;

                boardInstance = ChipView.Instance.CreateBoardInstanceDraging(chipInventory);
                ChipView.Instance.selectChip = boardInstance;
            }
        }

        if (boardInstance != null)
        {
            Vector2 lp;
            RectTransformUtility.ScreenPointToLocalPointInRectangle
                                                    (boardInstance.transform.parent.Rt(),
                                                    eventData.position,
                                                    GameManager.Instance.GCamera, out lp);

            boardInstance.transform.localPosition = lp;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        isChipDraging = false;

        if (boardInstance != null)
        {
            if (!ChipView.Instance.MatrixDragPut(boardInstance))
            {
                boardInstance = null;
            }
            else
            {
                ObjPool<ChipListItem>.Instance.RecycleObj(ChipView.Instance.itemName, id);
            }
        }
    }
}
