using System.Reflection;
using UnityEditor;
using UnityEngine;
using ZLC.SerializeSystem;
using ZLC.WindowSystem;
using ZLCEditor.Converter;
using ZLCEditor.Converter.Data;
using ZLCEditor.Converter.Data.Code;
using ZLCEditor.Converter.Data.Code.CodeTree;
namespace ZLCEditor.WindowSystem
{
    /// <summary>
    /// 根据WindowViewData生成View,Ctl,Model，WindowID等代码
    /// 许多方法的代码使用partial的原因：如果Unity支持增量原生成器，可以有机会进行优化，通过原生成器生成代码。
    /// </summary>
    public class WindowView_Code_Converter : IDisposable
    {
        private const string namespaceName = "ZLC.WindowSystem";

        public void Convert(IList<WindowViewData> viewDatas)
        {
            using var factory = NodeFactory.instance ?? new NodeFactory();
            using var writer = CodeFileWriter.instance ?? new CodeFileWriter();
            var windowManager = factory.CreateNamespace(namespaceName);
            windowManager.AddUsingNode("System");
            var windowManagerClass = windowManager.AddClass("WindowConfig", Arm.Public, CodeKeyword.@sealed,new List<Type>(){typeof(IWindowConfig)});
            var createMethod = windowManagerClass.AddMethod("CreateWindowCtl", Arm.Public, new[]
            {
                factory.CreateParamterNode(typeof(int), "id")
            }, typeof(IWindowCtl));
            createMethod.AddStatement("switch(id){");

            foreach (var viewData in viewDatas) {
                createMethod.AddStatement($"\tcase WindowID.{viewData.GetName()}:");
                createMethod.AddStatement($"\t\treturn new {viewData.GetCtlName()}();");
                Convert(viewData);
            }
            createMethod.AddStatement("\tdefault:");
            createMethod.AddStatement("\t\tthrow new ArgumentOutOfRangeException(nameof(id), id, null);");
            createMethod.AddStatement("}");
            writer.Write(new Node_String_Converter().Convert(windowManager), $"{WindowToolSO.Instance.codeDirectory}/WindowConfig.cs");
        }

        public void Convert(WindowViewData viewData)
        {
            var factory = NodeFactory.instance ?? new NodeFactory();
            var writer = CodeFileWriter.instance ?? new CodeFileWriter();
            var windowIDSO = AssetDatabase.LoadAssetAtPath<ILEnum>("Assets/Plugins/ZLCEngine/EditorConfigs/WindowID.asset");
            var nodeStringConverter = new Node_String_Converter();
            var prefab = viewData.prefab;
            var kvs = WindowToolSO.Instance.KV;
            var kvs2 = WindowToolSO.Instance.KV2;

            var windowName = viewData.GetName();

            // 刷新WindowID
            if (!windowIDSO.kvs.ContainsKey(windowName)) {
                windowIDSO.kvs.Add(windowName, 0);
                windowIDSO.FormalizeEnumValues();
                using var ilFactory = new ILFactory();
                var idCode = ILHelper.Convert<ILEnum, string>(windowIDSO);
                writer.Write(idCode, windowIDSO.GetPath());
            }
            if (!viewData.needGenerate) return;

            // 创建和刷新view
            var view = factory.CreateNamespace(namespaceName);
            var viewClass = view.AddClass(viewData.GetViewName(), Arm.Public, CodeKeyword.@sealed, new List<Type>()
            {
                typeof(AWindowView)
            });

            // 创建和刷新autoCtl
            var autoCtl = factory.CreateNamespace(namespaceName);
            var autoCtlClass = autoCtl.AddClass(viewData.GetCtlName(), Arm.Public, CodeKeyword.partial);
            autoCtlClass.AddStatement($@"private {viewClass.className} _view;
        private WindowID _id;
        public void SetView(AWindowView view)
        {{
            this._view = ({viewClass.className})view;
            this._id = this._view.windowID;
        }}

        public AWindowView GetView()
        {{
            return _view;
        }}
        
        public WindowID GetWindowID()
        {{
            return _id;
        }}");
            writer.Write(nodeStringConverter.Convert(autoCtl), viewData.GetCtlPath());

            // 创建selfCtl
            if (!File.Exists(viewData.GetSelfCtlPath())) {
                var selfCtl = factory.CreateNamespace(namespaceName);
                var selfCtlClass = selfCtl.AddClass(viewData.GetCtlName(), Arm.Public, CodeKeyword.partial, new List<Type>()
                {
                    typeof(IWindowCtl)
                });
                selfCtlClass.AddStatement($@"
        /// <summary>
        /// 设置参数
        /// </summary>
        public void SetValue(object args)
        {{
        }}

        /// <summary>
        /// 打开
        /// </summary>
        public void Open()
        {{
        }}

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {{
        }}

        /// <summary>
        /// 恢复
        /// </summary>
        public void Resume()
        {{
        }}

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {{
        }}

        /// <summary>
        /// 已开启界面的情况下再次打开(只有该界面在同层界面中的最上层会触发)
        /// </summary>
        public void ReOpen()
        {{
        }}");
                writer.Write(nodeStringConverter.Convert(selfCtl), viewData.GetSelfCtlPath());
            }


            // 提取需要生成的组件
            var usings = new HashSet<string>()
            {
                "UnityEngine"
            };
            var childs = ForEach(prefab.transform);
            foreach (var child in childs) {
                var childGo = child.gameObject;
                var name = childGo.name;
                var splits = name.Split('_');
                if (splits.Length <= 1) continue;
                var components = splits[0];
                var ks = components.Split('|');
                foreach (var k in ks) {
                    if (kvs.TryGetValue(k, out var v)) {
                        if (!usings.Contains(((Type)v).Namespace)) {
                            usings.Add(((Type)v).Namespace);
                        }
                        var fieldBaseName = name.Replace(components, "");
                        viewClass.AddField(v, $"{fieldBaseName}_{k}");
                    }
                    if (kvs2.TryGetValue(k, out v)) {
                        if (!usings.Contains(((Type)v).Namespace)) {
                            usings.Add(((Type)v).Namespace);
                        }
                        var fieldBaseName = name.Replace(components, "");
                        viewClass.AddField(v, $"{fieldBaseName}_{k}");
                    }
                }
            }
            foreach (string @using in usings) {
                view.AddUsingNode($"using {@using};");
            }
            writer.Write(nodeStringConverter.Convert(view), viewData.GetViewPath());
        }
        
        /// <summary>
        /// 遍历全部transform
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static IEnumerable<Transform> ForEach(Transform transform)
        {
            var childCount = transform.childCount;
            var kvs2 = WindowToolSO.Instance.KV2;
            for (int i = 0; i < childCount; i++) {
                var child = transform.GetChild(i);
                
                var childGo = child.gameObject;
                var name = childGo.name;
                var splits = name.Split('_');
                if (splits.Length > 1) {
                    var components = splits[0];
                    var ks = components.Split('|');
                    var ignoreChild = false;
                    foreach (var k in ks) {
                        if (kvs2.ContainsKey(k)) {
                            ignoreChild = true;
                            break;
                        }
                    }
                    yield return child;
                    if(ignoreChild)
                        continue;
                }
                
                var temp = ForEach(child);
                foreach (var childTemp in temp) {
                    yield return childTemp;
                }
            }
        }

        /// <summary>
        /// 同步View组件和Prefab间的组件引用
        /// </summary>
        public bool SyncViewReference(WindowViewData[] viewDatas)
        {
            var length = viewDatas.Length;
            if (EditorUtility.DisplayCancelableProgressBar("UGUI_Window", "组件同步中......", 0)) {
                return true;
            }

            var index = 0;
            foreach (var viewData in viewDatas) {
                if (EditorUtility.DisplayCancelableProgressBar("UGUI_Window", "组件同步中......", 1f * index / length)) {
                    break;
                }
                index++;
                var prefab = viewData.prefab;
                var viewScript = AssetDatabase.LoadAssetAtPath<MonoScript>(viewData.GetViewPath());
                if (viewScript == null || viewScript.GetClass() == null) {
                    EditorUtility.ClearProgressBar();
                    return false;
                }
                var componentType = viewScript.GetClass();
                // 给prefab添加view脚本，并为view脚本赋值
                if (prefab.TryGetComponent<AWindowView>(out var view)) {
                    if (view.GetType().Name != componentType.Name) {
                        GameObject.DestroyImmediate(view, true);
                        view = (AWindowView)prefab.AddComponent(componentType);
                    }
                } else {
                    view = (AWindowView)prefab.AddComponent(componentType);
                }
                if (int.TryParse(prefab.name,out int result)) {
                    view.windowID = result;
                }
                // 为组件的所有字段赋值
                var childs = ForEach(prefab.transform);
                var kvs = WindowToolSO.Instance.KV;
                var kv2s = WindowToolSO.Instance.KV2;

                foreach (var child in childs) {
                    var childGo = child.gameObject;
                    var name = childGo.name;
                    var splits = name.Split('_');
                    if (splits.Length <= 1) continue;
                    var components = splits[0];
                    var ks = components.Split('|');
                    foreach (var k in ks) {
                        if (kvs.TryGetValue(k, out SType _)) {
                            var fieldBaseName = name.Replace(components, "");
                            var fieldName = $"{fieldBaseName}_{k}";
                            var fieldInfo = componentType.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
                            if (fieldInfo != null) fieldInfo.SetValue(view, childGo.GetComponent(fieldInfo.FieldType));
                        }
                        if (kv2s.TryGetValue(k, out SType? _)) {
                            var fieldBaseName = name.Replace(components, "");
                            var fieldName = $"{fieldBaseName}_{k}";
                            var fieldInfo = componentType.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
                            if (fieldInfo != null) fieldInfo.SetValue(view, childGo.GetComponent(fieldInfo.FieldType));
                        }
                    }
                }

                PrefabUtility.SavePrefabAsset(viewData.prefab);
                var time = File.GetLastWriteTime(viewData.prefabPath);
                WindowToolSO.Instance.Set(viewData.prefabPath, time.Ticks, out _);
            }
            EditorUtility.ClearProgressBar();
            return true;
        }

        ~WindowView_Code_Converter()
        {
            Dispose();
        }
        public void Dispose()
        {
        }
    }
}