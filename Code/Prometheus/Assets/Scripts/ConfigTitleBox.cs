	
	
public class MapConfig : ConfigDataBase
{
    /// <summary>
    /// 空格资源
    /// </summary>
    public string res_emptys { get; set; }
    /// <summary>
    /// 阻挡资源
    /// </summary>
    public string res_blocks { get; set; }
    /// <summary>
    /// 地图模块
    /// </summary>
    public SuperArray<ulong> map_models { get; set; }
    /// <summary>
    /// 出口模块
    /// </summary>
    public SuperArray<ulong> exit_models { get; set; }
    /// <summary>
    /// 敌人列表
    /// </summary>
    public SuperArray<ulong> enemys { get; set; }
    /// <summary>
    /// BOSS列表
    /// </summary>
    public SuperArray<ulong> bosses { get; set; }
    /// <summary>
    /// 物品列表
    /// </summary>
    public SuperArray<ulong> items { get; set; }
    /// <summary>
    /// 敌人等级
    /// </summary>
    public SuperArray<int> enemy_level { get; set; }
#region 自定义区
    #endregion
}
public class ModuleConfig : ConfigDataBase
{
    /// <summary>
    /// 地图块内容
    /// </summary>
    public SuperArray<string> content { get; set; }
#region 自定义区
    #endregion
}
public class MonsterConfig : ConfigDataBase
{
    /// <summary>
    /// prefab资源
    /// </summary>
    public string res_face { get; set; }
    /// <summary>
    /// 名字
    /// </summary>
    public string m_name { get; set; }
    /// <summary>
    /// 伤害类型
    /// </summary>
    public DamageType damageType { get; set; }
    /// <summary>
    /// 攻击系数
    /// </summary>
    public float atk_coefficient { get; set; }
    /// <summary>
    /// 生命系数
    /// </summary>
    public float hp_coefficient { get; set; }
    /// <summary>
    /// 普通怪技能
    /// </summary>
    public SuperArray<string> skill_normal { get; set; }
    /// <summary>
    /// 稀有怪技能
    /// </summary>
    public SuperArray<string> skill_rare { get; set; }
    /// <summary>
    /// 精英怪技能
    /// </summary>
    public SuperArray<string> skill_elite { get; set; }
    /// <summary>
    /// BOSS怪技能
    /// </summary>
    public SuperArray<string> skill_boss { get; set; }
    /// <summary>
    /// 怪物AI
    /// </summary>
    public ulong ai { get; set; }
#region 自定义区
    #endregion
}
public class MonsterLevelDataConfig : ConfigDataBase
{
    /// <summary>
    /// 物理攻击，值
    /// </summary>
    public float m_pat { get; set; }
    /// <summary>
    /// 魔法攻击，值
    /// </summary>
    public float m_mat { get; set; }
    /// <summary>
    /// 生命值，值
    /// </summary>
    public float m_mhp { get; set; }
#region 自定义区
    #endregion
}
public class AIConfig : ConfigDataBase
{
    /// <summary>
    /// 警戒范围
    /// </summary>
    public int warning { get; set; }
    /// <summary>
    /// 声音，翻开|死亡
    /// </summary>
    public SuperArray<int> noise { get; set; }
    /// <summary>
    /// 危险等级
    /// </summary>
    public DangerousLevels dangerous_levels { get; set; }
#region 自定义区
#endregion
}
public class SummonSkillsConfig : ConfigDataBase
{
    /// <summary>
    /// 名称
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string describe { get; set; }
    /// <summary>
    /// 目标类型(释放方式)
    /// </summary>
    public TargetType targetType { get; set; }
    /// <summary>
    /// 射程
    /// </summary>
    public SuperArray<int> carry { get; set; }
    /// <summary>
    /// 属性
    /// </summary>
    public SuperArray<float> property { get; set; }
    /// <summary>
    /// 预制
    /// </summary>
    public string prefab { get; set; }
    /// <summary>
    /// 消耗时间，如果是怪物的技能，就表示CD
    /// </summary>
    public float costTime { get; set; }
    /// <summary>
    /// 行为模式（复制攻击、反击等）
    /// </summary>
    public SpecialAction specialAction { get; set; }
    /// <summary>
    /// 行为参数
    /// </summary>
    public SuperArray<string> speciaArg { get; set; }
#region 自定义区
    #endregion
}
public class ChipConfig : ConfigDataBase
{
    /// <summary>
    /// 名称
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string descrip { get; set; }
    /// <summary>
    /// 颜色
    /// </summary>
    public string color { get; set; }
    /// <summary>
    /// 等级
    /// </summary>
    public int level { get; set; }
    /// <summary>
    /// 升级对应ID
    /// </summary>
    public ulong upgradeId { get; set; }
    /// <summary>
    /// 能耗下限
    /// </summary>
    public int costDownwards { get; set; }
    /// <summary>
    /// 能耗上限
    /// </summary>
    public int costUpwards { get; set; }
    /// <summary>
    /// 绑定技能
    /// </summary>
    public ulong bindSkill { get; set; }
    /// <summary>
    /// 属性加成
    /// </summary>
    public SuperArray<string> propertyAddition { get; set; }
    /// <summary>
    /// 形状描述
    /// </summary>
    public SuperArray<int> model { get; set; }
#region 自定义区
#endregion
}
public class ActiveSkillsConfig : ConfigDataBase
{
    /// <summary>
    /// 名称
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string describe { get; set; }
    /// <summary>
    /// 目标类型(释放方式)
    /// </summary>
    public TargetType targetType { get; set; }
    /// <summary>
    /// 目标限制（比如某些怪物不能锁定，这是一个一维数组，表示多个限制条件）
    /// </summary>
    public SuperArray<TargetLimit> targetLimit { get; set; }
    /// <summary>
    /// 多目标，laser不支持同时攻击多个目标
    /// </summary>
    public MultipleType multipleType { get; set; }
    /// <summary>
    /// 目标参数(第一次分割代表目标个数，第二次分割代表攻击次数与每次伤害权重)
    /// </summary>
    public SuperArray<float> targetArg { get; set; }
    /// <summary>
    /// 飞行特效，laser不支持多飞行特效
    /// </summary>
    public SuperArray<string> effect_fly { get; set; }
    /// <summary>
    /// 命中特效
    /// </summary>
    public SuperArray<string> effect_hit { get; set; }
    /// <summary>
    /// 射程
    /// </summary>
    public SuperArray<int> carry { get; set; }
    /// <summary>
    /// 给自己附加状态
    /// </summary>
    public SuperArray<ulong> state_to_player { get; set; }
    /// <summary>
    /// 给目标附加状态
    /// </summary>
    public SuperArray<ulong> state_to_monster { get; set; }
    /// <summary>
    /// 直接伤害
    /// </summary>
    public string damage{
        get { return _damage; }
        set { _damage = SuperTool.ToRpn(value); }
    }
    /// <summary>
    /// 属性改变
    /// </summary>
    public SuperArray<string> pro_change{ private get; set; }
    /// <summary>
    /// 消耗时间，如果是怪物的技能，就表示CD
    /// </summary>
    public float costTime { get; set; }
    /// <summary>
    /// 能耗
    /// </summary>
    public float usePower { get; set; }

#region 自定义区
    public string _damage;
    private string[] _pro_change_to_rpn;
    public string[] Pro_change_to_rpn
    {
        get
        {
            if (pro_change != null && _pro_change_to_rpn == null)
            {
                string[] t = pro_change.ToArray();
                for (int i = 0; i < t.Length; i++) t[i] = SuperTool.ToRpn(t[i]);
                _pro_change_to_rpn = t;
            }
            return _pro_change_to_rpn;
        }
    }
    #endregion
}
