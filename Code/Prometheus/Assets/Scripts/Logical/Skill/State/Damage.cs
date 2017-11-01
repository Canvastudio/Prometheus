﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage  {

    public float damage;
    public LiveItem damageSource;
    public LiveItem damageTarget;
    public bool isTransfer;
    public bool isRebound;
    public DamageType damageType;

    public Damage(float _damage, LiveItem source, LiveItem target, DamageType _type, bool _isTransfer = false, bool _isRebound = false)
    {
        damage = _damage;
        damageSource = source;
        damageTarget = target;
        isTransfer = _isTransfer;
        isRebound = _isRebound;
    }
}