using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 配置表类的基类
/// 该配置表第一行为参数说明，第二行为参数类型，第三行为参数名字
/// </summary>
public abstract class ConfigDataBase
{
    /// <summary>
    /// 主键，唯一
    /// </summary>
    public ulong id { get; set; }

    /// <summary>
    /// 根据ID返回一个配置表对象
    /// </summary>
    public static T GetConfigDataById<T>(ulong id) where T : ConfigDataBase
    {
        return SuperConfig.Instance.GetConfigDataById<T>(id);
    }
    /// <summary>
    /// 根据ID返回一个配置表对象
    /// </summary>
    public static T GetConfigDataById<T>(string idStr) where T : ConfigDataBase
    {
        return GetConfigDataById<T>(ulong.Parse(idStr));
    }

    /// <summary>
    /// 根据类型返回配置表所有对象
    /// </summary>
    public static List<T> GetConfigDataList<T>() where T : ConfigDataBase
    {
        return SuperConfig.Instance.GetConfigDataList<T>();
    }

    /// <summary>
    /// 根据属性名称，查找配置表中第一个符合条件的对象
    /// </summary>
    /// <typeparam name="T">配置表类型</typeparam>
    /// <param name="pro">属性名</param>
    /// <param name="value">属性值</param>
    /// <returns></returns>
    public static T GetConfigDataByProperty<T>(string pro, object value) where T : ConfigDataBase
    {
        return SuperConfig.Instance.GetConfigDataByProperty<T>(pro, value);
    }

    /// <summary>
    /// 判断一个配置中是否存在该id的数据
    /// </summary>
    public static bool Exists<T>(ulong id) where T : ConfigDataBase
    {
        return SuperConfig.Instance.Exists<T>(id);
    }

    /// <summary>
    /// 判断一个配置中是否存在该id的数据
    /// </summary>
    public static bool Exists<T>(string idStr) where T : ConfigDataBase
    {
        return Exists<T>(ulong.Parse(idStr));
    }

}
