using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class LiveItem : GameItemBase
{
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
            if (!_cur_hp.HasValue) _cur_hp = baseProperty.GetFloatProperty(GameProperty.nhp);

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

                baseProperty.SetFloatProperty(GameProperty.nhp, value);

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
                _fmax_hp = baseProperty.GetFloatProperty(GameProperty.mhp) * (1 + baseProperty.GetFloatProperty(GameProperty.mhp_percent));

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
                _max_hp = baseProperty.GetFloatProperty(GameProperty.mhp);

            return _max_hp.Value;
        }
        set
        {
            if (value <= 0)
            {
                Debug.LogError("max hp 的值可以小于等于0？");
            }

            baseProperty.SetFloatProperty(GameProperty.mhp, value);

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
            if (!_melee.HasValue) _melee = baseProperty.GetFloatProperty(GameProperty.melee);

            return _melee.Value;
        }
        set
        {
            baseProperty.SetFloatProperty(GameProperty.melee, value);

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
    public LiveBasePropertys baseProperty = new LiveBasePropertys();

    /// <summary>
    /// 得到最终的属性，会算上被动和芯片的加成
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    public float GetFinalProperty(GameProperty property)
    {
        return 0;
    }

    public void SetProperty(GameProperty property, float value)
    {

    }

    public void AddHpPercent(float percent)
    {
        cur_hp += fmax_hp * percent;
        cur_hp = Mathf.Min(fmax_hp, cur_hp);
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

            yield return this.ExStartCoroutine
                (target.MeleeAttackByOther(this, damage));
        }
    }

    public virtual IEnumerator MeleeAttackByOther<T>(T other, float damage) where T : LiveItem
    {
        LeanTween.scale(transform.Rt(), new Vector3(0.9f, 0.9f, 0.9f), 0.1f).setLoopPingPong(3);

        yield return waitForSeconds;

        yield return TakeDamage(damage);
    }

    public virtual IEnumerator TakeDamage(float damage)
    {
        cur_hp = cur_hp - damage;

        if (cur_hp == 0)
        {
            yield return OnDead();
        }
    }
}


