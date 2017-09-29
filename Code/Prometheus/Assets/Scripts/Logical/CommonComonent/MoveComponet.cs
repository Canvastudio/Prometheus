using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponet : MonoBehaviour {

    private List<Pathfinding.Node> _path;
    private int _pathIndex;

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

        yield return new WaitForSeconds(1);
    }

    public IEnumerator MoveToNext()
    {
        if (_pathIndex >= _path.Count)
        {
            yield return 0;
        }
        else
        {
            yield return MoveTo(_path[_pathIndex]);
        }
    }

    public IEnumerator MoveTo(Brick brick)
    {
        bool move_Finish = false;

        LeanTween.move(this.gameObject, brick.transform.position, 1).setOnComplete(() =>
        {
            move_Finish = true;
        });

        while (!move_Finish)
        {
            yield return 0;
        }
    }

    public IEnumerator MoveTo(Pathfinding.Node node)
    {
        yield return MoveTo(node.behavirour as Brick);
    }
}
