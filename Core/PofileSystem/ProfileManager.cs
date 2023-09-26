using UnityEngine;
using ZLC.Application;
using ZLC.Common;
using ZLC.ConfigSystem;
using ZLC.IOCSystem;
namespace ZLC.PofileSystem;

/// <summary>
/// 调试管理器
/// </summary>
[Component(AppConstant.APP_LAUNCHER_MANAGER)]
public class ProfileManager : IManager
{
    /// <summary>
    /// 析构函数
    /// </summary>
    ~ProfileManager()
    {
        Dispose();
    }
    private ILogHandler _defaultHandler;

    /// <inheritdoc />
    public void Dispose()
    {
        Debug.unityLogger.logHandler = _defaultHandler;
    }
    /// <inheritdoc />
    public void Init()
    {
        if (AppConfigSO.Instance == null) {
            Debug.LogError("需要加载GameConfigSO");
            return;
        }
        _defaultHandler = Debug.unityLogger.logHandler;
        switch (AppConfigSO.Instance.logMode) {
            case LogMode.None:
                //Debug.unityLogger.logHandler = null;
                break;
            case LogMode.Default:
                break;
            case LogMode.File:
                Debug.unityLogger.logHandler = new FileLogHandler();
                break;
            default:
                // 复合日志
                var compositeHandler = new CompositeLogHandler();
                compositeHandler.AddHandler(Debug.unityLogger.logHandler);
                compositeHandler.AddHandler(new FileLogHandler());
                Debug.unityLogger.logHandler = compositeHandler;
                break;
        }
        #if ZLC_DEBUG
        Debug.Log("==== 启动ProfileManager =====");
        #endif
    }
}