namespace ZLC.DesignPatternSystem;

/// <summary>
/// 工厂接口
/// </summary>
public interface IFactory<T>
{
    /// <summary>
    /// 创建默认<typeparamref name="T"/>实例
    /// </summary>
    /// <returns></returns>
    T Create();
}

/// <summary>
/// 抽象工厂接口
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class AbstractFactory<T> : IFactory<IFactory<T>>
{
    /// <inheritdoc />
    public abstract IFactory<T> Create();
} 