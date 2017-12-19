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
    GameObject normal;
    [SerializeField]
    GameObject max;
    [SerializeField]
    Text curLv;
    [SerializeField]
    Text nextLv;

    [SerializeField]
    Image graphics;

    GlobalParameterConfig gc;

    int[] cost;
    MaterialPropertyBlock prop;
    int Rid;
    int Cid;
    int radius;
    public void Init()
    {
        Rid = Shader.PropertyToID("U");
        Cid = Shader.PropertyToID("C");

        HudEvent.Get(updateButton).onClick = OnUpdate;
        prop = new MaterialPropertyBlock();
        radius = GlobalParameterConfig.GetConfigDataById<GlobalParameterConfig>(1).initRadius;
        gc = GlobalParameterConfig.GetConfigDataById<GlobalParameterConfig>(1);
    }

    public void Show()
    {
        updateButton.gameObject.SetActive(true);

        mat.RefreshOwned();

        int t = ChipCore.Instance.chipBoardUpdate + 1;

        curLv.text = t.ToString();
        nextLv.text = (t+1).ToString();

        if (t <= gc.maxUpdateCount)
        {

            var ts = gc.chipDiskExtensions.Count(
                (int)StageCore.Instance.Player.playerId);

            if (t > ts)
            {
                t = ts;
            }

            cost = gc.chipDiskExtensions.ToArray(
                (int)StageCore.Instance.Player.playerId, t - 1);

            int cc = gc.costRatio.Count();

            if (ChipUpdateView.Instance.upTime >= cc)
            {
                cc = cc - 1;
            }
            else
            {
                cc = ChipUpdateView.Instance.upTime;
            }

            for (int i = 0; i < cost.Length; ++i)
            {
                Stuff s = (Stuff)i;
                int c = Mathf.FloorToInt(cost[i] * gc.costRatio[cc]);

                mat.SetCost(s, c);
            }

            max.SetActive(false);
            normal.SetActive(true);
            graphics.gameObject.SetActive(true);
        }
        else
        {
            normal.gameObject.SetActive(false);
            updateButton.interactable = false;
            max.SetActive(true);
            graphics.gameObject.SetActive(false);
        }
    }



    private void Update()
    {
        ShowGraphic();
    }

    private void ShowGraphic()
    {

        //prop.SetFloat("v1", 0.5f);
        //prop.SetFloat("v2", 0.25f);

        ////升级0次得时候 偏移得格子数量是4
        float n1 = ChipCore.Instance.chipBoardUpdate + radius + 0.3f;
        graphics.materialForRendering.SetFloat(Rid, n1);
        graphics.materialForRendering.SetFloat(Cid, 1);
    }
     
    public void Hide()
    {
        updateButton.gameObject.SetActive(false);
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
        ChipUpdateView.Instance.upTime += 1;

        Show();
    }
}
