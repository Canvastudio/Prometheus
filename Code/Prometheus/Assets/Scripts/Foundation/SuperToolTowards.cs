using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Towards方法需要给对象添加此组件
/// </summary>
public class SuperToolTowards : MonoBehaviour
{
    /// <summary>
    /// 该值不能赋值，只作为外部观察
    /// </summary>
    public bool 暂停;

    private TowardsArg args;
    private bool isGone;

    public void SetArg(TowardsArg args)
    {
        this.args = args;
    }

    public void DestroySelf()
    {
        if (args != null) args.destroy = true;
        Destroy(this);
    }

    void Update()
    {
        if (args == null || args.Pause) return;
        if (args.destroy)
        {
            DestroySelf();
            return;
        }

        暂停 = args.Pause;

        Vector2 tar;
        if (args.t_transform != null) tar = args.t_transform.position;
        else if (args.t_gameObject != null) tar = args.t_gameObject.transform.position;
        else tar = args.t_position;

        Vector2 targetVector = new Vector2(tar.x, tar.y);
        //计算目标与自己的夹角
        Vector2 a = targetVector - new Vector2(transform.position.x, transform.position.y);
        float angle = Vector2.Angle(transform.up, a);
        float realRoutateSpeed;
        float minAngle;
        if (args.routateMode == RoutateMode.Normal)
        {
            realRoutateSpeed = args.routateSpeed * SuperTimer.DeltaTime * 30;
            minAngle = realRoutateSpeed;
        }
        else
        {
            realRoutateSpeed = Mathf.Lerp(0, angle, args.routateSpeed * SuperTimer.DeltaTime) / 2;
            minAngle = 0.1f;
        }
        Vector3 localPoint =
         transform.InverseTransformPoint(new Vector3(targetVector.x, targetVector.y, 0));
        //得到的角度不分正负，所以要判断目的地在自己的左边还是右边
        //目标在右边
        if (localPoint.x > 0)
        {
            Near(angle);
            if (angle <= minAngle || args.routateSpeed < 0)
            {
                transform.Rotate(Vector3.forward, -angle);
                Gone();
            }
            else
            {
                transform.Rotate(Vector3.forward, -realRoutateSpeed);
                isGone = false;
            }
        }
        //在左边
        else if (localPoint.x < 0)
        {
            Near(angle);
            if (angle <= minAngle || args.routateSpeed < 0)
            {
                transform.Rotate(Vector3.forward, angle);
                Gone();
            }
            else
            {
                transform.Rotate(Vector3.forward, realRoutateSpeed);
                isGone = false;
            }
        }
        else
        {
            //如果是后方
            if (localPoint.y <= 0)
            {
                if (args.routateSpeed < 0)
                {
                    transform.Rotate(Vector3.forward, 180);
                }
                else
                {
                    transform.Rotate(Vector3.forward, realRoutateSpeed);
                    isGone = false;
                }
            }
            //正前方，不旋转
            else
            {
                Gone();
            }
        }
    }

    private void Gone()
    {
        if (!isGone)
        {
            if (args.goneCallback != null)
            {
                if (SuperTool.CheckDelegate(args.goneCallback))
                    args.goneCallback.Invoke(args.goneCallbackPar);
            }
            if (!args.followThrough) Destroy(this);
            isGone = true;
        }
        args.IsNear = true;
    }

    private void Near(float angle)
    {
        if (args.routateMode != RoutateMode.Lerp) return;
        args.IsNear = angle <= args.nearAngle;
    }
}

public enum RoutateMode
{
    /// <summary>
    /// 匀速旋转
    /// </summary>
    Normal,
    /// <summary>
    /// 差值速度旋转
    /// </summary>
    Lerp
}

/// <summary>
/// 旋转参数
/// </summary>
public class TowardsArg
{
    public static TowardsArg CreatArg(Transform curTransform, float routateSpeed)
    {
        var t = new TowardsArg
        {
            curTransform = curTransform,
            routateSpeed = routateSpeed
        };
        return t;
    }

    public static TowardsArg CreatArg(Transform curTransform, Transform target, float routateSpeed)
    {
        var t = new TowardsArg
        {
            curTransform = curTransform,
            t_transform = target,
            routateSpeed = routateSpeed
        };
        return t;
    }

    public static TowardsArg CreatArg(Transform curTransform, GameObject target, float routateSpeed)
    {
        var t = new TowardsArg
        {
            curTransform = curTransform,
            t_gameObject = target,
            routateSpeed = routateSpeed
        };
        return t;
    }

    public static TowardsArg CreatArg(Transform curTransform, Vector2 target, float routateSpeed)
    {
        var t = new TowardsArg
        {
            curTransform = curTransform,
            t_position = target,
            routateSpeed = routateSpeed
        };
        return t;
    }

    private TowardsArg() { }

    /// <summary>
    /// 旋转点位置
    /// </summary>
    public Transform curTransform;
    /// <summary>
    /// 是否在旋转到位后，持续追踪，否则会移除旋转控件
    /// </summary>
    public bool followThrough;
    /// <summary>
    /// 旋转到位的回调函数
    /// </summary>
    public SuperTool.MoveDownCallback goneCallback;
    /// <summary>
    /// 回调函数参数
    /// </summary>
    public object goneCallbackPar;

    /// <summary>
    /// Lerp最后几帧旋转很慢，这里提供一个更加自然的响应角度，
    /// 当旋转角度小于该值时，IsNear为true
    /// </summary>
    public float nearAngle = 8;


    /// <summary>
    /// 标记是否处于在近点的状态，外部通过检查该值判断是否旋转到Near
    /// </summary>
    public bool IsNear { set; get; }
    /// <summary>
    /// 目标位置，可以跟踪
    /// </summary>
    public Transform t_transform;
    /// <summary>
    /// 目标位置，可以跟踪
    /// </summary>
    public GameObject t_gameObject;
    /// <summary>
    /// 目标位置，不可跟踪
    /// </summary>
    public Vector2 t_position;
    /// <summary>
    /// 旋转模式：Normal匀速，Lerp差值
    /// </summary>
    public RoutateMode routateMode;
    /// <summary>
    /// 旋转速度，为负表示瞬时旋转
    /// </summary>
    public float routateSpeed;

    private bool pause;
    /// <summary>
    /// 为true会暂停旋转
    /// </summary>
    public bool Pause
    {
        set
        {
            pause = value;
            IsNear = false;
        }
        get { return pause; }
    }

    /// <summary>
    /// 是否销毁
    /// </summary>
    public bool destroy;

}