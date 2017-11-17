using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChipBoardInstance : BoardInstanceBase , IDragHandler, IBeginDragHandler, IEndDragHandler{

    public bool isPowered = false;

    public ChipGrid chipGrid;
    public ChipInventory chipInventory;

    [SerializeField]
    List<BoardInstanceNode> itemsList;
    [SerializeField]
    UnityEngine.UI.Button button;
    [SerializeField]
    UnityEngine.UI.Text pwr;

    public Vector3 lastLocalPos;
    private Vector3 temp_localpos;
    private Vector2 offset;

    public bool chipGridMeet = false;

    public int positiveIndex = int.MinValue;
    public int negativeIndex = int.MinValue;

    public bool hasPut = false;

    public void Init(ChipInventory _chipInventory)
    {
        chipInventory = _chipInventory;
        Color color = SuperTool.CreateColor(chipInventory.config.color);
        chipInventory.boardInstance = this;

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

        castPower = chipInventory.cost;
        Messenger.AddListener(ChipBoardEvent.CheckPowerState, OnPowerGridRefresh);
    }

    private void OnPowerGridRefresh()
    {
        depth = int.MaxValue;
        isPowered = false;
    }
    
    public void PutBack()
    {
        transform.localPosition = temp_localpos;
    }

    private void Awake()
    {
        HudEvent.Get(button.gameObject).onLongPress = OnLongPress;
    }

    private void OnLongPress()
    {
        ChipView.Instance.selectChip = this;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 local_pos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent.Rt(),
            eventData.position,
            ChipView.Instance.camera,
            out local_pos);

        transform.localPosition = local_pos - offset;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.Rt(),
            eventData.position,
            ChipView.Instance.camera,
            out offset);

        temp_localpos = transform.localPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //尝试放在拖动位置
        if (!ChipView.Instance.MatrixDragPut(this))
        {
            PutBack();
        }
    }

    public void GetOutPowerRC(out int r, out int c)
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

        r = row - 1 + positive_index / 3;
        c = col - 1 + positive_index % 3;
    }

    public void GetInPowerRC(out int r, out int c)
    {
        int positive_index = -1;

        for (int i = 0; i < 9; ++i)
        {
            int v = chipInventory.model[i];

            if (v == 2)
            {
                positive_index = i;
            }
        }

        r = row - 1 + positive_index / 3;
        c = col - 1 + positive_index % 3;
    }

    protected override void SetDepth(int depth)
    {
        base.SetDepth(depth);

        pwr.text = depth.ToString();
    }

    protected override void OnSetPowerState(int value)
    {
        base.OnSetPowerState(value);

        if (value == 1)
        {
            pwr.color = Color.blue;
        }
        else
        {
            pwr.color = Color.red;
        }
    }
}
