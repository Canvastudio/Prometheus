using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleManager {

    private static List<ISingleHandler> singleCores = new List<ISingleHandler>();

    public static void RegisterSingle(ISingleHandler single)
    {
        singleCores.Add(single);
    }
    
}
