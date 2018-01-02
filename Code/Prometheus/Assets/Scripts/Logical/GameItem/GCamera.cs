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
    public float speed = 0.10f;
    public float max;
    public float r;
    public void InitData()
    {
        rate = GlobalParameterConfig.GetConfigDataById<GlobalParameterConfig>(1).roundRate;

        int w = GlobalParameterConfig.GetConfigDataById<GlobalParameterConfig>(1).MapWidth;


        max = (w - 6) * 120f;

        r = (w - 6) * 1.2f;
    }

    public void InitPosition()
    {
        var Player = StageCore.Instance.Player;

        float x = Player.transform.Rt().anchoredPosition.x;

        if (x >= 0 && x < max)
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
            transform.position = new Vector3(r, 0, 0);
        }


        StartCoroutine(InvokeViewArea());
    }

    IEnumerator InvokeViewArea()
    {
        yield return 0;
        //Debug.Log("Invoke ViewArea");
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
            float ly = transform.position.y;
            float y1 = StageUIView.Instance.playerCeiling.position.y;
            float y2 = StageCore.Instance.Player.transform.position.y;

            if (y2 > y1)
            {
                float d = (y2 - y1) * speed;
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + d, transform.position.z), speed);
                distance -= d;
                distance = Mathf.Max(0, distance);
            }

            float x = Player.transform.Rt().anchoredPosition.x;
            float lx = transform.position.x;
            float offset;
            if (x >= 0 && x < max)
            {
                offset = transform.position.x - Player.transform.position.x;

                if (Mathf.Abs(offset) > 0.01)
                {
                    target = new Vector3(Player.transform.position.x, transform.position.y, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, target, speed);
                }
            }
            else if (x < 0)
            {
                offset = -transform.position.x;
                if (Mathf.Abs(offset) > 0.01)
                {
                    target = new Vector3(0, transform.position.y, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, target, speed);
                }
            }
            else
            {
                offset = r - transform.position.x;

                if (Mathf.Abs(offset) > 0.01)
                {
                    target = new Vector3(r, transform.position.y, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, target, speed);
                }
            }

            total2 += transform.position.x - lx;
            total1 += transform.position.y - ly;
            //同一次肯定不需要检查2次得
            bool view = false;


            if (total1 >= 1.2f) 
            {
                view = true;
                StartCoroutine(InvokeViewArea());
                total1 = 0;
            }

            if (Mathf.Abs(total2) >= 1.2f && !view)
            {
                total2 = 0;
                StartCoroutine(InvokeViewArea());
            }
        }
    }
}
