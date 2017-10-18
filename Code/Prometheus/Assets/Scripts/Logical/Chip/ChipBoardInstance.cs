using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChipBoardInstance : MonoBehaviour , IDragHandler, IBeginDragHandler, IEndDragHandler{

    public uint uid;

    public ChipGrid chipGrid;
    public ChipInventory chipInventory;
    public ChipSquare powerSquare;

    [SerializeField]
    List<BoardInstanceNode> itemsList;

    public Vector3 lastLocalPos;
    private Vector3 temp_localpos;
    private Vector2 offset;

    public int positiveIndex = int.MinValue;
    public int negativeIndex = int.MinValue;

    /// <summary>
    /// 9宫格中心所在的行列索引
    /// </summary>
    public Vector2 row_col;

    public bool hasPut = false;

    public void Init(ChipListItem chipItem)
    {
        Messenger.AddListener(ChipBoardEvent.CheckPowerState, OnCheckPower);
        chipInventory = chipItem.chipInventory;
        Color color = SuperTool.CreateColor(chipInventory.config.color);
        chipItem.chipInventory.boardInstance = this;

        for (int i = 0; i < itemsList.Count; ++i)
        {
            int v = chipInventory.model[i];

            itemsList[i].Set(v, color);

            if (v == 2)
            {
                positiveIndex = i;
            }

            else if (v == 3) negativeIndex = i;
        }
    }

    private void OnCheckPower()
    {
        powerSquare = null;
    }

    public void PutBack()
    {
        transform.localPosition = temp_localpos;
    }

    private void OnLongPress()
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 local_pos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent.Rt(),
            eventData.position,
            ChipBoard.Instance.camera,
            out local_pos);

        transform.localPosition = local_pos - offset;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.Rt(),
            eventData.position,
            ChipBoard.Instance.camera,
            out offset);

        temp_localpos = transform.localPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //尝试放在拖动位置
        if (!ChipBoard.Instance.MatrixDragPut(this))
        {
            PutBack();
        }
    }

    public void OnDisable()
    {
        Messenger.RemoveListener(ChipBoardEvent.CheckPowerState, OnCheckPower);
    }

    public void GetNegativeRC(out int r, out int c)
    {
        int positive_index = -1;

        for (int i = 0; i < 9; ++i)
        {
            int v = chipInventory.model[i];

            if (v ==3)
            {
                positive_index = i;
            }
        }

        r = (int)row_col.x - 1 + positive_index / 3;
        c = (int)row_col.y - 1 + positive_index % 3;
    }
}
