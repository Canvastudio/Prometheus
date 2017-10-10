using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ConfigTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        SuperConfig.Instance.Load();
        Debug.Log(BoxConfig.GetConfigDataById<BoxConfig>(201).name);


        //SuperTool.ToRpn()

        //var t=SuperTool.CreateWeightSection(new List<int> { 1, 2, 3 });
        //   Debug.Log(t.RanPoint());
        //var t = new SuperArray<int>("1:2|3",":|");
        //   Debug.Log(t[0,0]);



    }

    // Update is called once per frame
    void Update()
    {

    }
}
