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
    /// 怪物类型
    /// </summary>
    public MonsterType monsterType { get; set; }
    /// <summary>
    /// 伤害类型
    /// </summary>
    public DamageType damageType { get; set; }
    /// <summary>
    /// 过时属性
    /// </summary>
    public float atk_coefficient { get; set; }
    /// <summary>
    /// 过时属性
    /// </summary>
    public float hp_coefficient { get; set; }
    /// <summary>
    /// 属性系数：第一维长度为4，表示4种强度的怪物，第二维是每个属性缩放值
    /// </summary>
    public SuperArray<float> propertys { get; set; }
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
    /// 攻击，值
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
    /// 伤害
    /// </summary>
    public SuperArray<string> damage { get; set; }
    /// <summary>
    /// 属性改变
    /// </summary>
    public SuperArray<string> pro_change { get; set; }
    /// <summary>
    /// 特殊效果
    /// </summary>
    public SpecialEffect specialEffect { get; set; }
    /// <summary>
    /// 效果参数：根据不同效果参数的意义不同
    /// </summary>
    public SuperArray<string> specialEffectArgs { get; set; }
    /// <summary>
    /// 消耗时间，如果是怪物的技能，就表示CD
    /// </summary>
    public float costTime { get; set; }
    /// <summary>
    /// 能耗
    /// </summary>
    public float usePower { get; set; }
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
    /// 召唤物属性表达式，如果没有相应的属性表达，默认为0
    /// </summary>
    public SuperArray<string> propertys { get; set; }
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
public class SkillPointsConfig : ConfigDataBase
{
    /// <summary>
    /// 激活技能编号，这是一个一维数组，对应该技能点开启\升级的技能ID
    /// </summary>
    public SuperArray<ulong> skillIds { get; set; }
    /// <summary>
    /// 角色需求技能点：要开启/升级该技能需要的技能点数量。该数组的长度，不大于skillids的长度，如果该长度小于skillIds，表示该角色将不能学会该技能的最高等级。注意，该值可以为负。
    /// </summary>
    public SuperArray<int> characterActivate { get; set; }
#region 自定义区
#endregion
}
public class PassiveSkillsConfig : ConfigDataBase
{
    /// <summary>
    /// 技能名称
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string describe { get; set; }
    /// <summary>
    /// 作用类型
    /// </summary>
    public SuperArray<PassiveType> passiveType { get; set; }
    /// <summary>
    /// 参数，根据作用类型解释成不同的作用
    /// </summary>
    public SuperArray<string> args { get; set; }
#region 自定义区
#endregion
}
public class StateConfig : ConfigDataBase
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
    /// Buff/Debuff:true为buff
    /// </summary>
    public bool isBuff { get; set; }
    /// <summary>
    /// 最大叠加
    /// </summary>
    public int max { get; set; }
    /// <summary>
    /// 持续时间
    /// </summary>
    public int time { get; set; }
    /// <summary>
    /// 标记：美术表现
    /// </summary>
    public string mark { get; set; }
    /// <summary>
    /// 状态优先级
    /// </summary>
    public int priority { get; set; }
    /// <summary>
    /// 效果：一个状态可以有多个效果
    /// </summary>
    public SuperArray<StateEffect> stateEffects { get; set; }
    /// <summary>
    /// 参数：这是一个二维数组，第一维的长度等于效果（StateEffect）的长度，第二维会根据不同效果的不同而不同
    /// </summary>
    public SuperArray<string> args { get; set; }
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
public class MapConfig : ConfigDataBase
{
    /// <summary>
    /// 行走距离：玩家行走小于等级该距离时，使用该条数据，除非这是所有配置中，距离最大的数据
    /// </summary>
    public int distance { get; set; }
    /// <summary>
    /// 过时
    /// </summary>
    public string res_emptys { get; set; }
    /// <summary>
    /// 过时
    /// </summary>
    public string res_blocks { get; set; }
    /// <summary>
    /// 空格资源
    /// </summary>
    public SuperArray<string> emptyList { get; set; }
    /// <summary>
    /// 阻挡资源
    /// </summary>
    public SuperArray<string> obstructList { get; set; }
    /// <summary>
    /// 地图模块
    /// </summary>
    public SuperArray<ulong> map_models { get; set; }
    /// <summary>
    /// 过时
    /// </summary>
    public SuperArray<ulong> exit_models { get; set; }
    /// <summary>
    /// 敌人列表
    /// </summary>
    public SuperArray<ulong> enemys { get; set; }
    /// <summary>
    /// 过时
    /// </summary>
    public SuperArray<ulong> bosses { get; set; }
    /// <summary>
    /// 敌人等级
    /// </summary>
    public SuperArray<int> enemy_level { get; set; }
    /// <summary>
    /// 物品列表
    /// </summary>
    public SuperArray<ulong> items { get; set; }
#region 自定义区
#endregion
}
public class ModuleConfig : ConfigDataBase
{
    /// <summary>
    /// 地图块内容
    /// </summary>
    public SuperArray<string> contents { get; set; }

#region 自定义区
#endregion
}
