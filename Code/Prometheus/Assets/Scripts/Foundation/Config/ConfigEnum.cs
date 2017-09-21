//本文档是自动构建的内容，请勿手动输入与修改！

/// <summary>
/// 危险级别：怪物并不是在翻开后，就一定会对玩家进行远程攻击。
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
/// 决定技能的释放方式。注意Self和Target会根据不同的技能使用者，意义会不同
/// </summary>
public enum TargetType
{
    /// <summary>
    /// 不需要选择目标，通常用于Random或者ALL的技能。如果是怪物持有技能，等价于Target
    /// </summary>
    DontSelectTarget,
    /// <summary>
    /// 对自己释放，如果是玩家技能，表示不需要指定目标（目标是自己）。如果是怪物技能，表示该技能对自己释放
    /// </summary>
    Self,
    /// <summary>
    /// 如果是玩家技能，效果等同于Monster；如果是怪物技能，表示以玩家为目标，如果这个怪物被奴役，Target就等价于Monster
    /// </summary>
    Target,
    /// <summary>
    /// 选择一个射程内的可见怪物。如果是怪物技能，表示射程内，非自己的其他怪物
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
    /// <summary>
    /// 选择上下左右临近的一个格子来表方向。它通常用于直线AOE攻击
    /// </summary>
    Direction,
    /// <summary>
    /// 选择一个射程内，可见的机械系敌人
    /// </summary>
    Monster_iron,
    /// <summary>
    /// 选择一个射程内，可见的生命系敌人
    /// </summary>
    Monster_organisms,
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
/// 多目标，laser不支持同时攻击多个目标
/// </summary>
public enum MultipleType
{
    /// <summary>
    /// 默认，只会攻击一个目标
    /// </summary>
    Normal,
    /// <summary>
    /// 随机目标，目标不重复。可以结合targetArg实现随机选取多个目标
    /// </summary>
    Random,
    /// <summary>
    /// 随机目标，目标可重复。可以结合targetArg实现随机选取多个目标
    /// </summary>
    Random_2,
    /// <summary>
    /// 所有目标
    /// </summary>
    All,
    /// <summary>
    /// 对一条线上的敌人造成伤害，直线的起点是技能释放者，终点与射程有关。这种释放方式的技能，不能作为召唤物绑定技能
    /// </summary>
    Line,
    /// <summary>
    /// 上下左右扩散模式，1表示扩散半径为1。它会对半径内的其他目标同时造成效果（这里的效果一般指伤害，但如果技能释放目标是格子，那么也对格子有效）。这种扩散模式是基于上下左右的（斜向距离为2）。
    /// </summary>
    Diffuse_1,
    Diffuse_2,
    Diffuse_3,
    Diffuse_4,
    Diffuse_5,
    Diffuse_6,
    /// <summary>
    /// 含斜向扩散模式，1表示扩散半径为1。它会对半径内的其他目标同时造成效果（这里的效果一般指伤害，但如果技能释放目标是格子，那么也对格子有效）。这种扩散模式是基于上下左右与斜向的（斜向距离为1）。
    /// </summary>
    Round_1,
    Round_2,
    Round_3,
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
    LimitQuality_1,
    LimitQuality_2,
    LimitQuality_3,
    LimitQuality_4,
    /// <summary>
    /// 机械类型的怪物
    /// </summary>
    Limit_iron,
    /// <summary>
    /// 生命型怪物
    /// </summary>
    Limit_organisms,
}

/// <summary>
/// 被动技能作用效果。注意，该效果优先级是低于AI的，比如对怪物来说，死亡时可能翻开附近的怪物，而又有附近怪物AddState。由于AI优先级更高，所以会先执行翻开的操作。
/// </summary>
public enum PassiveType
{
    None,
    /// <summary>
    /// 改变属性，注意如果技能失效，属性会变回来。参数是属性表达式。这个算法的逻辑应该是，依次把玩家裸属性，芯片加成属性，被动技能增加属性，状态增加属性这4部分分别处理，当有某一部分变动时，重新计算那一部分。比如状态列表有变动，就应该重新计算整个状态属性加成，因为你没办法在移除一个状态后倒扣属性。
    /// </summary>
    Property,
    /// <summary>
    /// 光环，给自己以及范围M内可见的同伴或者敌对目标施加状态（ID）。注意通过这种方式附加的状态忽略状态持续时间，只有该被动技能失效（技能拥有者消失、被沉默，或者某种原因导致范围超出）状态才会消失。玩家同样可以持有该技能。参数是ID_M_T,注意状态ID在前,T是bool，true表示该效果针对敌对目标，否则针对自己（注意奴役状态会改变T的应对）。
    /// </summary>
    Halo,
    /// <summary>
    /// 对刚刚翻开的敌人发动近战攻击会有一些额外的属性改变。翻开一个敌人后，如果有任何移动、攻击、翻开格子的行为，该效果都会不成立，即在翻开一个敌人后，没有以上行为，直接对其进行近战攻击，才能享受此被动效果（界面行为除外）。仅玩家。参数是属性表达式。
    /// </summary>
    Just,
    /// <summary>
    /// 玩家每走1格，近战伤害增加N%。在攻击后（近战&远程都算）效果重置。仅玩家可持有该技能。假如走了5格，增加的伤害应该是5*N，而不是N的5次方。参数是N。
    /// </summary>
    Assault,
    /// <summary>
    /// 每1单位时间，实弹伤害增加N%，玩家使用实弹技能后重置，累积最高不超过M%。仅玩家可持有该技能。假如经过了5时间，累加的伤害应该是5*N，而不是N的5次方。参数是N_M
    /// </summary>
    CartridgeBoost,
    /// <summary>
    /// 每1单位时间，光线伤害增加N%，玩家使用实弹技能后重置，累积最高不超过M%。仅玩家可持有该技能。假如经过了5时间，累加的伤害应该是5*N，而不是N的5次方。参数是N_M
    /// </summary>
    LaserBoost,
    /// <summary>
    /// 每杀死一个机械单位，光线系伤害增加N%，最高不超过M%。使用光线系技能后重置。仅玩家。如果杀死5个机械单位，光线伤害提高5*N，而不是N的5次方。参数是N_M
    /// </summary>
    IronEng,
    /// <summary>
    /// 每杀死一个生物单位，实弹系伤害增加N%，最高不超过M%。使用实弹系技能后重置。仅玩家。如果杀死5个生命单位，实弹伤害提高5*N，而不是N的5次方。参数是N_M
    /// </summary>
    OrganismsEng,
    /// <summary>
    /// 近战攻击每消灭一个生命系敌人，恢复生命值N%。仅玩家。参数是N
    /// </summary>
    SoulRepair,
    /// <summary>
    /// 近战攻击一定概率N%使敌人昏迷M个单位时间。仅玩家。参数是N_M
    /// </summary>
    Bang,
    /// <summary>
    /// 使用N次对应的召唤技能，重复启用被动技能不能重复召唤。注意这种方式召唤出来的召唤物，其能量/技能冷却都是从0开始，而且忽略召唤技能的时间消耗。仅玩家。参数为N
    /// </summary>
    Summon,
    /// <summary>
    /// 预警雷达：目前保留，这属于另外一个模块功能
    /// </summary>
    WarningRadar,
    /// <summary>
    /// 寻宝雷达：目前保留，这属于另外一个模块功能
    /// </summary>
    TreasureRadar,
}

/// <summary>
/// 状态的效果，一个状态可以有多个效果同时起效（依次起效）
/// </summary>
public enum StateEffect
{
    None,
    /// <summary>
    /// 沉默，所有被动技能无效，有些被动技能是给自己/队友施加状态，那么这些状态此时也应该消失。玩家也能被沉默。注意这并不是驱散，它只会使那些永久性质的状态消失（因为导致这些状态的被动技能失效了）无参数
    /// </summary>
    Silent,
    /// <summary>
    /// 缴械。不能使用主动技能，怪物被缴械后，技能CD暂停。玩家也是可以被缴械的。无参数
    /// </summary>
    Disarm,
    /// <summary>
    /// 冻结，冻结后不能反击，不能使用技能，技能CD暂停，暂时解除周围格子锁定。玩家不会中该状态。无参数
    /// </summary>
    Freeze,
    /// <summary>
    /// 催眠，不能反击，不能使用技能,技能CD暂停，暂时解除周围格子锁定，受到任何伤害都会导致该状态失效，玩家不会中该状态。无参数
    /// </summary>
    Sleep,
    /// <summary>
    /// 临时改变属性，状态消失后，属性要变回来，除非修改的是当前生命值。参数是属性四则表达式。算法参考被动技能属性修改。
    /// </summary>
    Property,
    /// <summary>
    /// 伤害转移，受到N次伤害，随机转移给其他怪物（DOT不算），如果是怪物有此状态，则转移给玩家。参数即N（次数）。如果达到次数，状态自动消失
    /// </summary>
    DamageTransfer,
    /// <summary>
    /// 伤害吸收，宿主接下来的伤害会被吸收（宿主不受伤害），参数：吸收表达式（比如HP*5）。吸收足够的伤害后，状态消失
    /// </summary>
    DamageAbsorb,
    /// <summary>
    /// 伤害储存，记下下N次受到的伤害，在状态结束时，为自己回复M倍的生命值。注意，宿主还是会收到伤害。参数：N_M
    /// </summary>
    DamageStore,
    /// <summary>
    /// 除非是场上最后一个怪物，否则免疫一切伤害。仅怪物。注意“最后一个”是为方便理解，真正意义是“场上没有除了有该类型状态的其他怪物”，比如场上可能剩余两个拥有该类型状态的怪物，这时候他们俩都可以受到伤害。没有参数。
    /// </summary>
    Last,
    /// <summary>
    /// 免疫指向技能，即有此状态的怪物，不能在玩家手动选择目标时，被筛选出来（随机目标是可以的）。仅怪物，没有参数
    /// </summary>
    Imm,
    /// <summary>
    /// 状态自然消失时爆炸（怪物死亡不会引发爆炸，状态被驱散也不会爆炸），对玩家造成伤害。仅怪物。参数是伤害表达式。伤害表达式的参数一般取状态宿主的属性数值
    /// </summary>
    Explode,
    /// <summary>
    /// 死亡时，会随机杀死N个没有翻开的敌人。仅怪物。参数为N
    /// </summary>
    DeathMark,
    /// <summary>
    /// 如果附近有目标，将受到的光线伤害随机转移给一个目标（目标包含玩家，但不包含偏转发生的单位），怪物也可以有此状态。参数为“附近”所表示的距离
    /// </summary>
    LaserDeflection,
    /// <summary>
    /// 所有的远程攻击消耗时间减少N%。仅玩家。参数为N
    /// </summary>
    Rapidly,
    /// <summary>
    /// 一定概率N%受到的实弹伤害降低M%。参数为N_M
    /// </summary>
    CartridgeResist,
    /// <summary>
    /// 一定概率N%受到的激光伤害降低M%。参数为N_M
    /// </summary>
    LaserResist,
    /// <summary>
    /// 对生命值N%以下的敌人，造成的近战伤害额外提高M%。参数是N_M
    /// </summary>
    Vjua,
    /// <summary>
    /// 对生命值N%以上的敌人，造成的光线伤害额外提高M%。参数是N_M
    /// </summary>
    LaserEnhance,
    /// <summary>
    /// 对距离N以内的目标造成的实弹伤害额外增加M%。参数是N_M
    /// </summary>
    FuelDetonation,
    /// <summary>
    /// 场上每有一个翻开的格子，光线伤害额外增加N%。假如有5个翻开的格子，伤害增加5*N，而不是N的5次方。参数是N
    /// </summary>
    CleanAir,
    /// <summary>
    /// 场上每有一个翻开的格子，实弹伤害额外增加N%。假如有5个翻开的格子，伤害增加5*N，而不是N的5次方。参数是N
    /// </summary>
    EasyAim,
    /// <summary>
    /// 近战攻击一定概率N%造成1+M%伤害。参数是N_M
    /// </summary>
    Critical,
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
    None,
    /// <summary>
    /// 给技能目标释放状态。所以就可以有伤害是0的纯状态技能，注意状态附加是优先于伤害计算的。参数是状态ID
    /// </summary>
    AddState,
    /// <summary>
    /// 强制给自己释放状态，无视技能目标。注意这个BUFF是先优先伤害计算的。比如我释放了一个BUFF只持续非常短的时间，但是因为它是在技能前就附加上了，所以哪怕是一瞬间，这个BUFF也会起效。参数是状态ID
    /// </summary>
    AddStateToSelf,
    /// <summary>
    /// 变形，将目标的样子变成另外一个prefab，通常它会配合属性改变一起使用，被变形后的敌人会丢失所有技能。仅玩家拥有此技能。参数是变成目标prefab的名称
    /// </summary>
    Transfiguration,
    /// <summary>
    /// 玩家与目标怪物位置互换，互换后，怪物暂时不会封锁格子，但符合“邻接锁定原则”（即玩家靠近后会封锁）。仅玩家拥有此技能。无参数
    /// </summary>
    PositionExchange,
    /// <summary>
    /// 将怪物随机移动到一个已经翻开的空格子，转移后，怪物暂时不会封锁格子，但符合“邻接锁定原则”（即玩家靠近后会封锁）。仅玩家拥有此技能。无参数
    /// </summary>
    PositionTransfer,
    /// <summary>
    /// 奴役一个怪物，使这个怪物帮助自己攻击。玩家依然可以攻击这个怪物，但怪物不会反击，同时，怪物解除格锁定。仅玩家拥有此技能。无参数
    /// </summary>
    Enslave,
    /// <summary>
    /// 防御驱散，随机驱散目标身上N个Debuff。注意由光环\被动技能附加的持续时间无限的状态是不能被驱散的。参数为为N
    /// </summary>
    Disperse,
    /// <summary>
    /// 进攻驱散：随机驱散目标身上的N个Buff。注意由光环\被动技能附加的持续时间无限的状态是不能被驱散的。参数为N
    /// </summary>
    OffensiveDisperse,
    /// <summary>
    /// 满足条件时，会返还技能一半的消耗，这个效果只对玩家有效。参数即条件，是一个类型为EffectCondition的枚举类型（这里获取到的是字符串，你得自己转一下）
    /// </summary>
    HalfCostReturn,
    /// <summary>
    /// 翻开一个格子后，再翻开附近的格子（含斜向）。仅玩家拥有此技能。参数为翻开附近格子的距离。注意，在使用Round或者Diffuse时，也可以达到翻开附近格子的效果，他们的区别在于，使用Round或者Diffuse所翻开的格子，都可以被认定是“技能释放的目标位置”，而使用参数达到翻开附近格子的效果，这些格子是不会被判定为“技能释放的目标位置”的，这在使用EffectCondition做为判定条件时，有显著的区别。原则上，如果使用了Round或者Diffuse，而此效果参数又不为0，会先根据Round或者Diffuse翻开周围格子后，再在此基础上，又翻开一圈（后翻开的格子不作为“技能释放的目标位置”）。
    /// </summary>
    OpenBlockWithNear,
    /// <summary>
    /// 将玩家传送到一个格子上，只有玩家技能可以有该特性。参数即条件，是一个类型为EffectCondition的枚举类型（这里获取到的是字符串，你得自己转一下）。原则上，即便没有参数或者参数成立，目标位置不可站立时（有怪物或者机关）也会传送失败，但如果是物品，那么传送过去会直接拾取物品。
    /// </summary>
    TransferSelf,
    /// <summary>
    /// 将目标传送到一个已翻开的格子上，原则上怪物也可以使用这种技能（玩家会被传送到一个格子上）。没有参数
    /// </summary>
    TransferTarget,
}

/// <summary>
/// 敌人类型，以后可能会扩展
/// </summary>
public enum MonsterType
{
    /// <summary>
    /// 钢铁型敌人
    /// </summary>
    Iron,
    /// <summary>
    /// 生物型敌人
    /// </summary>
    Organisms,
}

/// <summary>
/// 用做SpecialEffect个别行为的生效条件。以释放技能的目标位置，格子是否满足特定条件为依据。
/// </summary>
public enum EffectCondition
{
    /// <summary>
    /// 没有条件，也就是无条件成立
    /// </summary>
    None,
    /// <summary>
    /// 如果技能释放位置有怪物，那么条件成立
    /// </summary>
    HasMonster,
    /// <summary>
    /// 如果技能释放位置没有怪物，那么条件成立
    /// </summary>
    NoMonster,
    /// <summary>
    /// 如果技能释放位置是空的格子，那么条件成立
    /// </summary>
    Empty,
}


