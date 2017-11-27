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

    public Monster owner;

    public void OnEnable()
    {
        owner = GetComponent<Monster>();

        t = 0;
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

                owner.artPop.OnNext();
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

        if (ins.stateConfig.iShow)
        {
            owner.artPop.Add(StageView.Instance.stateAtlas.GetSprite(ins.stateConfig.icon));
        }
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

        if (ins.stateConfig.iShow)
        {
            owner.artPop.Remove(StageView.Instance.stateAtlas.GetSprite(ins.stateConfig.icon));
        }
    }

    public void RemoveStateBuff(int count, bool helpful)
    {

    }

    public void RemoveHalo(HaloInfo halo)
    {
        RemoveStateIns(halo.passive.stateIns);

        halo_list.Remove(halo);
    }

    public void AddHalo(HaloInfo halo)
    {
        AddStateIns(halo.passive.stateIns);

        halo_list.Add(halo);
    }
}
