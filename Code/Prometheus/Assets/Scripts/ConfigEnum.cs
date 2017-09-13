//本文档是自动构建的内容，请勿手动输入与修改！

/// <summary>
/// 多目标类型
/// </summary>
public enum MultipleType
{
    /// <summary>
    /// 默认，只会攻击一个目标
    /// </summary>
    Normal,
    /// <summary>
    /// 随机目标，这个可以结合targetArg实现随机选取多个目标
    /// </summary>
    Random,
    /// <summary>
    /// 所有目标
    /// </summary>
    All,
    /// <summary>
    /// 扩散模式，1表示扩散半径为1。它会对半径内的其他目标同时造成伤害
    /// </summary>
    Diffuse_1,
    Diffuse_2,
    Diffuse_3,
    Diffuse_4,
    Diffuse_5,
    Diffuse_6,
}

/// <summary>
/// 决定技能的释放方式
/// </summary>
public enum TargetType
{
    /// <summary>
    /// 玩家对自己释放，不需要指定目标
    /// </summary>
    Player,
    /// <summary>
    /// 选择一个射程内的可见怪物
    /// </summary>
    Monster,
    /// <summary>
    /// 选择一个格子。原则上包含所有的格子，但应该把玩家所在位置的格子排除
    /// </summary>
    Block,
    /// <summary>
    /// 选择一个翻开且不含障碍的格子，排除玩家所在的格子
    /// </summary>
    LightBlock,
    /// <summary>
    /// 选择一个未翻开且不包含障碍的格子（目前这里有BUG）
    /// </summary>
    DarkBlock,
    /// <summary>
    /// 选择一个翻开且不含障碍，格子上没有物体，排除玩家所在的格子
    /// </summary>
    EmptyBlock,
    /// <summary>
    /// 选择一个障碍（障碍不存在翻开与未翻开的概念）
    /// </summary>
    ObstructBlock,
    /// <summary>
    /// 不需要选择目标，表示这是一次全屏攻击
    /// </summary>
    Aoe,
    /// <summary>
    /// 选择一个障碍，而且该位置没有炮台
    /// </summary>
    炮台位置,
    /// <summary>
    /// 同上一条，但它不需要玩家选择位置
    /// </summary>
    全炮台位置,
    /// <summary>
    /// 不需要选目标，表示召唤浮游炮
    /// </summary>
    卫星,
    /// <summary>
    /// 尚未被翻开的怪物，不需要玩家选择目标，它通常与multipleType为random时配合使用，表示随机一个没有被发现的怪物
    /// </summary>
    HideMonster,
}

/// <summary>
/// 召唤物的行为模式
/// </summary>
public enum SpecialAction
{
    /// <summary>
    /// 普通攻击
    /// </summary>
    NormalAttack,
    /// <summary>
    /// 复制玩家的主动技能
    /// </summary>
    CopyAttack,
}

/// <summary>
/// 这是一个过滤列表，符合这个条件的怪物，将不能被该技能列为可选目标。这里没有直接把怪物拿过来用，因为限制条件可能比怪物类型更复杂，比如生命值低于100的生物型怪物，这时候就可能要多加一种类型
/// </summary>
public enum TargetLimit
{
    /// <summary>
    /// 枚举默认值，表示没有任何过滤
    /// </summary>
    None,
    /// <summary>
    /// 怪物强度1
    /// </summary>
    MonsterQuality_1,
    MonsterQuality_2,
    MonsterQuality_3,
    MonsterQuality_4,
    /// <summary>
    /// 钢铁类型的怪物？
    /// </summary>
    MonsterType_iron,
    /// <summary>
    /// 生物型怪物？？
    /// </summary>
    MonsterType_organisms,
}

/// <summary>
/// 被动技能作用类型
/// </summary>
public enum PassiveType 
{
    /// <summary>
    /// 增加/减少属性
    /// </summary>
    Property,
    /// <summary>
    /// 全伤害减免，该类被动技能可以被负激活
    /// </summary>
    DamageReduction ,
}

public enum StateEffect
{
    /// <summary>
    /// 临时改变属性，状态消失后，属性要变回来
    /// </summary>
    Property,
    /// <summary>
    /// 沉默，不能释放技能，但可以近战攻击。没有参数
    /// </summary>
    Silent,
    /// <summary>
    /// 伤害转移，受到N次伤害，随机转移给其他怪物（DOT不算），如果是怪物有此状态，则转移给玩家。参数是：次数。如果达到次数，状态自动消失
    /// </summary>
    DamageTransfer,
    /// <summary>
    /// 伤害吸收，宿主接下来的伤害会被吸收（宿主不受伤害），参数：吸收表达式（比如HP*5）。吸收足够的伤害后，状态消失
    /// </summary>
    DamageAbsorb,
    /// <summary>
    /// 伤害储存，记下下N次受到的伤害，在状态结束时，为自己回复M倍的生命值。注意，宿主还是会收到伤害。参数：N，M
    /// </summary>
    DamageStore ,
    /// <summary>
    /// 冻结，冻结后不能反击，不能使用技能，仅怪物可以被释放。没有参数
    /// </summary>
    Freeze,
    /// <summary>
    /// 除非是场上最后一个怪物，否则不能攻击（远程&近战），怪物only。没有参数
    /// </summary>
    Last,
    /// <summary>
    /// 免疫指向技能，即有此状态的怪物，不能在玩家手动选择目标时，被筛选出来（随机目标是可以的）。怪物only，没有参数
    /// </summary>
    Imm,
    /// <summary>
    /// 爆炸，对于玩家造成伤害。参数是伤害表达式
    /// </summary>
    Explode,
}

/// <summary>
/// 伤害类型（请把之前同名枚举类型删除，在GlobalParameters.cs）
/// </summary>
public enum DamageType
{
    /// <summary>
    /// 物理伤害
    /// </summary>
    Physical,
    /// <summary>
    /// 实弹伤害
    /// </summary>
    Cartridge,
    /// <summary>
    /// 光线伤害
    /// </summary>
    Laser,
}

/// <summary>
/// 主动技能的特殊效果
/// </summary>
public enum SpecialEffect
{
    /// <summary>
    /// 变形，将目标的样子变成另外一个prefab，通常它会配合属性改变一起使用。参数是变成目标prefab的名称
    /// </summary>
    Transfiguration,
    /// <summary>
    /// 玩家与目标怪物位置互换，互换后，怪物暂时不会封锁格子，但符合“邻接锁定原则”（即玩家靠近后会封锁）。无参数
    /// </summary>
    PositionExchange,
    /// <summary>
    /// 将怪物随机移动到一个已经翻开的空格子，转移后，怪物暂时不会封锁格子，但符合“邻接锁定原则”（即玩家靠近后会封锁）。无参数
    /// </summary>
    PositionTransfer ,
    /// <summary>
    /// 奴役一个怪物（通常这个技能会有强限制），使这个怪物帮助自己攻击。玩家依然可以攻击这个怪物，但怪物不会反击，同时，怪物解除格锁定。无参数
    /// </summary>
    Enslave,
    /// <summary>
    /// 驱散，随机驱散目标身上N个Debuff。参数为为N，注意N很大的情况
    /// </summary>
    Disperse,
    /// <summary>
    /// 进攻驱散：随机驱散目标身上的N个Buff。参数为N
    /// </summary>
    OffensiveDisperse,
}


