using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterProperty : LivePropery {

    [SerializeField]
    MonsterType _type;
    public MonsterType type
    {
        get
        {
            return _type;
        }
    }


}
