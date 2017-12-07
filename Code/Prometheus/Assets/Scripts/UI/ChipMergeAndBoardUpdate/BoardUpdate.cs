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
    RectTransform center;
    [SerializeField]
    RectTransform[] flashs;
    [SerializeField]
    RectTransform[] darks;
    [SerializeField]
    Image graphics;

    int[] cost;
    MaterialPropertyBlock prop;
    int Rid;

    public void Init()
    {
        Rid = Shader.PropertyToID("R");
        HudEvent.Get(updateButton).onClick = OnUpdate;
        prop = new MaterialPropertyBlock();
    }

    public void Show()
    {
        updateButton.gameObject.SetActive(true);

        mat.RefreshOwned();

        int t = ChipCore.Instance.chipBoardUpdate + 1;

        curLv.text = ChipCore.Instance.chipBoardUpdate.ToString();
        nextLv.text = t.ToString();

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
        graphics.GetComponent<Renderer>().GetPropertyBlock(prop);
        //prop.SetFloat("v1", 0.5f);
        //prop.SetFloat("v2", 0.25f);

        ////升级0次得时候 偏移得格子数量是4
        float n1 = ChipCore.Instance.chipBoardUpdate + 3.3f;

        prop.SetFloat(Rid, n1);
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

        Show();
    }
}
