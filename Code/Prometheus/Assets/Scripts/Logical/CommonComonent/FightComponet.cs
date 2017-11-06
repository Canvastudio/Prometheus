using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SkillType
{
    Active,
    Passive,
    Summon,
    Invalid,
}

public class FightComponet : MonoBehaviour
{
    public float time = 0;

    public bool hitTarget = false;

    /// <summary>
    /// ref主动技能配置表
    /// </summary>
    private List<ActiveSkillsConfig> activeSkillConfigs = new List<ActiveSkillsConfig>();

    /// <summary>
    /// 被动技能实例
    /// </summary>
    private List<PassiveSkillIns> passiveInsList = new List<PassiveSkillIns>();

    /// <summary>
    /// ref召唤技能配置表
    /// </summary>
    private List<SummonSkillsConfig> summonSkillConfigs = new List<SummonSkillsConfig>();

    /// <summary>
    /// 是否激活使用技能
    /// </summary>
    public bool skillActive = false;

    protected bool active = false;

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

    public virtual void Active()
    {
        if (!active)
        {
            foreach(var ins in passiveInsList)
            {
                ins.Active();
            }
        }
    }

    public virtual void Deactive()
    {
        if (active)
        {
            foreach (var ins in passiveInsList)
            {
                ins.Deactive();
            }
        }
    }

    public virtual void AddSkill(ulong id)
    {
        switch (IdToSkillType(id))
        {
            case SkillType.Active:
                activeSkillConfigs.Add(ConfigDataBase.GetConfigDataById<ActiveSkillsConfig>(id));
                break;
            case SkillType.Passive:
                var config = ConfigDataBase.GetConfigDataById<PassiveSkillsConfig>(id);
                PassiveSkillIns passiveSkillIns = new PassiveSkillIns(id, ownerObject);
                if (active)
                {
                    passiveSkillIns.Active();
                }
                break;
            case SkillType.Summon:
                summonSkillConfigs.Add(ConfigDataBase.GetConfigDataById<SummonSkillsConfig>(id));
                break;
        }

    }
    
    public virtual void RemoveSkill(ulong id)
    {
        switch (IdToSkillType(id))
        {
            case SkillType.Active:
                activeSkillConfigs.Remove(ConfigDataBase.GetConfigDataById<ActiveSkillsConfig>(id));
                break;
            case SkillType.Passive:
                for(int i = 0; i < passiveInsList.Count; ++i)
                {
                     if (passiveInsList[i].stateConfig.id == id)
                    {
                        passiveInsList[i].Deactive();
                        passiveInsList.RemoveAt(i);
                        break;
                    }
                }
                break;
            case SkillType.Summon:
                summonSkillConfigs.Remove(ConfigDataBase.GetConfigDataById<SummonSkillsConfig>(id));
                break;
        }
    }

    public static SkillType IdToSkillType(ulong id)
    {
        ulong i = id / 1000000;
        if (i == 1)
        {
            return SkillType.Active;
        }
        else if (i == 2)
        {
            return SkillType.Passive;
        }
        else if (i == 3)
        {
            return SkillType.Summon;
        }

        return SkillType.Invalid;
    }

    public static float CalculageRPN(long[] damage_values, GameItemBase rpn_source, GameItemBase rpn_target, out GameProperty valueType, ActiveSkillsConfig skillsConfig = null)
    {
        Stack<float> stack = new Stack<float>();

        float[] fv = new float[2];

        SuperTool.GetValue(damage_values[damage_values.Length - 1], ref fv);

        valueType = (GameProperty)fv[0];

        for (int i = 0; i < damage_values.Length - 1; ++i)
        {
            SuperTool.GetValue(damage_values[i], ref fv);


            var property = (GameProperty)fv[0];

            if (fv[1] == 0)
            {
                stack.Push(fv[0]);
            }
            else
            {
                if (property == GameProperty.Eql)
                {
                    if (stack.Count != 0)
                    {
                        return stack.Pop();
                    }
                }
                else if (property == GameProperty.Plus)
                {
                    if (stack.Count < 2)
                    {
                        Debug.Log("逆波兰遇到操作符号的时候stack长度小于2");
                    }
                    float v1 = stack.Pop();
                    float v2 = stack.Pop();

                    stack.Push(v1 + v2);

                }
                else if (property == GameProperty.Sub)
                {
                    if (stack.Count < 2)
                    {
                        Debug.Log("逆波兰遇到操作符号的时候stack长度小于2");
                    }
                    float v1 = stack.Pop();
                    float v2 = stack.Pop();

                    stack.Push(v2 - v1);

                }
                else if (property == GameProperty.Mul)
                {
                    if (stack.Count < 2)
                    {
                        Debug.Log("逆波兰遇到操作符号的时候stack长度小于2");
                    }
                    float v1 = stack.Pop();
                    float v2 = stack.Pop();

                    stack.Push(v1 * v2);

                }
                else if (property == GameProperty.Div)
                {
                    if (stack.Count < 2)
                    {
                        Debug.Log("逆波兰遇到操作符号的时候stack长度小于2");
                    }
                    float v1 = stack.Pop();
                    float v2 = stack.Pop();

                    stack.Push(v2/ v1);

                }
                else if (property == GameProperty.skillTime)
                {
                    stack.Push(skillsConfig.costTime);
                }
                else
                {
                    LiveItem target = null;

                    if (fv[1] == 1)
                    {
                        target = rpn_source as LiveItem;
                    }
                    else
                    {
                        target = rpn_target as LiveItem;
                    }

                    stack.Push(target.Property.GetFloatProperty(property));
                }
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

        ownerObject.OnActionBegin();

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
                    StageCore.Instance.tagMgr.GetEntity(ref target_list, ETag.GetETag(ST.ENEMY, ST.DISCOVER));
                    break;
                case TargetType.Self:
                    target_list.Add(StageCore.Instance.Player);
                    break;
                case TargetType.Help:
                    StageCore.Instance.tagMgr.GetEntity(ref target_list, ETag.GetETag(ST.FRIEND));
                    break;
                case TargetType.HideMonster:
                    StageCore.Instance.tagMgr.GetEntity(ref target_list, ETag.GetETag(ST.MONSTER, ST.ENEMY, ST.UNDISCOVER));
                    break;
                case TargetType.LightBlock:
                    StageCore.Instance.tagMgr.GetEntity(ref target_list, ETag.GetETag(ST.DISCOVER, ST.BRICK));

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
                if (tt == TargetType.Enemy || tt == TargetType.Help)
                {
                    for (int i = target_list.Count - 1; i >= 0; --i)
                    {
                        LiveItem live = target_list[i] as LiveItem;

                        foreach (var state in live.state_list)
                        {
                            if (state.active)
                            {
                                foreach (var effect in state.stateEffects)
                                {
                                    if (effect.stateType == StateEffectType.SelectImmune)
                                    {
                                        target_list.RemoveAt(i);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

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

        if (ownerObject is Player)
        {
            var stuff = config.stuffCost.stuffs.ToArray();
            var count = config.stuffCost.values.ToArray();
            Player player = ownerObject as Player;

            for (int n = 0; n < stuff.Length; ++n)
            {
                player.inventory.ChangeStuffCount(stuff[n], -count[n]);
            }

            //使用技能之后就不符合just的状态了
            StageCore.Instance.JustdiscoverMonster = false;
        }

        if (config.beforeSpecialEffect != null)
        {
            var effects = config.beforeSpecialEffect.ToArray();
            yield return ApplyEffect(config, config.beforeArgs, effects, apply_list);
        }


        long[] rpn = GlobalParameterConfig.GetConfigDataById<GlobalParameterConfig>(1).reloadSpeedFormula.ToArray();

        GameProperty property;

        float time_cost = CalculageRPN(rpn, ownerObject, null, out property, config);
        RangeSkillCost rangeSkillCost = new RangeSkillCost(time_cost);

        foreach (var state in ownerObject.state_list)
        {
            foreach (var effect in state.stateEffects)
            {
                if (effect.stateType == StateEffectType.RangeSkillCost)
                {
                    effect.ApplyState(rangeSkillCost);
                }
            }
        }

        time_cost = rangeSkillCost.cost;

        StageCore.Instance.TimeCast(time_cost);

        yield return StageView.Instance.ShowEffectAndWaitHit(this, config);

        if (hitTarget)
        {
            foreach (var target in apply_list)
            {
                if (config.damage != null)
                {
                    var damage = CalculageRPN(config.damage.ToArray(), ownerObject, target, out property);

                    Damage damageInfo = new Damage(damage, ownerObject, target as LiveItem, config.damageType);

                    foreach (var state in ownerObject.state_list)
                    {
                        foreach (var ins in state.stateEffects)
                        {
                            if (ins.stateType == StateEffectType.OnGenerateDamage)
                            {
                                ins.ApplyState(damageInfo);
                            }
                        }
                    }

                (target as LiveItem).TakeDamage(damageInfo);
                }
            }

            if (config.afterSpecialEffect != null)
            {
                var effects = config.afterSpecialEffect.ToArray();
                yield return ApplyEffect(config, config.afterArgs, effects, apply_list);
            }
        }

        Messenger<ActiveSkillsConfig>.Invoke(SA.PlayerUseSkill, config);

        ownerObject.OnActionEnd();
    }

    private IEnumerator ApplyEffect(ActiveSkillsConfig config, SuperArrayObj<SkillArg> args, SpecialEffect[] effects, List<GameItemBase> apply_list)
    {
        Brick brick = null;
        EffectCondition condition = EffectCondition.None;
        ulong state_id = 0;
        int remove_count = 0;

        for (int i = 0; i < effects.Length; ++i)
        {
            switch (effects[i])
            {
                case SpecialEffect.AddState:
                    state_id = args[i].u[0];
                    foreach (var item in apply_list)
                    {
                        var state_config = ConfigDataBase.GetConfigDataById<StateConfig>(state_id);
                        (item as LiveItem).AddStateIns(new StateIns(state_config, item as LiveItem, false));
                    }
                    break;
                case SpecialEffect.Property:
                    GameProperty property;

                    long[] rpn_values = args[i].rpn.ToArray(0);

                    foreach (var item in apply_list)
                    {
                        var value = CalculageRPN(rpn_values, ownerObject, item, out property);

                        (item as LiveItem).Property.SetFloatProperty(property, value);
                    }

                    break;
                case SpecialEffect.Transfiguration:
                    //TODO
                    break;
                case SpecialEffect.Enslave:
                    foreach (var item in apply_list)
                    {
                        (item as Monster).enslave = true;
                    }
                    break;
                case SpecialEffect.OpenBlockWithNear:
                    int range = (int)args[i].f[0];
                    Brick stand_brick = ownerObject.standBrick;
                    foreach (var item in apply_list)
                    {
                        brick = item as Brick;

                        yield return brick.OnDiscoverd();

                        var brick_list = BrickCore.Instance.GetNearbyBrick(brick.row, brick.column, range);

                        foreach (var nearby_brick in brick_list)
                        {
                            yield return nearby_brick.OnDiscoverd();
                        }
                    }
                    break;
                case SpecialEffect.HalfCostReturn:
                    condition = args[i].ec[0];
                    foreach (var item in apply_list)
                    {
                        if (CheckEffectCondition(condition, item, config.damageType))
                        {
                            if (ownerObject is Player)
                            {
                                var stuff = config.stuffCost.stuffs.ToArray();
                                var count = config.stuffCost.values.ToArray();
                                Player player = ownerObject as Player;
                                for (int n = 0; n < stuff.Length; ++n)
                                {
                                    player.inventory.ChangeStuffCount(stuff[n], Mathf.FloorToInt(.5f * count[n]));
                                }
                            }
                            else
                            {
                                Debug.LogError("返还消耗的目标不是玩家.");
                            }
                        }
                    }
                    break;
                case SpecialEffect.TransferSelf:
                    brick = apply_list[0] as Brick;
                    condition = args[i].ec[0];
                    if (ownerObject is Player)
                    {
                        if (CheckEffectCondition(condition, brick, config.damageType))
                        {
                            yield return (ownerObject as Player).moveComponent.Transfer(brick);
                        }
                        else
                        {
                            Debug.LogError("传送自己的对象不是玩家.");
                        }
                    }
                    break;
                case SpecialEffect.TransferTarget:
                    brick = apply_list[0] as Brick;
                    yield return (ownerObject as LiveItem).moveComponent.Transfer(brick);
                    break;
                case SpecialEffect.OffensiveDisperse:
                    remove_count = (int)args[i].f[0];
                    foreach (var item in apply_list)
                    {
                        (item as LiveItem).RemoveStateBuff(remove_count, true);
                    }
                    break;
                case SpecialEffect.AddStateToSelf:
                    state_id = args[i].u[0];
                    ownerObject.AddStateIns(new StateIns(ConfigDataBase.GetConfigDataById<StateConfig>(state_id), ownerObject, false));
                    break;
                case SpecialEffect.Disperse:
                    remove_count = (int)args[i].f[0];
                    foreach (var item in apply_list)
                    {
                        (item as LiveItem).RemoveStateBuff(remove_count, false);
                    }
                    break;
                case SpecialEffect.PositionExchange:
                    yield return MoveComponet.ExchangePosition(ownerObject.moveComponent, (apply_list[0] as Monster).moveComponent);
                    break;
            }
        }
    }


    public static bool CheckEffectCondition(EffectCondition condition, GameItemBase item, DamageType damageType)
    {
        switch (condition)
        {
            case EffectCondition.HasMonster:
                if (item is Monster)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case EffectCondition.NoMonster:
                if (item is Monster)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            case EffectCondition.Empty:
                if ((item as Brick).item == null && (item as Brick).realBrickType == BrickType.EMPTY)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case EffectCondition.DamageTypeCartridge:
                return damageType == DamageType.Cartridge;
            case EffectCondition.DamageTypeLaser:
                return damageType == DamageType.Laser;
            case EffectCondition.DamageTypeMelee:
                return damageType == DamageType.Physical;
            case EffectCondition.MonsterTypeIron:
                return (item is Monster && (item as Monster).monsterType == MonsterType.Iron);
            case EffectCondition.MonsterTypeOrganisms:
                return (item is Monster && (item as Monster).monsterType == MonsterType.Organisms);
            case EffectCondition.None:
                return true;
            default:
                return false;
        }
    }

    private IEnumerator LightAndWaitSelect()
    {
        yield return 0;
    }

    public IEnumerator DoActiveSkill(List<ActiveSkillsConfig> configs)
    {
        foreach (var config in configs)
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


