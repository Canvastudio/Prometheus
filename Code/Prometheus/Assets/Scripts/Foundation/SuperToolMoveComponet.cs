using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SuperToolMoveComponet : MonoBehaviour
{
    private MoveArgs moveArgs;
    private float realSpeed;
    private float realRoutate;
    private float moveTime;
    private bool isArrive;


    void Awake()
    {
        moveTime = 0;
        isArrive = false;
    }

    public void DestroySelf()
    {
        if (moveArgs != null) moveArgs.destroy = true;
        Destroy(this);
    }

    public void SetPar(MoveArgs moveArgs)
    {
        CheckPar(moveArgs);
        this.moveArgs = moveArgs;
        FaceToTarget();
    }

    private void CheckPar(MoveArgs moveArgs)
    {
        if (moveArgs.callbackPar != null && moveArgs.callback == null)
        {
            Debug.LogError("有回调函数的参数，却没有指定回调函数？");
            throw new NullReferenceException();
        }
        if (moveArgs.CurTransform == null)
        {
            Debug.LogError("目标点都须有值");
            throw new NullReferenceException();
        }
        if (moveArgs.TarTransform == null)
        {
            //这里做什么呢？？？
        }
        if (moveArgs.CurTransform == moveArgs.TarTransform)
        {
            Debug.LogError("目标和发射点是同一个位置？");
            throw new Exception();
        }
        if (Math.Abs(moveArgs.speed) < 0.0001f)
        {
            Debug.LogError("移动速度为0");
            throw new Exception();
        }
        if (moveArgs.accelerationType != MoveArgs.AccelerationType.Normal && Math.Abs(moveArgs.acceleration) < 0.0001f)
        {
            Debug.LogError("移动加速度为0，又设置了加速模式？？");
            throw new Exception();
        }
        if (moveArgs.rotateAddSpeedType != MoveArgs.AccelerationType.Normal && Math.Abs(moveArgs.rotateSpeed) < 0.0001f)
        {
            Debug.LogError("旋转加速度为0，又设置了加速模式？？");
            throw new Exception();
        }
    }

    private void FaceToTarget()
    {
        if (moveArgs.faceTo)
        {
            Vector2 targetVector = new Vector2();
            if (moveArgs.TarTransform != null)
            {
                targetVector.x = moveArgs.TarTransform.position.x;
                targetVector.y = moveArgs.TarTransform.position.y;
            }
            else
            {
                targetVector = moveArgs.TarPosition;
            }
            Vector2 a = targetVector - new Vector2(transform.position.x, transform.position.y);
            float angle = Vector2.Angle(transform.up, a);
            Vector3 localPoint = transform.InverseTransformPoint(new Vector3(targetVector.x, targetVector.y, 0));
            if (localPoint.x > 0)
            {
                transform.Rotate(Vector3.forward, -angle);
            }
            else if (localPoint.x <= 0)
            {
                transform.Rotate(Vector3.forward, angle);
            }
        }
    }

    void Update()
    {
        if (moveArgs == null || moveArgs.pause) return;
        if (moveArgs.destroy)
        {
            DestroySelf();
            return;
        }

        if (isArrive)
        {
            if (moveArgs.TarTransform != null)
            {
                Vector3 temp = new Vector3(moveArgs.TarTransform.position.x, moveArgs.TarTransform.position.y,
                    transform.position.z);
                transform.position = temp;
            }
            else
                transform.position = moveArgs.TarPosition;
            if (moveArgs.callback != null) moveArgs.callback(moveArgs.callbackPar);
            Destroy(this);
            return;
        }
        realSpeed = (moveArgs.speed + moveArgs.acceleration * moveTime) * SuperTimer.DeltaTime;
        float moveSpeed = 0;
        switch (moveArgs.accelerationType)
        {
            case MoveArgs.AccelerationType.Normal:
                moveSpeed = realSpeed;
                break;
            case MoveArgs.AccelerationType.Squa:
                moveSpeed = Mathf.Pow(moveTime + 1, 2) * realSpeed;
                break;
            case MoveArgs.AccelerationType.Cube:
                moveSpeed = Mathf.Pow(moveTime + 1, 3) * realSpeed;
                break;
            case MoveArgs.AccelerationType.Quad:
                moveSpeed = Mathf.Pow(moveTime + 1, 4) * realSpeed;
                break;
        }

        float routateSpeed = 0;
        if (moveArgs.rotateSpeed > 0)
            realRoutate = (moveArgs.rotateSpeed + moveArgs.rotateAddSpeed * moveTime) * SuperTimer.DeltaTime;

        switch (moveArgs.rotateAddSpeedType)
        {
            case MoveArgs.AccelerationType.Normal:
                routateSpeed = realRoutate;
                break;
            case MoveArgs.AccelerationType.Squa:
                routateSpeed = Mathf.Pow(moveTime + 1, 2) * realRoutate;
                break;
            case MoveArgs.AccelerationType.Cube:
                routateSpeed = Mathf.Pow(moveTime + 1, 3) * realRoutate;
                break;
            case MoveArgs.AccelerationType.Quad:
                routateSpeed = Mathf.Pow(moveTime + 1, 4) * realRoutate;
                break;
        }
        Vector2 targetVector = new Vector2();
        if (moveArgs.TarTransform != null)
        {
            targetVector.x = moveArgs.TarTransform.position.x;
            targetVector.y = moveArgs.TarTransform.position.y;
        }
        else
        {
            targetVector = moveArgs.TarPosition;
        }
        if (moveArgs.rotateSpeed > 0 || moveArgs.rotateSpeed < 0)
        {
            //计算目标与自己的夹角
            Vector2 a = targetVector - new Vector2(transform.position.x, transform.position.y);
            float angle = Vector2.Angle(transform.up, a);
            //得到的夹角不分正负，所以之后要计算左右
            Vector3 localPoint =
                transform.InverseTransformPoint(new Vector3(targetVector.x, targetVector.y, 0));

            //得到的角度不分正负，所以要判断目的地在自己的左边还是右边
            //目标在右边
            if (localPoint.x > 0)
            {
                if (angle <= routateSpeed || moveArgs.rotateSpeed < 0)
                    transform.Rotate(Vector3.forward, -angle);
                else
                    transform.Rotate(Vector3.forward, -routateSpeed);
            }
            //在左边
            else if (localPoint.x < 0)
            {
                if (angle <= routateSpeed || moveArgs.rotateSpeed < 0)
                    transform.Rotate(Vector3.forward, angle);
                else
                    transform.Rotate(Vector3.forward, routateSpeed);
            }
            else
            {
                //如果是后方
                if (localPoint.y <= 0)
                {
                    transform.Rotate(Vector3.forward, routateSpeed);
                }
                //正前方不处理，不旋转
            }
            transform.Translate(new Vector3(0, moveSpeed, 0));
        }
        else
        {
            Vector2 temp = Vector2.MoveTowards(this.transform.position, targetVector, moveSpeed);
            transform.position = new Vector3(temp.x, temp.y, transform.position.z);
        }

        float arriveDis = moveArgs.arriveDistance;
        if (arriveDis < moveSpeed) arriveDis = moveSpeed;

        switch (moveArgs.arriveMode)
        {
            case MoveArgs.ArriveMode.Normal:
                if (Vector2.Distance(transform.position, targetVector) < arriveDis) isArrive = true;
                break;
            case MoveArgs.ArriveMode.X:
                if (Mathf.Abs(transform.position.x - targetVector.x) < arriveDis) isArrive = true;
                break;
            case MoveArgs.ArriveMode.Y:
                if (Mathf.Abs(transform.position.y - targetVector.y) < arriveDis) isArrive = true;
                break;
        }
        moveTime += SuperTimer.DeltaTime;
    }
}

