using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移动方法构建的参数类，已经内含SuperTimer.deltaTime
/// </summary>
public class MoveArgs
{

    private Transform _curTransform;
    /// <summary>
    /// 发射点位置
    /// </summary>
    public Transform CurTransform { set; get; }

    private Transform _tarTransform;

    /// <summary>
    /// 目标实时位置，可跟踪
    /// </summary>
    public Transform TarTransform
    {
        set
        {
            _tarTransform = value;
            TarPosition = value.position;
        }
        get { return _tarTransform; }
    }

    /// <summary>
    /// 目标指定位置，不可跟踪
    /// </summary>
    public Vector2 TarPosition { set; get; }

    /// <summary>
    /// 暂停
    /// </summary>
    public bool pause;

    /// <summary>
    /// 销毁
    /// </summary>
    public bool destroy;


    /// <summary>
    /// 初始速度
    /// </summary>
    public float speed;

    /// <summary>
    /// 移动加速度
    /// </summary>
    public float acceleration = 0;

    /// <summary>
    /// 加速模式
    /// </summary>
    public AccelerationType accelerationType;

    /// <summary>
    /// 是否在初始时，面向目标
    /// </summary>
    public bool faceTo = false;

    /// <summary>
    /// 旋转速度，等于0为不旋转，小于0为无限快
    /// </summary>
    public float rotateSpeed = 0;


    /// <summary>
    /// 旋转加速度
    /// </summary>
    public float rotateAddSpeed = 0;


    /// <summary>
    /// 旋转加速模式
    /// </summary>
    public AccelerationType rotateAddSpeedType;

    /// <summary>
    /// 抵达模式，可以仅在满足X或者Y到达就判定成功到达
    /// </summary>
    public ArriveMode arriveMode;

    /// <summary>
    /// 抵达判定距离，该值不小于speed*SuperTime.DeltaTime
    /// </summary>
    public float arriveDistance;

    /// <summary>
    /// 回调函数
    /// </summary>
    public SuperTool.MoveDownCallback callback;

    /// <summary>
    /// 回调函数的参数
    /// </summary>
    public object callbackPar;

    /// <summary>
    /// 加速类型，一共4种
    /// </summary>
    public enum AccelerationType
    {
        Normal,
        /// <summary>
        /// 平方
        /// </summary>
        Squa,
        /// <summary>
        /// 立方
        /// </summary>
        Cube,
        /// <summary>
        /// 四次方
        /// </summary>
        Quad
    }

    /// <summary>
    /// 到达模式，可以只在一维到就判定到达
    /// </summary>
    public enum ArriveMode
    {
        /// <summary>
        /// 正常情况，XY都要到才算
        /// </summary>
        Normal,
        /// <summary>
        /// 只要X达到
        /// </summary>
        X,
        /// <summary>
        /// 只要Y达到
        /// </summary>
        Y
    }


    /// <summary>
    /// 隐藏构造方法，只能通过CreateMoveArg创建
    /// </summary>
    private MoveArgs() { }

    /// <summary>
    /// 构建一个移动方法参数
    /// </summary>
    /// <param name="cur">要移动的物体</param>
    /// <param name="tar">要移动到的位置</param>
    /// <param name="speed">移动速度</param>
    /// <returns></returns>
    public static MoveArgs CreateMoveArg(GameObject cur, GameObject tar, float speed)
    {
        return CreateMoveArg(cur, tar.transform, speed);
    }

    /// <summary>
    /// 构建一个移动方法参数
    /// </summary>
    /// <param name="cur">要移动的物体</param>
    /// <param name="tar">要移动到的位置</param>
    /// <param name="speed">移动速度</param>
    /// <returns></returns>
    public static MoveArgs CreateMoveArg(GameObject cur, Transform tar, float speed)
    {
        MoveArgs arg = new MoveArgs
        {
            CurTransform = cur.transform,
            TarTransform = tar,
            speed = speed
        };
        return arg;
    }

    /// <summary>
    /// 构建一个移动方法参数
    /// </summary>
    /// <param name="cur">要移动的物体</param>
    /// <param name="tar">要移动到的位置</param>
    /// <param name="speed">移动速度</param>
    /// <returns></returns>
    public static MoveArgs CreateMoveArg(GameObject cur, Vector2 tar, float speed)
    {
        MoveArgs arg = new MoveArgs
        {
            CurTransform = cur.transform,
            TarPosition = tar,
            speed = speed
        };
        return arg;
    }

    /// <summary>
    /// 构建一个移动方法参数
    /// </summary>
    /// <param name="cur">要移动的物体</param>
    /// <param name="speed">移动速度</param>
    /// <returns></returns>
    public static MoveArgs CreateMoveArg(GameObject cur, float speed)
    {
        MoveArgs arg = new MoveArgs
        {
            CurTransform = cur.transform,
            speed = speed
        };
        return arg;
    }


}
