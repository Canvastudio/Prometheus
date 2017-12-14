using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏相机管理
/// </summary>
public class GCamera : SingleGameObject<GCamera> {

    public bool sliding = false;
    public float rate;
    public float distance;
    public void InitData()
    {
        rate = GlobalParameterConfig.GetConfigDataById<GlobalParameterConfig>(1).roundRate;
    }

    public void MoveDown(float _distance)
    {
        distance += _distance * 1.2f;
    }

    public void Update()
    {
        if (!GameTestData.Instance.NoSroll && distance > 0)
        {
            sliding = true;
            float d = rate * Time.deltaTime;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + d, transform.localPosition.z);
            distance -= d;
            distance = Mathf.Max(0, distance);
        }
        else
        {
            sliding = false;
        }
    }

    public void LateUpdate()
    {
        if (StageCore.Instance.Player != null && StageCore.Instance.isLooping)
        {
            float y1 = StageUIView.Instance.playerCeiling.position.y;
            float y2 = StageCore.Instance.Player.transform.position.y;

            if (y2 > y1)
            {
                float d = y2 - y1;
                //transform.position = new Vector3(transform.position.x, transform.position.y + (y2 - y1), transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + d, transform.position.z), 0.07f);
                distance -= d;
                distance = Mathf.Max(0, distance);
            }
        }
    }
}
