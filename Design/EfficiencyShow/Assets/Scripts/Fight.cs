using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : TempObj
{
    public GameObject player;
    public GameObject monster;
    public Animator animator;


    // Use this for initialization
    void Start()
    {
        SuperTimer.Instance.SetTimer(1, o =>
        {
            animator.SetBool("run",true);
            var effect = SuperResource.Instance.GetInstance("近战特效");
            SuperTool.SetParentWithLocal(player.transform,effect.transform);
        } );
    }


    public void OnHIt()
    {

    }

    public void OnMonsterHIt()
    {

    }
}
