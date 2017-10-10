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
            return _pathIndex > _path.Count - 1;
        }
    }


    public MoveComponet SetPaht(List<Pathfinding.Node> path)
    {
        _path = path;
        _pathIndex = 0;
        return this;
    }

    public IEnumerator Go(List<Pathfinding.Node> path)
    {
        _path = path;

        _pathIndex = 0;

        LTDescr descr;

        bool finish;

        while (_pathIndex < _path.Count)
        {
            move_Finish = false;

            Brick brick = _path[_pathIndex].behavirour as Brick;

            _brick = brick;

            descr = LeanTween.move(StageCore.Instance.Player.gameObject,
                brick.transform.position,
                0.3f).setOnComplete(OnMoveFinish);

            //yield return new WaitUntil(()=>finish);

            //++_pathIndex;

            while (!move_Finish)
            {
                yield return 0;
            }
        }
    }

    public IEnumerator MoveToNext(float time)
    {
        yield return MoveTo(_path[_pathIndex].behavirour as Brick, time);
    }

    bool move_Finish = false;

    Brick _brick;

    public IEnumerator MoveTo(Brick brick, float time)
    {
        move_Finish = false;

        _brick = brick;

        LeanTween.move(this.gameObject, brick.transform.position, time).setOnComplete(OnMoveFinish);

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

        BrickCore.Instance.OpenNearbyBrick(
            StageCore.Instance.Player.standBrick.pathNode.x,
            StageCore.Instance.Player.standBrick.pathNode.z);

        string formula = GlobalParameterConfig.GetConfig().motorizedFormula;

        Debug.Log(formula);

        string[] step = formula.Split(',');

        Stack<string> stacks = new Stack<string>();
    }

    public bool IsNextCanMove()
    {
        if (_pathIndex == _path.Count)
        {
            return false;
        }
        else
            return (_path[_pathIndex].behavirour as Brick).brickType == BrickType.EMPTY;
    }
}
