#region 配置表类_GlobalParameterConfig
/// <summary>全局参数</summary>
public class GlobalParameterConfig : ConfigDataBase
{
    /// <summary>机动时间流逝公式 (玩家每走1米的时间流逝)</summary>
    public string motorizedFormula { set; get; }
    /// <summary>近战攻击时间流逝公式(玩家每近战攻击1次的时间流逝)</summary>
    public string atkSpeedFormula { set; get; }
    /// <summary>填装速度时间流逝公式(使用技能最终消耗的时间)</summary>
    public string reloadSpeedFormula { set; get; }
    /// <summary>[2维]维护站参数。第二维含2个元素，第一个元素是“离上一次出现维护站，又经过了多长的距离”，第二个元素是“这次出现维护站的概率”（地图模块中新增出现维护站的表示）。如果距离已经超出了最大判断距离，则用最后一个元素作为判断依据</summary>
    public SuperArrayValue<float> maintenanceArg { set; get; }
    /// <summary>[3维]芯片盘扩展费用。第一维对应各个角色(所以扩展费用也是角色之间的重大区别)；第二维其元素序号对应扩展次数，如果扩展次数大于了元素个数，那么使用最后一个元素作为扩展费用系数；第三维3个元素，分别代表扩展需要的3种材料：金属（Stuff.Coherer），有机物（Stuff.Organics），生命核心（Stuff.Core），不需要灵魂这种材料。</summary>
    public SuperArrayValue<int> chipDiskExtensions { set; get; }
    /// <summary>[1维]芯片费用提升。在同一个维护站升级芯片盘时，费用会越来越高。这是一个记录每次升级提高倍率的系数数组，在同一个维护站升级芯片盘时，最终消耗的材料除了chipDiskExtensions表示的数量之外，还要乘以该值（取chipDiskExtensions的三种材料消耗，分别乘以costRatio[重复升级次数]）。同样，当升级次数超出该数组长度，取最后一个值参与计算。</summary>
    public SuperArrayValue<float> costRatio { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_GlobalParameterConfig



#region 配置表类_SupplyConfig
/// <summary>补给品（血瓶）</summary>
public class SupplyConfig : ConfigDataBase
{
    /// <summary>名称</summary>
    public string name { set; get; }
    /// <summary>描述</summary>
    public string describe { set; get; }
    /// <summary>外观</summary>
    public string prefab { set; get; }
    /// <summary>等级</summary>
    public short level { set; get; }
    /// <summary>参数</summary>
    public string arg { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_SupplyConfig



#region 配置表类_StuffConfig
/// <summary>材料</summary>
public class StuffConfig : ConfigDataBase
{
    /// <summary>名称</summary>
    public string name { set; get; }
    /// <summary>描述</summary>
    public string describe { set; get; }
    /// <summary>外观</summary>
    public string prefab { set; get; }
    /// <summary>材料类型</summary>
    public Stuff stuff { set; get; }
    /// <summary>是否死亡清空：有些（目前就一种）材料，即便是游戏结束，它也不会清空，比如用来购买新角色的材料</summary>
    public bool clear { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_StuffConfig



#region 配置表类_BoxDropConfig
/// <summary>掉落矩阵</summary>
public class BoxDropConfig : ConfigDataBase
{
    /// <summary>距离：该物品所在格离起点的距离，如果超出所有距离，就一直使用最后一条数据</summary>
    public int distance { set; get; }
    /// <summary>[2维]品质1对应的掉落格式：[次数1;次数2|权重1;权重2;权重3... | 物品1;物品2;物品3...| 类型1; 类型2; 类型3...]。level1[0]从这里面随机挑一个，作为掉落次数；level1[1]是权重数组，你可以使用SuperTool.CreateWeightSection(level1.ToList(1))得到一个WeightSection对象，这个对象的RanPoint()方法可以根据权重得到一个落点n；level1[2]是物品id，使用刚刚获得的下标n，level1[2,n]获得id；level1[3]是物品类型，其实就是用来告诉你刚刚获得的id属于哪个配置表，level1[3,n]得到字符串，目前只有Stuff和Chip两种，前者找StuffConfig，后者找ChipConfig。</summary>
    public SuperArrayValue<string> level1 { set; get; }
    /// <summary>[2维]品质2对应的掉落id</summary>
    public SuperArrayValue<string> level2 { set; get; }
    /// <summary>[2维]品质3对应的掉落id</summary>
    public SuperArrayValue<string> level3 { set; get; }
    /// <summary>[2维]品质4对应的掉落id</summary>
    public SuperArrayValue<string> level4 { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_BoxDropConfig



#region 配置表类_ModuleConfig
/// <summary>地图模块</summary>
public class ModuleConfig : ConfigDataBase
{
    /// <summary>[2维]地图块内容：[o]:阻碍 [x]:维护站 [r_敌人强度_出现概率] [g_补给品等级_出现概率] [b_宝箱品质_出现概率] [y_石碑id_出现概率]</summary>
    public SuperArrayValue<string> contents { set; get; }

    #region 自定义区
    public string GetBrickInfo(int row, int column)
    {
        return contents[row, column];
    }
    #endregion
}
#endregion 配置表类_ModuleConfig



#region 配置表类_MapConfig
/// <summary>关卡</summary>
public class MapConfig : ConfigDataBase
{
    /// <summary>行走距离：玩家行走小于等级该距离时，使用该条数据，除非这是所有配置中，距离最大的数据</summary>
    public int distance { set; get; }
    /// <summary>[1维]空格资源</summary>
    public SuperArrayValue<string> emptyList { set; get; }
    /// <summary>[1维]阻挡资源</summary>
    public SuperArrayValue<string> obstructList { set; get; }
    /// <summary>[1维]地图模块</summary>
    public SuperArrayValue<ulong> map_models { set; get; }
    /// <summary>[1维]敌人列表</summary>
    public SuperArrayValue<ulong> enemys { set; get; }
    /// <summary>[1维]敌人等级</summary>
    public SuperArrayValue<int> enemy_level { set; get; }
    /// <summary>[1维]物品列表</summary>
    public SuperArrayValue<ulong> items { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_MapConfig



#region 配置表类_AIConfig
/// <summary>怪物AI</summary>
public class AIConfig : ConfigDataBase
{
    /// <summary>警戒范围：怪物并不一定非要等到玩家踩脸才会翻开，玩家与怪物的距离小于等级该值时，怪物就会翻开。为0时候，就需要玩家踩脸才会翻开。注意翻开后的怪物，是否要锁定周围的格子，要符合“邻接锁定原则”。邻接锁定原则：如果是远处翻开，当该怪物周围均是未翻开的格子时，不会立即锁定周围的格子，当玩家接触（邻接）该怪物（翻开了怪物周围的格子）之后，锁定剩余的格子</summary>
    public int warning { set; get; }
    /// <summary>[1维]声音：两个元素。怪物翻开，死亡时，可以发出声音惊醒其他改为，这里两个值对应翻开与死亡时声音的距离</summary>
    public SuperArrayValue<int> noise { set; get; }
    /// <summary>危险级别：怪物并不是在翻开后，就一定会对玩家进行远程攻击。</summary>
    public DangerousLevels dangerous_levels { set; get; }
    /// <summary>[2维]即时技能：怪物在翻开/死亡时，允许释放一次主动技能（再死去），无视CD强制释放，当然我不会做一个在死亡时给自己加血或者无敌的技能（其实就算是释放了无敌，但因为之前已经触发了死亡，通常是生命值为0，所以它还是会死去）…第一维两个元素分别对应翻开与死亡释放，第二维表示多个技能id（可以翻开/死亡时，一次放N个技能）。id为0表示没有技能，参数为空表示啥技能都没有</summary>
    public SuperArrayValue<ulong> forceSkills { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_AIConfig



#region 配置表类_MonsterConfig
/// <summary>怪物个性</summary>
public class MonsterConfig : ConfigDataBase
{
    /// <summary>prefab资源</summary>
    public string res_face { set; get; }
    /// <summary>名字</summary>
    public string m_name { set; get; }
    /// <summary>描述</summary>
    public string describe { set; get; }
    /// <summary>敌人类型，以后可能会扩展</summary>
    public MonsterType monsterType { set; get; }
    /// <summary>[2维]属性系数：第一维长度为4，表示4种强度的怪物；第二维是每个属性缩放值，顺序是m_mhp，m_speed，m_melee，m_laser，m_cartridge</summary>
    public SuperArrayValue<float> propertys { set; get; }
    /// <summary>[2维]普通怪技能，第一维度的长度等于该怪物有几个技能； 第二维2~3个元素：第一个元素描述这个是主动技能还是被动技能（a主动，s召唤，p被动）；第二个元素技能ID；第三个只有主动技能才会有，表示该技能初始CD时间（怪物翻开时是允许直接就放技能的），但初始再大最多也就是让技能马上释放，不能让技能释放多次。</summary>
    public SuperArrayValue<string> skill_normal { set; get; }
    /// <summary>[2维]稀有怪技能</summary>
    public SuperArrayValue<string> skill_rare { set; get; }
    /// <summary>[2维]精英怪技能</summary>
    public SuperArrayValue<string> skill_elite { set; get; }
    /// <summary>[2维]BOSS怪技能</summary>
    public SuperArrayValue<string> skill_boss { set; get; }
    /// <summary>[1维]怪物AI：怪物的一些常规行为，关联AIConfig。如果长度为1，表示这个怪物不论什么强度都是这个ai，如果长度为4，则根据怪物强度招对应ai</summary>
    public SuperArrayValue<ulong> ai { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_MonsterConfig



#region 配置表类_MonsterLevelDataConfig
/// <summary>怪物模板属性</summary>
public class MonsterLevelDataConfig : ConfigDataBase
{
    /// <summary>生命值</summary>
    public float mhp { set; get; }
    /// <summary>速度</summary>
    public float speed { set; get; }
    /// <summary>近战攻击力</summary>
    public float melee { set; get; }
    /// <summary>光线攻击力</summary>
    public float laser { set; get; }
    /// <summary>实弹攻击力</summary>
    public float cartridge { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_MonsterLevelDataConfig



#region 配置表类_PassiveSkillsConfig
/// <summary>被动技能</summary>
public class PassiveSkillsConfig : ConfigDataBase
{
    /// <summary>技能名称</summary>
    public string name { set; get; }
    /// <summary>描述</summary>
    public string describe { set; get; }
    /// <summary>prefab资源名</summary>
    public string prefab { set; get; }
    /// <summary>[1维]被动技能作用效果。除了Property与Halo，被动技能其他作用都只对玩家生效。</summary>
    public SuperArrayValue<PassiveType> passiveType { set; get; }
    /// <summary>[1维]参数，根据作用类型解释成不同的作用</summary>
    public SuperArrayObj<SkillArg> passiveSkillArgs { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_PassiveSkillsConfig



#region 配置表类_SkillPointsConfig
/// <summary>技能点</summary>
public class SkillPointsConfig : ConfigDataBase
{
    /// <summary>[1维]激活技能编号，这是一个一维数组，对应该技能点开启\升级的技能ID</summary>
    public SuperArrayValue<ulong> skillIds { set; get; }
    /// <summary>[2维]角色需求技能点：要开启/升级该技能需要的技能点数量。该数组的长度，不大于skillids的长度，如果该长度小于skillIds，表示该角色将不能学会该技能的最高等级。注意，该值可以为负。</summary>
    public SuperArrayValue<int> characterActivate { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_SkillPointsConfig



#region 配置表类_SummonSkillsConfig
/// <summary>召唤技能</summary>
public class SummonSkillsConfig : ConfigDataBase
{
    /// <summary>名称</summary>
    public string name { set; get; }
    /// <summary>描述</summary>
    public string describe { set; get; }
    /// <summary>prefab资源名</summary>
    public string prefab { set; get; }
    /// <summary>决定技能的释放方式。注意Self、Target、Help会根据不同的技能使用者，意义会不同</summary>
    public TargetType targetType { set; get; }
    /// <summary>[1维]召唤距离：这是放置位置的距离，而不是攻击距离</summary>
    public SuperArrayValue<int> carry { set; get; }
    /// <summary>预制，浮游炮的外形</summary>
    public string prefab2 { set; get; }
    /// <summary>物资消耗：仅玩家技能生效。玩家使用技能需要消耗一定物资，为空表示不消耗。</summary>
    public StuffCostArg stuffCost { set; get; }
    /// <summary>消耗时间</summary>
    public float costTime { set; get; }
    /// <summary>召唤物的行为模式</summary>
    public SpecialAction specialAction { set; get; }
    /// <summary>[1维]行为参数：其不同行为，对应行为参数解释为不同内容。NormalAttack------主动攻击编号；CopyAttack------能量上限;恢复速度(几回合恢复至上限)</summary>
    public SuperArrayValue<string> speciaArg { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_SummonSkillsConfig



#region 配置表类_ActiveSkillsConfig
/// <summary>主动技能</summary>
public class ActiveSkillsConfig : ConfigDataBase
{
    /// <summary>名称</summary>
    public string name { set; get; }
    /// <summary>描述</summary>
    public string describe { set; get; }
    /// <summary>prefab资源名</summary>
    public string prefab { set; get; }
    /// <summary>决定技能的释放方式。注意Self、Target、Help会根据不同的技能使用者，意义会不同</summary>
    public TargetType targetType { set; get; }
    /// <summary>[1维]这是一个过滤列表，符合这个条件的怪物，将不能被该技能列为可选目标（即便AOE、随机都不会命中它）</summary>
    public SuperArrayValue<TargetLimit> targetLimit { set; get; }
    /// <summary>多目标：玩家选择目标后（因为我们最多只让玩家选择一次），或者干脆就是随机目标（不让玩家选），但这个目标不一定就是最终目标，它可能会通过扩散（AOE）、随机多次来达到多目标的选取。总之它和TargetType一同决定技能目标。注意激光和普通飞行物在表现上的不一样会导致他们的表现处理逻辑有很大不同（反正我自己做的时候差异还是蛮大的）</summary>
    public MultipleType multipleType { set; get; }
    /// <summary>[2维]目标参数(第一次分割代表目标个数，仅随机有效，第二次分割代表攻击次数与每次伤害权重)。当技能有伤害，或者multipleType为Random时，该项必须有值</summary>
    public SuperArrayValue<float> targetArg { set; get; }
    /// <summary>[1维]飞行特效，laser不支持多飞行特效</summary>
    public SuperArrayValue<string> effect_fly { set; get; }
    /// <summary>[1维]命中特效</summary>
    public SuperArrayValue<string> effect_hit { set; get; }
    /// <summary>[1维]射程</summary>
    public SuperArrayValue<int> carry { set; get; }
    /// <summary>伤害类型</summary>
    public DamageType damageType { set; get; }
    /// <summary>伤害表达式</summary>
    public string damage { set; get; }
    /// <summary>[1维]特效成功率：4个元素。“属性改变”与“特殊效果”都属于特效，它针对不同强度的怪物会有不同的成功率。如果技能目标是玩家，那么使用第一个元素</summary>
    public SuperArrayValue<float> successRate { set; get; }
    /// <summary>[1维]主动技能的特殊效果，一个技能可能包含多个效果，要依次执行</summary>
    public SuperArrayValue<SpecialEffect> specialEffects { set; get; }
    /// <summary>[1维]特殊效果的生效阶段，0表示伤害之强，1表示伤害之后。维度与特殊效果一致</summary>
    public SuperArrayValue<short> phase { set; get; }
    /// <summary>[1维]这是一个用来描述特殊效果的参数类，维度与特殊效果一致</summary>
    public SuperArrayObj<SkillArg> activeSkillArgs { set; get; }
    /// <summary>物资消耗：仅玩家技能生效。玩家使用技能需要消耗一定物资，为空表示不消耗。参数类。</summary>
    public StuffCostArg stuffCost { set; get; }
    /// <summary>消耗时间：仅作为玩家技能时生效</summary>
    public float costTime { set; get; }
    /// <summary>能耗：仅作为CopyAttack召唤物才起效</summary>
    public float usePower { set; get; }
    /// <summary>冷却时间：仅作为怪物技能，或者召唤物（NormalAttack）技能生效</summary>
    public float coolDown { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_ActiveSkillsConfig



#region 配置表类_TotemConfig
/// <summary>石碑</summary>
public class TotemConfig : ConfigDataBase
{
    /// <summary>名称</summary>
    public string name { set; get; }
    /// <summary>描述</summary>
    public string describe { set; get; }
    /// <summary>外观</summary>
    public string prefab { set; get; }
    /// <summary>石碑类型</summary>
    public TotemType totemType { set; get; }
    /// <summary>参数</summary>
    public string arg { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_TotemConfig



#region 配置表类_ChipConfig
/// <summary>芯片</summary>
public class ChipConfig : ConfigDataBase
{
    /// <summary>名称</summary>
    public string name { set; get; }
    /// <summary>描述：描述它增加的技能点与属性，这部分内容交由配置表来控制，不需要在游戏中去索引芯片关联的技能名字（不然还有点麻烦，中间要经过一次技能点配置表才能到技能），但是要支持富文本</summary>
    public string descrip { set; get; }
    /// <summary>颜色</summary>
    public string color { set; get; }
    /// <summary>等级</summary>
    public int level { set; get; }
    /// <summary>升级后对应ID：两个ID相同的芯片可以进行合成以升级。这是升级后对应下一级芯片ID，如果为0，就表示不能升级了。升级后的芯片随机继承两个合成芯片的形状（随机选一个），升级得到的芯片能耗并不是从power里随机得到，而是：两芯片能耗平均+两芯片power差之和/4</summary>
    public ulong upgradeId { set; get; }
    /// <summary>升级需要消耗的物资，为空表示不消耗。这是一个参数类</summary>
    public StuffCostArg stuffCost { set; get; }
    /// <summary>[1维]能耗：芯片在产出的时候，随机一个能耗。2个元素，分别是下限与上限</summary>
    public SuperArrayValue<int> power { set; get; }
    /// <summary>[2维]获得技能点：并不是所有芯片都会有技能点，多少芯片只加属性。每维度前一个元素是技能id，后一个是增加的技能点数（可以为负）</summary>
    public SuperArrayValue<int> skillPoint { set; get; }
    /// <summary>芯片获得技能点特殊区域需求。当芯片该项不为Normal时，表示芯片必须放置在合适的地方，它被激活后才会增加技能点，增加属性（propertyAddition）不受此项影响。芯片必须全部置于该类型的特殊区域才可以激活技能点。注意，如果是Normal，那么不论放置在任何位置（即便把它放在特殊区域），都是可以激活技能点的。</summary>
    public ChipGrid chipGridReqire { set; get; }
    /// <summary>属性加成：并不是所有芯片都有属性加成，它可以只提供技能点。这是一个表示属性表示类型，其增加的属性值可以为负</summary>
    public PropertyGainArg propertyAddition { set; get; }
    /// <summary>[2维]形状描述。每个芯片一个3*3的表示形式，其中0代表空，1代表占位，2表示入口，3表示出口（芯片不一定有出口）</summary>
    public SuperArrayValue<int> model { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_ChipConfig



#region 配置表类_ChipDiskConfig
/// <summary>芯片盘</summary>
public class ChipDiskConfig : ConfigDataBase
{
    /// <summary>[2维]芯片盘特殊区域。在芯片盘上，它用来描述每一个芯片格子；在芯片上，用来描述它对特殊格子的需求</summary>
    public SuperArrayValue<ChipGrid> chipGridMatrix { set; get; }
    /// <summary>[1维]在芯片盘上各电源的电量，它的count与芯片盘中电源的数量应该一致，否则请抛出异常</summary>
    public SuperArrayValue<int> power { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_ChipDiskConfig



#region 枚举_Stuff
/// <summary>材料类型</summary>
public enum Stuff
{
    /// <summary>灵魂，这是唯一在游戏结束后也不会清零的货币，它的用途是用来解锁新角色。</summary>
    Soul,
    /// <summary>金属,物资的一种</summary>
    Coherer,
    /// <summary>有机物，物资的一种</summary>
    Organics,
    /// <summary>生命核心，物资的一种</summary>
    Core,
}
#endregion 枚举_Stuff

#region 枚举_DangerousLevels
/// <summary>危险级别：怪物并不是在翻开后，就一定会对玩家进行远程攻击。</summary>
public enum DangerousLevels
{
    /// <summary>友善：受到伤害才会激活远程行为</summary>
    Friendly,
    /// <summary>中立：受到伤害或玩家邻接会激活远程行为</summary>
    Neutral,
    /// <summary>敌意：翻开就会激活远程攻击</summary>
    Hostility,
}
#endregion 枚举_DangerousLevels

#region 枚举_MonsterType
/// <summary>敌人类型，以后可能会扩展</summary>
public enum MonsterType
{
    /// <summary>钢铁型敌人</summary>
    Iron,
    /// <summary>生物型敌人</summary>
    Organisms,
}
#endregion 枚举_MonsterType

#region 枚举_GameProperty
/// <summary>各种属性，m=monster，p=player,t=target,s=self,u=user</summary>
public enum GameProperty
{
    /// <summary>等号</summary>
    Eql,
    /// <summary>加号</summary>
    Plus,
    /// <summary>减号</summary>
    Sub,
    /// <summary>乘号</summary>
    Mul,
    /// <summary>除号</summary>
    Div,
    /// <summary>左括号</summary>
    LBbr,
    /// <summary>右括号</summary>
    RBbr,
    /// <summary>最大生命值百分比，只有玩家在芯片中才会使用此属性</summary>
    mhp_percent,
    /// <summary>当前生命值，怪物和玩家都有</summary>
    nhp,
    /// <summary>最大生命值，怪物和玩家都有</summary>
    mhp,
    /// <summary>近战出手速度百分比，只有玩家芯片会用到此属性</summary>
    speed_percent,
    /// <summary>近战出手速度（决定先手），玩家和怪物都有此属性</summary>
    speed,
    /// <summary>机动，仅玩家有此属性</summary>
    motorized,
    /// <summary>机动百分比，仅玩家芯片会用此属性</summary>
    motorized_percent,
    /// <summary>近战攻击百分比，仅玩家芯片会用此属性</summary>
    melee_percentd,
    /// <summary>近战\物理攻击，怪物和玩家都有此属性</summary>
    melee,
    /// <summary>近战攻击速度，决定近战时间流逝。仅玩家有此属性</summary>
    atkSpeed,
    /// <summary>近战攻击速度百分比，仅玩家芯片会使用</summary>
    atkSpeed_percent,
    /// <summary>实弹攻击力，玩家和怪物都有</summary>
    cartridge,
    /// <summary>实弹攻击力百分比，仅玩家芯片使用</summary>
    cartridge_percent,
    /// <summary>激光攻击力，玩家和怪物都有</summary>
    laser,
    /// <summary>激光攻击力百分比，仅玩家芯片使用</summary>
    laser_percent,
    /// <summary>填装速度，远程攻击（实弹攻击，光线攻击）消耗时间的倍率，仅玩家有此属性</summary>
    reloadSpeed,
    /// <summary>填装速度百分比，仅玩家芯片使用</summary>
    reloadSpeed_percent,
    /// <summary>临时参数</summary>
    t1,
    /// <summary>临时参数</summary>
    t2,
    /// <summary>临时参数</summary>
    t3,
    /// <summary>临时参数</summary>
    t4,
    /// <summary>临时参数</summary>
    t5,
    /// <summary>用来表示自己与目标的距离</summary>
    dis,
    /// <summary>技能造成的伤害</summary>
    skillDamage,
    /// <summary>怪物从翻开到现在，总共经历了多久时间，仅怪物有此属性</summary>
    passTime,
    /// <summary>场上可见怪物数量</summary>
    monsterNum,
    /// <summary>场上可见生命系怪物数量</summary>
    organismsNum,
    /// <summary>场上可见机械系怪物数量</summary>
    ironNum,
    /// <summary>场上翻开格子的数量</summary>
    openGridNum,
    /// <summary>场上未翻开格子的数量</summary>
    darkGridNum,
    /// <summary>技能释放者的近战/物理攻击力，它与“自己”与“目标”都可以不同，通常用在状态上</summary>
    u_melee,
    /// <summary>技能释放者的实弹攻击力，它与“自己”与“目标”都可以不同，通常用在状态上</summary>
    u_cartridge,
    /// <summary>技能释放者的光线攻击力，它与“自己”与“目标”都可以不同，通常用在状态上</summary>
    u_laser,
    /// <summary>当前使用技能的消耗时间（对应主动/召唤技能的costTime）</summary>
    skillTime,
    /// <summary>功率。每1点功率，增加所有电源1%的电量</summary>
    capacity,
}
#endregion 枚举_GameProperty



#region 枚举_TargetType
/// <summary>决定技能的释放方式。注意Self、Target、Help会根据不同的技能使用者，意义会不同</summary>
public enum TargetType
{
    /// <summary>不需要选择目标，通常用于Random或者ALL的技能。如果是怪物持有技能，等价于Enemy</summary>
    DontSelectTarget,
    /// <summary>对自己释放，如果是玩家技能，表示不需要指定目标（目标是自己）。如果是怪物技能，表示该技能对自己释放</summary>
    Self,
    /// <summary>对友方释放，如果是玩家技能，等价于Self。如果是怪物技能，正常情况下等价于Monster，但如果该怪物被奴役，Help就表示玩家。</summary>
    Help,
    /// <summary>如果是玩家技能，效果等同于Monster；如果是怪物技能，表示以玩家为目标，如果这个怪物被奴役，Enemy就等价于Monster</summary>
    Enemy,
    /// <summary>选择一个射程内的可见怪物。如果是怪物技能，表示射程内，非自己的其他怪物</summary>
    Monster,
    /// <summary>选择一个格子。原则上包含所有的格子，但应该把玩家所在位置的格子排除</summary>
    Block,
    /// <summary>选择一个翻开且不含障碍的格子，排除玩家所在的格子</summary>
    LightBlock,
    /// <summary>选择一个未翻开且不包含障碍的格子</summary>
    DarkBlock,
    /// <summary>选择一个翻开且不含障碍，格子上没有物体，排除玩家所在的格子</summary>
    EmptyBlock,
    /// <summary>选择一个障碍（障碍不存在翻开与未翻开的概念）</summary>
    ObstructBlock,
    /// <summary>选择一个障碍，而且该位置没有炮台</summary>
    炮台位置,
    /// <summary>同上一条，但它不需要玩家选择位置</summary>
    全炮台位置,
    /// <summary>不需要选目标，表示召唤浮游炮</summary>
    卫星,
    /// <summary>尚未被翻开的怪物，不需要玩家选择目标，它通常与multipleType为random时配合使用，表示随机一个没有被发现的怪物</summary>
    HideMonster,
    /// <summary>选择上下左右临近的一个格子来表方向。它通常用于直线AOE攻击</summary>
    Direction,
    /// <summary>选择一个射程内，可见的机械系敌人</summary>
    Monster_iron,
    /// <summary>选择一个射程内，可见的生命系敌人</summary>
    Monster_organisms,
}
#endregion 枚举_TargetType

#region 枚举_SpecialAction
/// <summary>召唤物的行为模式</summary>
public enum SpecialAction
{
    /// <summary>普通攻击</summary>
    NormalAttack,
    /// <summary>复制玩家的主动技能</summary>
    CopyAttack,
}
#endregion 枚举_SpecialAction

#region 枚举_TargetLimit
/// <summary>这是一个过滤列表，符合这个条件的怪物，将不能被该技能列为可选目标（即便AOE、随机都不会命中它）</summary>
public enum TargetLimit
{
    /// <summary>枚举默认值，表示没有任何过滤</summary>
    None,
    /// <summary>怪物强度1</summary>
    LimitQuality_1,
    LimitQuality_2,
    LimitQuality_3,
    LimitQuality_4,
    /// <summary>机械类型的怪物</summary>
    Limit_iron,
    /// <summary>生命型怪物</summary>
    Limit_organisms,
}
#endregion 枚举_TargetLimit

#region 枚举_PassiveType
/// <summary>被动技能作用效果。除了Property与Halo，被动技能其他作用都只对玩家生效。</summary>
public enum PassiveType
{
    /// <summary>没有效果</summary>
    None,
    /// <summary>改变属性，注意如果技能失效，属性会变回来。如果是当前生命值减少，判定为物理伤害。参数是rpn数组，可以有多个表达式。这个算法的逻辑应该是，依次把玩家裸属性，芯片加成属性，被动技能增加属性，状态增加属性这4部分分别处理，当有某一部分变动时，重新计算那一部分。比如状态列表有变动，就应该重新计算整个状态属性加成，因为没办法在移除一个状态后倒扣属性。注意，在属性表达式中，有一些参数是随环境变化的，所以每次在这些环境发生变化时，相应计算要跟着变化。</summary>
    Property,
    /// <summary>光环，给自己以及范围f[0]内可见的同伴或者敌对（b[0]为true表示敌对，意奴役状态会改变目标应对）目标施加状态u[0]。注意通过这种方式附加的状态忽略状态持续时间，只有该被动技能失效（技能拥有者消失、被沉默，或者某种原因导致范围超出）状态才会消失。玩家同样可以持有该技能。</summary>
    Halo,
    /// <summary>对刚刚翻开的敌人发动近战攻击会有一些额外的属性改变（rpn[0]）。翻开一个敌人后，如果有任何移动、攻击、翻开格子的行为，该效果都会不成立，即在翻开一个敌人后，没有以上行为，直接对其进行近战攻击，才能享受此被动效果（界面行为除外）。仅玩家。</summary>
    Just,
    /// <summary>玩家每走1格，ec[0]类型的伤害增加f[0](%)。在攻击后（近战&远程都算，但要确实发生攻击行为，纯BUFF技能不算）效果重置。仅玩家可持有该技能。假如走了5格，增加的伤害应该是5*f[0]，而不是f[0]的5次方。ec[0]（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。</summary>
    Assault,
    /// <summary>每1单位时间，ec[0]类型伤害增加f[0](%)，玩家使用实弹技能后重置，累积最高不超过f[1](%)。仅玩家可持有该技能。假如经过了5时间，累加的伤害应该是5*f[0]，而不是f[0]的5次方。ec[0]（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。</summary>
    DamageBoost,
    /// <summary>每杀死一个ec[0]类型单位，ec[1]类型伤害增加f[0](%)，最高不超过f[1](%)。使用造成ec[1]伤害的技能后重置。仅玩家。如果杀死5个机械单位，光线伤害提高5*f[0]，而不是f[0]的5次方。ec[0]（EffectCondition）该效果的生效条件(目标类型)，它关心None，MonsterTypeIron，MonsterTypeOrganisms这几个状态。ec[1]（EffectCondition）该效果的生效条件（伤害类型），它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。</summary>
    Massacre,
    /// <summary>近战攻击每消灭一个ex[0]类型的敌人，恢复生命值f[0](%)。仅玩家。ec[0]（EffectCondition）该效果的生效条件，它关心None，MonsterTypeIron，MonsterTypeOrganisms这几个状态。</summary>
    SoulRepair,
    /// <summary>ec[0]类型攻击f[0](%)概率使敌人昏迷f[1]个回合。ec[0]（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。</summary>
    Bang,
    /// <summary>使用f[0]次对应id为[0]的召唤技能，重复启用被动技能不能重复召唤。注意这种方式召唤出来的召唤物，其能量/技能冷却都是从0开始，而且忽略召唤技能的时间消耗。</summary>
    Summon,
    /// <summary>预警雷达：目前保留，这属于另外一个模块功能</summary>
    WarningRadar,
    /// <summary>寻宝雷达：目前保留，这属于另外一个模块功能</summary>
    TreasureRadar,
}
#endregion 枚举_PassiveType

#region 枚举_StateEffect
/// <summary>状态的效果，一个状态可以有多个效果同时起效（依次起效）</summary>
public enum StateEffect
{
    /// <summary>没有任何要求</summary>
    None,
    /// <summary>沉默，所有被动技能无效，有些被动技能是给自己/队友施加状态，那么这些状态此时也应该消失。玩家也能被沉默。注意这并不是驱散，它只会使那些永久性质的状态消失（因为导致这些状态的被动技能失效了）无参数</summary>
    Silent,
    /// <summary>缴械。不能使用主动技能，怪物被缴械后，技能CD暂停。玩家也是可以被缴械的。无参数</summary>
    Disarm,
    /// <summary>冻结，冻结后不能反击，不能使用技能，技能CD暂停，暂时解除周围格子锁定。玩家不会中该状态。无参数</summary>
    Freeze,
    /// <summary>催眠，不能反击，不能使用技能,技能CD暂停，暂时解除周围格子锁定，受到任何伤害都会导致该状态失效，玩家不会中该状态。无参数</summary>
    Sleep,
    /// <summary>临时改变属性，状态消失后，属性要变回来，除非修改的是当前生命值（如果是当前生命值减少，判定为物理伤害）。参数为rpn数组，可以有多个表达式。如果有多个表达式。注意，在属性表达式中，有一些参数是随环境变化的，所以每次在这些环境发生变化时，相应计算要跟着变化。</summary>
    Property,
    /// <summary>伤害转移，受到f[0]次ec[0]型伤害，随机转移给范围f[1]内的其他可见怪物或者玩家，状态DOT也算（酌情，看实现难度）。怪物也可以有此状态，转移的伤害不会转移到伤害制造者身上。没有转移目标时，转移失败，宿主受到伤害，但也会消耗次数。转移发生时，宿主不受本次伤害。。如果达到转移次数，状态自动移除，但如果是被动技能施加的该状态，那么次数无效（真相是虽然达到了次数，但因为是被动技能施加，所以状态没法移除）。转移的伤害不能再次被转移（A/B都有转移状态...），转移的伤害类型与之前一样，所以它依然会使其他伤害判定的效果生效（比如DamageTransfer、DamageResist...）。ec[0]（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。</summary>
    DamageTransfer,
    /// <summary>伤害吸收，宿主接下来受到ec[0]类型的伤害会被吸收（宿主不受伤害）最多吸收rpn[0]点伤害或者吸收f[0]次伤害（DOT也算）。吸收足够的伤害或次数后，状态消失，除非它是被动技能施加的状态（总之，只要是被动技能施加的状态，除非被动技能失效，否则该状态绝对不会消失）。吸收伤害不会影响状态判定。ec[0]（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。</summary>
    DamageAbsorb,
    /// <summary>伤害储存，记下下f[0]次受到的伤害，在自然消失时（被驱散不算自然消失，这里持续时间到，或者达到次数f[0]视为自然消失），为自己回复f[1]倍的生命值。原则这种状态不会由被动技能施加，但如果真是这样，按照规则，它将在f[0]次伤害后发生效果，但该状态不会移除，再受到f[0]次伤害后还可以继续发生效果。注意，宿主还是会受到伤害。ec[0]（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。</summary>
    DamageStore,
    /// <summary>受到的ec[0]伤害f[0](%)返弹给伤害来源。机制与伤害转移类似，只是这个一定会转移到伤害发动者身上。为了简化流程，这里只考虑主动技能与近战的伤害（所以状态造成的生命值减少就不用检查了）。反弹的伤害不能再次被反弹（A/B都有反弹状态...），反弹的伤害类型与之前一样，所以它依然会使其他伤害判定的效果生效（比如DamageTransfer、DamageResist...）。ec[0]（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。</summary>
    DamageRebound,
    /// <summary>一定概率f[0](%)受到的ec[0]伤害降低f[1](%)。注意，每个状态都是独立计算/生效的，如果一个对象同时有DamageResist和MeleeResist，只要在每次判定该状态的时候去计算就好了，而不必把"减伤总和"算出来。ec[0]（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。</summary>
    DamageResist,
    /// <summary>除非是场上最后一个怪物，否则免疫一切伤害。仅怪物。注意“最后一个”是为方便理解，真正意义是“场上没有除了有该类型状态的其他怪物”，比如场上可能剩余两个拥有该类型状态的怪物，这时候他们俩都可以受到伤害。没有参数。</summary>
    Last,
    /// <summary>免疫指向技能，即有此状态的怪物，不能在玩家手动选择目标时，被筛选出来。注意这仅仅是不能被玩家手动选中，如果是AOE或者随机技能（不需要选择目标）依然可能打中这个怪物（打不打得中要看技能的TargetLimit）。仅怪物，没有参数</summary>
    SelectImmune,
    /// <summary>状态自然消失时爆炸（怪物死亡不会引发爆炸，状态被驱散也不会爆炸），对玩家造成rpn[0]伤害。仅怪物有此状态。</summary>
    Explode,
    /// <summary>状态自然消失时叫醒范围f[0]内的其他怪物（怪物死亡不会引发Alarm，状态被驱散也不会Alarm），在范围f[0]内的怪物（不论是不是被Alarm唤醒）危险等级dangerous_levels立即提升至Hostility。仅怪物。</summary>
    Alarm,
    /// <summary>死亡时，会随机杀死f[0]个没有翻开的敌人。仅怪物。</summary>
    DeathMark,
    /// <summary>所有的远程攻击消耗时间减少f[0](%)。仅玩家。参数为N</summary>
    Rapidly,
    /// <summary>对生命值f[0](%)以下的敌人，造成的ec[0]伤害额外提高f[1]。ec[0]（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。</summary>
    Vjua,
    /// <summary>对生命值f[0](%)以上的敌人，造成的ec[0]伤害额外提高f[1](%)。ec[0]（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。</summary>
    Enhance,
    /// <summary>对距离f[0]以内的目标造成的ec[0]伤害额外增加f[1](%)。ec[0]（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。</summary>
    FuelDetonation,
    /// <summary>ec[0]攻击一定概率f[0]造成f[1]伤害。ec[0]（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。</summary>
    Critical,
    /// <summary>每受到一次effectConditionArg[0]伤害（每次当前生命值减少），增加近战伤害numbers[0](%，后同),实弹伤害B%,激光伤害C%。有些技能会伤害多次，所以这会让这个怪物伤害累积非常高，DOT也一样。比如每次受到伤害增加近战攻击10%，那么受到5次伤害，近战伤害增加50%。仅怪物有此状态。参数effectConditionArg[0],numbers[0],numbers[1],numbers[2]T（EffectCondition）该效果的生效条件，它关心None，DamageTypeMelee，DamageTypeCartridge，DamageTypeLaser这几个状态。T</summary>
    Angry,
}
#endregion 枚举_StateEffect

#region 枚举_DamageType
/// <summary>伤害类型，以后可能会扩展</summary>
public enum DamageType
{
    /// <summary>物理伤害</summary>
    Physical,
    /// <summary>实弹伤害</summary>
    Cartridge,
    /// <summary>光线伤害</summary>
    Laser,
}
#endregion 枚举_DamageType

#region 枚举_SpecialEffect
/// <summary>主动技能的特殊效果</summary>
public enum SpecialEffect
{
    None,
    /// <summary>给技能目标释放状态。所以就可以有伤害是0的纯状态技能，注意状态附加是优先于伤害计算的。参数是状态ID</summary>
    AddState,
    /// <summary>强制给自己释放状态，无视技能目标。注意这个BUFF是先优先伤害计算的。比如我释放了一个BUFF只持续非常短的时间，但是因为它是在技能前就附加上了，所以哪怕是一瞬间，这个BUFF也会起效。参数是状态ID</summary>
    AddStateToSelf,
    /// <summary>永久改变属性。一般不会用来修改玩家的属性（除非修改的是当前生命值，如果是当前生命值减少，判定为物理伤害）。参数是属性四则表达式，如果有多个表达式，使用|分割。注意，和被动\状态不同，在主动技能Property的属性表达式中，虽然一些参数是随环境变化的，但在主动技能上，主动技能只在释放的一瞬间进行判定，所以只要属性加成完毕，那么之后不管外部环境怎么变，都不会影响之前对属性的改变。</summary>
    Property,
    /// <summary>变形，将目标的样子变成另外一个prefab，通常它会配合属性改变一起使用，被变形后的敌人会丢失所有技能。仅玩家拥有此技能。参数是变成目标prefab的名称</summary>
    Transfiguration,
    /// <summary>玩家与目标怪物位置互换，互换后，怪物暂时不会封锁格子，但符合“邻接锁定原则”（即玩家靠近后会封锁）。仅玩家拥有此技能。无参数</summary>
    PositionExchange,
    /// <summary>将怪物随机移动到一个已经翻开的空格子，转移后，怪物暂时不会封锁格子，但符合“邻接锁定原则”（即玩家靠近后会封锁）。仅玩家拥有此技能。无参数</summary>
    PositionTransfer,
    /// <summary>奴役一个怪物，使这个怪物帮助自己攻击。玩家依然可以攻击这个怪物，但怪物不会反击，同时，怪物解除格锁定。仅玩家拥有此技能。无参数</summary>
    Enslave,
    /// <summary>防御驱散，随机驱散目标身上N个Debuff。注意由光环\被动技能附加的持续时间无限的状态是不能被驱散的。参数为为N</summary>
    Disperse,
    /// <summary>进攻驱散：随机驱散目标身上的N个Buff。注意由光环\被动技能附加的持续时间无限的状态是不能被驱散的。参数为N</summary>
    OffensiveDisperse,
    /// <summary>满足条件时，会返还技能一半的消耗，这个效果只对玩家有效。参数即条件，是一个类型为EffectCondition的枚举类型（这里获取到的是字符串，你得自己转一下），它关心的是None，HasMonster，NoMonster，Empty这几个信息。</summary>
    HalfCostReturn,
    /// <summary>翻开一个格子后，再翻开附近的格子（含斜向）。仅玩家拥有此技能。参数为翻开附近格子的距离。注意，在使用Round或者Diffuse时，也可以达到翻开附近格子的效果，他们的区别在于，使用Round或者Diffuse所翻开的格子，都可以被认定是“技能释放的目标位置”，而使用参数达到翻开附近格子的效果，这些格子是不会被判定为“技能释放的目标位置”的，这对使用EffectCondition做为判定条件时，有显著的区别。虽然它自己没有EffectCondition参数，但它通常会与其他效果一并出现在一个技能中，其他效果就可能使用EffectCondition作为自己是否应该生效的判断依据。原则上，如果使用了Round或者Diffuse，而此效果参数又不为0，会先根据Round或者Diffuse翻开周围格子后，再在此基础上，又翻开一圈（后翻开的格子不作为“技能释放的目标位置”）。</summary>
    OpenBlockWithNear,
    /// <summary>将玩家传送到一个格子上，只有玩家技能可以有该特性。参数即条件，是一个类型为EffectCondition的枚举类型（这里获取到的是字符串，你得自己转一下），它关心的是None，HasMonster，NoMonster，Empty这几个信息。原则上，即便没有参数或者参数成立，目标位置不可站立时（有怪物或者机关）也会传送失败，但如果是物品，那么传送过去会直接拾取物品。</summary>
    TransferSelf,
    /// <summary>将目标传送到一个已翻开的格子上，原则上怪物也可以使用这种技能（玩家会被传送到一个格子上）。没有参数</summary>
    TransferTarget,
}
#endregion 枚举_SpecialEffect

#region 枚举_EffectCondition
/// <summary>SpecialEffect\PassiveType需要的参数。</summary>
public enum EffectCondition
{
    /// <summary>没有条件，也就是无条件成立</summary>
    None,
    /// <summary>如果技能释放位置有怪物，那么条件成立</summary>
    HasMonster,
    /// <summary>如果技能释放位置没有怪物，那么条件成立</summary>
    NoMonster,
    /// <summary>如果技能释放位置是空的格子，那么条件成立</summary>
    Empty,
    /// <summary>近战伤害类型成立</summary>
    DamageTypeMelee,
    /// <summary>实弹伤害成立</summary>
    DamageTypeCartridge,
    /// <summary>光线伤害成立</summary>
    DamageTypeLaser,
    /// <summary>机械系怪物</summary>
    MonsterTypeIron,
    /// <summary>生命系怪物</summary>
    MonsterTypeOrganisms,
}
#endregion 枚举_EffectCondition

#region 枚举_MultipleType
/// <summary>多目标：玩家选择目标后（因为我们最多只让玩家选择一次），或者干脆就是随机目标（不让玩家选），但这个目标不一定就是最终目标，它可能会通过扩散（AOE）、随机多次来达到多目标的选取。总之它和TargetType一同决定技能目标。注意激光和普通飞行物在表现上的不一样会导致他们的表现处理逻辑有很大不同（反正我自己做的时候差异还是蛮大的）</summary>
public enum MultipleType
{
    /// <summary>默认，只会攻击一个目标</summary>
    Normal,
    /// <summary>随机目标，目标不重复。可以结合targetArg实现随机选取多个目标</summary>
    Random,
    /// <summary>随机目标，目标可重复。可以结合targetArg实现随机选取多个目标</summary>
    Random_2,
    /// <summary>所有目标</summary>
    All,
    /// <summary>对一条线上的敌人造成伤害，直线的起点是技能释放者，终点与射程有关。这种释放方式的技能，不能作为召唤物绑定技能</summary>
    Line,
    /// <summary>上下左右扩散模式，1表示扩散半径为1。它会对半径内的其他目标同时造成效果（这里的效果一般指伤害，但如果技能释放目标是格子，那么也对格子有效）。这种扩散模式是基于上下左右的（斜向距离为2）。</summary>
    Diffuse_1,
    Diffuse_2,
    Diffuse_3,
    Diffuse_4,
    Diffuse_5,
    Diffuse_6,
    /// <summary>含斜向扩散模式，1表示扩散半径为1。它会对半径内的其他目标同时造成效果（这里的效果一般指伤害，但如果技能释放目标是格子，那么也对格子有效）。这种扩散模式是基于上下左右与斜向的（斜向距离为1）。</summary>
    Round_1,
    Round_2,
    Round_3,
}
#endregion 枚举_MultipleType



#region 枚举_TotemType
/// <summary>石碑类型</summary>
public enum TotemType
{
    /// <summary>保护石碑：每隔单位时间N，随机选择场上一个可见怪物恢复其全部生命值。参数N</summary>
    Protect,
    /// <summary>召唤石碑：如果可见怪物数量小于M，每隔单位时间N，在一个已翻开的位置，随机召唤一个合法怪物。参数为N_M。合法怪物，指在当前距离允许出现的怪物，召唤的怪物只会出现品质最低的白色。召唤的怪物遵循“邻接锁定原则”。</summary>
    Summon,
    /// <summary>复活石碑：石碑翻开后，记录上一次死亡的怪物。如果没有怪物死亡，每单位时间N，会在已翻开的位置复活上一个死亡的怪物，除非在复活石碑翻开后一直没有怪物死亡。参数为N。复活的怪物遵循“邻接锁定原则”。复活的怪物与死亡的怪物拥有一样的品质。</summary>
    Resurgence,
    /// <summary>重生石碑：每隔时间N，恢复所有可见怪物生命值至它们中生命值最大者的生命值，但不会超过怪物生命值上限。比如，有3只怪物， 生命值分别是10/15,20/30,15/40，它们中最大生命值为20，石碑起效胡，它们的生命值变为15/15，20/30,20/40。</summary>
    Renew,
}
#endregion 枚举_TotemType



#region 枚举_ChipGrid
/// <summary>芯片盘特殊区域。在芯片盘上，它用来描述每一个芯片格子；在芯片上，用来描述它对特殊格子的需求</summary>
public enum ChipGrid
{
    /// <summary>在芯片盘上，表示该位置为空（为空就不能放任何东西）</summary>
    None,
    /// <summary>在芯片盘上，表示一个普通的格子；在芯片上，表示这个芯片没有特殊区域需求</summary>
    Normal,
    /// <summary>在芯片盘上，表示一个蓝色特殊格子；在芯片上，表示这个芯片必须在蓝色格子上才能激活技能点</summary>
    Blue,
    /// <summary>在芯片盘上，表示一个红色特殊格子；在芯片上，表示这个芯片必须在红色格子上才能激活技能点</summary>
    Red,
    /// <summary>在芯片盘上，表示一个黄色特殊格子；在芯片上，表示这个芯片必须在黄色格子上才能激活技能点</summary>
    Yellow,
    /// <summary>在芯片盘上，表示是一个电源</summary>
    Power,
}
#endregion 枚举_ChipGrid
#region 配置表类_BoxConfig
/// <summary>宝箱</summary>
public class BoxConfig : ConfigDataBase
{
    /// <summary>名称</summary>
    public string name { set; get; }
    /// <summary>描述</summary>
    public string describe { set; get; }
    /// <summary>外观</summary>
    public string prefab { set; get; }
    /// <summary>等级，宝箱等级与宝箱出现的位置（离起点距离）共同决定宝箱掉落</summary>
    public short level { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_BoxConfig



#region 配置表类_PlayerInitConfig
/// <summary>玩家初始属性</summary>
public class PlayerInitConfig : ConfigDataBase
{
    /// <summary>生命值</summary>
    public float mhp { set; get; }
    /// <summary>速度</summary>
    public float speed { set; get; }
    /// <summary>机动</summary>
    public float pmotorized { set; get; }
    /// <summary>功率</summary>
    public float capacity { set; get; }
    /// <summary>近战攻击力</summary>
    public float melee { set; get; }
    /// <summary>攻击速度</summary>
    public float atkSpeed { set; get; }
    /// <summary>光线攻击力</summary>
    public float laser { set; get; }
    /// <summary>实弹攻击力</summary>
    public float cartridge { set; get; }
    /// <summary>填装速度</summary>
    public float reloadSpeed { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_PlayerInitConfig



#region 配置表类_PropertyGainArg
/// <summary>参数类，表示属性增益，用在芯片等地方（增加的属性可以为负）</summary>
public class PropertyGainArg : ConfigDataBase
{
    /// <summary>[1维]属性数组，它的维度与值相同，一个属性必须配合一个值</summary>
    public SuperArrayValue<GameProperty> gamePropertys { set; get; }
    /// <summary>[1维]值数组，它的维度与属性数值相同</summary>
    public SuperArrayValue<float> values { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_PropertyGainArg



#region 配置表类_StuffCostArg
/// <summary>参数类，表示物资消耗，用在芯片、技能消耗等地方</summary>
public class StuffCostArg : ConfigDataBase
{
    /// <summary>[1维]物资类型数组，它的维度与值相同，一个物资类型必须配合一个值</summary>
    public SuperArrayValue<Stuff> stuffs { set; get; }
    /// <summary>[1维]值数组，它的维度与物资类型相同</summary>
    public SuperArrayValue<int> values { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_StuffCostArg



#region 配置表类_SkillArg
/// <summary>参数类，技能与状态特殊效果描述参数</summary>
public class SkillArg : ConfigDataBase
{
    /// <summary>[1维]一些特效效果需要该参数辅助描述效果</summary>
    public SuperArrayValue<EffectCondition> ec { set; get; }
    /// <summary>[1维]逆波兰式</summary>
    public SuperArrayValue<string> rpn { set; get; }
    /// <summary>[1维]数字参数</summary>
    public SuperArrayValue<float> f { set; get; }
    /// <summary>[1维]编号参数</summary>
    public SuperArrayValue<ulong> u { set; get; }
    /// <summary>[1维]布尔参数</summary>
    public SuperArrayValue<bool> b { set; get; }
    /// <summary>[1维]字符串参数</summary>
    public SuperArrayValue<string> s { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_SkillArg



#region 配置表类_StateConfig
/// <summary>状态</summary>
public class StateConfig : ConfigDataBase
{
    /// <summary>名称</summary>
    public string name { set; get; }
    /// <summary>描述</summary>
    public string describe { set; get; }
    /// <summary>状态图标</summary>
    public string prefab { set; get; }
    /// <summary>Buff/Debuff:true为buff</summary>
    public bool isBuff { set; get; }
    /// <summary>最大叠加</summary>
    public int max { set; get; }
    /// <summary>持续时间</summary>
    public int time { set; get; }
    /// <summary>作用间隔：对当前生命值，需要多次进行计算（DOT），这里指定经过多久时间进行1次计算。为0表示仅需要起效1次（在附着上的时候），或者是没有实际意义。</summary>
    public float interval { set; get; }
    /// <summary>标记：美术表现</summary>
    public string mark { set; get; }
    /// <summary>状态优先级：值越大，优先级越高</summary>
    public int priority { set; get; }
    /// <summary>[1维]状态的效果，一个状态可以有多个效果同时起效（依次起效）</summary>
    public SuperArrayValue<StateEffect> stateEffects { set; get; }
    /// <summary>[1维]参数，根据作用类型解释成不同的作用</summary>
    public SuperArrayObj<SkillArg> stateArgs { set; get; }

    #region 自定义区
    #endregion
}
#endregion 配置表类_StateConfig



#region 自动参数类_SuperConfigInform
/// <summary>VBA自动生成的参数库</summary>
public class SuperConfigInform
{
    /// <summary>SuperConfig读取的默认路径</summary>
    public const string SuperConfigDefaultPathOfEasyConfig = "Config/DefaultPath";

    #region 自定义区
    #endregion
}
#endregion 自动参数类_SuperConfigInform
