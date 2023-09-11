using Sirenix.OdinInspector;
using UnityEditor;
using ZLC.ConfigSystem;
using ZLC.EditorSystem;
using FilePathAttribute = ZLC.FileSystem.FilePathAttribute;
namespace ZLCEditor.ResourceSystem
{
    /// <summary>
    /// Addressables资源同步目录
    /// </summary>
    [Tool("资源管理")]
    [FilePath("ResAutoConfigSO.asset",FilePathAttribute.PathType.XWEditor,true)]
    public sealed class ResAutoConfigSO : SOSingleton<ResAutoConfigSO>
    {
        /// <summary>
        /// 资源目录路径
        /// </summary>
        [LabelText("目录路径")]
        [FolderPath]
        public string[] dirs;

        /// <summary>
        /// 资源同步
        /// </summary>
        [Button(ButtonSizes.Medium,Name = "资源同步")]
        public void SyncAddressalbes()
        {
            EditorUtility.DisplayDialog("资源同步", ResEditorHelper.Sync(dirs),"确认");
        }

        /// <summary>
        /// 构建项目
        /// </summary>
        [Button(ButtonSizes.Medium,Name = "Build")]
        public void BuildProject()
        {
            EditorUtility.DisplayDialog("Build", ResEditorHelper.Build(dirs),"确认");
        }
    }
}