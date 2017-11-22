using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public GameObject stage;
    public GameObject ControlButton;
    void Awake()
    {
        SuperTimer.Instance.CreatAndBound(this);
        ControlButton.SetActive(false);
    }

    void Start()
    {
        SuperResource.Instance.LoadAsync();
        StartCoroutine(Check());
        var mb=MessageCenter.CreatMsgBuilder(this);
        mb.AddListener("召唤物", On_炮台);
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
            SuperTimer.Instance.SetTimer(Random.Range(0.1f,0.3f), CreatGuns);
        }

    }

    private void CreatGuns(object arg)
    {
        GameObject tar = Background.Instance.GetGrids();
        GameObject gun = SuperResource.Instance.GetInstance("炮台");
        Vector2 pos = tar.transform.position;
        gun.transform.position = pos;
        SuperTool.SetParentWithLocal(stage.transform, gun.transform);
        ClickLimit.UnLock(this,true);
    }



}
