using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipUpdateView : MuiSingleBase<ChipUpdateView> {

    [SerializeField]
    Button closeButton;
    
    public ChipMerge chipMerge;
    public string optionName = "OPN";

    public override IEnumerator Close(object param)
    {
        throw new System.NotImplementedException();
    }

    public override void Hide(object param)
    {
        gameObject.SetActive(false);
        chipMerge.ShowMergeBtns();
        chipMerge.Clean();
    }

    public override IEnumerator Init(object param)
    {
        chipMerge.Init();
        gameObject.SetActive(false);
  
        HudEvent.Get(closeButton).onClick = OnClose;

        return null;
    }

    private void OnClose()
    {
        MuiCore.Instance.Open(UiName.strStageView);
    }

    public override IEnumerator Open(object param)
    {
        gameObject.SetActive(true);
        chipMerge.gameObject.SetActive(true);
        chipMerge.ShowMergeBtns();

        return null;
    }
}
