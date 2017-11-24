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
    /// 是否激活使用技能
    /// </summary>
    public bool skillActive = false;

    [SerializeField]
    protected bool activePassive = false;

    [SerializeField]
    public List<ActiveSkillIns> activeInsList = new List<ActiveSkillIns>(10);

    /// <summary>
    /// 被动技能实例
    /// </summary>
    [SerializeField]
    public List<PassiveSkillIns> passiveInsList = new List<PassiveSkillIns>();

    /// <summary>
    /// ref召唤技能配置表
    /// </summary>
    public List<SummonSkillsConfig> summonSkillConfigs = new List<SummonSkillsConfig>();

    private void Awake()
    {
        wuf = new WaitUntil(() => targetAttackFinsh == apply_list.Count);
    }

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

    public virtual void CleanData()
    {
 
        for (int i = activeInsList.Count - 1; i >= 0; --i)
        {
            activeInsList[i].Deactive();
            activeInsList.RemoveAt(i);
        }

        for (int i = passiveInsList.Count - 1; i >= 0; --i)
        {
            passiveInsList[i].Deactive();
            passiveInsList.RemoveAt(i);
        }

        //summonSkillConfigs.Remove(ConfigDataBase.GetConfigDataById<SummonSkillsConfig>(id));
    }

    /// <summary>
    /// 激活主动技能
    /// </summary>
    public virtual void ActiveSkill()
    {

    }

    public virtual void DeactiveSkill()
    {

    }

    public virtual void ActivePassive()
    {
        if (!activePassive)
        {
            foreach(var ins in passiveInsList)
            {
                ins.Active();
            }

            activePassive = true;
        }
    }

    public virtual void DeactivePassive()
    {
        if (activePassive)
        {
            foreach (var ins in passiveInsList)
            {
                ins.Deactive();
            }
        }
    }

    public virtual void AddSkill(ulong id, SkillPoint point = null)
    {
        switch (IdToSkillType(id))
        {
            case SkillType.Active:
                AddActiveIns(id, point);
                break;
            case SkillType.Passive:
                var config = ConfigDataBase.GetConfigDataById<PassiveSkillsConfig>(id);
                PassiveSkillIns passiveSkillIns = new PassiveSkillIns(id, point, ownerObject);
                if (activePassive)
                {
                    passiveSkillIns.Active();
                }
                passiveInsList.Add(passiveSkillIns);
                break;
            case SkillType.Summon:
                summonSkillConfigs.Add(ConfigDataBase.GetConfigDataById<SummonSkillsConfig>(id));
                break;
        }

    }
    
    protected virtual void AddActiveIns(ulong id, SkillPoint point)
    {
        ActiveSkillsConfig aconfig = ConfigDataBase.GetConfigDataById<ActiveSkillsConfig>(id);
        activeInsList.Add(new ActiveSkillIns(aconfig, ownerObject, point));
    }

    public virtual void RemoveSkill(ulong id)
    {
        switch (IdToSkillType(id))
        {
            case SkillType.Active:
                for (int i = 0; i < activeInsList.Count; ++i)
                {
                    if (activeInsList[i].config.id == id)
                    {
                        activeInsList[i].Deactive();
                        activeInsList.RemoveAt(i);
                        break;
                    }
                }
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
        else if (i == 3)
        {
            return SkillType.Passive;
        }
        else if (i == 2)
        {
            return SkillType.Summon;
        }

        return SkillType.Invalid;
    }

    

    List<GameItemBase> target_list = new List<GameItemBase>(10);
    List<GameItemBase> apply_list = new List<GameItemBase>(10);
    List<GameItemBase> damage_list;
    SelectType st;
    TargetType tt;

    protected IEnumerator FindAndConfrimTarget(ActiveSkillsConfig config)
    {
        Debug.Log("主动技能寻找和确认目标, id: " + config.id);

        target_list.Clear();
        apply_list.Clear();

        st = config.selectType;
        tt = config.targetType;

        switch (tt)
        {
            case TargetType.Enemy:
                if (ownerObject.Side == LiveItemSide.SIDE0)
                {
                    StageCore.Instance.tagMgr.GetEntity(ref target_list, ETag.GetETag(ST.SIDE1, ST.DISCOVER));
                }
                else
                {
                    StageCore.Instance.tagMgr.GetEntity(ref target_list, ETag.GetETag(ST.SIDE0, ST.DISCOVER));
                }
                break;
            case TargetType.Self:
                target_list.Add(ownerObject);
                break;
            case TargetType.Help:
                if (ownerObject.Side == LiveItemSide.SIDE0)
                {
                    StageCore.Instance.tagMgr.GetEntity(ref target_list, ETag.GetETag(ST.SIDE0, ST.DISCOVER));
                }
                else
                {
                    StageCore.Instance.tagMgr.GetEntity(ref target_list, ETag.GetETag(ST.SIDE1, ST.DISCOVER));
                }

                for (int i = target_list.Count - 1; i >= 0; --i)
                {
                    if (target_list[i].itemId == ownerObject.itemId)
                    {
                        target_list.RemoveAt(i);
                    }
                }

                break;
            case TargetType.HideMonster:
                StageCore.Instance.tagMgr.GetEntity(ref target_list, ETag.GetETag(ST.MONSTER, ST.ENEMY, ST.UNDISCOVER));
                break;
            case TargetType.LightBlock:
                StageCore.Instance.tagMgr.GetEntity(ref target_list, ETag.GetETag(ST.DISCOVER, ST.BRICK));

                for (int i = target_list.Count - 1; i >= 0; --i)
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

                for (int i = target_list.Count - 1; i >= 0; --i)
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
                for (int i = target_list.Count - 1; i >= 0; --i)
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
            case TargetType.Block:
                if (st != SelectType.Direct)
                {
                    target_list = StageCore.Instance.tagMgr.GetEntity(ref target_list, ETag.GetETag(ST.BRICK));
                }
                break;
        }


        int[] range = config.carry.ToArray();
        Debug.Log("range length: " + range.Length);
        int min_range = config.carry.ToArray()[0];
        int max_range = config.carry.ToArray()[1];

        for (int i = target_list.Count - 1; i >= 0; --i)
        {
            if (config.targetLimit != null)
            {
                Monster monster = target_list[i] as Monster;

                var mc = monster.config;
                var q = monster.pwr;

                if (!config.targetLimit.CheckLimit(mc, q))
                {
                    target_list.RemoveAt(i);
                    continue;
                }
            }

            int distance = ownerObject.standBrick.pathNode.Distance(target_list[i].standBrick.pathNode);

            if (distance < min_range || distance > max_range)
            {
                target_list.RemoveAt(i);
            }
        }

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

        if (st == SelectType.Direct)
        {
            target_list.Clear();

            foreach (var b in BrickCore.Instance.GetNearbyBrick(ownerObject.standBrick, 1))
            {
                target_list.Add(b);
            }

            yield return LightAndWaitSelect();

            Brick brick = apply_list[0] as Brick;
            damage_list = new List<GameItemBase>(12);
            List<Brick> bricks = null;

            if (brick.row == ownerObject.standBrick.row)
            {
                bricks = BrickCore.Instance.GetBrickOnRow(brick.row);

                for (int i = bricks.Count-1; i >= 0; --i)
                {
                    if (brick.column < ownerObject.standBrick.column)
                    {
                        if (!bricks[i].inViewArea || bricks[i].column > ownerObject.standBrick.column)
                        {
                            bricks.RemoveAt(i);
                        }
                    }
                    else
                    {
                        if (!bricks[i].inViewArea || bricks[i].column < ownerObject.standBrick.column)
                        {
                            bricks.RemoveAt(i);
                        }
                    }
                }
            }
            else
            {
                bricks = BrickCore.Instance.GetBrickOnColumn(brick.column);

                for (int i = bricks.Count - 1; i >= 0; --i)
                {
                    if (brick.row < ownerObject.standBrick.row)
                    {
                        if (!bricks[i].inViewArea || bricks[i].row > ownerObject.standBrick.row)
                        {
                            bricks.RemoveAt(i);
                        }
                    }
                    else
                    {
                        if (!bricks[i].inViewArea || bricks[i].row < ownerObject.standBrick.row)
                        {
                            bricks.RemoveAt(i);
                        }
                    }
                }
            }

            foreach(var b in bricks)
            {
                if (b.item != null && b.item is LiveItem)
                {
                    damage_list.Add(b.item);
                }
            }
        }
        else
        {
            if (target_list.Count > 0)
            {
                if (st == SelectType.One)
                {
                    yield return LightAndWaitSelect();
                }
                else if (st == SelectType.RA)
                {
                    int count = config.selectArg;

                    while (count > 0 && target_list.Count > 0)
                    {
                        int i = Random.Range(0, target_list.Count);

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
                        int i = Random.Range(0, target_list.Count);

                        apply_list.Add(target_list[i]);

                        count -= 1;
                    }
                }

                damage_list = null;
            }
            else
            {
                Debug.Log("技能找不到符合条件的目标..");
            }
        }
    }

    public IEnumerator DoActiveSkill(ActiveSkillsConfig config)
    {
        if (gameObject == null || config == null)
        {
            Debug.Log("奇怪？？？");
        }

        Debug.Log(gameObject.name + " 释放技能: id: " + config.id);
        Debug.Log("伤害公式: " + config.damage);

        ownerObject.OnActionBegin();

        targetAttackFinsh = 0;

        yield return FindAndConfrimTarget(config);

        if (apply_list.Count <= 0)
        {
            PopTipView.Instance.Show("target_none");
            ownerObject.OnActionEnd();
            yield break;
        }

        //扣除消耗
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
            GContext.Instance.JustdiscoverMonster = false;
        }


        //时间消耗
        if (ownerObject is Player)
        {
            long[] rpn = GlobalParameterConfig.GetConfigDataById<GlobalParameterConfig>(1).reloadSpeedFormula.ToArray();
            GameProperty property;
            float time_cost = Rpn.CalculageRPN(rpn, ownerObject, null, out property, config);
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
        }

        float[] successArray = null;
        float[] apperanceArray = null;
        if (config.damageArg != null)
        {
            apperanceArray = config.damageArg.ToArray();
        }

        if (config.successRate != null)
        {
            successArray = config.successRate.ToArray();
        }

        bool successEffect = true;

        if (use_fxlock)
        {
            ObjPool<ParticleSystem>.Instance.RecyclePool(FxCore.Instance.str_fxlock);
        }

        for (int i = 0; i < apply_list.Count; ++i)
        {
            if (config.successRate != null)
            {
                float r = 0;

                if (apply_list[i] is Player)
                {
                    r = successArray[0];
                }
                else if (apply_list[i] is Monster)
                {
                    r = successArray[(apply_list[i] as Monster).pwr];
                }
                else
                {
                    Debug.LogError("SuccessRate的技能的目标不是玩家也不是怪物?????");
                }

                if (r <= Random.Range(0f, 1f))
                {
                    successEffect = false;
                }
            }

            StartCoroutine (DoSkillOnTarget(apply_list[i], config, successEffect, apperanceArray, damage_list));
        }

        Messenger<ActiveSkillsConfig>.Invoke(SA.PlayerUseSkill, config);


        yield return wuf;

        ownerObject.OnActionEnd();

        Debug.Log("技能释放完毕: " + name);
    }

    static WaitUntil wuf;

    private int targetAttackFinsh = 0;

    private IEnumerator DoSkillOnTarget(GameItemBase target, ActiveSkillsConfig config, bool specialEffect, float[] damageApperance, List<GameItemBase> damageList = null)
    {
        if (config.beforeSpecialEffect != null && specialEffect)
        {
            var effects1 = config.beforeSpecialEffect.ToArray();

            if (damageList == null)
            {
                ApplyEffect(config, config.beforeArgs, effects1, target);
            }
            else
            {
                ApplyEffect(config, config.beforeArgs, effects1, damageList);
            }
        }

        GameProperty property;
        var damage = Rpn.CalculageRPN(config.damage.ToArray(), ownerObject, target, out property);
        int damageTimes = damageApperance.Length;

        Dictionary<int, float> realDamages = new Dictionary<int, float>();

        int i = 0;
   
        Debug.Log("播放技能特效: " + config.effect);

        yield return ArtSkill.DoSkillIE(config.effect, ownerObject.transform, target.transform, () =>
        {
            if (config.damage != null)
            {
                if (damageList == null)
                {
                    Damage damageInfo = new Damage(damage * damageApperance[i++], ownerObject, target as LiveItem, config.damageType);
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

                    float v = 0;

                    if (realDamages.TryGetValue(target.itemId, out v))
                    {
                        realDamages[target.itemId] = v + (target as LiveItem as LiveItem).TakeDamage(damageInfo);
                    }
                    else
                    {
                        realDamages.Add(target.itemId, (target as LiveItem as LiveItem).TakeDamage(damageInfo));
                    }
                }
                else
                {
                    foreach (var item in damageList)
                    {
                        Damage damageInfo = new Damage(damage * damageApperance[i], ownerObject, item as LiveItem, config.damageType);
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

                        float v = 0;

                        if (realDamages.TryGetValue(item.itemId, out v))
                        {
                            realDamages[item.itemId] = v + (item as LiveItem as LiveItem).TakeDamage(damageInfo);
                        }
                        else
                        {
                            realDamages.Add(item.itemId, (item as LiveItem as LiveItem).TakeDamage(damageInfo));
                        }
                    }

                    ++i;
                }
            }
        });



        if (config.afterSpecialEffect != null && specialEffect)
        {
            var effects2 = config.afterSpecialEffect.ToArray();

            if (damageList == null)
            {
                ApplyEffect(config, config.afterArgs, effects2, target, realDamages[target.itemId]);
            }
            else
            {

                ApplyEffect(config, config.afterArgs, effects2, damageList, realDamages);
            }
        }

        targetAttackFinsh++;
    }

    private void ApplyEffect(ActiveSkillsConfig config, SuperArrayObj<SkillArg> args, SpecialEffect[] effects, GameItemBase item, float skillDamage = 0)
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

                    var state_config = ConfigDataBase.GetConfigDataById<StateConfig>(state_id);
                    (item as LiveItem).AddStateIns(new StateIns(state_config, item as LiveItem, false, ownerObject));

                    break;
                case SpecialEffect.Property:
                    GameProperty property;

                    long[] rpn_values = args[i].rpn.ToArray(0);


                    var value = Rpn.CalculageRPN(rpn_values, ownerObject, item, out property, config, skillDamage);

                    (item as LiveItem).Property.SetFloatProperty(property, value);


                    break;
                case SpecialEffect.Transfiguration:
                    //TODO
                    break;
                case SpecialEffect.Enslave:

                    (item as Monster).enslave = true;

                    break;
                case SpecialEffect.OpenBlockWithNear:
                    int range = (int)args[i].f[0];
                    Brick stand_brick = ownerObject.standBrick;

                    brick = item as Brick;

                    StartCoroutine(brick.OnDiscoverd());

                    var brick_list = BrickCore.Instance.GetNearbyBrick(brick.row, brick.column, range);

                    foreach (var nearby_brick in brick_list)
                    {
                        StartCoroutine(nearby_brick.OnDiscoverd());
                    }

                    break;
                case SpecialEffect.HalfCostReturn:
                    condition = args[i].ec[0];

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
                    break;
                case SpecialEffect.TransferSelf:
                    brick = apply_list[0] as Brick;
                    condition = args[i].ec[0];
                    if (ownerObject is Player)
                    {
                        if (CheckEffectCondition(condition, brick, config.damageType))
                        {
                            StartCoroutine((ownerObject as Player).moveComponent.Transfer(brick));
                        }
                        else
                        {
                            Debug.LogError("传送自己的对象不是玩家.");
                        }
                    }
                    break;
                case SpecialEffect.TransferTarget:
                    brick = apply_list[0] as Brick;
                    StartCoroutine((ownerObject as LiveItem).moveComponent.Transfer(brick));
                    break;
                case SpecialEffect.OffensiveDisperse:
                    remove_count = (int)args[i].f[0];

                    (item as LiveItem).RemoveStateBuff(remove_count, true);

                    break;
                case SpecialEffect.AddStateToSelf:
                    state_id = args[i].u[0];
                    ownerObject.AddStateIns(new StateIns(ConfigDataBase.GetConfigDataById<StateConfig>(state_id), ownerObject, false));
                    break;
                case SpecialEffect.Disperse:
                    remove_count = (int)args[i].f[0];

                    (item as LiveItem).RemoveStateBuff(remove_count, false);

                    break;
                case SpecialEffect.PositionExchange:
                    StartCoroutine(MoveComponet.ExchangePosition(ownerObject.moveComponent, (apply_list[0] as Monster).moveComponent));
                    break;
            }
        }
    }
    private void ApplyEffect(ActiveSkillsConfig config, SuperArrayObj<SkillArg> args, SpecialEffect[] effects, List<GameItemBase> apply_list, Dictionary<int ,float> realDamages = null)
    {
        foreach(var item in apply_list)
        {
            if (realDamages != null)
            {
                ApplyEffect(config, args, effects, item, realDamages[item.itemId]);
            }
            else
            {
                ApplyEffect(config, args, effects, item);
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

    private WaitForMsg waitForClick = new WaitForMsg();

    bool use_fxlock = false;

    private IEnumerator LightAndWaitSelect()
    {
        foreach (var item in target_list)
        {
            use_fxlock = true;
            var p = ObjPool<ParticleSystem>.Instance.GetObjFromPool(FxCore.Instance.str_fxlock);
            p.transform.position = item.transform.position;
            p.gameObject.SetActive(true);
            p.Play();
        }


        yield return waitForClick.BeginWaiting<Brick>(SA.PlayerClickBrick);

        if (waitForClick.result.msg == SA.PlayerClickBrick)
        {
            Brick brick = waitForClick.result.para as Brick;
            
            foreach(var item in target_list)
            {
                if (item is Brick)
                {
                    if (item.Equals(brick))
                    {
                        apply_list.Add(item);
                    }
                }
                else
                {
                    if (item.standBrick.Equals(brick))
                    {
                        apply_list.Add(item);
                    }
                }
            }
        }
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


