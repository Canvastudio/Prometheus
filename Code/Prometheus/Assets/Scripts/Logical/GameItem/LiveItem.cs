using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LiveItemSide
{
    SIDE0,
    SIDE1,
    Nil,
}


public abstract class LiveItem : GameItemBase
{
    [SerializeField]
    private int _silent = 0;
    /// <summary>
    /// 沉默，所有被动技能无效，有些被动技能是给自己/队友施加状态，那么这些状态此时也应该消失。玩家也能被沉默。注意这并不是驱散，它只会使那些永久性质的状态消失（因为导致这些状态的被动技能失效了）
    /// </summary>
    public int Silent
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
    private int _freeze = 0;
    /// <summary>
    /// 冻结，冻结后不能反击，不能使用技能，技能CD暂停，暂时解除周围格子锁定。玩家不会中该状态。无参数
    /// </summary>
    public int Freeze
    {
        get { return _freeze; }
        set
        {
            if (_freeze == value) return;
            _freeze = value;

            RefreshActiveSKillState();

            if (this is Monster)
            {
                if ((this as Monster).block_other)
                {
                    if (value > 0)
                    {
                        BrickCore.Instance.CancelBlockNearbyBrick(standBrick.row, standBrick.column);
                    }
                    else
                    {
                        BrickCore.Instance.BlockNearbyBrick(standBrick.row, standBrick.column);
                    }
                }

            }
        }
    }

    public void RefreshActiveSKillState()
    {
        if (fightComponet != null)
        {
            bool b = (Disarm == 0 && Sleep == 0 && Freeze == 0 && isDiscovered);

            if (activeSkillCanUsed != b)
            {
                if (b)
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
            bool b = (Silent == 0 && isDiscovered);

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
    private int _sleep = 0;
    /// <summary>
    /// 催眠，不能反击，不能使用技能,技能CD暂停，暂时解除周围格子锁定，受到任何伤害都会导致该状态失效，玩家不会中该状态。无参数
    /// </summary>
    public int Sleep
    {
        get { return _sleep; }
        set
        {
            if (_sleep == value) return;
            _sleep = value;

            RefreshActiveSKillState();

            if (this is Monster)
            {
                if ((this as Monster).block_other)
                {
                    if (Sleep > 0)
                    {
                        BrickCore.Instance.CancelBlockNearbyBrick(standBrick.row, standBrick.column);
                    }
                    else
                    {
                        BrickCore.Instance.BlockNearbyBrick(standBrick.row, standBrick.column);
                    }
                }

            }
        }
    }

    [SerializeField]
    private int _disarm = 0;
    /// <summary>
    /// 缴械。不能使用主动技能，怪物被缴械后，技能CD暂停。玩家也是可以被缴械的。无参数
    /// </summary>
    public int Disarm
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
        Property.owner = this;

        OriginProperty = new LiveBasePropertys();
        OriginProperty.owner = this;

        Property.changeCallback = OnPropertyChange;
    }

    public FightComponet fightComponet;


    /// <summary>
    /// 在哪边, 0是敌对，1是玩家这边
    /// </summary>
    [SerializeField]
    private LiveItemSide _side = LiveItemSide.Nil;
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

    [SerializeField]
    Text hp_value;
    [SerializeField]
    Text atk_value;

    [SerializeField]
    Text shield_value;
    [SerializeField]
    GameObject shield_bg;
    [SerializeField]
    Text armor_value;


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

                Property.SetFloatProperty(GameProperty.nhp, value, false);

                _cur_hp = value;

                if (hp_value != null)
                {
                    //hp_value.FloatText(_cur_hp.Value);
                    hp_value.SetIconHpText(this);

                    if (this is Player)
                    {
                        StageUIView.Instance.upUIView.RefreshHpUI();
                    }
                }
            }
        }
    }

    [SerializeField]
    private float? _fmax_hp;
    public float fmax_hp
    {
        get
        {
            _fmax_hp = Property.GetFloatProperty(GameProperty.mhp); // * (1 + Property.GetFloatProperty(GameProperty.mhp_percent));

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

    public virtual void InitInfoUI()
    {
        if (hp_value != null)
        {
            //hp_value.text = cur_hp.ToString();
            hp_value.SetIconHpText(this);
        }

        if (atk_value != null)
        {
            atk_value.SetIconColorText(this, GameProperty.attack);
        }

        if (shield_value != null)
        {
            shield_value.SetIconColorText(this, GameProperty.nshield);
        }

        if (armor_value != null)
        {
            armor_value.SetIconColorText(this, GameProperty.guard);
        }
    }

    public void OnPropertyChange(GameProperty property)
    {
        if (property == GameProperty.nhp)
        {
            if (cur_hp == 0)
            {
                if (gameObject.activeInHierarchy && isAlive)
                {
                    Damage damage = new Damage(int.MaxValue, null, this, DamageType.Physical);

                    StartCoroutine(OnDead(damage));
                }
            }

            if (hp_value != null)
            {
                //hp_value.FloatText(cur_hp);
                hp_value.SetIconHpText(this);
            }

            if (this is Player)
            {
                Messenger.Invoke(SA.PlayHpChange);
            }
        }
        else if (property == GameProperty.attack)
        {
            if (atk_value != null)
            {
                atk_value.SetIconColorText(this, GameProperty.attack);
            }
        }
        else if (property == GameProperty.nshield)
        {
            if (shield_value != null)
            {
                float s = Property.GetFloatProperty(GameProperty.nshield);

                if (s == 0)
                {
                    shield_value.gameObject.SetActive(false);

                    if (shield_bg != null)
                    {
                        shield_bg.SetActive(false);
                    }
                }
                else
                {
                    shield_value.gameObject.SetActive(true);

                    if (shield_bg != null)
                    {
                        shield_bg.SetActive(true);
                    }

                    shield_value.SetIconColorText(this, GameProperty.nshield);
                }
            }
        }
        else if (property == GameProperty.guard)
        {
            if (armor_value != null)
            {
                armor_value.SetIconColorText(this, GameProperty.guard);
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

    /// <summary>
    /// 原始数据用作对比，因为RPN公式 我无法知晓每一次操作数据得情况
    /// </summary>
    public LiveBasePropertys OriginProperty;

    public void AddHpPercent(float percent)
    {
        float v = cur_hp + fmax_hp * percent;
        cur_hp = Mathf.Min(fmax_hp, v);
    }

    public void AddHp(float hp)
    {
        float new_hp = cur_hp + hp;
        cur_hp = Mathf.Min(fmax_hp, new_hp);
    }

    public virtual IEnumerator OnDead(Damage damageInfo)
    {
       Debug.Log("LiveItem OnDead: " + gameObject.name);

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

   

        yield return 0;
    }

    WaitForSeconds wait0_1Seconds = new WaitForSeconds(0.1f);
    WaitForSeconds wait0_2Seconds = new WaitForSeconds(0.2f);

    public virtual IEnumerator MeleeAttackTarget<T>(T target) where T : LiveItem
    {
        if (target != null)
        {
            float damage = 0;

            damage = Property.GetFloatProperty(GameProperty.attack);

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

            transform.SetAsLastSibling();

            StartCoroutine(ArtSkill.DoSkillIE("pugong", transform.GetChild(0), target.transform));

            yield return wait0_2Seconds;

            yield return target.MeleeAttackByOther(this, damageInfo);
        }
    }



    public virtual IEnumerator MeleeAttackByOther<T>(T other, Damage damageInfo) where T : LiveItem
    {
        TakeDamage(damageInfo);

        yield return 0;
    }

    public virtual float TakeDamage(Damage damageInfo)
    {
        //Debug.Log("对象：" + gameObject.name + " 收到伤害: 来源: " + damageInfo.damageSource);
        state.RemoveStateBuff(StateEffectType.Freeze);

        Sleep = 0;

        for(int i = state.state_list.Count -1; i >= 0; --i)
        {
            var _state = state.state_list[i];

            foreach (var ins in _state.stateEffects)
            {
                if (ins.stateType == StateEffectType.OnTakenDamage)
                {
                    ins.ApplyState(damageInfo);
                }

                if (ins.stateType == StateEffectType.Last)
                {
                    var last_count = StageCore.Instance.tagMgr.GetEntity(ETag.GetETag(ST.LAST)).Count;

                    if (last_count < GContext.Instance.discover_monster)
                    {
                        damageInfo.damage = 0;
                    }
                }
            }
        }

        float s = Property.GetFloatProperty(GameProperty.nshield);

        if (damageInfo.damageType == DamageType.Physical)
        {
            if (s > 0)
            {
                float v = s - damageInfo.damage / 2;

                if (v > 0)
                {
                    damageInfo.damage *= 0.5f;
                    Property.SetFloatProperty(GameProperty.nshield, v);
                }
                else if (v <= 0)
                {
                    damageInfo.damage -= s;
                    Property.SetFloatProperty(GameProperty.nshield, 0);
                }
            }
           
            damageInfo.damage -= Property.GetFloatProperty(GameProperty.guard);
        }
        else if (s > 0 && damageInfo.damageType == DamageType.Laser)
        {
            float v = s - damageInfo.damage;

            if (v > 0)
            {
                damageInfo.damage = 0;
                Property.SetFloatProperty(GameProperty.nshield, v);
            }
            else
            {
                damageInfo.damage -= s;
                Property.SetFloatProperty(GameProperty.nshield, 0);
            }
        }

        if (cur_hp > 0 && damageInfo.damage > 0)
        {
            cur_hp = cur_hp - damageInfo.damage;

            if (damageInfo.attach_state > 0)
            {
                var config = StateConfig.GetConfigDataById<StateConfig>(damageInfo.attach_state);
                StateIns ins = new StateIns(config, this, null);
                state.AddStateIns(ins);
            }

            if (cur_hp == 0 && gameObject.activeInHierarchy)
            {
                StartCoroutine(OnDead(damageInfo));
            }
            else
            {
                Messenger<Damage>.Invoke(SA.ItemTakeDamage, damageInfo);
            }

            return damageInfo.damage;
        }
        else
        {
            return 0;
        }
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
        ActiveReset();
        fightComponet.Clean();
        state.Clean();

        Freeze = 0;
        Disarm = 0;
        Silent = 0;
        Sleep = 0;

        StageCore.Instance.tagMgr.RemoveEntity(this);
    }
}


