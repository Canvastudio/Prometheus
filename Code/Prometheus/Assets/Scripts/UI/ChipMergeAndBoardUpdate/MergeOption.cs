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

    public int id;

    public ChipInventory chip;

    public void Init(ChipInventory _chip,int _id)
    {
        chip = _chip;
        chipName.text = chip.config.name;
        chipDescribe.text = chip.config.descrip;
        id = _id;
        HudEvent.Get(button).onClick = OnClick;
        chipLink.Set(_chip);
    }

    private void OnClick()
    {
        
    }
}
