using System.Reflection;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using ZLC.ConfigSystem;
using ZLC.EditorSystem;
using ZLC.Utils;
using ZLCEditor.Utils;
namespace ZLCEditor.Tool
{

    /// <summary>
    /// 工具的菜单界面
    /// </summary>
    internal sealed class ToolMenu : OdinMenuEditorWindow
    {
        #region NonSerialized
        /// <summary>
        /// 当前脚本所在的程序集
        /// </summary>
        private Assembly defaultAssembly = typeof(ToolMenu).Assembly;
        /// <summary>
        /// 所有工具的缓存列表
        /// </summary>
        private List<object> tools = new List<object>();
        #endregion

        /// <summary>
        /// 展示工具菜单
        /// </summary>
        [MenuItem("Tools/XW菜单")]
        private static void ExcuteToolMenu()
        {
            var window = EditorWindow.GetWindow<ToolMenu>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }
        
        /// <summary>
        /// 构建工具菜单
        /// </summary>
        /// <returns></returns>
        protected override OdinMenuTree BuildMenuTree()
        {
            RefreshAllTools();
            OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true);
            tree.Config.DrawSearchToolbar = true;
            tree.Config.DefaultMenuStyle.Height = 22;
            var treeTypeParam = new Type[]
            {
                typeof(OdinMenuTree)
            };
            var treeObjParam = new object[]
            {
                tree
            };
            foreach (var temp in tools) {
                var tempTool = temp.GetType().GetCustomAttribute<ToolAttribute>();
                tree.Add(tempTool.path, temp);
                if (tempTool.needInit) {
                    var methodInfo = temp.GetType().GetMethod("InitTool", treeTypeParam);
                    if (methodInfo != null) {
                        methodInfo.Invoke(temp, treeObjParam);
                    } else {
                        Debug.LogWarning($"{tempTool.path}的needInit=true,但是并没有实现InitTool(OdinMenuTree tree)的方法。");
                    }
                }
            }

            tree.SortMenuItemsByName();
            return tree;
        }

        #region Utils
        /// <summary>
        /// 获取所有工具
        /// 1.继承自ScriptableObject的会查找.asset资产，有多个会展示多个
        /// 2.非ScriptableObject会创建一个实例
        /// </summary>
        /// <returns></returns>
        internal void RefreshAllTools()
        {
            tools.Clear();
            var types = new List<Type>();
            if (AssemblyForToolSO.Instance) {
                var aft = AssemblyForToolSO.Instance;
                EditorAssemblyHelper.GetAttributedTypes<ToolAttribute>(aft.selfAssemblies,types);
                EditorAssemblyHelper.GetAttributedTypes<ToolAttribute>(aft.selfDlls,types);
                EditorAssemblyHelper.GetAttributedTypes<ToolAttribute>(aft.defaultAssemblies,types);
                EditorAssemblyHelper.GetAttributedTypes<ToolAttribute>(aft.defaultDlls,types);
            } else {
                AssemblyHelper.GetAttributedTypes<ToolAttribute>(defaultAssembly,types);
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                AssemblyHelper.GetAttributedTypes<ToolAttribute>(assemblies, defaultAssembly, types);
            }

            foreach (var temp0 in types) {
                if (TypeHelper.IsChildOf(temp0,typeof(UnityEngine.ScriptableObject))) {
                    var tempSos = AssetDatabase.FindAssets($"t:{temp0.Name}")
                        .Select(temp => AssetDatabase.GUIDToAssetPath(temp))
                        .Select(temp => AssetDatabase.LoadAssetAtPath<ScriptableObject>(temp));
                    if (tempSos.Count() == 0) {
                        if (TypeHelper.IsChildOf(temp0, typeof(SOSingleton<>))) {
                            SOSingletonEditor.Check(temp0);
                        }
                        Debug.LogWarning($"{temp0.FullName}没有创建实例");
                    }

                    tools.AddRange(tempSos);
                } else {
                    tools.Add(Activator.CreateInstance(temp0));
                }
            }
        }
        #endregion
    }
}