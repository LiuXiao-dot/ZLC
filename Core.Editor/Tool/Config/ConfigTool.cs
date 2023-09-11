using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using ZLC.EditorSystem;
namespace ZLCEditor.Tool.Config
{
    /// <summary>
    /// 配置表工具
    /// 功能：
    /// 1.可以选择启用不同的配置表工具
    /// </summary>
    [Serializable]
    [Tool("基础配置/Luban")]
    public class ConfigTool
    {
        [LabelText("Luban配置")]
        [SerializeField][ReadOnly]private string _lubanDefine = "LUBAN_ENABLE";
        
        [LabelText("启用鲁班配置工具")]
        [ShowInInspector]
        public bool enableComplex
        {
            get => PlayerSettings
                .GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup)
                .Contains(_lubanDefine);
            set
            {
                var defines = PlayerSettings
                    .GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                var hasPartialDefine = defines
                    .Contains(_lubanDefine);
                if (value && !hasPartialDefine)
                {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                        $"{defines};{_lubanDefine}");
                }
                else if (!value && hasPartialDefine)
                {
                    defines = defines.Remove(defines.IndexOf(_lubanDefine, StringComparison.Ordinal),
                        _lubanDefine.Length);
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                        defines);
                }

            }   
        }
    }
}