using ZLC.Common;
namespace ZLC.UISystem;

/// <summary>
/// 窗口管理器
/// </summary>
public interface IWindowManager : IManager, ILoader
{
    /// <summary>
    /// 打开主窗口
    /// </summary>
    void OpenMainWindow();
    /// <summary>
    /// 打开默认加载窗口
    /// </summary>
    /// <param name="objects"></param>
    void OpenLoadingWindow(params object[] objects);
    /// <summary>
    /// 关闭窗口
    /// </summary>
    /// <param name="windowID"></param>
    void CloseWindow(int windowID);
}