using UnityEngine;
using ZLC.Utils;
using ZLCEditor.Utils;
namespace ZLCEditor.Tool
{
    public sealed class ToolHelper
    {
        [Flags]
        public enum AssemblyFilterType
        {
            /// <summary>
            /// 自定义的程序集
            /// </summary>
            Custom = 1,
            /// <summary>
            /// Unity的程序集
            /// </summary>
            Unity = 2,
            /// <summary>
            /// 第三方程序集
            /// </summary>
            Other = 4,
            /// <summary>
            /// ZLC内置dll
            /// </summary>
            Internal = 8,
            /// <summary>
            /// 全部
            /// </summary>
            All = Custom | Unity | Other | Internal
        }

        public static void GetAllChildType(List<Type> temp, AssemblyFilterType filterType, Type targetType)
        {
            Action<AssemblyFilterType> getAllChildType = singleType =>
            {
                var assemblyForToolSO = AssemblyForToolSO.Instance;
                switch (singleType) {
                    case AssemblyFilterType.Custom:
                        EditorAssemblyHelper.GetAllChildType(assemblyForToolSO.selfDlls, temp, targetType);
                        EditorAssemblyHelper.GetAllChildType(assemblyForToolSO.selfAssemblies, temp, targetType);
                        break;
                    case AssemblyFilterType.Unity:
                        EditorAssemblyHelper.GetAllChildType(assemblyForToolSO.unityDlls, temp, targetType);
                        EditorAssemblyHelper.GetAllChildType(assemblyForToolSO.unityAssemblies, temp, targetType);
                        break;
                    case AssemblyFilterType.Other:
                        EditorAssemblyHelper.GetAllChildType(assemblyForToolSO.otherDlls, temp, targetType);
                        EditorAssemblyHelper.GetAllChildType(assemblyForToolSO.otherAssemblies, temp, targetType);
                        break;
                    case AssemblyFilterType.Internal:
                        EditorAssemblyHelper.GetAllChildType(assemblyForToolSO.defaultDlls, temp, targetType);
                        EditorAssemblyHelper.GetAllChildType(assemblyForToolSO.defaultAssemblies, temp, targetType);
                        break;
                    default:
                        Debug.LogError($"filterType数据分割错误.source:{filterType} single:{singleType}");
                        break;
                }
            };
            
            EnumHelper.ForEachFlag(filterType, getAllChildType);
        }
        
        /*public static void GetAllChildType(List<Type> temp, AssemblyFilterType filterType, Type targetType)
        {
            var assemblyForToolSO = AssemblyForToolSO.Instance;
            switch (filterType) {
                case AssemblyFilterType.Custom:
                    EditorAssemblyHelper.GetAllChildType(assemblyForToolSO.selfDlls, temp, targetType);
                    EditorAssemblyHelper.GetAllChildType(assemblyForToolSO.selfAssemblies, temp, targetType);
                    break;
                case AssemblyFilterType.Unity:
                    EditorAssemblyHelper.GetAllChildType(assemblyForToolSO.unityDlls, temp, targetType);
                    EditorAssemblyHelper.GetAllChildType(assemblyForToolSO.unityAssemblies, temp, targetType);
                    break;
                case AssemblyFilterType.Other:
                    EditorAssemblyHelper.GetAllChildType(assemblyForToolSO.otherDlls, temp, targetType);
                    EditorAssemblyHelper.GetAllChildType(assemblyForToolSO.otherAssemblies, temp, targetType);
                    break;
                case AssemblyFilterType.All:
                    EditorAssemblyHelper.GetAllChildType(assemblyForToolSO.selfDlls, temp, targetType);
                    EditorAssemblyHelper.GetAllChildType(assemblyForToolSO.selfAssemblies, temp, targetType);
                    EditorAssemblyHelper.GetAllChildType(assemblyForToolSO.unityDlls, temp, targetType);
                    EditorAssemblyHelper.GetAllChildType(assemblyForToolSO.unityAssemblies, temp, targetType);
                    EditorAssemblyHelper.GetAllChildType(assemblyForToolSO.otherDlls, temp, targetType);
                    EditorAssemblyHelper.GetAllChildType(assemblyForToolSO.otherAssemblies, temp, targetType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterType), filterType, null);
            }
        }*/

        public static void GetAllChildType<T>(List<Type> temp, AssemblyFilterType filterType)
        {
            GetAllChildType(temp, filterType, typeof(T));
        }

        public static void GetAllMarkedType<T>(List<Type> temp, AssemblyFilterType filterType) where T : Attribute
        {
            GetAllMarkedType(temp,filterType,typeof(T));
        }

        public static void GetAllMarkedType(List<Type> temp, AssemblyFilterType filterType, Type targetType)
        {
            Action<AssemblyFilterType> getAllChildType = singleType =>
            {
                var assemblyForToolSO = AssemblyForToolSO.Instance;
                switch (singleType) {
                    case AssemblyFilterType.Custom:
                        EditorAssemblyHelper.GetAttributedTypes(assemblyForToolSO.selfDlls, temp, targetType);
                        EditorAssemblyHelper.GetAttributedTypes(assemblyForToolSO.selfAssemblies, temp, targetType);
                        break;
                    case AssemblyFilterType.Unity:
                        EditorAssemblyHelper.GetAttributedTypes(assemblyForToolSO.unityDlls, temp, targetType);
                        EditorAssemblyHelper.GetAttributedTypes(assemblyForToolSO.unityAssemblies, temp, targetType);
                        break;
                    case AssemblyFilterType.Other:
                        EditorAssemblyHelper.GetAttributedTypes(assemblyForToolSO.otherDlls, temp, targetType);
                        EditorAssemblyHelper.GetAttributedTypes(assemblyForToolSO.otherAssemblies, temp, targetType);
                        break;
                    case AssemblyFilterType.Internal:
                        EditorAssemblyHelper.GetAttributedTypes(assemblyForToolSO.defaultDlls, temp, targetType);
                        EditorAssemblyHelper.GetAttributedTypes(assemblyForToolSO.defaultAssemblies, temp, targetType);
                        break;
                    default:
                        Debug.LogError($"filterType数据分割错误.source:{filterType} single:{singleType}");
                        break;
                }
            };
            EnumHelper.ForEachFlag(filterType, getAllChildType);
        }
    }
}