using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : SingleGameObject<Main>
{
    public  GameObject stage;
    public  GameObject uiStage;
    public GameObject ControlButton;


    protected override void Init()
    {
        SuperTimer.Instance.CreatAndBound(this);
        ControlButton.SetActive(false);
    }

    void Start()
    {
        SuperResource.Instance.LoadAsync();
        StartCoroutine(Check());
        var mb = MessageCenter.CreatMsgBuilder(this);
        mb.AddListener("召唤炮台", On_炮台);
        mb.AddListener("状态气泡", On_气泡);
        mb.AddListener("浮动文字", On_浮动文字);
        mb.AddListener("复活石碑", On_复活石碑);
        mb.AddListener("近战演示", On_近战演示);
    }

    private IEnumerator Check()
    {
        while (!SuperResource.Instance.IsDone)
        {
            yield return new WaitForEndOfFrame();
        }
        ControlButton.SetActive(true);
    }

    void Destroy()
    {
        MessageCenter.RemoveMsgBuilder(this);
    }

    private void On_炮台(object arg)
    {
        if (!ClickLimit.AlowClick) return;
        Background.Instance.Reset();
        int count = Random.Range(1, 6);
        ClickLimit.AddLock(this, count);
        for (int i = 0; i < count; i++)
        {
            SuperTimer.Instance.SetTimer(Random.Range(0.1f, 0.3f), CreatGuns);
        }

    }

    private void CreatGuns(object arg)
    {
        GameObject tar = Background.Instance.GetGrids();
        GameObject gun = SuperResource.Instance.GetInstance("炮台");
        Vector2 pos = tar.transform.position;
        gun.transform.position = pos;
        SuperTool.SetParentWithLocal(stage.transform, gun.transform);
        ClickLimit.UnLock(this, true);
    }


    private void On_气泡(object arg)
    {
        if (!ClickLimit.AlowClick) return;
        Background.Instance.Reset();
        var t = SuperResource.Instance.GetInstance("怪物");
        SuperTool.SetParentWithLocal(stage.transform, t.transform);

    }

    private void On_浮动文字(object arg)
    {
        if (!ClickLimit.AlowClick) return;
        var t = SuperResource.Instance.GetInstance("浮动文字");
        SuperTool.SetParentWithLocal(uiStage.transform, t.transform);

    }

    private void On_复活石碑(object arg)
    {
        if (!ClickLimit.AlowClick) return;
        var stone = SuperResource.Instance.GetInstance("复活石碑");
        SuperTool.SetParentWithLocal(stage.transform, stone.transform);

        SuperTimer.Instance.SetTimer(4f, Resu, stone);
        SuperTimer.Instance.SetTimer(1f, o =>
        {
            if (stone != null)
            {
                var t = stone.GetComponent<MonsterResurrection>();
                if (t != null) t.MonsterDie();
            }
        });

    }


    private void On_近战演示(object arg)
    {
        if (!ClickLimit.AlowClick) return;
        var t = SuperResource.Instance.GetInstance("近战演示");
        SuperTool.SetParentWithLocal(stage.transform, t.transform);

    }
    



    private void Resu(object arg)
    {
        GameObject stone = arg as GameObject;
        if (stone == null) return;
        var resu = stone.GetComponent<MonsterResurrection>();
        resu.SetRun(true);
        var v = SuperResource.Instance.GetInstance("法阵");
        v.GetComponent<VffaControl>().SetDeploy(true);
    }
}
