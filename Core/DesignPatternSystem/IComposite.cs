namespace ZLC.DesignPatternSystem;

/// <summary>
/// 组合
/// </summary>
public interface IComposite<in T, in F>
{
    /// <summary>
    /// 将<typeparamref name="T"/>和<typeparamref name="F"/>组合到一起
    /// </summary>
    /// <param name="t"></param>
    /// <param name="f"></param>
    void Composite(T t, F f);
}