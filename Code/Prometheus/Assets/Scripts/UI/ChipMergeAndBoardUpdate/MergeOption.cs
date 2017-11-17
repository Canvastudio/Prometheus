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
    [SerializeField]
    Text good;

    string best = "(极限)";
    string better = "(优秀)";

    public int id;
    public ChipInventory chip;
    Callback<MergeOption> callback;

    public void Init(ChipInventory _chip,int _id = -1, Callback<MergeOption> _callback = null)
    {
        chip = _chip;
        chipName.text = chip.config.name;
        chipCost.text = chip.cost.ToString();
        int v = chip.config.PowerPerformance(Mathf.FloorToInt(chip.cost));
        if (v == 1)
        {
            good.text = best;
        }
        else if (v ==2)
        {
            good.text = better;
        }
        else
        {
            good.text = null;
        }

        chipDescribe.text = chip.config.descrip;
        id = _id;

        if (_callback != null)
        {
            callback = _callback;
            HudEvent.Get(button).onClick = OnClick;
        }

        chipLink.Set(_chip);
    }

    private void OnClick()
    {
        callback.Invoke(this);
    }


}
