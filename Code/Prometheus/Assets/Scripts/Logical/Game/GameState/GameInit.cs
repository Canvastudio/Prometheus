using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : IState
{
    public string name
    {
        get
        {
            return Predefine.GAME_INIT;
        }
    }

    public IEnumerator DoState()
    {
        yield return CoroCore.Instance.ExStartCoroutine(SuperConfig.Instance.LoadAsync());

        BrickNameCore.Instance.SetCurrentScene(0);

        yield return FxCore.Instance.PreLoadStageFx();

        yield return MuiCore.Instance.Init(UiName.strStageView);

        yield return MuiCore.Instance.OpenIE(UiName.strStageView);

        yield return MuiCore.Instance.Init(UiName.strChipView);

        //StageCore.Instance.Player.inventory.ChangeStuffCount(Stuff.Coherer, 50);
        //StageCore.Instance.Player.inventory.ChangeStuffCount(Stuff.Core, 50);
        //StageCore.Instance.Player.inventory.ChangeStuffCount(Stuff.Organics, 50);
        //StageCore.Instance.Player.inventory.ChangeStuffCount(Stuff.Soul, 50);

        yield return MuiCore.Instance.Init(UiName.strChipUpdateView);

        yield return MuiCore.Instance.Init(UiName.strSkillInfoView);

        //测试代码
        StageCore.Instance.Player.inventory.AddChip(100001);
        StageCore.Instance.Player.inventory.AddChip(100001);
        StageCore.Instance.Player.inventory.AddChip(100007);
        StageCore.Instance.Player.inventory.AddChip(100136);


    }

    public IState GetNextState()
    {
        return GameStateMachine.Instance.GetStateByName(Predefine.GAME_STAGE);
    }

    public IEnumerator StopState()
    {
        throw new NotImplementedException();
    }
}
