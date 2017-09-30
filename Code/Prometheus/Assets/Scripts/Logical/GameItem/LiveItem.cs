using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveItem : MonoBehaviour {

    public Brick standBrick;

    [SerializeField]
    private MoveComponet _moveComponet;
    [HideInInspector]
    public MoveComponet moveComponent
    {
        get
        {
            if (_moveComponet == null)
            {
                _moveComponet = GetComponent<MoveComponet>();
                _moveComponet.owner = this;
            }

            return _moveComponet;
        }
    }
}
