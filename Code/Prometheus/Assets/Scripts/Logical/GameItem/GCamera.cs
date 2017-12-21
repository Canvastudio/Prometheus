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
    public float total1 = 0;
    public float total2 = 0;
    public void InitData()
    {
        rate = GlobalParameterConfig.GetConfigDataById<GlobalParameterConfig>(1).roundRate;
    }

    public void InitPosition()
    {
        var Player = StageCore.Instance.Player;

        float x = Player.transform.Rt().anchoredPosition.x;

        if (x >= 0 && x < 1095)
        {
            if (Mathf.Abs(transform.position.x - Player.transform.position.x) > 0.01)
            {
                target = new Vector3(Player.transform.position.x, transform.position.y, transform.position.z);

                //transform.position = Vector3.MoveTowards(transform.position, target, 0.07f);
                transform.position = target;

                //total2 += (Player.transform.position.x - transform.position.x);
            }
        }
        else if ( x < 0)
        {
            transform.position = Vector3.zero;
        }
        else
        {
            transform.position = new Vector3(10.8f, 0, 0);
        }


        StartCoroutine(InvokeViewArea());
    }

    IEnumerator InvokeViewArea()
    {
        yield return 0;
        Debug.Log("Invoke ViewArea");
        Messenger.Invoke(SA.ViewArea);
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

            total1 += d;
        }
        else
        {
            sliding = false;
        }
    }

    Vector3 target;

    public void LateUpdate()
    {
        var Player = StageCore.Instance.Player;
        if (Player != null && StageCore.Instance.isLooping)
        {
            float y1 = StageUIView.Instance.playerCeiling.position.y;
            float y2 = StageCore.Instance.Player.transform.position.y;

            if (y2 > y1)
            {
                float d = (y2 - y1) * 0.07f;
                //transform.position = new Vector3(transform.position.x, transform.position.y + (y2 - y1), transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + d, transform.position.z), 0.07f);
                distance -= d;
                distance = Mathf.Max(0, distance);
                total1 += d;
            }

            float x = Player.transform.Rt().anchoredPosition.x;
            
            if (x >= 0 && x < 1095)
            {
                float offset = Mathf.Abs(transform.position.x - Player.transform.position.x);

                if (offset > 0.01)
                {
                    total2 += offset;

                    target = new Vector3(Player.transform.position.x, transform.position.y, transform.position.z);

                    //transform.position = Vector3.MoveTowards(transform.position, target, 0.07f);
                    transform.position = target;
                }
            }
            else if (x < 0)
            {
                transform.position = new Vector3(0, transform.position.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(10.8f, transform.position.y, transform.position.z);
            }

            if (total1 >= 1.2f || total2 >= 1.2f)
            {
                StartCoroutine(InvokeViewArea());
                total1 = 0;
                total2 = 0;
            }
        }
    }
}
