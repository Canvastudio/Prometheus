#region 自动生成的参数
public class SuperConfigInform
{
    /// <summary>
    /// SuperConfig读取的默认路径
    /// </summary>
    public const string SuperConfigDefaultPathOfEasyConfig = "Config/DefaultPath";
}
#endregion
/******************配置表表头******************/
public class GlobalParameterConfig : ConfigDataBase
{
    /// <summary>
    /// 机动参数1
    /// </summary>
    public int motorized_1 { get; set; }
    /// <summary>
    /// 机动参数2
    /// </summary>
    public int motorized_2 { get; set; }
    /// <summary>
    /// 机动参数3
    /// </summary>
    public int motorized_3 { get; set; }
    /// <summary>
    /// 攻击速度参数1
    /// </summary>
    public int atkSpeed_1 { get; set; }
    /// <summary>
    /// 攻击速度参数2
    /// </summary>
    public int atkSpeed_2 { get; set; }
    /// <summary>
    /// 攻击速度参数3
    /// </summary>
    public int atkSpeed_3 { get; set; }
    /// <summary>
    /// 填装速度参数1
    /// </summary>
    public int reloadSpeed_1 { get; set; }
    /// <summary>
    /// [2维]维护站参数。第二维含2个元素，第一个元素是“离上一次出现维护站，又经过了多长的距离”，第二个元素是“这次出现维护站的概率”（地图模块中新增出现维护站的表示）。如果距离已经超出了最大判断距离，则用最后一个元素作为判断依据
    /// </summary>
    public SuperArray<float> maintenanceArg { get; set; }
    /// <summary>
    /// [3维]芯片盘扩展费用。第一维对应各个角色(所以扩展费用也是角色之间的重大区别)；第二维其元素序号对应扩展次数，如果扩展次数大于了元素个数，那么使用最后一个元素作为扩展费用系数；第三维3个元素，分别代表扩展需要的3种材料：金属（Stuff.Coherer），有机物（Stuff.Organics），生命核心（Stuff.Core），不需要灵魂这种材料。
    /// </summary>
    public SuperArray<int> chipDiskExtensions { get; set; }
    /// <summary>
    /// [1维]芯片费用提升。在同一个维护站升级芯片盘时，费用会越来越高。这是一个记录每次升级提高倍率的系数数组，在同一个维护站升级芯片盘时，最终消耗的材料除了chipDiskExtensions表示的数量之外，还要乘以该值（取chipDiskExtensions的三种材料消耗，分别乘以costRatio[重复升级次数]）。同样，当升级次数超出该数组长度，取最后一个值参与计算。
    /// </summary>
    public SuperArray<float> costRatio { get; set; }
#region 自定义区
#endregion
}
public class SupplyConfig : ConfigDataBase
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
    /// 外观
    /// </summary>
    public string prefab { get; set; }
    /// <summary>
    /// 补给品种类
    /// </summary>
    public SupplyType supplyType { get; set; }
    /// <summary>
    /// 等级
    /// </summary>
    public short level { get; set; }
    /// <summary>
    /// 参数
    /// </summary>
    public string arg { get; set; }
#region 自定义区
#endregion
}
public class BoxDropConfig : ConfigDataBase
{
    /// <summary>
    /// 距离：该物品所在格离起点的距离，如果超出所有距离，就一直使用最后一条数据
    /// </summary>
    public int distance { get; set; }
    /// <summary>
    /// [2维]品质1对应的掉落格式：[次数1;次数2|权重1;权重2;权重3... | 物品1;物品2;物品3...| 类型1; 类型2; 类型3...]。level1[0]从这里面随机挑一个，作为掉落次数；level1[1]是权重数组，你可以使用SuperTool.CreateWeightSection(level1.ToList(1))得到一个WeightSection对象，这个对象的RanPoint()方法可以根据权重得到一个落点n；level1[2]是物品id，使用刚刚获得的下标n，level1[2,n]获得id；level1[3]是物品类型，其实就是用来告诉你刚刚获得的id属于哪个配置表，level1[3,n]得到字符串，目前只有Stuff和Chip两种，前者找StuffConfig，后者找ChipConfig。PS：刚刚测试发现WeightSection使用莫名报错，但是在我之前的项目中却可以正常使用...所以我建议还是不要把SuperTool拆开。
    /// </summary>
    public SuperArray<string> level1 { get; set; }
    /// <summary>
    /// [2维]品质2对应的掉落id
    /// </summary>
    public SuperArray<string> level2 { get; set; }
    /// <summary>
    /// [2维]品质3对应的掉落id
    /// </summary>
    public SuperArray<string> level3 { get; set; }
    /// <summary>
    /// [2维]品质4对应的掉落id
    /// </summary>
    public SuperArray<string> level4 { get; set; }
#region 自定义区
#endregion
}
public class StuffConfig : ConfigDataBase
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
    /// 外观
    /// </summary>
    public string prefab { get; set; }
    /// <summary>
    /// 材料类型
    /// </summary>
    public Stuff stuff { get; set; }
    /// <summary>
    /// 是否死亡清空：有些（目前就一种）材料，即便是游戏结束，它也不会清空，比如用来购买新角色的材料
    /// </summary>
    public bool clear { get; set; }
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
    /// [1维]空格资源
    /// </summary>
    public SuperArray<string> emptyList { get; set; }
    /// <summary>
    /// [1维]阻挡资源
    /// </summary>
    public SuperArray<string> obstructList { get; set; }
    /// <summary>
    /// [1维]地图模块
    /// </summary>
    public SuperArray<ulong> map_models { get; set; }
    /// <summary>
    /// [1维]敌人列表
    /// </summary>
    public SuperArray<ulong> enemys { get; set; }
    /// <summary>
    /// [1维]敌人等级
    /// </summary>
    public SuperArray<int> enemy_level { get; set; }
    /// <summary>
    /// [1维]物品列表
    /// </summary>
    public SuperArray<ulong> items { get; set; }
#region 自定义区
#endregion
}
public class ModuleConfig : ConfigDataBase
{
    /// <summary>
    /// [2维]地图块内容：[o]:阻碍 [x]:维护站 [r_敌人强度_出现概率] [g_补给品等级_出现概率] [b_宝箱品质_出现概率] [y_石碑id_出现概率]
    /// </summary>
    public SuperArray<string> contents { get; set; }
#region 自定义区
     public string GetBrickInfo(int row, int column)
    {
        return contents[row, column];
    }
#endregion
}
public class MonsterLevelDataConfig : ConfigDataBase
{
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
    /// [1维]声音：两个元素。怪物翻开，死亡时，可以发出声音惊醒其他改为，这里两个值对应翻开与死亡时声音的距离
    /// </summary>
    public SuperArray<int> noise { get; set; }
    /// <summary>
    /// 危险级别：怪物并不是在翻开后，就一定会对玩家进行远程攻击。
    /// </summary>
    public DangerousLevels dangerous_levels { get; set; }
    /// <summary>
    /// [2维]即时技能：怪物在翻开/死亡时，允许释放一次主动技能（再死去），无视CD强制释放，当然我不会做一个在死亡时给自己加血或者无敌的技能（其实就算是释放了无敌，但因为之前已经触发了死亡，通常是生命值为0，所以它还是会死去）…第一维两个元素分别对应翻开与死亡释放，第二维表示多个技能id（可以翻开/死亡时，一次放N个技能）。id为0表示没有技能，参数为空表示啥技能都没有
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
    /// 决定技能的释放方式。注意Self、Target、Help会根据不同的技能使用者，意义会不同
    /// </summary>
    public TargetType targetType { get; set; }
    /// <summary>
    /// [1维]这是一个过滤列表，符合这个条件的怪物，将不能被该技能列为可选目标（即便AOE、随机都不会命中它）
    /// </summary>
    public SuperArray<TargetLimit> targetLimit { get; set; }
    /// <summary>
    /// 多目标：玩家选择目标后（因为我们最多只让玩家选择一次），或者干脆就是随机目标（不让玩家选），但这个目标不一定就是最终目标，它可能会通过扩散（AOE）、随机多次来达到多目标的选取。总之它和TargetType一同决定技能目标。注意激光和普通飞行物在表现上的不一样会导致他们的表现处理逻辑有很大不同（反正我自己做的时候差异还是蛮大的）
    /// </summary>
    public MultipleType multipleType { get; set; }
    /// <summary>
    /// [2维]目标参数(第一次分割代表目标个数，仅随机有效，第二次分割代表攻击次数与每次伤害权重)。当技能有伤害，或者multipleType为Random时，该项必须有值
    /// </summary>
    public SuperArray<float> targetArg { get; set; }
    /// <summary>
    /// [1维]飞行特效，laser不支持多飞行特效
    /// </summary>
    public SuperArray<string> effect_fly { get; set; }
    /// <summary>
    /// [1维]命中特效
    /// </summary>
    public SuperArray<string> effect_hit { get; set; }
    /// <summary>
    /// [1维]射程
    /// </summary>
    public SuperArray<int> carry { get; set; }
    /// <summary>
    /// [1维]伤害表达式
    /// </summary>
    public SuperArray<string> damage { get; set; }
    /// <summary>
    /// [1维]特效成功率：4个元素。“属性改变”与“特殊效果”都属于特效，它针对不同强度的怪物会有不同的成功率。如果技能目标是玩家，那么使用第一个元素
    /// </summary>
    public SuperArray<float> successRate { get; set; }
    /// <summary>
    /// [2维]主动技能的特殊效果，一个主动技能可以有多个特效，依次执行。第一维表示多个效果；第二维表示【效果生效阶段 : 效果类型】，生效阶段用a或者b（after\before）来表示在伤害前还是后生效
    /// </summary>
    public SuperArray<string> specialEffect { get; set; }
    /// <summary>
    /// [1维]效果参数：根据不同效果参数的意义不同。它的维度应该和specialEffectBefore相同
    /// </summary>
    public SuperArray<string> specialEffectArgs { get; set; }
    /// <summary>
    /// [2维]物资消耗：仅玩家技能生效。玩家使用技能需要消耗一定物资，为空表示不消耗。第二维是一个长度2的数组，第一个元素表示物资类型（可以转成枚举Stuff），第二个表示消耗数量
    /// </summary>
    public SuperArray<string> stuffCost { get; set; }
    /// <summary>
    /// 消耗时间：仅作为玩家技能时生效
    /// </summary>
    public float costTime { get; set; }
    /// <summary>
    /// 能耗：仅作为CopyAttack召唤物才起效
    /// </summary>
    public float usePower { get; set; }
    /// <summary>
    /// 冷却时间：仅作为怪物技能，或者召唤物（NormalAttack）技能生效
    /// </summary>
    public float coolDown { get; set; }
#region 自定义区
#endregion
}
public class TotemConfig : ConfigDataBase
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
    /// 外观
    /// </summary>
    public string prefab { get; set; }
    /// <summary>
    /// 石碑类型
    /// </summary>
    public TotemType totemType { get; set; }
    /// <summary>
    /// 参数
    /// </summary>
    public string arg { get; set; }
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
    /// 决定技能的释放方式。注意Self、Target、Help会根据不同的技能使用者，意义会不同
    /// </summary>
    public TargetType targetType { get; set; }
    /// <summary>
    /// [1维]召唤距离：这是放置位置的距离，而不是攻击距离
    /// </summary>
    public SuperArray<int> carry { get; set; }
    /// <summary>
    /// 预制，浮游炮的外形
    /// </summary>
    public string prefab { get; set; }
    /// <summary>
    /// [2维]物资消耗：仅玩家技能生效。玩家使用技能需要消耗一定物资，为空表示不消耗。第二维是一个长度2的数组，第一个元素表示物资类型（可以转成枚举Stuff），第二个表示消耗数量
    /// </summary>
    public SuperArray<string> stuffCost { get; set; }
    /// <summary>
    /// 消耗时间
    /// </summary>
    public float costTime { get; set; }
    /// <summary>
    /// 召唤物的行为模式
    /// </summary>
    public SpecialAction specialAction { get; set; }
    /// <summary>
    /// [1维]行为参数：其不同行为，对应行为参数解释为不同内容。NormalAttack------主动攻击编号；CopyAttack------能量上限;恢复速度(几回合恢复至上限)
    /// </summary>
    public SuperArray<string> speciaArg { get; set; }
#region 自定义区
#endregion
}
public class SkillPointsConfig : ConfigDataBase
{
    /// <summary>
    /// [1维]激活技能编号，这是一个一维数组，对应该技能点开启\升级的技能ID
    /// </summary>
    public SuperArray<ulong> skillIds { get; set; }
    /// <summary>
    /// [2维]角色需求技能点：要开启/升级该技能需要的技能点数量。该数组的长度，不大于skillids的长度，如果该长度小于skillIds，表示该角色将不能学会该技能的最高等级。注意，该值可以为负。
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
    /// [1维]被动技能作用效果。除了Property与Halo，被动技能其他作用都只对玩家生效。
    /// </summary>
    public SuperArray<PassiveType> passiveType { get; set; }
    /// <summary>
    /// [1维]参数，根据作用类型解释成不同的作用。注意，Property如果有多个属性修改，它的参数是xxxx|xxxx这种形式，虽然用竖线分割了，但它被放在一个字符串里面，需要你再split后，依次执行
    /// </summary>
    public SuperArray<string> args { get; set; }
#region 自定义区
#endregion
}
public class ChipDiskConfig : ConfigDataBase
{
    /// <summary>
    /// [2维]芯片盘特殊区域。在芯片盘上，它用来描述每一个芯片格子；在芯片上，用来描述它对特殊格子的需求
    /// </summary>
    public SuperArray<ChipGrid> chipGridMatrix { get; set; }
    /// <summary>
    /// [1维]在芯片盘上各电源的电量，它的count与芯片盘中电源的数量应该一致，否则请抛出异常
    /// </summary>
    public SuperArray<int> power { get; set; }
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
    /// 状态图标
    /// </summary>
    public string icon { get; set; }
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
    /// 作用间隔：对当前生命值，需要多次进行计算（DOT），这里指定经过多久时间进行1次计算。为0表示仅需要起效1次（在附着上的时候），或者是没有实际意义。
    /// </summary>
    public float interval { get; set; }
    /// <summary>
    /// 标记：美术表现
    /// </summary>
    public string mark { get; set; }
    /// <summary>
    /// 状态优先级：值越大，优先级越高
    /// </summary>
    public int priority { get; set; }
    /// <summary>
    /// [1维]状态的效果，一个状态可以有多个效果同时起效（依次起效）
    /// </summary>
    public SuperArray<StateEffect> stateEffects { get; set; }
    /// <summary>
    /// [1维]参数：stateEffects的参数，其维度应该和stateEffects一样。注意，Property如果有多个属性修改，它的参数是xxxx|xxxx这种形式，虽然用竖线分割了，但它被放在一个字符串里面，需要你再split后，依次执行
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
    /// 描述：描述它增加的技能点与属性，这部分内容交由配置表来控制，不需要在游戏中去索引芯片关联的技能名字（不然还有点麻烦，中间要经过一次技能点配置表才能到技能），但是要支持富文本
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
    /// 升级后对应ID：两个ID相同的芯片可以进行合成以升级。这是升级后对应下一级芯片ID，如果为0，就表示不能升级了。升级后的芯片随机继承两个合成芯片的形状（随机选一个），升级得到的芯片能耗并不是从power里随机得到，而是：两芯片能耗平均+两芯片power差之和/4
    /// </summary>
    public ulong upgradeId { get; set; }
    /// <summary>
    /// [2维]升级需要消耗的物资，为空表示不消耗。第二维是一个长度2的数组，第一个元素表示物资类型（可以转成枚举Stuff），第二个表示消耗数量
    /// </summary>
    public SuperArray<string> stuffCost { get; set; }
    /// <summary>
    /// [1维]能耗：芯片在产出的时候，随机一个能耗。2个元素，分别是下限与上限
    /// </summary>
    public SuperArray<int> power { get; set; }
    /// <summary>
    /// [2维]获得技能点：并不是所有芯片都会有技能点，多少芯片只加属性。每维度前一个元素是技能id，后一个是增加的技能点数（可以为负）
    /// </summary>
    public SuperArray<int> skillPoint { get; set; }
    /// <summary>
    /// 芯片获得技能点特殊区域需求。当芯片该项不为Normal时，表示芯片必须放置在合适的地方，它被激活后才会增加技能点，增加属性（propertyAddition）不受此项影响。芯片必须全部置于该类型的特殊区域才可以激活技能点。注意，如果是Normal，那么不论放置在任何位置（即便把它放在特殊区域），都是可以激活技能点的。
    /// </summary>
    public ChipGrid chipGridReqire { get; set; }
    /// <summary>
    /// [2维]属性加成：并不是所有芯片都有属性加成，它可以只提供技能点。注意这不是属性表达式，仅仅是一个表示某个属性增加多少的形式（因为这里不能用“独立增伤”的属性加成方式），可以为负
    /// </summary>
    public SuperArray<string> propertyAddition { get; set; }
    /// <summary>
    /// [2维]形状描述。每个芯片一个3*3的表示形式，其中0代表空，1代表占位，2表示入口，3表示出口（芯片不一定有出口）
    /// </summary>
    public SuperArray<int> model { get; set; }
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
    /// 描述
    /// </summary>
    public string describe { get; set; }
    /// <summary>
    /// 敌人类型，以后可能会扩展
    /// </summary>
    public MonsterType monsterType { get; set; }
    /// <summary>
    /// [2维]属性系数：第一维长度为4，表示4种强度的怪物；第二维是每个属性缩放值，顺序是m_mhp，m_speed，m_melee，m_laser，m_cartridge
    /// </summary>
    public SuperArray<float> propertys { get; set; }
    /// <summary>
    /// [2维]普通怪技能，第一维度的长度等于该怪物有几个技能； 第二维2~3个元素：第一个元素描述这个是主动技能还是被动技能（a主动，s召唤，p被动）；第二个元素技能ID；第三个只有主动技能才会有，表示该技能初始CD时间（怪物翻开时是允许直接就放技能的），但初始再大最多也就是让技能马上释放，不能让技能释放多次。
    /// </summary>
    public SuperArray<string> skill_normal { get; set; }
    /// <summary>
    /// [2维]稀有怪技能
    /// </summary>
    public SuperArray<string> skill_rare { get; set; }
    /// <summary>
    /// [2维]精英怪技能
    /// </summary>
    public SuperArray<string> skill_elite { get; set; }
    /// <summary>
    /// [2维]BOSS怪技能
    /// </summary>
    public SuperArray<string> skill_boss { get; set; }
    /// <summary>
    /// [1维]怪物AI：怪物的一些常规行为，关联AIConfig。如果长度为1，表示这个怪物不论什么强度都是这个ai，如果长度为4，则根据怪物强度招对应ai
    /// </summary>
    public SuperArray<ulong> ai { get; set; }
#region 自定义区
#endregion
}



public class PlayerInitConfig : ConfigDataBase
{
    /// <summary>
    /// 生命值
    /// </summary>
    public float p_mhp { get; set; }
    /// <summary>
    /// 速度
    /// </summary>
    public float p_speed { get; set; }
    /// <summary>
    /// 机动
    /// </summary>
    public float p_motorized { get; set; }
    /// <summary>
    /// 功率
    /// </summary>
    public float p_capacity { get; set; }
    /// <summary>
    /// 近战攻击力
    /// </summary>
    public float p_melee { get; set; }
    /// <summary>
    /// 攻击速度
    /// </summary>
    public float p_atkSpeed { get; set; }
    /// <summary>
    /// 光线攻击力
    /// </summary>
    public float p_laser { get; set; }
    /// <summary>
    /// 实弹攻击力
    /// </summary>
    public float p_cartridge { get; set; }
    /// <summary>
    /// 填装速度
    /// </summary>
    public float p_reloadSpeed { get; set; }
#region 自定义区
#endregion
}

