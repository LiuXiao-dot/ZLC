namespace ZLC.Application;
/// <summary>
/// 程序中的常量或静态不变的值
/// </summary>
public sealed class AppConstant
{
    private static string basePath = string.Empty;

    /// <summary>
    /// Assets目录的路径
    /// </summary>
    public static string BasePath
    {
        get {
            if (basePath == string.Empty) {
                basePath = UnityEngine.Application.dataPath.Remove(UnityEngine.Application.dataPath.Length - 7, 7);
            }

            return basePath;
        }
    }

    /// <summary>
    /// 添加管理器的key(IOC使用)
    /// </summary>
    public const string APP_LAUNCHER_MANAGER = "APP_LAUNCHER_MANAGER";
    /// <summary>
    /// 添加预加载器(IOC使用)
    /// </summary>
    public const string APP_LAUNCHER_PRELOADERS = "APP_LAUNCHER_PRELOADERS";
}