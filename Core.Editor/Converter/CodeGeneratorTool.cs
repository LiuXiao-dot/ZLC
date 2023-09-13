using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using ZLC.ConfigSystem;
using ZLC.EditorSystem;
using ZLC.Utils;
using ZLCEditor.Converter.Data;
using ZLCEditor.Data;
using ZLCEditor.Tool;
namespace ZLCEditor.Converter
{
    /// <summary>
    /// 转换器工具
    /// </summary>
    [Tool("生成工具/代码生成", needInit: true)]
    public sealed class CodeGeneratorTool : IDisposable
    {
        private List<Type> _types;
        private List<object> _instances;
        public List<TempButton> _buttons;

        public void RefreshInstances()
        {
            if (_instances != null) return;
            _types = new List<Type>();
            EditorHelper.GetAllChildType<IILData>(_types, EditorHelper.AssemblyFilterType.Custom | EditorHelper.AssemblyFilterType.Internal);
            _instances = new List<object>(_types.Count);
            _buttons = new List<TempButton>();
            // 添加各个类到子树中，
            var index = 0;
            for (int i = 0; i < _types.Count; i++) {
                var type = _types[i];
                if (TypeHelper.IsChildOf(type, typeof(UnityEngine.ScriptableObject))) {
                    var tempSos = AssetDatabase.FindAssets($"t:{type.Name}")
                        .Select(temp => AssetDatabase.GUIDToAssetPath(temp))
                        .Select(temp => AssetDatabase.LoadAssetAtPath<ScriptableObject>(temp));
                    if (tempSos.Count() == 0) {
                        if (TypeHelper.IsChildOf(type, typeof(SOSingleton<>))) {
                            var check = type.GetGenericBaseType(typeof(SOSingleton<>)).GetMethod("Check", BindingFlags.Static | BindingFlags.NonPublic);
                            check?.Invoke(null, null);
                        }
                        Debug.LogWarning($"{type.FullName}没有创建实例");
                    } else {
                        foreach (ScriptableObject tempSo in tempSos) {
                            _instances.Add(tempSo);
                            _buttons.Add(new TempButton()
                            {
                                index = index++,
                                name = tempSo.name,
                            });
                        }
                    }
                }
            }
        }
        
        public void InitTool(OdinMenuTree tree)
        {
            RefreshInstances();
            foreach (var instance in _instances) {
                var so = (ScriptableObject)instance;
                tree.Add($"生成工具/代码生成/{instance.GetType().Name}/{so.name}", so);
            }
        }

        public void OnClickTempButton(TempButton tempButton)
        {
            var instance = _instances[tempButton.index];
            if (instance is IILCode ilCode) {
                using (var factory = ILHelper.Instance) {
                    var code = (string)ILHelper.Convert(instance, typeof(string));
                    using (var writer = new CodeFileWriter()) {
                        writer.Write(code, ilCode.GetPath());
                    }
                }
            }
        }

        [Button("刷新全部代码")]
        public void RefreshAll()
        {
            RefreshInstances();
            using (var factory = ILHelper.Instance) {
                using (var writer = new CodeFileWriter()) {
                    foreach (var instance in _instances) {
                        if (instance is IILCode ilCode) {
                            var code = (string)ILHelper.Convert(instance, typeof(string));
                            writer.Write(code, ilCode.GetPath());
                        }
                    }
                }
            }
        }
        public void Dispose()
        {
            _types.Clear();
            _instances.Clear();
            _buttons.Clear();
        }
    }
}