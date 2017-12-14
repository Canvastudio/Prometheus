using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GContext : SingleGameObject<GContext> {

    [SerializeField]
    public int enemy_count
    {
        get
        {
            return discover_monster - enslave_count;
        }
    }
    [SerializeField]
    private int _discover_monster = 0;
    public int discover_monster
    {
        get { return _discover_monster; }
        set
        {
            if (_discover_monster != value)
            {
                _discover_monster = value;
                Messenger.Invoke(SA.DiscoverMonsterChange);
                Messenger.Invoke(SA.EnmeyCountChange);
            }
        }
    }
    [SerializeField]
    private int _enslave = 0;
    public int enslave_count
    {
        get { return _enslave; }
        set
        {
            if (_enslave != value)
            {
                Messenger.Invoke(SA.EnmeyCountChange);
            }
        }
    }
    [SerializeField]
    private int _discover_brick = 0;
    public int discover_brick
    {
        get { return _discover_brick; }
        set
        {
            if (_discover_brick != value)
            {
                if (_discover_brick > value)
                {
                    Messenger.Invoke(SA.OpenBrick);
                }

                _discover_brick = value;
                Messenger.Invoke(SA.DiscoverBrickChange);
            }
        }
    }
    [SerializeField]
    private int _dark_brick = 0;
    public int dark_brick
    {
        get { return _dark_brick; }
        set
        {
            if (_dark_brick != value)
            {
                Messenger.Invoke(SA.DarkBrickChange);
            }
        }
    }




    /// <summary>
    /// 最后一个翻开的怪物
    /// </summary>
    [SerializeField]
    private Monster _lastDiscoverMonster = null;
    public Monster lastDiscoverMonster
    {
        get { return _lastDiscoverMonster; }
        set
        {
            if (value != null)
            {
                JustdiscoverMonster = true;
            }

            _lastDiscoverMonster = value;
        }
    }

    public bool JustdiscoverMonster = false;
}
