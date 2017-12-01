using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 保存状态数据，所有状态的具体逻辑都放在状态的实例中
/// </summary>
public class StateComponent : MonoBehaviour {

    /// <summary>
    /// 受到的状态
    /// </summary>
    public List<StateIns> state_list = new List<StateIns>(8);

    public List<HaloInfo> halo_list = new List<HaloInfo>(4);

    public LiveItem owner;

    public void Awake()
    {
        owner = GetComponent<LiveItem>();
    }

    float t;

    private void Update()
    {
        if (owner != null && owner.isDiscovered && owner.isAlive && state_list.Count > 0)
        {
            t += Time.deltaTime;

            if (t >= 1)
            {
                t -= 1;

                //owner.artPop.OnNext();
            }
        }
    }

    public void Clean()
    {
        state_list.Clear();
        halo_list.Clear();
    }

    public virtual void AddStateIns(StateIns ins)
    {
        int max = ins.stateConfig.max;

        state_list.Add(ins);

        for (int i = state_list.Count - 1; i >= 0; --i)
        {
            if (state_list[i].stateConfig.id == ins.stateConfig.id)
            {
                --max;

                if (max < 0)
                {
                    state_list[i].DeactiveIns();
                    state_list.RemoveAt(i);
                }
            }
        }

        if (owner != null)
            owner.AddStateUI(ins);
        
    }

    public virtual void RemoveStateIns(StateIns ins)
    {
        ins.DeactiveIns();

        ins.stateEffects = null;

        for (int i = state_list.Count - 1; i >= 0; --i)
        {
            if (state_list[i].stateConfig.id == ins.stateConfig.id)
            {
                state_list[i].DeactiveIns();
                state_list.RemoveAt(i);
            }
        }


        
    }

    public void RemoveStateBuff(int count, bool helpful)
    {
        state_list.Reverse();

        for (int i = state_list.Count - 1; i >= 0; --i)
        {
            if (state_list[i].passive == null)
            {
                if (state_list[i].stateConfig.isBuff == helpful)
                {
                    state_list[i].DeactiveIns();
                    state_list.RemoveAt(i);
                }
            }
        }
    }

    public void RemoveHalo(HaloInfo halo)
    {
        for (int i = state_list.Count - 1; i >= 0; --i)
        {
            if (state_list[i].passive != null)
            {
                if (state_list[i].passive.id == halo.passive.id)
                {
                    owner.RemoveStateUI(state_list[i]);
                    state_list[i].DeactiveIns();
                    //state_list.RemoveAt(i);
                }
            }
        }

        for (int i = 0; i < halo_list.Count; ++i)
        {
            if (halo_list[i].id == halo.id)
            {
                halo_list.RemoveAt(i);
            }
        }

    }

    public void AddHalo(HaloInfo halo)
    {
        foreach(var h in halo_list)
        {
            if (h.id == halo.id)
            {
                return;
            }
        }
           

        var stateIns = new StateIns(halo.passive.stateConfig, owner, halo.passive, halo.owner);

        AddStateIns(stateIns);

        if (owner.isDiscovered)
        {
            stateIns.ActiveIns();
        }

        halo_list.Add(halo);
    }
}
