namespace ZLC.Common;
/// <summary>
/// 功能管理器
/// </summary>
/// <remarks>继承IDisposable接口，当Manager被消耗时执行Destroy操作</remarks>
public interface IManager : IDisposable
{
    /// <summary>
    /// 初始化管理器
    /// </summary>
    void Init();
}