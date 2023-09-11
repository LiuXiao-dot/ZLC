using ZLC.Common;
namespace ZLC.Application;

/// <summary>
/// 程序启动器
/// </summary>
/// <remarks>
/// 在程序刚刚启动时创建，作为整个程序的启动类，可以进行各自初始化操作。
/// </remarks>
public interface IAppLauncher
{
    /// <summary>
    /// 程序启动器单例，需要继承IAppLauncher的类主动赋值
    /// </summary>
    private static IAppLauncher _instance;
    /// <summary>
    /// 程序启动器单例，需要继承IAppLauncher的类主动赋值
    /// </summary>
    public static IAppLauncher Instance
    {
        get {
            return _instance;
        }
        set {
                #if ZLC_DEBUG
            if (_instance != null) {
                UnityEngine.Debug.LogWarning($"存在多个IAppLauncher，后添加的将会被忽略.已有的为：{_instance}被忽略的为：{value}");
                return;
            }
                #endif
            _instance = value;
        }
    }

    /// <summary>
    /// 获取管理器
    /// </summary>
    public static M Get<M>() where M : IManager
    {
        return Instance.GetManager<M>();
    }

    /// <summary>
    /// 获取程序中的管理器
    /// </summary>
    /// <typeparam name="M">管理器类型</typeparam>
    /// <returns>管理器实例</returns>
    M GetManager<M>() where M : IManager;
}