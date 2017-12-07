using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipUpdateView : MuiSingleBase<ChipUpdateView> {

    [SerializeField]
    Button closeButton;
    [SerializeField]
    Button mergeButon;
    [SerializeField]
    Button boardButton;
    [SerializeField]
    MaterialsInfo stuff;

    public BoardUpdate board;
    public ChipMerge chipMerge;
    public string optionName = "chipItem";

    int state = 0;

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
        board.Init();
        gameObject.SetActive(false);
  
        HudEvent.Get(closeButton).onClick = OnClose;
        HudEvent.Get(mergeButon).onClick = OnMerge;
        HudEvent.Get(boardButton).onClick = OnBoard;

        return null;
    }


    private void OnMerge()
    {
        board.Hide();
        chipMerge.ShowMergeBtns();
        chipMerge.gameObject.SetActive(true);
        board.gameObject.SetActive(false);
        state = 0;
    }

    private void OnBoard()
    {
        chipMerge.Hide();
        chipMerge.gameObject.SetActive(false);
        board.gameObject.SetActive(true);
        board.Show();
        state = 1;
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
        board.gameObject.SetActive(false);
        stuff.RefreshOwned();
        stuff.CleanCost();
        return null;
    }
}
