using UnityEditor;
using UnityEngine;
using ZLC.Application;
using ZLC.ConfigSystem;
using ZLC.FileSystem;
namespace ZLCEditor.ConfigSystem;

/// <summary>
/// <see cref="AppConfigSO"/>游戏配置数据编辑器
/// </summary>
[CustomEditor(typeof(AppConfigSO))]
public class AppConfigSOEditor : Editor
{
    /// <inheritdoc />
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("清除日志")) {
            FileHelper.ClearDirectory($"{AppConstant.BasePath}/log");
        }
    }
}