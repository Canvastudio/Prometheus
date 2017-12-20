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
    [SerializeField]
    protected bool _skillActive = false;
    public bool skillActive
    {
        get { return _skillActive; }
    }


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

    public bool has_kill = false;

    private void Awake()
    {
        wuf = new WaitUntil(CheckFinish);
    }

    private bool CheckFinish()
    {
        return targetAttackFinsh == apply_list.Count;
    }

    private LiveItem _ownerObject;
    public LiveItem ownerObject
    {
        set
        {
            _ownerObject = value;
        }
        get
        {
            if (_ownerObject == null)
            {
                _ownerObject = gameObject.GetComponent<LiveItem>();
            }

            return _ownerObject;
        }
    }

    public virtual void Clean()
    {
        for (int i = activeInsList.Count - 1; i >= 0; --i)
        {
            activeInsList[i].Deactive();
            activeInsList.RemoveAt(i);
        }

        for (int i = passiveInsList.Count - 1; i >= 0; --i)
        {
            passiveInsList[i].Remove();
            passiveInsList.RemoveAt(i);
        }

        activePassive = false;
        _skillActive = false;
        has_kill = false;
        //summonSkillConfigs.Remove(ConfigDataBase.GetConfigDataById<SummonSkillsConfig>(id));
    }

    /// <summary>
    /// 激活主动技能
    /// </summary>
    public virtual void ActiveSkill()
    {
        _skillActive = true;
    }

    public virtual void DeactiveSkill()
    {
        _skillActive = false;
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
        var ins = new ActiveSkillIns(aconfig, ownerObject, point);
        activeInsList.Add(ins);
        StageUIView.Instance.AddChipSkillIntoSkillList(ins);
    }

    public virtual void AddOrganSkill(ActiveSkillIns ins)
    {
        activeInsList.Add(ins);
    }

    public virtual void RemoveOrganSkill(ulong id)
    {
        for (int i = 0; i < activeInsList.Count; ++i)
        {
            if (activeInsList[i].config.id == id && activeInsList[i].count == 0)
            {
                activeInsList[i].Deactive();
                activeInsList.RemoveAt(i);
                break;
            }
        }

        //Debug.LogError("无法移除指定的organ skill: " + id.ToString());
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
                        passiveInsList[i].Remove();
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
                //target_list = StageCore.Instance.tagMgr.GetEntity(ref target_list, ETag.GetETag(ST.UNDISCOVER, ST.BRICK));
                var bricks = BrickCore.Instance.GetNearbyBrick(ownerObject.standBrick, config.carry.ToArray()[1]);
                for (int i = bricks.Count - 1; i >= 0; --i)
                {
                    if (!bricks[i].isDiscovered 
                        && bricks[i].pathNode.Distance(ownerObject.standBrick.pathNode) >= config.carry.ToArray()[0]
                        && bricks[i].realBrickType != BrickType.OBSTACLE)
                    {
                        target_list.Add(bricks[i]);
                    }
                }
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

                var q = monster.pwr;

                if (config.targetLimit.CheckLimit(monster.monsterType, q))
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

                foreach (var state in live.state.state_list)
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
        }
        else
        {
            if (target_list.Count > 0)
            {
                if (st == SelectType.One)
                {
                    if (ownerObject is Player)
                    {
                        yield return LightAndWaitSelect();
                    }
                    else
                    {
                        apply_list.Add(target_list[0]);
                    }
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
                else if (st == SelectType.ALL)
                {
                    apply_list.AddRange(target_list);
                }
            }
            else
            {
                if (ownerObject is Player)
                {
                    PopTipView.Instance.Show("target_none");
                }                                                   
            }
        }
    }

    public IEnumerator CopySkill(ActiveSkillsConfig config, List<GameItemBase> applys)
    {
        ownerObject.OnActionBegin();
        has_kill = false;
        targetAttackFinsh = 0;

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

        apply_list = applys;

        bool successEffect = true;

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
                    r = 100;
                }

                if (r <= Random.Range(0f, 1f))
                {
                    successEffect = false;
                }
            }

            StartCoroutine(DoSkillOnTarget(apply_list[i], config, successEffect, apperanceArray));
        }

        yield return new WaitUntil(CheckFinish);

        ownerObject.OnActionEnd();

        Debug.Log("技能释放完毕: " + name);
    }

    public IEnumerator DoActiveSkill(ActiveSkillIns ins, ActiveSkillsConfig _config = null, int c = -1)
    {
        ActiveSkillsConfig config;
        int _count;
        if (ins != null)
        {
            config = ins.config;
            _count = ins.count;
        }
        else
        {
            config = _config;
            _count = c;
        }

        if (gameObject == null )
        {
            Debug.Log("奇怪？？？gameObject 为空");
        }

        if (config == null)
        {
            Debug.Log("奇怪？？？config 为空");
        }

        if (ownerObject is Player && _count == -1)
        {
            var stuff = config.stuffCost.stuffs.ToArray();
            var count = config.stuffCost.values.ToArray();
            for (int n = 0; n < stuff.Length; ++n)
            {
                StageUIView.Instance.mat.SetCost(stuff[n], count[n]);
            }
        }

        Debug.Log(gameObject.name + " 释放技能: id: " + config.id);
        //Debug.Log("伤害公式: " + config.damage);

        ownerObject.OnActionBegin();
        has_kill = false;
        targetAttackFinsh = 0;

        yield return FindAndConfrimTarget(config);

        if (apply_list.Count <= 0)
        {
            ownerObject.OnActionEnd();
            StageUIView.Instance.IniMat();
            yield break;
        }

        //扣除消耗
        if (ownerObject is Player && _count == -1)
        {
            //使用技能之后就不符合just的状态了
            GContext.Instance.JustdiscoverMonster = false;

            var stuff = config.stuffCost.stuffs.ToArray();
            var count = config.stuffCost.values.ToArray();

            Player player = ownerObject as Player;


            for (int n = 0; n < stuff.Length; ++n)
            {
                if (player.inventory.GetStuffCount(stuff[n]) < count[n])
                {
                    PopTipView.Instance.Show("deficient_resources");
                    ownerObject.OnActionEnd();
                    yield break;
                }
            }

            for (int n = 0; n < stuff.Length; ++n)
            {
                player.inventory.ChangeStuffCount(stuff[n], -count[n]);
            }
        }

        if (_count > 0)
        {
            StageUIView.Instance.AddOrganSkillCount(ins.config.id, -1);
        }

        StageUIView.Instance.IniMat();
        SkillInfo info = new SkillInfo(config, ownerObject, apply_list);
        Messenger<SkillInfo>.Invoke(SA.LivePreuseSkill, info);

        //时间消耗
        if (ownerObject is Player)
        {
            long[] rpn = GlobalParameterConfig.GetConfigDataById<GlobalParameterConfig>(1).reloadSpeedFormula.ToArray();
            float[] v;
            float time_cost = Rpn.CalculageRPN(rpn, ownerObject, null, out v, config);
 
            RangeSkillCost rangeSkillCost = new RangeSkillCost(time_cost);

            foreach (var state in ownerObject.state.state_list)
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


 

        for (int i = 0; i < apply_list.Count; ++i)
        {
            Debug.Log("Skill Target: " + apply_list[i].name);

            if (config.successRate != null)
            {
                float r = 0;

                if (apply_list[i] is Player)
                {
                    r = successArray[0];
                }
                else if (apply_list[i] is Monster)
                {
                    if ((apply_list[i] as Monster).isDiscovered == false)
                    {
                        Debug.LogError("技能对象是一个没有被发现的怪物??");
                    }

                    r = successArray[(apply_list[i] as Monster).pwr];

                }
                else
                {
                    r = 100;
                }

                if (r < Random.Range(0f, 1f))
                {
                    successEffect = false;
                }
            }

            StartCoroutine (DoSkillOnTarget(apply_list[i], config, successEffect, apperanceArray));
        }

        yield return new WaitUntil(CheckFinish);

        ownerObject.OnActionEnd();

        Debug.Log("技能释放完毕: " + name);
    }

    static WaitUntil wuf;

    private int targetAttackFinsh = 0;

    public IEnumerator CopySkill(SkillInfo skill)
    {
        var config = skill.config;

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
                    r = 100;
                }

                if (r <= Random.Range(0f, 1f))
                {
                    successEffect = false;
                }
            }

            StartCoroutine(DoSkillOnTarget(apply_list[i], config, successEffect, apperanceArray));
        }

        yield return new WaitUntil(CheckFinish);

        ownerObject.OnActionEnd();

        Debug.Log("技能释放完毕: " + name);
    }

    public List<GameItemBase> CheckFinalEffectItems(GameItemBase target, ActiveSkillsConfig config)
    {
        List<GameItemBase> finalEffectItems = new List<GameItemBase>(10);

        if (config.selectType == SelectType.Direct)
        {
            Brick brick = target as Brick;
            List<Brick> bricks = null;

            if (brick.row == ownerObject.standBrick.row)
            {
                bricks = BrickCore.Instance.GetBrickOnRow(brick.row);

                for (int i = bricks.Count - 1; i >= 0; --i)
                {
                    if (brick.column < ownerObject.standBrick.column)
                    {
                        if (!bricks[i].inViewArea || bricks[i].column > ownerObject.standBrick.column || !brick.isDiscovered)
                        {
                            bricks.RemoveAt(i);
                        }
                    }
                    else
                    {
                        if (!bricks[i].inViewArea || bricks[i].column < ownerObject.standBrick.column || !brick.isDiscovered)
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

            foreach (var b in bricks)
            {
                if (b.item != null && b.item is LiveItem)
                {
                    if (b.item.isDiscovered)
                    {
                        finalEffectItems.Add(b.item);
                    }
                }
            }
        }
        else
        {
            finalEffectItems.Add(target);
        }

        if (config.multipleType != MultipleType.None)
        {
            int i = 0;

            if (finalEffectItems.Count > 0)
            {
                int count = finalEffectItems.Count;

                for (i = count - 1; i >= 0; --i)
                {
                    var liveItems = BrickCore.Instance.GetNearbyLiveItem(finalEffectItems[i].standBrick, config.multipleArg, config.multipleType == MultipleType.Round);

                    foreach (var item in liveItems)
                    {
                        if (item.isDiscovered)
                        {
                            finalEffectItems.Add(item);
                        }
                    }
                }

                if (config.targetLimit != null)
                {
                    for (i = finalEffectItems.Count - 1; i >= 0; --i)
                    {
                        LiveItem liveItem = finalEffectItems[i] as LiveItem;
                        MonsterType type = liveItem.monsterType;

                        if (!config.targetLimit.CheckLimit(liveItem.monsterType, liveItem.pwr))
                        {
                            finalEffectItems.RemoveAt(i);
                            continue;
                        }
                    }
                }
            }
        }

        return finalEffectItems;
    }
    float[] _damageApperance;
    private IEnumerator DoSkillOnTarget(GameItemBase ShowTarget, ActiveSkillsConfig config, bool specialEffect, float[] damageApperance)
    {
        int i = 0;
        _damageApperance = damageApperance;
        var items = CheckFinalEffectItems(ShowTarget, config);

        //Debug.Log("技能： " + config.name + " 得造成伤害得目标数量为: " + items.Count);

        if (config.beforeSpecialEffect != null && specialEffect)
        {
            var effects1 = config.beforeSpecialEffect.ToArray();
            ApplyEffect(config, config.beforeArgs, effects1, items);
        }


        Dictionary<ulong, float> realDamages = new Dictionary<ulong, float>();

        i = 0;

        //Debug.Log("播放技能特效: " + config.effect);

        yield return ArtSkill.DoSkillIE(config.effect, ownerObject.transform, ShowTarget.transform, () =>
        {
            if (config.damage != null && ownerObject.isAlive)
            {
                foreach (var item in items)
                {
                    float[] value;

                    var damage = Rpn.CalculageRPN(config.damage.ToArray(), ownerObject, item, out value);

                    Damage damageInfo = new Damage(damage * _damageApperance[i], ownerObject, item as LiveItem, config.damageType);

                    foreach (var state in ownerObject.state.state_list)
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
                        realDamages[item.itemId] = v + (item as LiveItem ).TakeDamage(damageInfo);
                    }
                    else
                    {
                        realDamages.Add(item.itemId, (item as LiveItem).TakeDamage(damageInfo));
                    }

                    if((item as LiveItem).cur_hp == 0)
                    {
                        has_kill = true;
                    }

                    ++i;
                }

     
            }
        });



        if (config.afterSpecialEffect != null && specialEffect)
        {
            var effects2 = config.afterSpecialEffect.ToArray();

            foreach (var item in items)
            {
                float rd;

                if (!realDamages.TryGetValue(item.itemId, out rd))
                {
                    rd = 0;
                }

                ApplyEffect(config, config.afterArgs, effects2, item, rd);
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
                    StateIns ins = new StateIns(state_config, item as LiveItem, null, ownerObject);
                    ins.ActiveIns();

                    Debug.Log("为：" + item.gameObject.name + " 添加状态: " + state_config.name);
                    (item as LiveItem).state.AddStateIns(ins);

                    break;
                case SpecialEffect.StateAtTarget:

                    Brick _b = item as Brick;

                    if (_b.realBrickType == BrickType.PLAYER || _b.realBrickType == BrickType.MONSTER)
                    {
                        LiveItem live = _b.item as LiveItem;

                        state_id = args[i].u[0];
                        var _state_config = ConfigDataBase.GetConfigDataById<StateConfig>(state_id);
                        StateIns _ins = new StateIns(_state_config, live, null, ownerObject);
                        _ins.ActiveIns();

                        Debug.Log("为：" + live.gameObject.name + " 添加状态: " + _state_config.name);
                        live.state.AddStateIns(_ins);
                    }
                    else
                    {
                        Debug.LogError("StateAtTarget: 对象错误...");
                    }
                    break;
                case SpecialEffect.Property:

                    GameProperty property;

                    int arg_count = args[i].rpn.Count();

                    for (int m = 0; m < arg_count; ++m)
                    {
                        long[] rpn_values = args[i].rpn.ToArray(m);

                        float[] f;

                        var value = Rpn.CalculageRPN(rpn_values, ownerObject, item, out f, config, skillDamage);

                        property = (GameProperty)(f[0]);

                        if (f[1] == 2)
                        {
                            (item as LiveItem).Property.SetFloatProperty(property, value);
                        }
                        else if (f[1]== 1)
                        {
                            (ownerObject as LiveItem).Property.SetFloatProperty(property, value);
                        }
                    }
                    break;
                case SpecialEffect.PropertyAtTarget:

                    if (item is Brick)
                    {
                        Brick cur_brick = item as Brick;

                        if (cur_brick.item != null && cur_brick.item is LiveItem)
                        {
                            var t_item = cur_brick.item as LiveItem;

                            GameProperty _property;

                            int _arg_count = args[i].rpn.Count();

                            for (int m = 0; m < _arg_count; ++m)
                            {
                                long[] rpn_values = args[i].rpn.ToArray(m);

                                float[] f;

                                var value = Rpn.CalculageRPN(rpn_values, ownerObject, t_item, out f, config, skillDamage);

                                _property = (GameProperty)(f[0]);

                                (t_item as LiveItem).Property.SetFloatProperty(_property, value);

                                if (_property == GameProperty.nhp && value == 0)
                                {
                                    has_kill = true;
                                }
                            }
                        }
                    }
                    break;
                case SpecialEffect.Transfiguration:
                    //TODO
                    Monster liveItem = item as Monster;
                    liveItem.icon.sprite = StageView.Instance.itemAtlas.GetSprite("chicken_0");
                    liveItem.fightComponet.DeactiveSkill();
                    liveItem.fightComponet.DeactivePassive();
                    liveItem.fightComponet.Clean();
                    foreach(var state in liveItem.state.state_list)
                    {
                        state.DeactiveIns();
                    }
                    liveItem.state.state_list.Clear();
                    liveItem.state.halo_list.Clear();
                    break;
                //case SpecialEffect.Enslave:

                //    (item as Monster).enslave = true;

                //    break;
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
                    LiveItem _live = apply_list[0] as LiveItem;

                    var bricks = StageCore.Instance.tagMgr.GetEntity<Brick>(ETag.GetETag(ST.DISCOVER, ST.BRICK));

                    for (int k = bricks.Count - 1; k >= 0; --k)
                    {
                        if (bricks[k].realBrickType != BrickType.EMPTY)
                        {
                            bricks.RemoveAt(k);
                        }
                    }

                    if (bricks.Count > 0)
                    {
                        int _ii = Random.Range(0, bricks.Count);
                        Debug.Log("random trans: " + _ii.ToString());
                        var _brick = bricks[_ii];
                        StartCoroutine(_live.moveComponent.Transfer(_brick));
                    }
                    break;
                case SpecialEffect.OffensiveDisperse:
                    remove_count = (int)args[i].f[0];

                    (item as LiveItem).state.RemoveStateBuff(remove_count, true);

                    break;
                case SpecialEffect.AddStateToSelf:
                    state_id = args[i].u[0];
                    ownerObject.state.AddStateIns(new StateIns(ConfigDataBase.GetConfigDataById<StateConfig>(state_id), ownerObject, null));
                    break;
                case SpecialEffect.Disperse:
                    remove_count = (int)args[i].f[0];

                    (item as LiveItem).state.RemoveStateBuff(remove_count, false);

                    break;
                case SpecialEffect.PositionExchange:
                    StartCoroutine(MoveComponet.ExchangePosition(ownerObject.moveComponent, (apply_list[0] as Monster).moveComponent));
                    break;
            }
        }
    }
    private void ApplyEffect(ActiveSkillsConfig config, SuperArrayObj<SkillArg> args, SpecialEffect[] effects, List<GameItemBase> apply_list, Dictionary<ulong, float> realDamages = null)
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
                    if (item is Brick)
                    {
                        Brick b = item as Brick;
                        if (b.realBrickType == BrickType.MONSTER)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                        return false;
                }
            case EffectCondition.NoMonster:
                if (item is Monster)
                {
                    return false;
                }
                else
                {
                    if (item is Brick)
                    {
                        Brick b = item as Brick;

                        if (b.realBrickType != BrickType.MONSTER)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                        return true;
                }
            case EffectCondition.Empty:
                if ((item as Brick).realBrickType == BrickType.EMPTY)
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

        if (use_fxlock)
        {
            ObjPool<ParticleSystem>.Instance.RecyclePool(FxCore.Instance.str_fxlock);
        }
    }

    //public IEnumerator DoActiveSkill(List<ActiveSkillsConfig> configs)
    //{
    //    foreach (var config in configs)
    //    {
    //        yield return DoActiveSkill(config);
    //    }
    //}

    //public IEnumerator DoActiveSkill(List<ulong> ids)
    //{
    //    foreach (var id in ids)
    //    {
    //        var config = ConfigDataBase.GetConfigDataById<ActiveSkillsConfig>(id);
    //        yield return DoActiveSkill(config);
    //    }
    //}

    //public IEnumerator DoActiveSkill(ulong[] ids)
    //{
    //    foreach (var id in ids)
    //    {
    //        var config = ConfigDataBase.GetConfigDataById<ActiveSkillsConfig>(id);
    //        yield return DoActiveSkill(config);
    //    }
    //}
}


