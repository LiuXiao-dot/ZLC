namespace ZLC.DesignPatternSystem;

/// <summary>
/// 建造器
/// </summary>
public interface IBuilder<T>
{
    /// <summary>
    /// 建造
    /// </summary>
    /// <returns></returns>
    T Build();
}

/// <summary>
/// 指挥者
/// 通过指挥者修改建造器的构建流程
/// </summary>
public interface IDirector<T,F> where T : IBuilder<F>
{
    // 需要子类自己实现修改建造器的方法
}