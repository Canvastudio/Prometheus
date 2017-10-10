using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LiveItem : GameItemBase
{
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

    /// <summary>
    /// 基础属性，血量，速度等
    /// </summary>
    public LiveBaseProperty baseProperty = new LiveBaseProperty();

    public void AddHpPercent(float percent)
    {
        var max_Hp = baseProperty.GetFloatProperty(PDName.maxHp);
        var cur_Hp = baseProperty.GetFloatProperty(PDName.curHp);

        cur_Hp += max_Hp * percent;
        cur_Hp = Mathf.Min(max_Hp, cur_Hp);

        baseProperty.SetFloatProperty(PDName.curHp, cur_Hp);
    }

    public virtual void OnDead()
    {

    }

    public abstract IEnumerator AttackTarget<T>(T target) where T : LiveItem;


    public abstract IEnumerator AttackByOther<T>(T other) where T : LiveItem;

}

/// <summary>
/// 怪物和玩家数据名字缓存
/// </summary>
public class PDName
{
    public const string maxHp = "mhp";
    public const string curHp = "nhp";
    public const string speed = "speed";
}
