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
            finish = false;

            Brick brick = _path[_pathIndex].behavirour as Brick;

            descr = LeanTween.move(StageCore.Instance.Player.gameObject,
                brick.transform.position,
                0.3f).setOnComplete(() =>
                {
                    finish = true;
                    owner.standBrick = brick;
                });

            yield return new WaitUntil(()=>finish);

            ++_pathIndex;
        }
    }

    public IEnumerator MoveToNext(float time)
    {
        yield return MoveTo(_path[_pathIndex].behavirour as Brick, time);
    }

    public IEnumerator MoveTo(Brick brick, float time)
    {
        bool move_Finish = false;

        LeanTween.move(this.gameObject, brick.transform.position, time).setOnComplete(() =>
        {
            move_Finish = true;
            owner.standBrick = brick;
            _pathIndex++;
        });

        while (!move_Finish)
        {
            yield return 0;
        }
    }
}
