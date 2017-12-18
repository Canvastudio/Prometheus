using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragableScrollItem : MonoBehaviour , IBeginDragHandler, IEndDragHandler, IDragHandler{

    [SerializeField]
    ScrollRect scroll;

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        scroll.OnBeginDrag(eventData);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        scroll.OnDrag(eventData);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        scroll.OnEndDrag(eventData);
    }
}
