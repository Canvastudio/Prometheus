using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class LiveItem : GameItemBase
{
    /// <summary>
    /// 状态Buff
    /// </summary>
    private List<StateConfig> buff_list = new List<StateConfig>();

    /// <summary>
    /// 受伤结算得状态
    /// </summary>
    private List<StateIns> defend_buff = new List<StateIns>();

    /// <summary>
    /// 状态deBuff
    /// </summary>
    private List<StateConfig> debuff_list = new List<StateConfig>();

    /// <summary>
    /// 光环给予的buff
    /// </summary>
    private List<StateConfig> halo_list = new List<StateConfig>();

    private bool silent = false;


    public bool Silent
    {
        get
        {
            return silent;
        }

        set
        {
            silent = value;
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

    /// <summary>
    /// 是否被玩家奴役
    /// </summary>
    [SerializeField]
    private bool _enslave;
    public bool enslave
    {
        get
        {
            return _enslave;
        }
        set
        {
            StageCore.Instance.tagMgr.SetEntityTag(this, ETag.Tag(ST.FRIEND), value);
            _enslave = value;
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

    public virtual IEnumerator OnDead()
    {
        standBrick.CleanItem();

        StageCore.Instance.UnRegisterItem(this);

        return null;

        //GameObject.Destroy(gameObject);
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

        yield return TakeDamage(damageInfo);
    }

    public virtual IEnumerator TakeDamage(Damage damageInfo)
    {
        cur_hp = cur_hp - damageInfo.damage;

        if (cur_hp == 0)
        {
            yield return OnDead();
        }
    }

    public void AddStateBuff(StateConfig config, bool passive = false)
    {
        var effects = config.stateEffects.ToArray();

        for (int i = 0; i < effects.Length; ++i)
        {
            switch (effects[i])
            {
                case StateEffect.DamageAbsorb:
                    DamageAbsorb absorb = new DamageAbsorb(this, config, i, passive);
                    defend_buff.Add(absorb);
                    break;
            }
        }
    }

    public void RemoveStateBuff(StateConfig config)
    {

    }

    public void RemoveStateBuff(int count, bool isBuff)
    {

    }

    public void AddHaloBuff(StateConfig config)
    {

    }

    public void RemoveHaloBuff(StateConfig config)
    {

    }

    public void RemoveDefendState(StateIns defendState)
    {
        defend_buff.Remove(defendState);
    }
}


