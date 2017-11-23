using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VffaControl : TempObj
{
    public Animator animator;
    public void SetDeploy(bool arg)
    {
        animator.SetBool("deploy", arg);
    }

    public void _VffaMax()
    {
        SuperTimer.Instance.SetTimer(0.5f, o =>
        {
            var m = SuperResource.Instance.GetInstance("怪物2");
            m.transform.position = transform.position;
            SuperTool.SetParentWithLocal(Main.Instance.stage.transform, m.transform);
        });
        SuperTimer.Instance.SetTimer(1, o => { SetDeploy(false); });
    }


    public void _VffaMin()
    {
        Destroy(gameObject);
    }
}
