namespace ZLC.DesignPatternSystem;

/// <summary>
/// 命令模式
/// 可以在构造函数中传入命令的执行者
/// </summary>
public interface ICommand
{
    /// <summary>
    /// 触发
    /// </summary>
    void Excute();
}