using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using ZLC.EditorSystem;
namespace ZLCEditor.Tool.Compile
{
    [Serializable]
    [Tool("基础配置/编译选项")]
    public class ScriptDefineTool
    {
        [LabelText("代码协定")]
        [SerializeField][ReadOnly]private string _contractDefine = "CONTRACTS_FULL";
        
        [LabelText("启用代码协定")]
        [ShowInInspector]
        public bool enableComplex
        {
            get => PlayerSettings
                .GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup)
                .Contains(_contractDefine);
            set
            {
                var defines = PlayerSettings
                    .GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                var hasPartialDefine = defines
                    .Contains(_contractDefine);
                if (value && !hasPartialDefine)
                {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                        $"{defines};{_contractDefine}");
                }
                else if (!value && hasPartialDefine)
                {
                    defines = defines.Remove(defines.IndexOf(_contractDefine, StringComparison.Ordinal),
                        _contractDefine.Length);
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                        defines);
                }

            }   
        }
    }
}