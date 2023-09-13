using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using ZLC.ConfigSystem;
using ZLC.EditorSystem;
using ZLC.Utils;
using ZLCEditor.Attributes;
using ZLCEditor.Converter;
using Object = System.Object;
using FilePathAttribute = ZLC.FileSystem.FilePathAttribute;

namespace ZLCEditor.Tool
{
    [Tool("生成工具", true)]
    [FilePath("GenToolSO.asset", FilePathAttribute.PathType.XWEditor,true)]
    public class GenToolSO : SOSingleton<GenToolSO>
    {
        private List<Type> _types;
        private List<Object> _instances;

        /// <summary>
        /// 初始化生成工具
        /// </summary>
        /// <param name="tree"></param>
        public void InitTool(OdinMenuTree tree)
        {
            _types = new List<Type>();
            EditorHelper.GetAllMarkedType<GeneratorAttribute>(_types, EditorHelper.AssemblyFilterType.Custom | EditorHelper.AssemblyFilterType.Internal);
            _instances = new List<Object>(_types.Count);
            // 添加各个类到子树中，
            foreach (var temp0 in _types) {
                if (TypeHelper.IsChildOf(temp0, typeof(UnityEngine.ScriptableObject))) {
                    var tempSos = AssetDatabase.FindAssets($"t:{temp0.Name}")
                        .Select(temp => AssetDatabase.GUIDToAssetPath(temp))
                        .Select(temp => AssetDatabase.LoadAssetAtPath<ScriptableObject>(temp));
                    if (tempSos.Count() == 0) {
                        if (TypeHelper.IsChildOf(temp0, typeof(SOSingleton<>))) {
                            SOSingletonEditor.Check(temp0);
                        }
                        Debug.LogWarning($"{temp0.FullName}没有创建实例");
                    } else {
                        foreach (ScriptableObject tempSo in tempSos) {
                            tree.Add($"生成工具/{temp0.Name}", tempSo);
                            _instances.Add(tempSo);
                        }
                    }
                } else {
                    var tempInstance = Activator.CreateInstance(temp0);
                    tree.Add($"生成工具/{temp0.Name}", tempInstance);
                    _instances.Add(tempInstance);
                }
            }
        }

        [Button("一键生成")]
        private void GenerateAll()
        {
            if(_instances == null) return;
            foreach (var instance in _instances) {
                var method = instance.GetType().GetMethod("Generate",BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                method?.Invoke(instance,null);
            }
            
            // 代码生成
            using (var codeTool = new CodeGeneratorTool()) {
                codeTool.RefreshAll();
            }
            EditorUtility.DisplayDialog("一键生成","生成完成","确认");
        }
    }
}