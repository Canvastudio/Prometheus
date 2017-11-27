using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaloComponent : MonoBehaviour {

    public List<HaloInfo> halo_list = new List<HaloInfo>(4);

    public LiveItem oldLive = null;

    public Brick owner;

    public void OnEnable()
    {
        owner = GetComponent<Brick>();
    }

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

    public void RemoveHalo(HaloInfo halo)
    {
        if (owner.IsLiveItemBrick())
        {
            LiveItem item = owner.item as LiveItem;
            item.state.RemoveHalo(halo);
        }

        halo_list.Remove(halo);

        if (halo_list.Count == 0)
        {
            owner.icon.color = Color.white;
        }
    }

    public void AddHalo(HaloInfo halo)
    {
        if (owner.IsLiveItemBrick())
        {
            LiveItem item = owner.item as LiveItem;
            item.state.AddHalo(halo);
        }

        halo_list.Add(halo);

        owner.icon.color = Color.red;
    }
}
