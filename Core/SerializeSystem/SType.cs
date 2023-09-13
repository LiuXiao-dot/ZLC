namespace ZLC.SerializeSystem;

/// <summary>
/// 可序列化的Type,编辑器中展示为Type和所有继承自Type的类
/// </summary>
[Serializable]
public class SType : IEquatable<SType>
{
    /// <summary>
    /// 类型的完整名称
    /// </summary>
    public string fullName;

    /// <summary>
    /// 无参构造
    /// </summary>
    public SType()
    {
        fullName = "";
    }
    
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="type">实际类型</param>
    public SType(Type type)
    {
        fullName = type.FullName;
    }

    /// <summary>
    /// 将Type装欢为SType
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static implicit operator SType(Type type)
    {
        return new SType(type);
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

    public bool Equals(SType other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return fullName == other.fullName;
    }
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((SType)obj);
    }
    public override int GetHashCode()
    {
        return (fullName != null ? fullName.GetHashCode() : 0);
    }
}