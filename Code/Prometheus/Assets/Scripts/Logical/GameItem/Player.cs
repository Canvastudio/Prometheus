using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    /// <summary>
    /// 是否处自动移动状态
    /// </summary>
    public bool auotMoving = false;

    public Brick standBrick;

    public MoveComponet moveComponent;

    private List<Pathfinding.Node> _path;
    private int _pathIndex;

}
