using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterResurrection : TempObj
{
    private Animator animator;

    protected override void Init()
    {
        animator = GetComponent<Animator>();
    }

    public void SetRun()
    {
        animator.SetBool("run", false);
    }

    public void SetRun(bool arg)
    {
        animator.SetBool("run", arg);
    }

    public void MonsterDie()
    {
        var monsters = SuperTool.GetComponentsInChildren<KillSelf>(this);
        foreach (var var in monsters)
        {
            var.StartExplode();
        }
    }

}
