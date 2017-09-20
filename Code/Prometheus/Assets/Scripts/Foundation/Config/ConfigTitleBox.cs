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
    /// <summary>
    /// 过时
    /// </summary>
    public SuperArray<string> content { get; set; }
#region 自定义区
     public string GetBrickInfo(int row, int column)
    {
        return contents[row, column];
    }
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
    /// 敌人类型，以后可能会扩展
    /// </summary>
    public MonsterType monsterType { get; set; }
    /// <summary>
    /// 过时：这个字段本来是用来表示怪物近战攻击类型，但近战一律为物理，所以它就没有存在必要了
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
    /// 属性系数：第一维长度为4，表示4种强度的怪物；第二维是每个属性缩放值，顺序是m_mhp，m_speed，m_melee，m_laser，m_cartridge
    /// </summary>
    public SuperArray<float> propertys { get; set; }
    /// <summary>
    /// 普通怪技能，第一维度的长度等于该怪物有几个技能，第二维两个元素，第一个元素描述技能ID，第二个描述所需要的参数。格式如下：[技能ID1_技能类型:冷却_初始|技能ID2_技能类型:冷却_初始]
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
    /// 怪物AI：怪物的一些常规行为，关联AIConfig
    /// </summary>
    public ulong ai { get; set; }
#region 自定义区
#endregion
}
public class MonsterLevelDataConfig : ConfigDataBase
{
    /// <summary>
    /// 过时
    /// </summary>
    public float m_pat { get; set; }
    /// <summary>
    /// 过时
    /// </summary>
    public float m_mat { get; set; }
    /// <summary>
    /// 生命值
    /// </summary>
    public float m_mhp { get; set; }
    /// <summary>
    /// 速度
    /// </summary>
    public float m_speed { get; set; }
    /// <summary>
    /// 近战攻击力
    /// </summary>
    public float m_melee { get; set; }
    /// <summary>
    /// 光线攻击力
    /// </summary>
    public float m_laser { get; set; }
    /// <summary>
    /// 实弹攻击力
    /// </summary>
    public float m_cartridge { get; set; }
#region 自定义区
#endregion
}
public class AIConfig : ConfigDataBase
{
    /// <summary>
    /// 警戒范围：怪物并不一定非要等到玩家踩脸才会翻开，玩家与怪物的距离小于等级该值时，怪物就会翻开。为0时候，就需要玩家踩脸才会翻开。注意翻开后的怪物，是否要锁定周围的格子，要符合“邻接锁定原则”。邻接锁定原则：如果是远处翻开，当该怪物周围均是未翻开的格子时，不会立即锁定周围的格子，当玩家接触（邻接）该怪物（翻开了怪物周围的格子）之后，锁定剩余的格子
    /// </summary>
    public int warning { get; set; }
    /// <summary>
    /// 声音：两个元素。怪物翻开，死亡时，可以发出声音惊醒其他改为，这里两个值对应翻开与死亡时声音的距离
    /// </summary>
    public SuperArray<int> noise { get; set; }
    /// <summary>
    /// 危险级别：怪物并不是在翻开后，就一定会对玩家进行远程攻击。
    /// </summary>
    public DangerousLevels dangerous_levels { get; set; }
    /// <summary>
    /// 即时技能：怪物在翻开/死亡时，允许释放一次主动技能（再死去），无视CD强制释放，当然我不会做一个在死亡时给自己加血或者无敌的技能（其实就算是释放了无敌，但因为之前已经触发了死亡，通常是生命值为0，所以它还是会死去）…两个元素分别对应翻开与死亡释放时释放技能的ID，id为0表示没有技能，为空表示啥技能都没有
    /// </summary>
    public SuperArray<ulong> forceSkills { get; set; }
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
    /// 决定技能的释放方式。注意Self和Target会根据不同的技能使用者，意义会不同
    /// </summary>
    public TargetType targetType { get; set; }
    /// <summary>
    /// 这是一个过滤列表，符合这个条件的怪物，将不能被该技能列为可选目标。这里没有直接把怪物拿过来用，因为限制条件可能比怪物类型更复杂，比如生命值低于100的生物型怪物，这时候就可能要多加一种类型
    /// </summary>
    public SuperArray<TargetLimit> targetLimit { get; set; }
    /// <summary>
    /// 多目标，laser不支持同时攻击多个目标
    /// </summary>
    public MultipleType multipleType { get; set; }
    /// <summary>
    /// 目标参数(第一次分割代表目标个数，仅随机有效，第二次分割代表攻击次数与每次伤害权重)
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
    /// 伤害表达式
    /// </summary>
    public SuperArray<string> damage { get; set; }
    /// <summary>
    /// 特效成功率：4个元素。“属性改变”与“特殊效果”都属于特效，它针对不同强度的怪物会有不同的成功率。如果技能目标是玩家，那么使用第一个元素
    /// </summary>
    public SuperArray<float> successRate { get; set; }
    /// <summary>
    /// 属性改变：这是一个四则表达式数组，请依次计算每个表达式。该过程不可逆，所以一般不会修改玩家属性（除当前生命值）。为避免属性混淆，一般有此特性的技能，targetType不会是Self和Target
    /// </summary>
    public SuperArray<string> propertyChange { get; set; }
    /// <summary>
    /// 主动技能的特殊效果
    /// </summary>
    public SuperArray<SpecialEffect> specialEffect { get; set; }
    /// <summary>
    /// 效果参数：根据不同效果参数的意义不同。它的维度应该和specialEffect相同
    /// </summary>
    public SuperArray<string> specialEffectArgs { get; set; }
    /// <summary>
    /// 消耗时间，如果是怪物或者召唤物（NormalAttack）的技能，就表示CD
    /// </summary>
    public float costTime { get; set; }
    /// <summary>
    /// 能耗：CopyAttack召唤物才会用到
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
    /// 决定技能的释放方式。注意Self和Target会根据不同的技能使用者，意义会不同
    /// </summary>
    public TargetType targetType { get; set; }
    /// <summary>
    /// 召唤距离：这是放置位置的距离，而不是攻击距离
    /// </summary>
    public SuperArray<int> carry { get; set; }
    /// <summary>
    /// 预制，浮游炮的外形
    /// </summary>
    public string prefab { get; set; }
    /// <summary>
    /// 消耗时间
    /// </summary>
    public float costTime { get; set; }
    /// <summary>
    /// 召唤物的行为模式
    /// </summary>
    public SpecialAction specialAction { get; set; }
    /// <summary>
    /// 行为参数：其不同行为，对应行为参数解释为不同内容。NormalAttack------主动攻击编号；CopyAttack------能量上限;恢复速度(几回合恢复至上限)
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
    /// 被动技能作用效果。注意，该效果优先级是低于AI的，比如对怪物来说，死亡时可能翻开附近的怪物，而又有附近怪物AddState。由于AI优先级更高，所以会先执行翻开的操作。
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
    /// 状态优先级：值越大，优先级越高
    /// </summary>
    public int priority { get; set; }
    /// <summary>
    /// 状态的效果，一个状态可以有多个效果同时起效（依次起效）
    /// </summary>
    public SuperArray<StateEffect> stateEffects { get; set; }
    /// <summary>
    /// 参数：stateEffects的参数，其维度应该和stateEffects一样
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
    /// 升级后对应下一级芯片ID，如果为0，就表示不能升级了
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
    /// 过时
    /// </summary>
    public ulong bindSkill { get; set; }
    /// <summary>
    /// 获得技能点：并不是所有芯片都会有技能点，多少芯片只加属性
    /// </summary>
    public ulong skillPoint { get; set; }
    /// <summary>
    /// 属性加成：并不是所有芯片都有属性加成，它可以只提供技能点。注意这不是属性表达式，仅仅是一个表示某个属性增加多少的形式（因为这里不能用“独立增伤”的属性加成方式），可以为负
    /// </summary>
    public SuperArray<string> propertyAddition { get; set; }
    /// <summary>
    /// 形状描述
    /// </summary>
    public SuperArray<int> model { get; set; }
#region 自定义区
#endregion
}
