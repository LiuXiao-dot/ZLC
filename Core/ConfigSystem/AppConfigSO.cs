using Sirenix.OdinInspector;
using UnityEngine;
using ZLC.EditorSystem;
using ZLC.PofileSystem;
using FilePathAttribute = ZLC.FileSystem.FilePathAttribute;
namespace ZLC.ConfigSystem;

/// <summary>
/// 游戏配置数据
/// </summary>
[FilePath("AppConfigSO.asset", FilePathAttribute.PathType.XW, true)]
[Tool("基础配置/启动项")]
public class AppConfigSO : SOSingleton<AppConfigSO>
{
    /// <summary>
    /// UI场景的名字
    /// </summary>
    [Header("场景")]
    [LabelText("UI场景")]
    [Sirenix.OdinInspector.FilePath(Extensions = "unity")]
    public string uiSceneName;
    /// <summary>
    /// 游戏场景的名字
    /// </summary>
    [Sirenix.OdinInspector.FilePath(Extensions = "unity")]
    [LabelText("游戏场景")]
    public string gameSceneName;

    /// <summary>
    /// 是否开启默认调试功能
    /// </summary>
    [Header("调试")]
    public LogMode logMode;
}