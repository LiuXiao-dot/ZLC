using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using ZLC.IOCSystem;
using ZLCEditor.Attributes;
using ZLCEditor.Converter;
using ZLCEditor.Converter.Data.Code.CodeTree;
namespace ZLCEditor.Tool;
/// <summary>
/// 通过IOC功能生成代码的工具类
/// </summary>
[Serializable]
[Generator()]
public class IOCGeneratorHelper
{
    [ValueDropdown("CheckType")]
    [ShowInInspector]
    public object container;
    
    [FolderPath]
    public string folderPath;

    private IEnumerable<Type> CheckType()
    {
        var temp = new List<Type>();
        EditorHelper.GetAllMarkedType<ContainerAttribute>(temp,EditorHelper.AssemblyFilterType.Custom | EditorHelper.AssemblyFilterType.Internal | EditorHelper.AssemblyFilterType.Other);
        return temp;
    }

    public void Generate()
    {
        
    }

    /// <summary>
    /// 生成IOC代码
    /// </summary>
    /// <returns></returns>
    [Button]
    public void GenerateCode()
    {
        if (container == null) return;
        var containers = ((Type)container).GetAttributes<ContainerAttribute>();
        foreach (var container in containers) {
            var key = container.key;
            CheckComponent(key);
        }
        void CheckComponent(string key)
        {
            // 找到所有key的Component
            var temp = new List<Type>();
            EditorHelper.GetAllMarkedType<ComponentAttribute>(temp,EditorHelper.AssemblyFilterType.Custom | EditorHelper.AssemblyFilterType.Internal);
            foreach (var component in temp) {
                var cs = component.GetAttributes<ComponentAttribute>();
                foreach (var c in cs) {
                    if (c.key == key) {
                        Debug.Log($"{component.FullName}");
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 生成AppLauncher
    /// </summary>
    private void GenerateAppLauncher()
    {
        // 生成IManager和IResLoader的初始化代码
        
        using var factory = NodeFactory.instance ?? new NodeFactory();
        using var writer = new CodeFileWriter();

        var _namespaceNode = factory.CreateNamespace("ZLC.Application");
        _namespaceNode.AddUsingNode("ZLC.IOCSystem");
        _namespaceNode.AddUsingNode("ZLC.Common");
        _namespaceNode.AddUsingNode("System.Collections.Generic");
        _namespaceNode.AddStatement(@$"
    [Container(AppConstant.APP_LAUNCHER_MANAGER)]
    [Container(AppConstant.APP_LAUNCHER_PRELOADERS)]
    public partial class AppLauncher : AAppLauncher
    {{
        protected override void RegisterManagers()
        {{
            
        }}
        protected override IEnumerator<ILoader> InitPreloaders()
        {{
            return null;
        }}
    }}
");
    }
}