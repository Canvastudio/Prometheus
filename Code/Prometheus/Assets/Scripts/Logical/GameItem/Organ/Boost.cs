using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : GameItemBase, IReactive
{
    public StateConfig stateConfig;

    public void Reactive()
    {
        var player = StageCore.Instance.Player;
        StateIns ins = new StateIns(stateConfig, player, null);
        player.state.AddStateIns(ins);
    }
}
