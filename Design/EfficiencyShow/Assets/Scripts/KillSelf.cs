using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSelf : MonoBehaviour
{

    public void StartExplode()
    {
        var animate = GetComponentInChildren<Animator>();
        Debug.Log(animate);
        animate.SetBool("explode",true);
    }

    public void HideSelf()
    {
        gameObject.SetActive(false);
        GameObject soul = SuperResource.Instance.GetInstance("鬼魂");
        Vector3 temp = transform.position;
        temp.z = soul.transform.position.z;
        soul.transform.position = temp;

        Transform sbTransform = SuperTool.GetComponentUpward<MonsterResurrection>(this).transform;
        SuperTool.SetParentWithLocal(sbTransform, soul.transform);
        
        soul.GetComponent<SoulRun>().SetTarget(sbTransform.Find("起效"));


    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
