using Sirenix.OdinInspector;
using UnityEditorInternal;
using ZLC.ConfigSystem;
using ZLC.EditorSystem;
using ZLC.FileSystem;
using FilePathAttribute = ZLC.FileSystem.FilePathAttribute;
namespace ZLCEditor.Converter
{
    [Tool("基础配置/转换器配置")]
    [FilePath("ConvertToolSO.asset", FilePathAttribute.PathType.XW,true)]
    public sealed class ConvertToolSO : SOSingleton<ConvertToolSO>
    {
        [PropertyTooltip("使用了转换器的程序集")]
        public AssemblyDefinitionAsset[] assemblies;

        [Unity.Collections.ReadOnly] public string codeGenPath = "Assets/AutoScript";

        protected override void OnEnable()
        {
            base.OnEnable();
            // 检测自动生成代码的路径的文件夹是否存在
            FileHelper.CheckDirectory(codeGenPath);
            // 检测是否存在自动生成代码的程序集
            CheckAssemblyDefinition();
        }

        public void CheckAssemblyDefinition()
        {
            var path = Path.Combine(Instance.codeGenPath, "AutoGenerate.asmdef");
            if (File.Exists(path)) {
                return;
            }
            // 创建AssemblyDefinitionAsset资源文件 
            var assemblyDefinitionJson = @"{
            ""name"": ""AutoGenerate"",
            ""rootNamespace"": ""AutoGenerate"",
            ""references"": [
                ""GUID:1aa8bc75fb05ac449974d81d0c61be61"",
                ""GUID:067a053eaf2a3424e9a3ecec1946d5f4"",
                ""GUID:9e24947de15b9834991c9d8411ea37cf"",
                ""GUID:115b5c1946ca8b0489d954d39dbaf616"",
                ""GUID:77aa4aff2a839454dacfd2987407bea2""
                ],
            ""includePlatforms"": [],
            ""excludePlatforms"": [],
            ""allowUnsafeCode"": true,
            ""overrideReferences"": false,
            ""precompiledReferences"": [],
            ""autoReferenced"": true,
            ""defineConstraints"": [],
            ""versionDefines"": [],
            ""noEngineReferences"": false
        }";
            File.WriteAllText(path, assemblyDefinitionJson);
        }
    }
}
