using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragableScrollItem : MonoBehaviour , IBeginDragHandler, IEndDragHandler, IDragHandler{

    [SerializeField]
    ScrollRect scroll;

    public void OnBeginDrag(PointerEventData eventData)
    {
        scroll.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        scroll.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        scroll.OnEndDrag(eventData);
    }
}
