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

    int state = 0;

    bool firstShow = true;

    public void Init()
    {
        ObjPool<MergeOption>.Instance.InitOrRecyclePool(ChipUpdateView.Instance.optionName, optionItem);
    }
    
    private void OnChipButton1()
    {

    }

    private void OnChipButton2()
    {

    }

    private void OnChipButton3()
    {

    }

    private void ShowChipOption(ulong cid = 0)
    {
        if (state == 0)
        {
            LeanTween.moveLocalX(this.gameObject, -750, 0.5f);

            var list = StageCore.Instance.Player.inventory.GetChipList();

            foreach (var chip in list)
            {
                if (cid != 0)
                {
                    if (chip.config.id != cid)
                    {
                        continue;
                    }
                }

                int id;
                var opt = ObjPool<MergeOption>.Instance.GetObjFromPoolWithID(out id, ChipUpdateView.Instance.optionName);
                opt.Init(chip, id);
                opt.SetParentAndNormalize(optionalRoot);
            }

            state = 1;
        }
    }

    public void ShowMergeBtns()
    {
        if (state == 1)
        {
            LeanTween.moveLocalX(this.gameObject, 0, 0.5f);

            state = 0;
        }
    }



   
}
