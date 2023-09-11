using UnityEngine;
using UnityEngine.SceneManagement;
using ZLC.CacheSystem;
using ZLC.Common;
using ZLC.ConfigSystem;
using ZLC.ResSystem;
using ZLC.UISystem;
namespace ZLC.Application;

/// <summary>
/// 程序启动器抽象类
/// <remarks>由于各个管理器都是继承自接口，为了不使用反射就将管理器注册到AppLauncher中，需要生成继承自AAppLauncher的类，并生成注册的代码</remarks>
/// </summary>
public abstract class AAppLauncher : MonoBehaviour, IAppLauncher
{
    /// <summary>
    /// 游戏配置
    /// </summary>
    protected AppConfigSO appConfig;

    /// <summary>
    /// 包含所有启动时注册的管理器，可以通过该游戏启动器获取管理器，也可以自行为管理器设置单例模式
    /// </summary>
    private Dictionary<Type, IManager> _components;

    private void Awake()
    {
        if (IAppLauncher.Instance != null) {
            Debug.LogError($"存在多个GameLauncher:\r\n1:{((MonoBehaviour)IAppLauncher.Instance).gameObject.name}\r\n2:{gameObject.name}");
            return;
        }
        IAppLauncher.Instance = this;
        _components = new Dictionary<Type, IManager>();
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 初始化资源加载器与配置
    /// </summary>
    private void Start()
    {
        RegisterManagers();
        var loader = GetManager<IResLoader>();
        if (loader == null) {
            Debug.LogError("无资源加载器，如果要启用GameLauncher，需要继承IResLoader实现，并使用ComponentAttribute标记");
            return;
        }
        loader.Load((finished, all) =>
        {
            if (finished == all) {
                loader.LoadAsset<AppConfigSO>("AppConfigSO.asset", EnterGame);
            }
        });
    }

    /// <summary>
    /// 通过反射注册所有管理器
    /// 使用IOC,通过IOCAttribtue下的几个Attribtue进行标记
    /// key值为：APP_LAUNCHER_MANAGER
    /// </summary>
    protected abstract void RegisterManagers();

    /// <summary>
    /// 初始化所有管理器
    /// 此时已经加载完了GameConfigSO
    /// </summary>
    private void InitManagers()
    {
        foreach (var component in _components) {
            component.Value.Init();
        }
    }
    
    /// <summary>
    /// 初始化预加载器
    /// 使用IOC,通过IOCAttribtue下的几个Attribtue进行标记
    /// key值为：APP_LAUNCHER_PRELOADERS
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator<ILoader> InitPreloaders();

    /// <summary>
    /// 正式进入游戏
    /// 0.初始化所有注册过的管理器
    /// 1.展示启动场景（默认已展示）
    /// 2.加载并打开UI场景
    /// 3.移动端从配置文件中读取权限需求并检测
    /// 4.加载配置数据
    /// 5.展示加载进度条
    /// </summary>
    private void EnterGame(bool success, AppConfigSO appConfig)
    {
        if (!success) {
            Debug.LogError("加载游戏配置数据失败,请尝试重新启动游戏");
            return;
        }
        this.appConfig = appConfig;
        InitManagers();
        ShowUIScene();
        #if UNITY_ANDROID || UNIRY_IOS
        CheckPermission();
        #endif
    }
    
    /// <summary>
    /// 展示UI场景
    /// </summary>
    private void ShowUIScene()
    {
#if ZLC_DEBUG
        appConfig.uiSceneName = Path.GetExtension(appConfig.uiSceneName) == ".unity" ? appConfig.uiSceneName : $"{appConfig.uiSceneName}.unity";
#endif
        GetManager<IResLoader>().OpenScene(appConfig.uiSceneName, LoadSceneMode.Single, (success, uiScene) =>
        {
            if (!success) {
                Debug.LogError("加载UI场景失败");
                return;
            }
            ShowMainWindow();
        });
    }

    /// <summary>
    /// 展示主窗口
    /// </summary>
    private void ShowMainWindow()
    {
        var windowManager = GetManager<IWindowManager>();
        void OnProgress(int progress, int total)
        {
            if (progress == total) {

                // 初始化窗口
                Action loadFinished = () =>
                {
                    windowManager.OpenMainWindow();
                };
                windowManager.OpenLoadingWindow(new object[]
                {
                    (FloatCache)1, loadFinished
                });
                var resLoader = GetManager<IResLoader>();
                resLoader.EnableRecord();
                var preloaders = InitPreloaders();
                while (preloaders.MoveNext()) {
                    preloaders.Current?.Load();
                }
                resLoader.DisableRecord();
            }
        }
        windowManager.Load(OnProgress);
    }
#if UNITY_ANDROID || UNIRY_IOS 
    /// <summary>
    /// 权限检测
    /// </summary>
    private void CheckPermission()
    {

    }
#endif
    /// <inheritdoc />
    public M GetManager<M>() where M : IManager
    {
        if (_components.TryGetValue(typeof(M), out var instance)) {
            return (M)instance;
        }
        return default(M);
    }
    
    private void OnDestroy()
    {
        foreach (var kv in _components) {
            kv.Value.Dispose();
        }
        _components.Clear();
    }
}