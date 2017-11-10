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

    public int id;

    public ChipInventory chip;

    public void Init(ChipInventory _chip,int _id)
    {
        chipName.text = chip.config.name;
        chipDescribe.text = chip.config.descrip;
        id = _id;
        HudEvent.Get(button).onClick = OnClick;
    }

    private void OnClick()
    {
        
    }
}
