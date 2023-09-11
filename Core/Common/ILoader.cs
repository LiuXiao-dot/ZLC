namespace ZLC.Common;
/// <summary>
/// 加载器
/// </summary>
public interface ILoader
{
    /// <summary>
    /// 当加载进度发生变化时，调用onProgress方法.
    /// </summary>
    /// <param name="onProgress">
    /// 加载进度发生变化
    /// 参数1：已完成任务数量
    /// 参数2：总任务数量
    /// </param>
    void Load(Action<int, int> onProgress = null);
}
/// <summary>
/// 加载器的监听器
/// </summary>
/// <remarks>可以监听<see cref="ILoader"/></remarks>
public interface ILoadListener
{
    /// <summary>
    /// 有加载完成项时调用
    /// </summary>
    /// <param name="finished">加载进度</param>
    /// <param name="total">全部加载项数量</param>
    void OnLoad(int finished, int total);

    /// <summary>
    /// 加载完成时调用
    /// </summary>
    void OnLoadFinshed();
}