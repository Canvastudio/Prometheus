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
    [SerializeField]
    Button mergeButton;
    [SerializeField]
    Button backButton;

    [Space(5)]
    [SerializeField]
    RectTransform optionalRoot;
    [SerializeField]
    MergeOption optionItem;

    [SerializeField]
    MergeOption[] chipInfo;

    [SerializeField]
    GameObject result;
    [SerializeField]
    Text resultCost;
    [SerializeField]
    Text resultName;
    [SerializeField]
    Text resultDescr;

    [SerializeField]
    MaterialsInfo matInfo;

    int state = 0;
    int index = 0;

    bool firstShow = true;

    ChipInventory[] chips = new ChipInventory[2];

    public void Init()
    {
        ObjPool<MergeOption>.Instance.InitOrRecyclePool(ChipUpdateView.Instance.optionName, optionItem);

        HudEvent.Get(chipButton1).onClick = OnChipButton1;
        HudEvent.Get(chipButton2).onClick = OnChipButton2;
        HudEvent.Get(mergeButton).onClick = OnMerge;
        HudEvent.Get(backButton).onClick = ShowMergeBtns;

        matInfo.RefreshOwned();
        matInfo.CleanCost();
    }
    
    private void OnChipButton1()
    {
        index = 0;

        ulong cid = 0;

        if (chips[1] != null)
        {
            cid = chips[1].config.id;
        }

        ShowChipOption(cid);
    }

    private void OnChipButton2()
    {
        index = 1;

        ulong cid = 0;

        if (chips[0] != null)
        {
            cid = chips[0].config.id;
        }

        ShowChipOption(cid);
    }

    public void SetChip(ChipInventory chip)
    {
        chips[index] = chip;

        if (chip != null)
        {
            chipInfo[index].Init(chip, -1, false);
            chipInfo[index].gameObject.SetActive(true);
        }
        else
        {
            chipInfo[index].gameObject.SetActive(false);
        }

        CheckMergeResult();
    }

    ChipConfig mergeConfig;
    int mergeCost;

    private void CheckMergeResult()
    {
        matInfo.CleanCost();

        if (chips[0] != null && chips[1] != null)
        {
            mergeConfig = ChipCore.Instance.ChipMerge(chips[0], chips[1], out mergeCost);

            resultName.text = mergeConfig.name;
            resultCost.text = mergeCost.ToString();
            resultDescr.text = mergeConfig.descrip;

            result.SetActive(true);

            var sts =chips[0].config.stuffCost.stuffs.ToArray();
            var vas = chips[0].config.stuffCost.values.ToArray();

            for (int i = 0; i < sts.Length; ++i)
            {
                matInfo.SetCost(sts[i], vas[i]);
            }
        }
        else
        {
 
            result.SetActive(false);
        }

        
    }

    private void OnMerge()
    {
        var sts = chips[0].config.stuffCost.stuffs.ToArray();
        var vas = chips[0].config.stuffCost.values.ToArray();

        for (int i = 0; i < sts.Length; ++i)
        {
            if (StageCore.Instance.Player.inventory.GetStuffCount(sts[i]) < vas[i])
            {
                PopTipView.Instance.Show("资源不足？？？");
                return;
            }
        }

        StageCore.Instance.Player.inventory.RemoveChip(chips[0]);
        StageCore.Instance.Player.inventory.RemoveChip(chips[1]);
        StageCore.Instance.Player.inventory.AddChip(mergeConfig.id, mergeCost);
    }

    private void ShowChipOption(ulong cid = 0)
    {
        if (state == 0)
        {
            LeanTween.moveLocalX(this.gameObject, -750, 0.3f);

            ObjPool<MergeOption>.Instance.RecyclePool(ChipUpdateView.Instance.optionName);

            var list = StageCore.Instance.Player.inventory.GetChipList();

            foreach (var chip in list)
            {
                if (cid != 0)
                {
                    if (chip.config.id != cid)
                    {
                        continue;
                    }

                    if (chips[0] != null && chip.uid == chips[0].uid) continue;
                    if (chips[1] != null && chip.uid == chips[1].uid) continue;
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
            LeanTween.moveLocalX(this.gameObject, 0, 0.3f);

            state = 0;
        }
    }
}
