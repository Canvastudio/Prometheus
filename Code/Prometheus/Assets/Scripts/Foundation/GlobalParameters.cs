using System.Collections.Generic;
using UnityEngine;

public class GlobalParameters
{
    private static GlobalParameters _instance;
    public static GlobalParameters Instance
    {
        get { return _instance ?? (_instance = new GlobalParameters()); }
    }

    public Color Color_Normal;
    public Color Color_Rare;
    public Color Color_Elite;
    public Color Color_Boss;


    private static Dictionary<string, object> _globalDic;
    public static Dictionary<string, object> GlobalDic
    {
        set { _globalDic = value; }
        get { return _globalDic ?? (_globalDic = new Dictionary<string, object>()); }
    }

    private GlobalParameters()
    {
        Color_Normal = SuperTool.CreateColor("818181FF");
        Color_Rare = SuperTool.CreateColor("008BFFFF");
        Color_Elite = SuperTool.CreateColor("E5CC00FF");
        Color_Boss = SuperTool.CreateColor("AB4BFFFF");
    }


}

public enum DamageType
{
    Physical,
}

public struct Attack
{
    public Attack(float damage, DamageType damageType)
    {
        this.damage = damage;
        this.damageType = damageType;
    }
    public float damage;
    public DamageType damageType;
}

public enum WaitType
{
    Skill,
}

/// <summary>
/// 控制协程停止模式
/// </summary>
public enum CoroutineStopMode
{
    /// <summary>
    /// 即时没有停止的目标，也可以正常运行
    /// </summary>
    Normal,
    /// <summary>
    /// 没有停止的目标，会发出警告
    /// </summary>
    Warning,
    /// <summary>
    /// 没有停止的目标，会出错
    /// </summary>
    Error,
}



