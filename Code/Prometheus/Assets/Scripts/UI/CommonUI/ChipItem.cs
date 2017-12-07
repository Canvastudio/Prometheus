using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChipItem : DragableScrollItem {

    [SerializeField]
    Text chipName;
    [SerializeField]
    Text chipUse;
    [SerializeField]
    Text chipQuality;
    [SerializeField]
    Text chipDescribe;
    [SerializeField]
    Text chipCost;
    [SerializeField]
    Button button;
    [SerializeField]
    ChipConnectionItem connectionItem;

    public ChipInventory chip;
    public int id;
    public bool isUsed = false;
    private string best = "极限";
    private string better = "优秀";

    Callback<ChipItem> cb;

    public void Awake()
    {
        HudEvent.Get(button).onClick = OnClick;
        //gameObject.SetActive(false);
    }

    private void OnClick()
    {
        if (cb != null)
        {
            cb.Invoke(this);
        }
    }

    public void ShowChipInfo(ChipInventory chip,int id, Callback<ChipItem> _cb)
    {
        this.chip = chip;
        gameObject.SetActive(true);
        cb = _cb;
        connectionItem.ShowChipConnection(chip, true);
        chipName.text = chip.config.name;
        isUsed = chip.boardInstance != null;
        chipUse.gameObject.SetActive(isUsed);
        chipCost.text = chip.cost.ToString();
        int quality = chip.config.PowerPerformance(Mathf.CeilToInt(chip.cost));
        if (quality == 0)
        {
            chipQuality.gameObject.SetActive(true);
            chipQuality.text = best;
        }
        else if (quality == 1)
        {
            chipQuality.gameObject.SetActive(true);
            chipQuality.text = better;
        }
        else
        {
            chipQuality.gameObject.SetActive(false);
        }
        chipDescribe.text = chip.config.descrip;
    }
}
