using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using ZLC.ConfigSystem;
using ZLC.EditorSystem;
using ZLC.Utils;
using FilePathAttribute = ZLC.FileSystem.FilePathAttribute;
namespace ZLCEditor.Tool
{
    [Tool("基础配置/查找的程序集")]
    [FilePath("AssemblyForToolSO.asset", FilePathAttribute.PathType.XWEditor, true)]
    public sealed class AssemblyForToolSO : SOSingleton<AssemblyForToolSO>
    {
        [BoxGroup("ZLC框架提供的程序集")]
        public AssemblyDefinitionAsset[] defaultAssemblies;

        [BoxGroup("ZLC框架提供的程序集")]
        [AssetList(CustomFilterMethod = "CheckName")]
        public DefaultAsset[] defaultDlls;
        
        [PropertyTooltip("许多工具都不会扫描官方或第三方程序集，只扫描自定义程序集。")]
        [BoxGroup("自定义程序集(需要手动添加)")]
        public AssemblyDefinitionAsset[] selfAssemblies;

        [BoxGroup("自定义程序集(需要手动添加)")]
        [AssetList(CustomFilterMethod = "CheckName")]
        public DefaultAsset[] selfDlls;
        
        [BoxGroup("unity官方的程序集")]
        public AssemblyDefinitionAsset[] unityAssemblies;

        [BoxGroup("unity官方的程序集")]
        [AssetList(CustomFilterMethod = "CheckName")]
        public DefaultAsset[] unityDlls;

        [Tooltip("(自动刷新，会添加Assets目录下Plugins目录的程序集)")]
        [BoxGroup("第三方程序集")]
        public AssemblyDefinitionAsset[] otherAssemblies;

        [Tooltip("(自动刷新，会添加Assets目录下Plugins目录的程序集)")]
        [BoxGroup("第三方程序集")]
        [AssetList(CustomFilterMethod = "CheckName")]
        public DefaultAsset[] otherDlls;

        /// <summary>
        /// 检测后缀为dll的文件
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        private bool CheckName(DefaultAsset asset)
        {
            return Path.GetExtension(AssetDatabase.GetAssetPath(asset)) == ".dll";
        }

        /// <summary>
        /// 重置时调用一次刷新程序集
        /// </summary>
        private void Reset()
        {
            RefreshAssemblies();
        }

        [Button("刷新程序集")]
        public void RefreshAssemblies()
        {
            // 刷新ZLC提供的程序集
            var zlcAssembliesPromot = "ZLC*.asmdef"; 
            var zlcDllsPromot = "ZLC*.dll"; 
            EditorHelper.SearchPackagesAssets<AssemblyDefinitionAsset>(zlcAssembliesPromot, temp =>
            {
                defaultAssemblies = temp.ToArray();
            });
            EditorHelper.SearchPackagesAssets<DefaultAsset>(zlcDllsPromot, temp =>
            {
                defaultDlls = temp.ToArray();
            });
            
            // 不处理自定义程序集，如果有空值则移除
            IListHelper.RemoveNulls(selfAssemblies);
            IListHelper.RemoveNulls(selfDlls);
            
            // 刷新Unity自带的程序集
            // Core,UnityEditor.UI,UnityEngine.UI
            var tempAssembliesPromot = "UnityEngine*.asmdef or Unity*.asmdef or UnityEditor*.asmdef";
            var tempDllsPromot = "Unity*.dll or UnityEngine*.dll or UnityEditor*.dll";
            EditorHelper.SearchPackagesAssets<AssemblyDefinitionAsset>(tempAssembliesPromot, temp =>
            {
                unityAssemblies = temp.ToArray();
            });
            EditorHelper.SearchPackagesAssets<DefaultAsset>(tempDllsPromot, temp =>
            {
                unityDlls = temp.ToArray();
            });
            
            // 刷新第三方程序集
            var pluginAssembliesPromot = "dir=Plugins ext:asmdef";
            var pluginDllsPromot = "dir=Plugins ext:dll";
            EditorHelper.SearchPackagesAssets<AssemblyDefinitionAsset>(pluginAssembliesPromot, temp =>
            {
                otherAssemblies = temp.ToArray();
            });
            EditorHelper.SearchPackagesAssets<DefaultAsset>(pluginDllsPromot, temp =>
            {
                otherDlls = temp.ToArray();
            });
        }
    }
}