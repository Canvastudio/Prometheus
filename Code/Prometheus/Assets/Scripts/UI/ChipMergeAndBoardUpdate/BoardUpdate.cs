using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardUpdate : MonoBehaviour {

    [SerializeField]
    MaterialsInfo mat;
    [SerializeField]
    Button updateButton;

    [SerializeField]
    Text lvText;
    int[] cost;

    public void Init()
    {
        HudEvent.Get(updateButton).onClick = OnUpdate;

    }

    public void Show()
    {
        mat.RefreshOwned();

        int t = ChipCore.Instance.chipBoardUpdate + 1;
        lvText.text = t.ToString();

        if (t <= GlobalParameterConfig.GetConfigDataById<GlobalParameterConfig>(1).maxUpdateCount)
        {

            var ts = GlobalParameterConfig.GetConfigDataById<GlobalParameterConfig>(1).chipDiskExtensions.Count(
                (int)StageCore.Instance.Player.playerId);

            if (t > ts)
            {
                t = ts;
            }

            cost = GlobalParameterConfig.GetConfigDataById<GlobalParameterConfig>(1).chipDiskExtensions.ToArray(
                (int)StageCore.Instance.Player.playerId, t - 1);

            for (int i = 0; i < cost.Length; ++i)
            {
                Stuff s = (Stuff)i;
                int c = cost[i];

                mat.SetCost(s, c);
            }
        }
        else
        {
            updateButton.interactable = false;
        }


    }

    public void OnUpdate()
    {
        for (int i = 0; i < cost.Length; ++i)
        {
            Stuff s = (Stuff)i;
            int c = cost[i];

            if (StageCore.Instance.Player.inventory.GetStuffCount(s) < c)
            {
                PopTipView.Instance.Show("deficient_resources");
                return;
            }
        }

        for (int i = 0; i < cost.Length; ++i)
        {
            Stuff s = (Stuff)i;
            int c = cost[i];

            StageCore.Instance.Player.inventory.ChangeStuffCount(s, -c);

        }

        ChipCore.Instance.chipBoardUpdate += 1;

        Show();
    }
}
