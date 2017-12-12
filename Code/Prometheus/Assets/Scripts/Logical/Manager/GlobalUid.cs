using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUid :SingleObject<GlobalUid>  {

    private ulong _id;

    public ulong GetUid()
    {
        return _id++;
    }
}
