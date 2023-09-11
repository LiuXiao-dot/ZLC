using System.Reflection;
namespace ZLC.SerializeSystem;

/// <summary>
/// 可序列化的Type,编辑器中展示为Type和所有继承自Type的类
/// </summary>
[Serializable]
public class SType
{
    /// <summary>
    /// 同一个type对应同一个SerializableType
    /// </summary>
    private static Dictionary<Type, SType> pools = new Dictionary<Type, SType>();

    /// <summary>
    /// 类型的完整名称
    /// </summary>
    public string fullName;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="type">实际类型</param>
    public SType(Type type)
    {
        fullName = type.Assembly.FullName;
    }

    /// <summary>
    /// 将Type装欢为SType
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static implicit operator SType(Type type)
    {
        if (!pools.ContainsKey(type))
        {
            pools.Add(type,new SType(type));
        }
        return pools[type];
    }

    /// <summary>
    /// 将SType转换为Type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static implicit operator Type(SType type)
    {
        return Type.GetType(type.fullName);;
    }
}