using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChipListItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler  {

    [SerializeField]
    List<Image> itemsList;
    [SerializeField]
    GameObject btn;
    [SerializeField]
    GameObject positive;
    [SerializeField]
    GameObject negative;
    [SerializeField]
    ScrollRect scrollRect;

    private Color color;

    public IEnumerator InitItem(ChipInventory chipInventory)
    {
        yield return GameExtend.waitForEndOfFrame;

        HudEvent.Get(btn).onLongPress = OnLongPress;
        HudEvent.Get(btn).onClick = OnClick;

        color = SuperTool.CreateColor(chipInventory.config.color);

        for (int i = 0; i < itemsList.Count; ++i)
        {
            int v = chipInventory.model[i];

            if (v > 0)
            {
                itemsList[i].color = color;

                if (v == 2)
                {
                    positive.transform.position = itemsList[i].transform.position;
                }
                else if (v == 3)
                {
                    negative.transform.position = itemsList[i].transform.position;
                }
            }
            else
            {
                itemsList[i].color = Color.white;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        scrollRect.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        scrollRect.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        scrollRect.OnEndDrag(eventData);
    }

    private void OnLongPress()
    {

    }

    /// <summary>
    /// 点击的是偶需要生成一个实例
    /// </summary>
    private void OnClick()
    {

    }
}
