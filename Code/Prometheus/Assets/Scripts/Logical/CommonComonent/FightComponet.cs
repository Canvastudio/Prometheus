using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FightComponet : MonoBehaviour {

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

    public float time = 0;

    public bool hitTarget = false;

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

    private List<ActiveSkillState> activeSort = new List<ActiveSkillState>();

    /// <summary>
    /// 是否激活使用技能
    /// </summary>
    public bool skillActive = false;

    private LiveItem _ownerObject;
    public LiveItem ownerObject
    {
        get
        {
            if (_ownerObject == null)
            {
                _ownerObject = gameObject.GetComponent<LiveItem>();
            }

            return _ownerObject;
        }
    }


    public void SortAcitveSkill()
    {
        activeSort.Clear();

        foreach (var active_Skill in activeSkillConfigs)
        {
            activeSort.Add(new ActiveSkillState() { config = active_Skill });
        }

        activeSort.Sort();
    }

    public void OnEnable()
    {
        Messenger<float>.AddListener(SA.StageTimeCast, ChangeTime);
    }

    public void OnDisable()
    {
        Messenger<float>.RemoveListener(SA.StageTimeCast, ChangeTime);
    }
    public void ChangeTime(float _time)
    {
        if (ownerObject.isDiscovered && skillActive)
        {
            time += _time;

            if (time >= 1)
            {
                OnTurn();
                time -= 1;
            }
        }
    }

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
                activeSort[0].activeTime += 1;
                DoActiveSkill(activeSort[0].config);
            }
        }
    }
    public float CalculateDamageValue(ActiveSkillsConfig config)
    {
        Stack<float> stack = new Stack<float>();

        long[] damage_values = config.damage.ToArray();
        float[] fv = new float[2];

        for (int i = 0; i < damage_values.Length; ++i)
        {
            SuperTool.GetValue(damage_values[0], ref fv);

            if (fv[1] == 1)
            {
                var property = (GameProperty)fv[0];

                if (property == GameProperty.Eql)
                {
                    if (stack.Count != 0)
                    {
                        Debug.LogError("逆波兰遇到等号的时候stack的长度不为1");
                        return stack.Pop();
                    }
                }
                else if (property == GameProperty.Plus)
                {
                    if (stack.Count < 2)
                    {
                        Debug.Log("逆波兰遇到操作符号的时候stack长度小于2");
                        float v1 = stack.Pop();
                        float v2 = stack.Pop();

                        stack.Push(v1 + v2);
                    }
                }
                else if (property == GameProperty.Sub)
                {
                    if (stack.Count < 2)
                    {
                        Debug.Log("逆波兰遇到操作符号的时候stack长度小于2");
                        float v1 = stack.Pop();
                        float v2 = stack.Pop();

                        stack.Push(v1 - v2);
                    }
                }
                else if (property == GameProperty.Sub)
                {
                    if (stack.Count < 2)
                    {
                        Debug.Log("逆波兰遇到操作符号的时候stack长度小于2");
                        float v1 = stack.Pop();
                        float v2 = stack.Pop();

                        stack.Push(v1 - v2);
                    }
                }
                else if (property == GameProperty.Mul)
                {
                    if (stack.Count < 2)
                    {
                        Debug.Log("逆波兰遇到操作符号的时候stack长度小于2");
                        float v1 = stack.Pop();
                        float v2 = stack.Pop();

                        stack.Push(v1 * v2);
                    }
                }
                else if (property == GameProperty.Mul)
                {
                    if (stack.Count < 2)
                    {
                        Debug.Log("逆波兰遇到操作符号的时候stack长度小于2");
                        float v1 = stack.Pop();
                        float v2 = stack.Pop();

                        stack.Push(v1 / v2);
                    }
                }
                else
                {
                    stack.Push((ownerObject as LiveItem).property.GetFloatProperty(property));
                }
            }
            else if (fv[1] == 2)
            {
                stack.Push(fv[0]);
            }
            else if (fv[1] == 3)
            {
                stack.Push(-fv[0]);
            }
        }

        Debug.Log("逆波兰没有出口.");

        return 0;
    }

    List<GameItemBase> target_list = new List<GameItemBase>(10);
    List<GameItemBase> apply_list = new List<GameItemBase>(10);

    public IEnumerator DoActiveSkill(ActiveSkillsConfig config)
    {
        Debug.Log(gameObject.name + " 释放技能: id: " + config.id);
        Debug.Log("伤害公式: " + config.damage);

        yield return SelectTarget(config);
    }

    public IEnumerator SelectTarget(ActiveSkillsConfig config)
    {
        //1.确定目标
        target_list.Clear();
        apply_list.Clear();

        var st = config.selectType;
        var tt = config.targetType;

        if (st == SelectType.One)
        {
            switch (tt)
            {
                case TargetType.Enemy:
                    target_list = StageCore.Instance.tagMgr.GetEntity(ref target_list, ETag.GetETag(ST.ENEMY, ST.DISCOVER));
                    break;
                case TargetType.Self:
                    target_list.Add(StageCore.Instance.Player);
                    break;
                case TargetType.Help:
                    target_list = StageCore.Instance.tagMgr.GetEntity(ref target_list, ETag.GetETag(ST.FRIEND));
                    break;
                case TargetType.HideMonster:
                    target_list = StageCore.Instance.tagMgr.GetEntity(ref target_list, ETag.GetETag(ST.MONSTER, ST.ENEMY, ST.UNDISCOVER));
                    break;
                case TargetType.LightBlock:
                    target_list = StageCore.Instance.tagMgr.GetEntity(ref target_list, ETag.GetETag(ST.DISCOVER, ST.BRICK));

                    for (int i = target_list.Count - 1; i >= 0; ++i)
                    {
                        Brick brick = target_list[i] as Brick;

                        if (brick.realBrickType == BrickType.OBSTACLE)
                        {
                            target_list.RemoveAt(i);
                        }
                    }
                    break;
                case TargetType.EmptyBlock:
                    target_list = StageCore.Instance.tagMgr.GetEntity(ref target_list, ETag.GetETag(ST.DISCOVER, ST.BRICK));

                    for (int i = target_list.Count - 1; i >= 0; ++i)
                    {
                        Brick brick = target_list[i] as Brick;

                        if (brick.realBrickType != BrickType.EMPTY)
                        {
                            target_list.RemoveAt(i);
                        }
                    }
                    break;
                case TargetType.DarkBlock:
                    target_list = StageCore.Instance.tagMgr.GetEntity(ref target_list, ETag.GetETag(ST.UNDISCOVER, ST.BRICK));
                    break;
                case TargetType.ObstructBlock:
                    target_list = StageCore.Instance.tagMgr.GetEntity(ref target_list, ETag.GetETag(ST.OBSTACLE));
                    break;
                case TargetType.Fort:
                    target_list = StageCore.Instance.tagMgr.GetEntity(ref target_list, ETag.GetETag(ST.OBSTACLE));
                    for (int i = target_list.Count - 1; i >= 0; ++i)
                    {
                        Obstacle ob = target_list[i] as Obstacle;

                        if (ob.occupy)
                        {
                            target_list.RemoveAt(i);
                        }
                    }
                    break;
                case TargetType.Satellite:
                    target_list.Add(ownerObject);
                    break;
            }
        }

        if (target_list.Count > 0)
        {
            if (st == SelectType.One)
            {
                yield return LightAndWaitSelect();
            }
            else if (st == SelectType.Direct)
            {

            }
            else if (st == SelectType.RA)
            {
                int count = config.selectArg;

                while (count > 0 && target_list.Count > 0)
                {
                    int i = Random.Range(0, count);

                    apply_list.Add(target_list[i]);

                    target_list.RemoveAt(i);

                    count -= 1;
                }

            }
            else if (st == SelectType.RB)
            {
                int count = config.selectArg;
                while (count > 0 && target_list.Count > 0)
                {
                    int i = Random.Range(0, count);

                    apply_list.Add(target_list[i]);

                    count -= 1;
                }

            }
        }
        else
        {
            Debug.Log("技能找不到符合条件的目标..");
        }

        yield return StageView.Instance.ShowEffectAndWaitHit(this, config);

    }

    private IEnumerator LightAndWaitSelect()
    {
        yield return 0;
    }

    public IEnumerator DoActiveSkill(List<ActiveSkillsConfig> configs)
    {
        foreach(var config in configs)
        {
            yield return DoActiveSkill(config);
        }
    }

    public IEnumerator DoActiveSkill(List<ulong> ids)
    {
         foreach (var id in ids)
        {
            var config = ConfigDataBase.GetConfigDataById<ActiveSkillsConfig>(id);
            yield return DoActiveSkill(config);
        }
    }

    public IEnumerator DoActiveSkill(ulong[] ids)
    {
        foreach (var id in ids)
        {
            var config = ConfigDataBase.GetConfigDataById<ActiveSkillsConfig>(id);
            yield return DoActiveSkill(config);
        }
    }
}


