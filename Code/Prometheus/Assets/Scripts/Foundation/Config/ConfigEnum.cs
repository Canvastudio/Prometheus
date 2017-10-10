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
/// 决定技能的释放方式。注意Self、Target、Help会根据不同的技能使用者，意义会不同
/// </summary>
public enum TargetType
{
    /// <summary>
    /// 不需要选择目标，通常用于Random或者ALL的技能。如果是怪物持有技能，等价于Enemy
    /// </summary>
    DontSelectTarget,
    /// <summary>
    /// 对自己释放，如果是玩家技能，表示不需要指定目标（目标是自己）。如果是怪物技能，表示该技能对自己释放
    /// </summary>
    Self,
    /// <summary>
    /// 对友方释放，如果是玩家技能，等价于Self。如果是怪物技能，正常情况下等价于Monster，但如果该怪物被奴役，Help就表示玩家。
    /// </summary>
    Help,
    /// <summary>
    /// 如果是玩家技能，效果等同于Monster；如果是怪物技能，表示以玩家为目标，如果这个怪物被奴役，Enemy就等价于Monster
    /// </summary>
    Enemy,
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
    /// 选择一个未翻开且不包含障碍的格子
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
/// 多目标：玩家选择目标后（因为我们最多只让玩家选择一次），或者干脆就是随机目标（不让玩家选），但这个目标不一定就是最终目标，它可能会通过扩散（AOE）、随机多次来达到多目标的选取。总之它和TargetType一同决定技能目标。注意激光和普通飞行物在表现上的不一样会导致他们的表现处理逻辑有很大不同（反正我自己做的时候差异还是蛮大的）
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
/// 这是一个过滤列表，符合这个条件的怪物，将不能被该技能列为可选目标（即便AOE、随机都不会命中它）
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
/// 被动技能作用效果。除了Property与Halo，被动技能其他作用都只对玩家生效。
/// </summary>
public enum PassiveType
{
    None,
    /// <summary>
    /// 改变属性，注意如果技能失效，属性会变回来。如果是当前生命值减少，判定为物理伤害。参数是属性表达式，如果有多个表达式，使用|分割。这个算法的逻辑应该是，依次把玩家裸属性，芯片加成属性，被动技能增加属性，状态增加属性这4部分分别处理，当有某一部分变动时，重新计算那一部分。比如状态列表有变动，就应该重新计算整个状态属性加成，因为你没办法在移除一个状态后倒扣属性。注意，在属性表达式中，有一些参数是随环境变化的，所以每次在这些环境发生变化时，相应计算要跟着变化（基本等于重新计算被动效果列表属性加成），但对passTime，因为变化维度可以很小，所以这里对它进行整数化处理，也就是说只有passTime整数发生变化，才重新计算列表。PS：其实如果开销并不大的话，最好每帧计算，否则有些逻辑处理起来有点麻烦
    /// </summary>
    Property,
    /// <summary>
    /// 光环，给自己以及范围M内可见的同伴或者敌对目标施加状态（ID）。注意通过这种方式附加的状态忽略状态持续时间，只有该被动技能失效（技能拥有者消失、被沉默，或者某种原因导致范围超出）状态才会消失。玩家同样可以持有该技能。参数是ID_M_T,注意状态ID在前,T是bool，true表示该效果针对敌对目标，否则针对己方目标（注意奴役状态会改变T的应对）。
    /// </summary>
    Halo,
    /// <summary>
    /// 对刚刚翻开的敌人发动近战攻击会有一些额外的属性改变。翻开一个敌人后，如果有任何移动、攻击、翻开格子的行为，该效果都会不成立，即在翻开一个敌人后，没有以上行为，直接对其进行近战攻击，才能享受此被动效果（界面行为除外）。仅玩家。参数是属性表达式。
    /// </summary>
    Just,
    /// <summary>
    /// 玩家每走1格，T伤害增加N%。在攻击后（近战&远程都算，但要确实发生攻击行为，纯BUFF技能不算）效果重置。仅玩家可持有该技能。假如走了5格，增加的伤害应该是5*N，而不是N的5次方。参数是N_T。T（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。
    /// </summary>
    Assault,
    /// <summary>
    /// 每1单位时间，T伤害增加N%，玩家使用实弹技能后重置，累积最高不超过M%。仅玩家可持有该技能。假如经过了5时间，累加的伤害应该是5*N，而不是N的5次方。参数是N_M_T。T（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。
    /// </summary>
    DamageBoost,
    /// <summary>
    /// 每杀死一个X单位，T伤害增加N%，最高不超过M%。使用造成T伤害的技能后重置。仅玩家。如果杀死5个机械单位，光线伤害提高5*N，而不是N的5次方。参数是N_M_X_T.X（EffectCondition）该效果的生效条件，它关心None，MonsterTypeIron，MonsterTypeOrganisms这几个状态。T（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。
    /// </summary>
    Massacre,
    /// <summary>
    /// 近战攻击每消灭一个X系敌人，恢复生命值N%。仅玩家。参数是N_X.X（EffectCondition）该效果的生效条件，它关心None，MonsterTypeIron，MonsterTypeOrganisms这几个状态。
    /// </summary>
    SoulRepair,
    /// <summary>
    /// T攻击一定概率N%使敌人昏迷M个单位时间。仅玩家。参数是N_M_T.T（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。
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
    /// 临时改变属性，状态消失后，属性要变回来，除非修改的是当前生命值（如果是当前生命值减少，判定为物理伤害）。参数是属性四则表达式，表达式关键字以状态的宿主为主体，但也有以u开头的关键是表示的技能释放者（状态总是有主/被动技能引发的，所以总会找到释放者）。如果有多个表达式，使用|分割。注意，在属性表达式中，有一些参数是随环境变化的，所以每次在这些环境发生变化时，相应计算要跟着变化（基本等于重新计算状态列表属性加成），但对passTime，因为变化维度可以很小，所以这里对它进行整数化处理，也就是说只有passTime整数发生变化，才重新计算状态列表。PS：其实如果开销并不大的话，最好每帧计算，否则有些逻辑处理起来有点麻烦
    /// </summary>
    Property,
    /// <summary>
    /// 伤害转移，受到N次T型伤害，随机转移给范围M内的其他可见怪物或者玩家，怪物也可以有此状态，转移的伤害不会转移到伤害制造者身上。没有转移目标时，转移失败，宿主受到伤害，但也会消耗次数。转移发生时，宿主不受本次伤害。参数即N（次数）_M(范围)_T(伤害类型)。如果达到次数N，状态自动移除，但如果是被动技能施加的该状态，那么N无效（真相是虽然达到了N，但因为是被动技能施加，所以状态没法移除）。T（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。为了简化流程，这里只考虑主动技能与近战的伤害（所以状态造成的生命值减少就不用检查了）。转移的伤害不能再次被转移（A/B都有转移状态...），转移的伤害类型与之前一样，所以它依然会使其他伤害判定的效果生效（比如DamageTransfer、DamageResist...）
    /// </summary>
    DamageTransfer,
    /// <summary>
    /// 伤害吸收，宿主接下来的伤害会被吸收（宿主不受伤害），DOT也算，参数：吸收表达式（比如HP*5）_T(伤害类型条件)。吸收足够的伤害后，状态消失，除非它是被动技能施加的状态（总之，只要是被动技能施加的状态，除非被动技能失效，否则该状态绝对不会消失）。T（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。
    /// </summary>
    DamageAbsorb,
    /// <summary>
    /// 伤害储存，记下下N次受到的伤害，在自然消失时（被驱散不算自然消失，这里持续时间到，或者达到次数N视为自然消失），为自己回复M倍的生命值。原则这种状态不会由被动技能施加，但如果真是这样，按照规则，它将在N次伤害后发生效果，但该状态不会移除，再受到N次伤害后还可以继续发生效果。注意，宿主还是会收到伤害。参数：N_M_T。T（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。
    /// </summary>
    DamageStore,
    /// <summary>
    /// 受到的T伤害N%返弹给伤害来源。参数为N_T。机制与伤害转移类似，只是这个一定会转移到伤害发动者身上。为了简化流程，这里只考虑主动技能与近战的伤害（所以状态造成的生命值减少就不用检查了）。反弹的伤害不能再次被反弹（A/B都有反弹状态...），反弹的伤害类型与之前一样，所以它依然会使其他伤害判定的效果生效（比如DamageTransfer、DamageResist...）。T（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。
    /// </summary>
    DamageRebound,
    /// <summary>
    /// 一定概率N%受到的T伤害降低M%。注意，每个状态都是独立计算/生效的，如果一个对象同时有DamageResist和MeleeResist，只要在每次判定该状态的时候去计算就好了，而不必把"减伤总和"算出来。参数为N_M_T。T（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。
    /// </summary>
    DamageResist,
    /// <summary>
    /// 除非是场上最后一个怪物，否则免疫一切伤害。仅怪物。注意“最后一个”是为方便理解，真正意义是“场上没有除了有该类型状态的其他怪物”，比如场上可能剩余两个拥有该类型状态的怪物，这时候他们俩都可以受到伤害。没有参数。
    /// </summary>
    Last,
    /// <summary>
    /// 免疫指向技能，即有此状态的怪物，不能在玩家手动选择目标时，被筛选出来。注意这仅仅是不能被玩家手动选中，如果是AOE或者随机技能（不需要选择目标）依然可能打中这个怪物（打不打得中要看技能的TargetLimit）。仅怪物，没有参数
    /// </summary>
    SelectImmune,
    /// <summary>
    /// 状态自然消失时爆炸（怪物死亡不会引发爆炸，状态被驱散也不会爆炸），对玩家造成伤害。仅怪物有此状态。参数是伤害表达式，该爆炸产生的伤害类型始终是实弹型（DamageType.Cartridge）。伤害表达式的参数取状态宿主的属性数值
    /// </summary>
    Explode,
    /// <summary>
    /// 状态自然消失时叫醒范围N内的其他怪物（怪物死亡不会引发Alarm，状态被驱散也不会Alarm），在范围N内的怪物（不论是不是被Alarm唤醒）危险等级dangerous_levels立即提升至Hostility。仅怪物。参数是范围N
    /// </summary>
    Alarm,
    /// <summary>
    /// 死亡时，会随机杀死N个没有翻开的敌人。仅怪物。参数为N
    /// </summary>
    DeathMark,
    /// <summary>
    /// 所有的远程攻击消耗时间减少N%。仅玩家。参数为N
    /// </summary>
    Rapidly,
    /// <summary>
    /// 对生命值N%以下的敌人，造成的T伤害额外提高M%。参数是N_M_T。T（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。
    /// </summary>
    Vjua,
    /// <summary>
    /// 对生命值N%以上的敌人，造成的T伤害额外提高M%。参数是N_M_T。T（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。
    /// </summary>
    Enhance,
    /// <summary>
    /// 对距离N以内的目标造成的T伤害额外增加M%。参数是N_M_T。T（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。
    /// </summary>
    FuelDetonation,
    /// <summary>
    /// 场上每有一个翻开的格子，T伤害额外增加N%。假如有5个翻开的格子，伤害增加5*N，而不是N的5次方。参数是N_T。T（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。
    /// </summary>
    CleanAir,
    /// <summary>
    /// T攻击一定概率N%造成1+M%伤害。参数是N_M_T。T（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。
    /// </summary>
    Critical,
    /// <summary>
    /// 每受到一次T伤害（每次当前生命值减少），增加近战伤害A%,实弹伤害B%,激光伤害C%。有些技能会伤害多次，所以这会让这个怪物伤害累得非常好，DOT也一样。比如每次受到伤害增加近战攻击10%，那么受到5次伤害，近战伤害增加50%。仅怪物有此状态。参数T_A_B_CT（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。T
    /// </summary>
    Angry,
}

/// <summary>
/// 伤害类型，以后可能会扩展
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
/// 主动技能的特殊效果，一个主动技能可以有多个特效，依次执行
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
    /// 永久改变属性。一般不会用来修改玩家的属性（除非修改的是当前生命值，如果是当前生命值减少，判定为物理伤害）。参数是属性四则表达式，如果有多个表达式，使用|分割。注意，和被动\状态不同，在主动技能Property的属性表达式中，虽然一些参数是随环境变化的，但在主动技能上，主动技能只在释放的一瞬间进行判定，所以只要属性加成完毕，那么之后不管外部环境怎么变，都不会影响之前对属性的改变。
    /// </summary>
    Property,
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
    /// 满足条件时，会返还技能一半的消耗，这个效果只对玩家有效。参数即条件，是一个类型为EffectCondition的枚举类型（这里获取到的是字符串，你得自己转一下），它关心的是None，HasMonster，NoMonster，Empty这几个信息。
    /// </summary>
    HalfCostReturn,
    /// <summary>
    /// 翻开一个格子后，再翻开附近的格子（含斜向）。仅玩家拥有此技能。参数为翻开附近格子的距离。注意，在使用Round或者Diffuse时，也可以达到翻开附近格子的效果，他们的区别在于，使用Round或者Diffuse所翻开的格子，都可以被认定是“技能释放的目标位置”，而使用参数达到翻开附近格子的效果，这些格子是不会被判定为“技能释放的目标位置”的，这对使用EffectCondition做为判定条件时，有显著的区别。虽然它自己没有EffectCondition参数，但它通常会与其他效果一并出现在一个技能中，其他效果就可能使用EffectCondition作为自己是否应该生效的判断依据。原则上，如果使用了Round或者Diffuse，而此效果参数又不为0，会先根据Round或者Diffuse翻开周围格子后，再在此基础上，又翻开一圈（后翻开的格子不作为“技能释放的目标位置”）。
    /// </summary>
    OpenBlockWithNear,
    /// <summary>
    /// 将玩家传送到一个格子上，只有玩家技能可以有该特性。参数即条件，是一个类型为EffectCondition的枚举类型（这里获取到的是字符串，你得自己转一下），它关心的是None，HasMonster，NoMonster，Empty这几个信息。原则上，即便没有参数或者参数成立，目标位置不可站立时（有怪物或者机关）也会传送失败，但如果是物品，那么传送过去会直接拾取物品。
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
/// SpecialEffect\PassiveType需要的参数。
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
    /// <summary>
    /// 近战伤害类型成立
    /// </summary>
    DamageTypeMelee,
    /// <summary>
    /// 实弹伤害成立
    /// </summary>
    DamageTypeCartridge,
    /// <summary>
    /// 光线伤害成立
    /// </summary>
    DamageTypeLaser,
    /// <summary>
    /// 机械系怪物
    /// </summary>
    MonsterTypeIron,
    /// <summary>
    /// 生命系怪物
    /// </summary>
    MonsterTypeOrganisms,
}

/// <summary>
/// 材料类型
/// </summary>
public enum Stuff
{
    /// <summary>
    /// 灵魂，这是唯一在游戏结束后也不会清零的货币，它的用途是用来解锁新角色。
    /// </summary>
    Soul,
    /// <summary>
    /// 金属,物资的一种
    /// </summary>
    Coherer,
    /// <summary>
    /// 有机物，物资的一种
    /// </summary>
    Organics,
    /// <summary>
    /// 生命核心，物资的一种
    /// </summary>
    Core,
}

/// <summary>
/// 石碑类型
/// </summary>
public enum TotemType
{
    /// <summary>
    /// 保护石碑：每隔单位时间N，随机选择场上一个可见怪物恢复其全部生命值。参数N
    /// </summary>
    Protect,
    /// <summary>
    /// 召唤石碑：每隔单位时间N，如果可见怪物数量小于M，在一个已翻开的位置，随机召唤一个合法怪物。参数为N_M。合法怪物，指在当前距离允许出现的怪物，召唤的怪物只会出现品质最低的白色。召唤的怪物遵循“邻接锁定原则”。
    /// </summary>
    Summon,
    /// <summary>
    /// 复活石碑：石碑翻开后，记录上一次死亡的怪物。每单位时间N，如果没有怪物死亡，会在已翻开的位置复活上一个死亡的怪物，除非在复活石碑翻开后一直没有怪物死亡。参数为N。复活的怪物遵循“邻接锁定原则”。复活的怪物与死亡的怪物拥有一样的品质。
    /// </summary>
    Resurgence,
    /// <summary>
    /// 重生石碑：每隔时间N，恢复所有可见怪物生命值至它们中生命值最大者的生命值，但不会超过怪物生命值上限。比如，有3只怪物，生命值分别是10/15,20/30,15/40，它们中最大生命值为20，石碑起效胡，它们的生命值变为15/15，20/30,20/40。
    /// </summary>
    Renew,
}

/// <summary>
/// 各种属性，m=monster，p=player,t=target,s=self,u=user
/// </summary>
public enum GameProperty
{
    /// <summary>
    /// 等号
    /// </summary>
    Eql,
    /// <summary>
    /// 加号
    /// </summary>
    Plus,
    /// <summary>
    /// 减号
    /// </summary>
    Sub,
    /// <summary>
    /// 乘号
    /// </summary>
    Mul,
    /// <summary>
    /// 除号
    /// </summary>
    Div,
    /// <summary>
    /// 左括号
    /// </summary>
    LBbr,
    /// <summary>
    /// 右括号
    /// </summary>
    RBbr,
    /// <summary>
    /// 最大生命值百分比，只有玩家在芯片中才会使用此属性
    /// </summary>
    mhp_percent,
    /// <summary>
    /// 当前生命值，怪物和玩家都有
    /// </summary>
    nhp,
    /// <summary>
    /// 最大生命值，怪物和玩家都有
    /// </summary>
    mhp,
    /// <summary>
    /// 近战出手速度百分比，只有玩家芯片会用到此属性
    /// </summary>
    speed_percent,
    /// <summary>
    /// 近战出手速度（决定先手），玩家和怪物都有此属性
    /// </summary>
    speed,
    /// <summary>
    /// 机动，仅玩家有此属性
    /// </summary>
    motorized,
    /// <summary>
    /// 机动百分比，仅玩家芯片会用此属性
    /// </summary>
    motorized_percent,
    /// <summary>
    /// 近战攻击百分比，仅玩家芯片会用此属性
    /// </summary>
    melee_percentd,
    /// <summary>
    /// 近战\物理攻击，怪物和玩家都有此属性
    /// </summary>
    melee,
    /// <summary>
    /// 近战攻击速度，决定近战时间流逝。仅玩家有此属性
    /// </summary>
    atkSpeed,
    /// <summary>
    /// 近战攻击速度百分比，仅玩家芯片会使用
    /// </summary>
    atkSpeed_percent,
    /// <summary>
    /// 实弹攻击力，玩家和怪物都有
    /// </summary>
    cartridge,
    /// <summary>
    /// 实弹攻击力百分比，仅玩家芯片使用
    /// </summary>
    cartridge_percent,
    /// <summary>
    /// 激光攻击力，玩家和怪物都有
    /// </summary>
    laser,
    /// <summary>
    /// 激光攻击力百分比，仅玩家芯片使用
    /// </summary>
    laser_percent,
    /// <summary>
    /// 填装速度，远程攻击（实弹攻击，光线攻击）消耗时间的倍率，仅玩家有此属性
    /// </summary>
    reloadSpeed,
    /// <summary>
    /// 填装速度百分比，仅玩家芯片使用
    /// </summary>
    reloadSpeed_percent,
    /// <summary>
    /// 临时参数
    /// </summary>
    t1,
    /// <summary>
    /// 临时参数
    /// </summary>
    t2,
    /// <summary>
    /// 临时参数
    /// </summary>
    t3,
    /// <summary>
    /// 临时参数
    /// </summary>
    t4,
    /// <summary>
    /// 临时参数
    /// </summary>
    t5,
    /// <summary>
    /// 用来表示自己与目标的距离
    /// </summary>
    dis,
    /// <summary>
    /// 技能造成的伤害
    /// </summary>
    skillDamage,
    /// <summary>
    /// 怪物从翻开到现在，总共经历了多久时间，仅怪物有此属性
    /// </summary>
    passTime,
    /// <summary>
    /// 场上可见怪物数量
    /// </summary>
    monsterNum,
    /// <summary>
    /// 场上可见生命系怪物数量
    /// </summary>
    organismsNum,
    /// <summary>
    /// 场上可见机械系怪物数量
    /// </summary>
    ironNum,
    /// <summary>
    /// 场上翻开格子的数量
    /// </summary>
    openGridNum,
    /// <summary>
    /// 场上未翻开格子的数量
    /// </summary>
    darkGridNum,
    /// <summary>
    /// 技能释放者的近战/物理攻击力，它与“自己”与“目标”都可以不同，通常用在状态上
    /// </summary>
    u_melee,
    /// <summary>
    /// 技能释放者的实弹攻击力，它与“自己”与“目标”都可以不同，通常用在状态上
    /// </summary>
    u_cartridge,
    /// <summary>
    /// 技能释放者的光线攻击力，它与“自己”与“目标”都可以不同，通常用在状态上
    /// </summary>
    u_laser,
    /// <summary>
    /// 当前使用技能的消耗时间（对应主动/召唤技能的costTime）
    /// </summary>
    skillTime,
}

/// <summary>
/// 芯片盘特殊区域。在芯片盘上，它用来描述每一个芯片格子；在芯片上，用来描述它对特殊格子的需求
/// </summary>
public enum ChipGrid
{
    /// <summary>
    /// 在芯片盘上，表示该位置为空（为空就不能放任何东西）
    /// </summary>
    None,
    /// <summary>
    /// 在芯片盘上，表示一个普通的格子；在芯片上，表示这个芯片没有特殊区域需求
    /// </summary>
    Normal,
    /// <summary>
    /// 在芯片盘上，表示一个蓝色特殊格子；在芯片上，表示这个芯片必须在蓝色格子上才能激活技能点
    /// </summary>
    Blue,
    /// <summary>
    /// 在芯片盘上，表示一个红色特殊格子；在芯片上，表示这个芯片必须在红色格子上才能激活技能点
    /// </summary>
    Red,
    /// <summary>
    /// 在芯片盘上，表示一个黄色特殊格子；在芯片上，表示这个芯片必须在黄色格子上才能激活技能点
    /// </summary>
    Yellow,
    /// <summary>
    /// 在芯片盘上，表示是一个电源
    /// </summary>
    Power,
}


