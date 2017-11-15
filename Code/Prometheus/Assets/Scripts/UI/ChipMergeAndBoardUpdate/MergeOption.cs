using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeOption : DragableScrollItem {

    [SerializeField]
    Button button;
    [SerializeField]
    Text chipName;
    [SerializeField]
    Text chipDescribe;
    [SerializeField]
    ChipLink chipLink;
    [SerializeField]
    Text chipCost;
    [SerializeField]
    ChipMerge merge;

    public int id;
    public ChipInventory chip;

    public void Init(ChipInventory _chip,int _id = -1, bool btn = true)
    {
        chip = _chip;
        chipName.text = chip.config.name;
        chipCost.text = chip.cost.ToString();
        chipDescribe.text = chip.config.descrip;
        id = _id;

        if (btn)
        {
            HudEvent.Get(button).onClick = OnClick;
        }

        chipLink.Set(_chip);
    }

    private void OnClick()
    {
        merge.SetChip(chip);
        merge.ShowMergeBtns();
    }
}
