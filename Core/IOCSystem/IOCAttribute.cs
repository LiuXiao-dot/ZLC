namespace ZLC.IOCSystem;
/// <summary>
/// 组件
/// </summary>
/// <remarks>
/// 使用ComponentAttribute标记类,会自动在Container中创建并缓存被标记的类.
/// Container指使用<see cref="ContainerAttribute"/>标记的类型
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = true)]
public class ComponentAttribute : Attribute
{
    /// <summary>
    /// 通过唯一key与<see cref="ContainerAttribute"/>匹配
    /// </summary>
    public string key;

    /// <inheritdoc />
    public ComponentAttribute(string key)
    {
        this.key = key;
    }
}
/// <summary>
/// 容器
/// </summary>
/// <remarks>标记的类被成为Container，可以自动添加<see cref="ComponentAttribute"/>标记的类型,被标记的类型需要是partial类型</remarks>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,AllowMultiple = true)]
public class ContainerAttribute : Attribute
{
    /// <summary>
    /// 通过唯一key与<see cref="ComponentAttribute"/>匹配
    /// </summary>
    public string key;

    /// <inheritdoc />
    public ContainerAttribute(string key)
    {
        this.key = key;
    }
}