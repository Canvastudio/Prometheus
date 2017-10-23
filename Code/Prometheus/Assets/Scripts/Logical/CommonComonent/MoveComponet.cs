using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponet : MonoBehaviour {

    private List<Pathfinding.Node> _path;
    private int _pathIndex;

    public LiveItem owner;

    public bool PathFinish
    {
        get
        {
            return (_pathIndex > _path.Count - 1) || blocking;
        }
    }

    private bool blocking = false;

    public MoveComponet SetPaht(List<Pathfinding.Node> path)
    {
        _path = path;
        _pathIndex = 0;
        blocking = false;
        return this;
    }

    public IEnumerator Go(List<Pathfinding.Node> path)
    {
        _path = path;

        _pathIndex = 0;

        LTDescr descr;

        while (_pathIndex < _path.Count)
        {
            move_Finish = false;

            Brick brick = _path[_pathIndex].behavirour as Brick;

            _brick = brick;

            float cast_time = 1f;

            StageCore.Instance.TimeCast(cast_time);

            descr = LeanTween.moveLocal(
                this.gameObject, 
                transform.parent.InverseTransformPoint(brick.transform.position),
                cast_time)
                .setOnComplete(OnMoveFinish);

            while (!move_Finish)
            {
                yield return 0;
            }
        }

        Debug.Log("Go 移动完成！");
    }

    public IEnumerator MoveToNext()
    {
        Brick brick = _path[_pathIndex].behavirour as Brick;
        float time = 1;
        if (!IsNextBlock())
        {
            yield return MoveTo(brick, time);
        }
        else
        {
            yield return Open(brick, time);
            blocking = true;
        }
    }

    bool move_Finish = false;

    Brick _brick;

    public IEnumerator Open(Brick brick, float time)
    {
        StageCore.Instance.TimeCast(1);

        BrickCore.Instance.OpenBrick(brick);

        yield return new WaitForSeconds(time);

        CastMoveTime();
    }

    public IEnumerator MoveTo(Brick brick, float time)
    {
        move_Finish = false;

        _brick = brick;

        StageCore.Instance.TimeCast(time);

        LeanTween.moveLocal(this.gameObject, transform.parent.InverseTransformPoint(brick.transform.position), time).setOnComplete(OnMoveFinish);

        while (!move_Finish)
        {
            yield return 0;
        }
    }

    public void OnMoveFinish()
    {
        move_Finish = true;

        owner.standBrick = _brick;

        _pathIndex++;

        //BrickCore.Instance.OpenNearbyBrick(
        //    StageCore.Instance.Player.standBrick.pathNode.x,
        //    StageCore.Instance.Player.standBrick.pathNode.z);
    }

    public void CastMoveTime()
    {
        StageCore.Instance.TimeCast(1);
    }

    public bool IsNextBlock()
    {
        Brick brick = (_path[_pathIndex].behavirour) as Brick;

        return !(brick.realBrickType == BrickType.EMPTY) || brick.brickBlock != 0;
    }

    public bool MoveEnd()
    {
        return PathFinish;
    }

}
