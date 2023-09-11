namespace ZLC.DesignPatternSystem;

/// <summary>
/// 装饰器模式
/// </summary>
public interface IDecorator<T>
{
    /// <summary>
    /// 使用该方法装饰<paramref name="t"/>,并返回装饰后的新的<typeparamref name="T"/>
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    T Decorate(T t);
}