//本文档是自动构建的内容，请勿手动输入与修改！

/// <summary>
/// 危险级别
/// </summary>
public enum DangerousLevels
{
    /// <summary>
    /// 友善：受到伤害才会激活远程行为
    /// </summary>
    Friendly,
    /// <summary>
    /// 中立：受到伤害或玩家邻接会激活远程行为
    /// </summary>
    Neutral,
    /// <summary>
    /// 敌意：翻开就会激活远程攻击
    /// </summary>
    Hostility,
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
/// 这是一个过滤列表，符合这个条件的怪物，将不能被该技能列为可选目标
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
    /// 血肉型怪物？？
    /// </summary>
    MonsterType_blood,
}


