using UnityEngine;
using ZLC.Application;
using ZLC.ResSystem;
namespace ZLC.ConfigSystem;

/// <summary>
/// 单例ScriptableObject
/// </summary>
public abstract class SOSingleton<T> : ScriptableObject where T : SOSingleton<T>
{
    /// <summary>
    /// SO单例
    /// </summary>
    public static T Instance
    {
        get {
            if (_instance != null) return _instance;
            if (UnityEngine.Application.isPlaying) {
                // 运行中，使用加载器加载资源
                IAppLauncher.Get<IResLoader>().LoadAssetSync($"{typeof(T).Name}.asset", out _instance);
            }
            return _instance;
        }
    }
    /// <summary>
    /// SO单例
    /// </summary>
    private static T _instance;

    /// <summary>
    /// 激活时为<see cref="Instance"/>赋值.
    /// </summary>
    protected virtual void OnEnable()
    {
        if (_instance != null && this != _instance) {
            Debug.LogWarning($"存在多个单例SO的实例{typeof(T).FullName},单例会被最新的替换");
        }

        _instance = (T)this;
        if (_instance.name != typeof(T).Name) {
            Debug.LogError($"单例{_instance.name}名称建议是:{typeof(T).Name},可以自动加载，如果是自定义名称,需要手动提前加载才能正常使用.");
        }
    }

    private void OnDisable()
    {
        if (_instance == this) _instance = null;
    }
}