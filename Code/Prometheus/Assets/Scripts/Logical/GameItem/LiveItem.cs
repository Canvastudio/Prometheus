using System;
using System.Collections;
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
            if (!_cur_hp.HasValue) _cur_hp = property.GetFloatProperty(GameProperty.nhp);

            return _cur_hp.Value;
        }
        set
        {
            if (value <= 0)
            {
                value = 0;

                OnDead();
            }
      
            property.SetFloatProperty(GameProperty.nhp, value);

            _cur_hp = value;

            if (hp_value != null)
            {
                hp_value.text = value.ToString();
            }
        }
    }

    [SerializeField]
    private float? _melee;
    public float melee
    {
        get
        {
            if (!_melee.HasValue) _melee = property.GetFloatProperty(GameProperty.melee);

            return _melee.Value;
        }
        set
        {
            property.SetFloatProperty(GameProperty.melee, value);

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
    public LiveBasePropertys property = new LiveBasePropertys();

    public void AddHpPercent(float percent)
    {
        var max_Hp = property.GetFloatProperty(GameProperty.mhp);
        var cur_Hp = property.GetFloatProperty(GameProperty.nhp);

        cur_Hp += max_Hp * percent;
        cur_Hp = Mathf.Min(max_Hp, cur_Hp);

        property.SetFloatProperty(GameProperty.nhp, cur_Hp);
    }

    public virtual void OnDead()
    {
        standBrick.CleanItem();

        GameObject.Destroy(gameObject);
    }

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.8f);

    public virtual IEnumerator MeleeAttackTarget<T>(T target) where T : LiveItem
    {
        if (target != null)
        {
            var damage = melee;

            yield return StartCoroutine(target.MeleeAttackByOther(this, damage));
        }
    }

    public virtual IEnumerator MeleeAttackByOther<T>(T other, float damage) where T : LiveItem
    {
        LeanTween.scale(transform.Rt(), new Vector3(0.9f, 0.9f, 0.9f), 0.1f).setLoopPingPong(3);

        yield return waitForSeconds;

        cur_hp = cur_hp - damage;
    }

}


