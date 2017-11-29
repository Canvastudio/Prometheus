using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LiveItemSide
{
    SIDE0,
    SIDE1,
}


public abstract class LiveItem : GameItemBase
{
    [SerializeField]
    private bool _silent = false;
    /// <summary>
    /// 沉默，所有被动技能无效，有些被动技能是给自己/队友施加状态，那么这些状态此时也应该消失。玩家也能被沉默。注意这并不是驱散，它只会使那些永久性质的状态消失（因为导致这些状态的被动技能失效了）
    /// </summary>
    public bool Silent
    {
        get { return _silent; }
        set
        {
            if (_silent == value) return;
            _silent = value;

            RefreshPassiveSKillState();
        }
    }



    public MonsterType monsterType;

    public StateComponent state;

    /// <summary>
    /// 强度
    /// </summary>
    public int pwr = 0;

    [SerializeField]
    private bool _freeze = false;
    /// <summary>
    /// 冻结，冻结后不能反击，不能使用技能，技能CD暂停，暂时解除周围格子锁定。玩家不会中该状态。无参数
    /// </summary>
    public bool Freeze
    {
        get { return _freeze; }
        set
        {
            if (_freeze == value) return;
            _freeze = value;

            RefreshActiveSKillState();
        }
    }

    public void RefreshActiveSKillState()
    {
        if (fightComponet != null)
        {
            bool b = (!Disarm && !Sleep && !Freeze && isDiscovered);

            if (activeSkillCanUsed != b)
            {
                if (activeSkillCanUsed)
                {
                    fightComponet.ActiveSkill();
                }
                else
                {
                    fightComponet.DeactiveSkill();
                }

                activeSkillCanUsed = b;
            }
        }
    }

    public void RefreshPassiveSKillState()
    {
        if (fightComponet != null)
        {
            bool b = (!Silent && isDiscovered);

            if (passiveSkillCanUsed != b)
            {
                if (b)
                {
                    fightComponet.ActivePassive();
                }
                else
                {
                    fightComponet.DeactivePassive();
                }
                
                passiveSkillCanUsed = b;
            }
        }
    }

    public bool activeSkillCanUsed = false;
    public bool passiveSkillCanUsed = false;

    [SerializeField]
    private bool _sleep = false;
    /// <summary>
    /// 催眠，不能反击，不能使用技能,技能CD暂停，暂时解除周围格子锁定，受到任何伤害都会导致该状态失效，玩家不会中该状态。无参数
    /// </summary>
    public bool Sleep
    {
        get { return _sleep; }
        set
        {
            if (_sleep == value) return;
            _sleep = value;

            RefreshActiveSKillState();
        }
    }

    [SerializeField]
    private bool _disarm = false;
    /// <summary>
    /// 缴械。不能使用主动技能，怪物被缴械后，技能CD暂停。玩家也是可以被缴械的。无参数
    /// </summary>
    public bool Disarm
    {
        get { return _disarm; }
        set
        {
            if (_disarm == value) return;
            _disarm = value;

            RefreshActiveSKillState();
        }
    }

    public void Awake()
    {
        Property = new LiveBasePropertys();
        Property.changeCallback = OnPropertyChange;
    }

    public FightComponet fightComponet;


    /// <summary>
    /// 在哪边, 0是敌对，1是玩家这边
    /// </summary>
    [SerializeField]
    private LiveItemSide _side = LiveItemSide.SIDE0;
    public LiveItemSide Side
    {
        get { return _side; }
        set
        {
            if (_side == value) return;

            bool b = (value == LiveItemSide.SIDE0);

            StageCore.Instance.tagMgr.SetEntityTag(this, ETag.Tag(ST.SIDE0), b);
            StageCore.Instance.tagMgr.SetEntityTag(this, ETag.Tag(ST.SIDE1), !b);

            _side = value;
        }
    }


    public bool isAlive = true;

    [SerializeField]
    private MoveComponet _moveComponet;
    [HideInInspector]
    public MoveComponet moveComponent
    {
        get
        {
            if (_moveComponet == null)
            {
                _moveComponet = transform.GetOrAddComponet<MoveComponet>();
                _moveComponet.owner = this;
            }

            return _moveComponet;
        }
    }

    [Space(10f)]
    [SerializeField]
    Text hp_value;
    [SerializeField]
    Text atk_value;

    [Space(10f)]
    [SerializeField]
    private float? _cur_hp;
    public float cur_hp
    {
        get
        {
            _cur_hp = Property.GetFloatProperty(GameProperty.nhp);

            return _cur_hp.Value;
        }
        set
        {
            if (cur_hp != value)
            {
                if (value <= 0)
                {
                    value = 0;
                }

                Property.SetFloatProperty(GameProperty.nhp, value);

                _cur_hp = value;
            }
        }
    }

    [SerializeField]
    private float? _fmax_hp;
    public float fmax_hp
    {
        get
        {
             _fmax_hp = Property.GetFloatProperty(GameProperty.mhp) * (1 + Property.GetFloatProperty(GameProperty.mhp_percent));

            return _fmax_hp.Value;
        }
    }

    [SerializeField]
    private float? _max_hp;
    public float max_hp
    {
        get
        {

             _max_hp = Property.GetFloatProperty(GameProperty.mhp);

            return _max_hp.Value;
        }
        set
        {
            if (value <= 0)
            {
                Debug.LogError("max hp 的值可以小于等于0？");
            }

            Property.SetFloatProperty(GameProperty.mhp, value);

            _max_hp = value;

            if (_fmax_hp < cur_hp)
            {
                cur_hp = _fmax_hp.Value;
            }
        }
    }

    [SerializeField]
    private float? _melee;
    public float melee
    {
        get
        {
            _melee = Property.GetFloatProperty(GameProperty.melee);

            return _melee.Value;
        }
        set
        {
            Property.SetFloatProperty(GameProperty.melee, value);

            _melee = value;

            if (atk_value != null)
            {
                atk_value.text = value.ToString();
            }
        }
    }

    public virtual void InitInfoUI()
    {
        if (hp_value != null)
        {
            hp_value.text = cur_hp.ToString();
        }

        if (atk_value != null)
        {
            atk_value.text = melee.ToString();
        }
    }

    public void OnPropertyChange(GameProperty property)
    {
        if (property == GameProperty.nhp)
        {
            if (cur_hp == 0)
            {
                //有可能这里本身就由ondead发起得一次属性改变，所以无需再重复一次
                if (gameObject.activeInHierarchy)
                {
                    Damage damage = new Damage(int.MaxValue, null, this, DamageType.Physical);

                    StartCoroutine(OnDead(damage));
                }
            }
            if (hp_value != null)
            {
                hp_value.FloatText(cur_hp);
            }
        }
        else if (property == GameProperty.melee)
        {
            if (atk_value != null)
            {
                atk_value.FloatText(melee);
            }
        }

        if (this is Player)
        {
            Messenger.Invoke(SA.PlayHpChange);
        }
    }

    /// <summary>
    /// 基础属性，血量，速度等
    /// </summary>
    public LiveBasePropertys Property;

    public void AddHpPercent(float percent)
    {
        cur_hp += fmax_hp * percent;
        cur_hp = Mathf.Min(fmax_hp, cur_hp);
    }

    public void AddHp(float hp)
    {
        float new_hp = cur_hp + hp;
        cur_hp = Mathf.Min(fmax_hp, new_hp);
    }

    public virtual IEnumerator OnDead(Damage damageInfo)
    {
        var list = state.state_list;

        foreach (var state in list)
        {
            foreach (var ins in state.stateEffects)
            {
                if (ins.stateType == StateEffectType.OnDeadth)
                {
                    ins.ApplyState(damageInfo);
                }
            }
        }

       StageCore.Instance.UnRegisterItem(this);

        isAlive = false;

        standBrick.CleanItem();

        return null;
    }

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.8f);

    public virtual IEnumerator MeleeAttackTarget<T>(T target) where T : LiveItem
    {
        if (target != null)
        {
            var damage = melee;

            Damage damageInfo = new Damage(damage, this, target, DamageType.Physical);

            foreach (var state in state.state_list)
            {
                foreach (var ins in state.stateEffects)
                {
                    if (ins.stateType == StateEffectType.OnGenerateDamage)
                    {
                        ins.ApplyState(damageInfo);
                    }
                }
            }

            yield return target.MeleeAttackByOther(this, damageInfo);
        }
    }

    public virtual IEnumerator MeleeAttackByOther<T>(T other, Damage damageInfo) where T : LiveItem
    {
        Debug.Log("被攻击： " + damageInfo.damageTarget);

        LeanTween.scale(transform.Rt(), new Vector3(0.9f, 0.9f, 0.9f), 0.1f).setLoopPingPong(3);

        yield return waitForSeconds;

        TakeDamage(damageInfo);
    }

    public virtual float TakeDamage(Damage damageInfo)
    {
        Debug.Log("对象：" + gameObject.name + " 收到伤害: 来源: " + damageInfo.damageSource);

        Sleep = false;

        foreach (var state in state.state_list)
        {
            foreach (var ins in state.stateEffects)
            {
                if (ins.stateType == StateEffectType.OnTakenDamage)
                {
                    ins.ApplyState(damageInfo);
                }

                if (ins.stateType == StateEffectType.Last)
                {
                    var last_count = StageCore.Instance.tagMgr.GetEntity(ETag.GetETag(ST.LAST)).Count;

                    if (last_count <= GContext.Instance.discover_monster)
                    {
                        damageInfo.damage = 0;
                    }
                }
            }
        }

        cur_hp = cur_hp - damageInfo.damage;

        if (damageInfo.attach_state > 0)
        {
            var config = StateConfig.GetConfigDataById<StateConfig>(damageInfo.attach_state);
            StateIns ins = new StateIns(config, this, false);
            state.AddStateIns(ins);
        }

        if (cur_hp == 0 && isAlive)
        {
           StartCoroutine(OnDead(damageInfo));
        }
        else
        {

            Messenger<Damage>.Invoke(SA.ItemTakeDamage, damageInfo);
        }

        return damageInfo.damage;
    }

    public  virtual void AddStateUI(StateIns ins)
    {

    }

    public virtual void RemoveStateUI(StateIns ins)
    {

    }

    public override void Recycle()
    {
        base.Recycle();

        fightComponet.Clean();
        state.Clean();


        Freeze = false;
        Disarm = false;
        Silent = false;
    }
}


