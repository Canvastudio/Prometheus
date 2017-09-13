public class ActiveSkillsConfig : ConfigDataBase
{
    /// <summary>
    /// ����
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// ����
    /// </summary>
    public string describe { get; set; }
    /// <summary>
    /// Ŀ������(�ͷŷ�ʽ)
    /// </summary>
    public TargetType targetType { get; set; }
    /// <summary>
    /// Ŀ�����ƣ�����ĳЩ���ﲻ������������һ��һά���飬��ʾ�������������
    /// </summary>
    public SuperArray<TargetLimit> targetLimit { get; set; }
    /// <summary>
    /// ��Ŀ�꣬laser��֧��ͬʱ�������Ŀ��
    /// </summary>
    public MultipleType multipleType { get; set; }
    /// <summary>
    /// Ŀ�����(��һ�ηָ����Ŀ��������ڶ��ηָ������������ÿ���˺�Ȩ��)
    /// </summary>
    public SuperArray<float> targetArg { get; set; }
    /// <summary>
    /// ������Ч��laser��֧�ֶ������Ч
    /// </summary>
    public SuperArray<string> effect_fly { get; set; }
    /// <summary>
    /// ������Ч
    /// </summary>
    public SuperArray<string> effect_hit { get; set; }
    /// <summary>
    /// ���
    /// </summary>
    public SuperArray<int> carry { get; set; }
    /// <summary>
    /// ���Լ�����״̬
    /// </summary>
    public SuperArray<ulong> state_to_player { get; set; }
    /// <summary>
    /// ��Ŀ�긽��״̬
    /// </summary>
    public SuperArray<ulong> state_to_monster { get; set; }
    /// <summary>
    /// �˺�
    /// </summary>
    public SuperArray<string> damage { get; set; }
    /// <summary>
    /// ���Ըı�
    /// </summary>
    public SuperArray<string> pro_change { get; set; }
    /// <summary>
    /// ����Ч��
    /// </summary>
    public SpecialEffect specialEffect { get; set; }
    /// <summary>
    /// Ч�����������ݲ�ͬЧ�����������岻ͬ
    /// </summary>
    public SuperArray<string> specialEffectArgs { get; set; }
    /// <summary>
    /// ����ʱ�䣬����ǹ���ļ��ܣ��ͱ�ʾCD
    /// </summary>
    public float costTime { get; set; }
    /// <summary>
    /// �ܺ�
    /// </summary>
    public float usePower { get; set; }
#region �Զ�����
#endregion
}
public class SummonSkillsConfig : ConfigDataBase
{
    /// <summary>
    /// ����
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// ����
    /// </summary>
    public string describe { get; set; }
    /// <summary>
    /// Ŀ������(�ͷŷ�ʽ)
    /// </summary>
    public TargetType targetType { get; set; }
    /// <summary>
    /// ���
    /// </summary>
    public SuperArray<int> carry { get; set; }
    /// <summary>
    /// �ٻ������Ա��ʽ�����û����Ӧ�����Ա�Ĭ��Ϊ0
    /// </summary>
    public SuperArray<string> propertys { get; set; }
    /// <summary>
    /// Ԥ��
    /// </summary>
    public string prefab { get; set; }
    /// <summary>
    /// ����ʱ�䣬����ǹ���ļ��ܣ��ͱ�ʾCD
    /// </summary>
    public float costTime { get; set; }
    /// <summary>
    /// ��Ϊģʽ�����ƹ����������ȣ�
    /// </summary>
    public SpecialAction specialAction { get; set; }
    /// <summary>
    /// ��Ϊ����
    /// </summary>
    public SuperArray<string> speciaArg { get; set; }
#region �Զ�����
#endregion
}
public class SkillPointsConfig : ConfigDataBase
{
    /// <summary>
    /// ����ܱ�ţ�����һ��һά���飬��Ӧ�ü��ܵ㿪��\�����ļ���ID
    /// </summary>
    public SuperArray<ulong> skillIds { get; set; }
    /// <summary>
    /// ��ɫ�����ܵ㣺Ҫ����/�����ü�����Ҫ�ļ��ܵ�������������ĳ��ȣ�������skillids�ĳ��ȣ�����ó���С��skillIds����ʾ�ý�ɫ������ѧ��ü��ܵ���ߵȼ���ע�⣬��ֵ����Ϊ����
    /// </summary>
    public SuperArray<int> characterActivate { get; set; }
#region �Զ�����
#endregion
}
public class PassiveSkillsConfig : ConfigDataBase
{
    /// <summary>
    /// ��������
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// ����
    /// </summary>
    public string describe { get; set; }
    /// <summary>
    /// ��������
    /// </summary>
    public SuperArray<PassiveType> passiveType { get; set; }
    /// <summary>
    /// �����������������ͽ��ͳɲ�ͬ������
    /// </summary>
    public SuperArray<string> args { get; set; }
#region �Զ�����
#endregion
}
public class StateConfig : ConfigDataBase
{
    /// <summary>
    /// ����
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// ����
    /// </summary>
    public string describe { get; set; }
    /// <summary>
    /// Buff/Debuff:trueΪbuff
    /// </summary>
    public bool isBuff { get; set; }
    /// <summary>
    /// ������
    /// </summary>
    public int max { get; set; }
    /// <summary>
    /// ����ʱ��
    /// </summary>
    public int time { get; set; }
    /// <summary>
    /// ��ǣ���������
    /// </summary>
    public string mark { get; set; }
    /// <summary>
    /// ״̬���ȼ�
    /// </summary>
    public int priority { get; set; }
    /// <summary>
    /// Ч����һ��״̬�����ж��Ч��
    /// </summary>
    public SuperArray<StateEffect> stateEffects { get; set; }
    /// <summary>
    /// ����������һ����ά���飬��һά�ĳ��ȵ���Ч����StateEffect���ĳ��ȣ��ڶ�ά����ݲ�ͬЧ���Ĳ�ͬ����ͬ
    /// </summary>
    public SuperArray<string> args { get; set; }

#region �Զ�����
#endregion
}
