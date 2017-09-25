using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightComponet :MonoBehaviour, ITimeDrive {

    /// <summary>
    /// ref主动技能配置表
    /// </summary>
    public List<ActiveSkillsConfig> activeSkillConfigs = new List<ActiveSkillsConfig>();

    /// <summary>
    /// ref被动技能配置表
    /// </summary>
    public List<PassiveSkillsConfig> passiveSkillConfigs = new List<PassiveSkillsConfig>();

    /// <summary>
    /// ref召唤技能配置表
    /// </summary>
    public List<SummonSkillsConfig> summonSkillConfigs = new List<SummonSkillsConfig>();

    public MonoBehaviour ownerObject;

    private List<ActiveSkillState> activeSort = new List<ActiveSkillState>();

    private class ActiveSkillState : IComparer<ActiveSkillState>
    {
        public ActiveSkillsConfig config;

        /// <summary>
        /// 已经释放的次数，同样的cooldown技能，当都处于可以释放时，优先释放time小的技能。
        /// </summary>
        public int activeTime = 0;

        /// <summary>
        /// 回合数，会一直增加到等于cooldown，并在可以释放时释放activeskll
        /// </summary>
        public int turn
        {
            get
            {
                return _turn;
            }
            set
            {
                _turn = Math.Min((int)config.coolDown, value);
            }
        }

        private int _turn;

        /// <summary>
        /// 1-》可以释放，0-》还不可以释放
        /// </summary>
        public int ready
        {
            get
            {
                if (turn == config.coolDown) return 1;
                else return 0;
            }
        }

        public int Compare(ActiveSkillState x, ActiveSkillState y)
        {
            if (x.ready > y.ready)
            {
                return 1;
            }
            else if (x.ready < y.ready)
            {
                return -1;
            }
            else
            {
                if (x.config.coolDown > y.config.coolDown)
                {
                    return 1;
                }
                else if (x.config.coolDown < y.config.coolDown)
                {
                    return -1;
                }
                else
                {
                    if (x.activeTime < y.activeTime)
                    {
                        return 1;
                    }
                    else if (x.activeTime > y.activeTime)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
    }

#region active skill region
    public void SortAcitveSkill()
    {
        activeSort.Clear();

        foreach(var active_Skill in activeSkillConfigs)
        {
            activeSort.Add(new ActiveSkillState() { config = active_Skill });
        }

        activeSort.Sort();
    }
#endregion

    public void OnTurn()
    {
        //检查主动技能是否可以释放
        if (activeSort.Count > 0)
        {
            //1.所有的activeSort中的元素的turn += 1;
            foreach (var skill in activeSort)
            {
                skill.turn += 1;
            }

            //2.排序找到第一个，如果可以释放则释放
            activeSort.Sort();

            if (activeSort[0].ready == 1)
            {

            }
        }
    }

    public void DoActiveSkill(ActiveSkillsConfig config)
    {

    }
}


