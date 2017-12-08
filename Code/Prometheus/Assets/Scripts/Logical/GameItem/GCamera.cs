﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏相机管理
/// </summary>
public class GCamera : SingleGameObject<GCamera> {

    public void MoveDown(float distance)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + distance, transform.localPosition.z);
    }

    public void Update()
    {
        
    }

    public void LateUpdate()
    {
        if (StageCore.Instance.Player != null && StageCore.Instance.isLooping)
        {
            float y1 = StageUIView.Instance.playerCeiling.position.y;
            float y2 = StageCore.Instance.Player.transform.position.y;

            if (y2 > y1)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + (y2 - y1), transform.position.z);
            }
        }
    }
}
