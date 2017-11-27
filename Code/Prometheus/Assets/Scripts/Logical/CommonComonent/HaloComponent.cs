using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaloComponent : MonoBehaviour {

    public List<HaloInfo> halo_list = new List<HaloInfo>(4);

    public LiveItem oldLive = null;

    public void OnStandItemChange(LiveItem liveItem)
    {
        if (liveItem != null)
        {
            var halos = liveItem.state.halo_list;

            for (int i = halos.Count - 1; i >= 0; --i)
            {
                if (!halo_list.Contains(halos[i]))
                {
                    liveItem.state.RemoveHalo(halos[i]);
                }
            }

            for (int i = 0; i < halo_list.Count; ++i)
            {
                if (!halos.Contains(halo_list[i]))
                {
                    liveItem.state.AddHalo(halo_list[i]);
                }
            }
        }
    }

    public void Active()
    {

    }

    public void Deactive()
    {

    }
}
