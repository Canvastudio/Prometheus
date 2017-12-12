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
    Button backMerge;

    [Space(5)]
    [SerializeField]
    RectTransform optionalRoot;
    [SerializeField]
    ChipItem optionItem;

    [SerializeField]
    GameObject result;
    [SerializeField]
    Text resultCost;
    [SerializeField]
    Text resultName;
    [SerializeField]
    Text resultDescr;

    [SerializeField]
    Image stateBackground;

    [SerializeField]
    MaterialsInfo matInfo;

    [SerializeField]
    Sprite state1;
    [SerializeField]
    Sprite state2;
    [SerializeField]
    Sprite state3;

    [SerializeField]
    ChipConnectionItem con1;
    [SerializeField]
    ChipConnectionItem con2;
    [SerializeField]
    ChipConnectionItem con3;

    [SerializeField]
    GameObject icon1;
    [SerializeField]
    GameObject icon2;
    [SerializeField]
    GameObject icon3;
    [SerializeField]
    Text mergedDescribe;

    [SerializeField]
    GameObject mergeLight;
    [SerializeField]
    GameObject chipList;
    [SerializeField]
    GameObject chipSelect;


    int state = 0;
    int index = 0;

    bool firstShow = true;

    private string str1 = "合成芯片";
    private string str2 = "返回";
    private string str3 = "升级芯片盘";

    ChipInventory[] chips = new ChipInventory[2];

    public void Init()
    {
        ObjPool<ChipItem>.Instance.InitOrRecyclePool(ChipUpdateView.Instance.optionName, optionItem);

        HudEvent.Get(chipButton1).onClick = OnChipButton1;
        HudEvent.Get(chipButton2).onClick = OnChipButton2;
        //HudEvent.Get(mergeButton).onClick = OnMerge;
        //HudEvent.Get(backButton).onClick = ShowMergeBtns;
        HudEvent.Get(mergeButton).onClick = OnMerge;
        HudEvent.Get(backMerge).onClick = ShowMergeBtns;

        matInfo.RefreshOwned();
        matInfo.CleanCost();

        stateBackground.sprite = state1;
    }
    
    private void CleanChip1Chip2()
    {
        icon1.gameObject.SetActive(false);
        icon2.gameObject.SetActive(false);
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

        CheckMergeResult();

        if (index == 0)
        {
            icon1.gameObject.SetActive(true);
            con1.ShowChipConnection(chip, true);
        }
        else
        {
            icon2.gameObject.SetActive(true);
            con2.ShowChipConnection(chip, true);
        }
        

        if (chips[0] != null)
        {
            mergedDescribe.SetChipDescrible(chips[0].config);
            mergedDescribe.gameObject.SetActive(true);
        }
        else
        if (chips[1] != null)
        {
            mergedDescribe.SetChipDescrible(chips[1].config);
            mergedDescribe.gameObject.SetActive(true);
        }
        else
        {
            mergedDescribe.gameObject.SetActive(false);
        }

        mergeLight.gameObject.SetActive(false);
    }

    ChipConfig mergeConfig;
    int mergeCost;
    Stuff[] sts;
    int[] vas;
    private void CheckMergeResult()
    {
        matInfo.CleanCost();

        icon3.gameObject.SetActive(false);
        mergeLight.gameObject.SetActive(false);

        if (chips[0] != null && chips[1] != null)
        {
            mergeConfig = ChipCore.Instance.ChipMerge(chips[0], chips[1], out mergeCost);

            resultName.text = mergeConfig.name;
            resultCost.text = mergeCost.ToString();
            resultDescr.SetChipDescrible(mergeConfig);

            result.SetActive(true);

            sts =chips[0].config.stuffCost.stuffs.ToArray();
            vas = chips[0].config.stuffCost.values.ToArray();

            for (int i = 0; i < sts.Length; ++i)
            {
                matInfo.SetCost(sts[i], vas[i]);
            }
        }
        else
        {
            icon3.gameObject.SetActive(false);
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
                PopTipView.Instance.Show("deficient_resources");
                return;
            }
        }

        StageCore.Instance.Player.inventory.RemoveChip(chips[0]);
        StageCore.Instance.Player.inventory.RemoveChip(chips[1]);
        var chip = StageCore.Instance.Player.inventory.AddChip(mergeConfig.id, mergeCost);

        chips[0] = null;
        chips[1] = null;

        for (int i = 0; i < sts.Length; ++i)
        {
            StageCore.Instance.Player.inventory.ChangeStuffCount(sts[i], -vas[i]);
        }

        matInfo.RefreshOwned();
        matInfo.CleanCost();

        stateBackground.sprite = state3;
        icon3.gameObject.SetActive(true);
        con3.ShowChipConnection(chip);

        mergeLight.gameObject.SetActive(true);
        CleanChip1Chip2();
    }

    private void ShowChipOption(ulong cid = 0)
    {
        if (state == 0)
        {
            chipList.gameObject.SetActive(true);

            LeanTween.moveLocalX(chipSelect, -750, 0.3f);
            LeanTween.moveLocalX(chipList, 0, 0.3f);
            ObjPool<ChipItem>.Instance.RecyclePool(ChipUpdateView.Instance.optionName);

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

                ulong id;
                var opt = ObjPool<ChipItem>.Instance.GetObjFromPoolWithID(out id, ChipUpdateView.Instance.optionName);
                opt.ShowChipInfo(chip, id, OnMergeOptionClick);
                opt.SetParentAndNormalize(optionalRoot);
            }

            mergeButton.gameObject.SetActive(false);
            backMerge.gameObject.SetActive(true);

            state = 1;
        }
    }

    public void Clean()
    {
        chips[0] = null;
        chips[1] = null;
        icon1.gameObject.SetActive(false);
        icon2.gameObject.SetActive(false);
        icon3.gameObject.SetActive(false);
        result.gameObject.SetActive(false);
    }

    public void ShowMergeBtns()
    {
        LeanTween.moveLocalX(chipSelect, 0, 0.3f);
        LeanTween.moveLocalX(chipList, 750f, 0.3f);
        chipList.gameObject.SetActive(false);
        mergeButton.gameObject.SetActive(true);
        backMerge.gameObject.SetActive(false);
        state = 0;

        if (chips[0] != null && chips[1] != null)
        {
            mergeButton.interactable = true;
        }
        else
        {
            mergeButton.interactable = false;
        }
    }
    

    public void Hide()
    {
        ShowMergeBtns();
        mergeButton.gameObject.SetActive(false);
        backMerge.gameObject.SetActive(false);
    }

    public void OnMergeOptionClick(ChipItem op)
    {
        SetChip(op.chip);
        ShowMergeBtns();
    }
}
