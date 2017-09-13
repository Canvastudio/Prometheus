//���ĵ����Զ����������ݣ������ֶ��������޸ģ�

/// <summary>
/// ��Ŀ������
/// </summary>
public enum MultipleType
{
    /// <summary>
    /// Ĭ�ϣ�ֻ�ṥ��һ��Ŀ��
    /// </summary>
    Normal,
    /// <summary>
    /// ���Ŀ�꣬������Խ��targetArgʵ�����ѡȡ���Ŀ��
    /// </summary>
    Random,
    /// <summary>
    /// ����Ŀ��
    /// </summary>
    All,
    /// <summary>
    /// ��ɢģʽ��1��ʾ��ɢ�뾶Ϊ1������԰뾶�ڵ�����Ŀ��ͬʱ����˺�
    /// </summary>
    Diffuse_1,
    Diffuse_2,
    Diffuse_3,
    Diffuse_4,
    Diffuse_5,
    Diffuse_6,
}

/// <summary>
/// �������ܵ��ͷŷ�ʽ
/// </summary>
public enum TargetType
{
    /// <summary>
    /// ��Ҷ��Լ��ͷţ�����Ҫָ��Ŀ��
    /// </summary>
    Player,
    /// <summary>
    /// ѡ��һ������ڵĿɼ�����
    /// </summary>
    Monster,
    /// <summary>
    /// ѡ��һ�����ӡ�ԭ���ϰ������еĸ��ӣ���Ӧ�ð��������λ�õĸ����ų�
    /// </summary>
    Block,
    /// <summary>
    /// ѡ��һ�������Ҳ����ϰ��ĸ��ӣ��ų�������ڵĸ���
    /// </summary>
    LightBlock,
    /// <summary>
    /// ѡ��һ��δ�����Ҳ������ϰ��ĸ��ӣ�Ŀǰ������BUG��
    /// </summary>
    DarkBlock,
    /// <summary>
    /// ѡ��һ�������Ҳ����ϰ���������û�����壬�ų�������ڵĸ���
    /// </summary>
    EmptyBlock,
    /// <summary>
    /// ѡ��һ���ϰ����ϰ������ڷ�����δ�����ĸ��
    /// </summary>
    ObstructBlock,
    /// <summary>
    /// ����Ҫѡ��Ŀ�꣬��ʾ����һ��ȫ������
    /// </summary>
    Aoe,
    /// <summary>
    /// ѡ��һ���ϰ������Ҹ�λ��û����̨
    /// </summary>
    ��̨λ��,
    /// <summary>
    /// ͬ��һ������������Ҫ���ѡ��λ��
    /// </summary>
    ȫ��̨λ��,
    /// <summary>
    /// ����ҪѡĿ�꣬��ʾ�ٻ�������
    /// </summary>
    ����,
    /// <summary>
    /// ��δ�������Ĺ������Ҫ���ѡ��Ŀ�꣬��ͨ����multipleTypeΪrandomʱ���ʹ�ã���ʾ���һ��û�б����ֵĹ���
    /// </summary>
    HideMonster,
}

/// <summary>
/// �ٻ������Ϊģʽ
/// </summary>
public enum SpecialAction
{
    /// <summary>
    /// ��ͨ����
    /// </summary>
    NormalAttack,
    /// <summary>
    /// ������ҵ���������
    /// </summary>
    CopyAttack,
}

/// <summary>
/// ����һ�������б�������������Ĺ�������ܱ��ü�����Ϊ��ѡĿ�ꡣ����û��ֱ�Ӱѹ����ù����ã���Ϊ�����������ܱȹ������͸����ӣ���������ֵ����100�������͹����ʱ��Ϳ���Ҫ���һ������
/// </summary>
public enum TargetLimit
{
    /// <summary>
    /// ö��Ĭ��ֵ����ʾû���κι���
    /// </summary>
    None,
    /// <summary>
    /// ����ǿ��1
    /// </summary>
    MonsterQuality_1,
    MonsterQuality_2,
    MonsterQuality_3,
    MonsterQuality_4,
    /// <summary>
    /// �������͵Ĺ��
    /// </summary>
    MonsterType_iron,
    /// <summary>
    /// �����͹����
    /// </summary>
    MonsterType_organisms,
}

/// <summary>
/// ����������������
/// </summary>
public enum PassiveType 
{
    /// <summary>
    /// ����/��������
    /// </summary>
    Property,
    /// <summary>
    /// ȫ�˺����⣬���౻�����ܿ��Ա�������
    /// </summary>
    DamageReduction ,
}

public enum StateEffect
{
    /// <summary>
    /// ��ʱ�ı����ԣ�״̬��ʧ������Ҫ�����
    /// </summary>
    Property,
    /// <summary>
    /// ��Ĭ�������ͷż��ܣ������Խ�ս������û�в���
    /// </summary>
    Silent,
    /// <summary>
    /// �˺�ת�ƣ��ܵ�N���˺������ת�Ƹ��������DOT���㣩������ǹ����д�״̬����ת�Ƹ���ҡ������ǣ�����������ﵽ������״̬�Զ���ʧ
    /// </summary>
    DamageTransfer,
    /// <summary>
    /// �˺����գ��������������˺��ᱻ���գ����������˺��������������ձ��ʽ������HP*5���������㹻���˺���״̬��ʧ
    /// </summary>
    DamageAbsorb,
    /// <summary>
    /// �˺����棬������N���ܵ����˺�����״̬����ʱ��Ϊ�Լ��ظ�M��������ֵ��ע�⣬�������ǻ��յ��˺���������N��M
    /// </summary>
    DamageStore ,
    /// <summary>
    /// ���ᣬ������ܷ���������ʹ�ü��ܣ���������Ա��ͷš�û�в���
    /// </summary>
    Freeze,
    /// <summary>
    /// �����ǳ������һ����������ܹ�����Զ��&��ս��������only��û�в���
    /// </summary>
    Last,
    /// <summary>
    /// ����ָ���ܣ����д�״̬�Ĺ������������ֶ�ѡ��Ŀ��ʱ����ɸѡ���������Ŀ���ǿ��Եģ�������only��û�в���
    /// </summary>
    Imm,
    /// <summary>
    /// ��ը�������������˺����������˺����ʽ
    /// </summary>
    Explode,
}

/// <summary>
/// �˺����ͣ����֮ǰͬ��ö������ɾ������GlobalParameters.cs��
/// </summary>
public enum DamageType
{
    /// <summary>
    /// �����˺�
    /// </summary>
    Physical,
    /// <summary>
    /// ʵ���˺�
    /// </summary>
    Cartridge,
    /// <summary>
    /// �����˺�
    /// </summary>
    Laser,
}

/// <summary>
/// �������ܵ�����Ч��
/// </summary>
public enum SpecialEffect
{
    /// <summary>
    /// ���Σ���Ŀ������ӱ������һ��prefab��ͨ������������Ըı�һ��ʹ�á������Ǳ��Ŀ��prefab������
    /// </summary>
    Transfiguration,
    /// <summary>
    /// �����Ŀ�����λ�û����������󣬹�����ʱ����������ӣ������ϡ��ڽ�����ԭ�򡱣�����ҿ��������������޲���
    /// </summary>
    PositionExchange,
    /// <summary>
    /// ����������ƶ���һ���Ѿ������Ŀո��ӣ�ת�ƺ󣬹�����ʱ����������ӣ������ϡ��ڽ�����ԭ�򡱣�����ҿ��������������޲���
    /// </summary>
    PositionTransfer ,
    /// <summary>
    /// ū��һ�����ͨ��������ܻ���ǿ���ƣ���ʹ�����������Լ������������Ȼ���Թ��������������ﲻ�ᷴ����ͬʱ�����������������޲���
    /// </summary>
    Enslave,
    /// <summary>
    /// ��ɢ�������ɢĿ������N��Debuff������ΪΪN��ע��N�ܴ�����
    /// </summary>
    Disperse,
    /// <summary>
    /// ������ɢ�������ɢĿ�����ϵ�N��Buff������ΪN
    /// </summary>
    OffensiveDisperse,
}


