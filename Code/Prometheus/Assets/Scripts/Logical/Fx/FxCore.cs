using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxCore : SingleGameObject<FxCore> {

    [SerializeField]
    List<ParticleSystem> preloadParticle;

    public string str_fxlock = "FL";

    public IEnumerator PreLoadStageFx()
    {
        foreach(var particle in preloadParticle)
        {
            ObjPool<ParticleSystem>.Instance.InitOrRecyclePool(str_fxlock, particle, 5);

            yield return 0;
        }
    }
}
