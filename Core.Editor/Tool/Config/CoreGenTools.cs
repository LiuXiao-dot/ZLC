using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using ZLC.ConfigSystem;
using ZLCEditor.Attributes;
namespace ZLCEditor.Tool.Config
{
    /// <summary>
    /// 单例SO生成器
    /// </summary>
    [Generator()]
    public class SOSingletonGenTool
    {
        /// <summary>
        /// 创建全部SO单例资产
        /// </summary>
        [Button]
        private void Generate()
        {
           init();
        }
        
        [InitializeOnLoadMethod]
        private static void init()
        {
            SOSingletonEditor.Check(typeof(AssemblyForToolSO));
            // 初始化全部单例
            var soTypes = new List<Type>(10);
            EditorHelper.GetAllChildType(soTypes,EditorHelper.AssemblyFilterType.Custom | EditorHelper.AssemblyFilterType.Internal,typeof(SOSingleton<>));
            foreach (Type temp in soTypes) {
                SOSingletonEditor.Check(temp);
            }
        }
    }
}