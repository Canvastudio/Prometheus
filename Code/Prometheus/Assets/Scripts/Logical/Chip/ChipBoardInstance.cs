using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChipBoardInstance : MonoBehaviour , IDragHandler, IBeginDragHandler, IEndDragHandler{

    public ChipGrid chipGrid;

    public ChipInventory chipInventory;

    [SerializeField]
    List<BoardInstanceNode> itemsList;

    public Vector3 lastLocalPos;

    private Vector3 temp_localpos;

    private Vector2 offset;

    /// <summary>
    /// 9宫格中心所在的行列索引
    /// </summary>
    public Vector2 row_col;

    public bool hasPut = false;

    public void Init(ChipListItem chipItem)
    {
        chipInventory = chipItem.chipInventory;
        Color color = SuperTool.CreateColor(chipInventory.config.color);
        chipItem.chipInventory.boardInstance = this;

        for (int i = 0; i < itemsList.Count; ++i)
        {
            int v = chipInventory.model[i];

            itemsList[i].Set(v, color);
        }
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
}
