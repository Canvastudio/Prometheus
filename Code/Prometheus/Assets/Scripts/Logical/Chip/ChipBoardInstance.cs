using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChipBoardInstance : MonoBehaviour , IDragHandler, IBeginDragHandler, IEndDragHandler{

    public ChipGrid chipGrid;

    public ChipInventory chipInventory;

    [SerializeField]
    List<BoardInstanceNode> itemsList;

    public Vector3 lastLocalPos;

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

    private void OnLongPress()
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
