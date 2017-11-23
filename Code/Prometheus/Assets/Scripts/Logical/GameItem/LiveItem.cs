﻿using System;
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



    public FightComponet fightComponet;
    /// <summary>
    /// 自己拥有的光环，不包括别人影响自己的
    /// </summary>
    public List<HaloInfo> halo_list = new List<HaloInfo>(2);

    public List<StateIns> state_list = new List<StateIns>(8);

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
            StageCore.Instance.tagMgr.SetEntityTag(this, ETag.Tag(ST.SIDE1), b);
            _side = value;
        }
    }
    

    public bool isAlive
    {
        get
        {
            return cur_hp > 0;
        }
    }

    [SerializeField]
    private MoveComponet _moveComponet;
    [HideInInspector]
    public MoveComponet moveComponent
    {
        get
        {
            if (_moveComponet == null)
            {
                _moveComponet = GetComponent<MoveComponet>();
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
            if (!_cur_hp.HasValue) _cur_hp = Property.GetFloatProperty(GameProperty.nhp);

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

                if (hp_value != null)
                {
                    hp_value.text = value.ToString();
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
            if (!_fmax_hp.HasValue)
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
            if (!_max_hp.HasValue)
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
            if (!_melee.HasValue) _melee = Property.GetFloatProperty(GameProperty.melee);

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

    /// <summary>
    /// 基础属性，血量，速度等
    /// </summary>
    public LiveBasePropertys Property = new LiveBasePropertys();

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
        standBrick.CleanItem();

        StageCore.Instance.UnRegisterItem(this);

        return null;
    }

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.8f);

    public virtual IEnumerator MeleeAttackTarget<T>(T target) where T : LiveItem
    {
        if (target != null)
        {
            var damage = melee;

            Damage damageInfo = new Damage(damage, this, target, DamageType.Physical);

            yield return this.ExStartCoroutine
                (target.MeleeAttackByOther(this, damageInfo));
        }
    }

    public virtual IEnumerator MeleeAttackByOther<T>(T other, Damage damageInfo) where T : LiveItem
    {
        LeanTween.scale(transform.Rt(), new Vector3(0.9f, 0.9f, 0.9f), 0.1f).setLoopPingPong(3);

        yield return waitForSeconds;

        TakeDamage(damageInfo);
    }

    public virtual float TakeDamage(Damage damageInfo)
    {
        foreach (var state in state_list)
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
            AddStateIns(ins);
        }

        if (cur_hp == 0)
        {
           StartCoroutine(OnDead(damageInfo));
        }
        else
        {

            Messenger<Damage>.Invoke(SA.ItemTakeDamage, damageInfo);
        }

        return damageInfo.damage;
    }

    public virtual void AddStateIns(StateIns ins)
    {
        int max = ins.stateConfig.max;

        state_list.Add(ins);
        ins.ActiveIns();

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
    }

    public virtual void RemoveStateIns(StateIns ins)
    {
        ins.DeactiveIns();

        ins.stateEffects = null;

        for (int i = state_list.Count - 1; i >= 0; ++i)
        {
            if (state_list[i].stateConfig.id == ins.stateConfig.id)
            {
                state_list.RemoveAt(i);
            }
        }
    }

    public void RemoveStateBuff(int count, bool isBuff)
    {

    }
}


