using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipMerge : MonoBehaviour {

    [Space(5)]
    [SerializeField]
    Button chipButton1;
    [SerializeField]
    Button chipButton2;
    [SerializeField]
    Button chipButton3;

    [Space(5)]
    [SerializeField]
    RectTransform optionalRoot;
    [SerializeField]
    MergeOption optionItem;

    bool firstShow = true;

    public void Init()
    {
        ObjPool<MergeOption>.Instance.InitOrRecyclePool(ChipUpdateView.Instance.optionName, optionItem);
    }
    
    public void Show()
    {
        var list = StageCore.Instance.Player.inventory.GetChipList();

        foreach (var chip in list)
        {
            int id;
            var opt = ObjPool<MergeOption>.Instance.GetObjFromPoolWithID(out id, ChipUpdateView.Instance.optionName);
            opt.Init(chip, id);
            opt.SetParentAndNormalize(optionalRoot);
        }
    }

   
}
