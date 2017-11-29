using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage  {

    public float damage;
    public LiveItem damageSource;
    public LiveItem damageTarget;
    public bool isTransfer;
    public bool isRebound;
    public DamageType damageType;
    public ulong attach_state;

    public Damage(float _damage, LiveItem source, LiveItem target, DamageType _type, bool _isTransfer = false, bool _isRebound = false, ulong _attach_state = 0)
    {
        damage = _damage;
        damageSource = source;
        damageTarget = target;
        isTransfer = _isTransfer;
        isRebound = _isRebound;
        attach_state = _attach_state;
        damageType = _type;
    }
}
