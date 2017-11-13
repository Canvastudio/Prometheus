using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipUpdateView : SingleGameObject<ChipUpdateView> {

    public ChipMerge chipMerge;
    public string optionName = "OPN";

    public void InitView()
    {
        chipMerge.Init();

        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
}
