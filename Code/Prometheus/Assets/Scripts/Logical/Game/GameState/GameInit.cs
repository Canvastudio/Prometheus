﻿using System;
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

        yield return MuiCore.Instance.Init(UiName.strChipUpdateView);

        yield return MuiCore.Instance.Init(UiName.strSkillInfoView);

        yield return MuiCore.Instance.Init(UiName.strChipDetailVew);

        //测试代码
        {
            foreach (var value in GameTestData.Instance.AddChips)
            {
                if (value > 0)
                {

                    StageCore.Instance.Player.inventory.AddChip(value);
                }
            }

            for (int i = 0; i < GameTestData.Instance.Add_4_Stuff.Length; ++i)
            {
                StageCore.Instance.Player.inventory.ChangeStuffCount((Stuff)i, GameTestData.Instance.Add_4_Stuff[i]);
            }
        }
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
