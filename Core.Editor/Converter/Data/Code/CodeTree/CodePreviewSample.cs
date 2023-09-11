/*using Sirenix.OdinInspector;
using UnityEngine;
using ZLC;
namespace ZLCEditor.CodeTree
{
    [Tool("生成工具/代码生成/CodeTree测试")]
    public class CodePreviewSample
    {
        [TextArea(5,30)]
        public string code;
        
        [Button]
        public void GenerateClass()
        {
            using (var factory = new NodeFactory()) {
                using (var ilFactory = new ILFactory()) {
                    var namespaceNode = factory.CreateNamespace("NamespaceName");
                    var classNode = namespaceNode.AddClass("ClassName",Arm.Public,CodeKeyword.partial, new List<Type>(1){typeof(MonoBehaviour)});
                    classNode.AddField(typeof(Button),"testBtn",Arm.Public,CodeKeyword.none);
                    classNode.AddMethod("TestMethod",Arm.Public,null,typeof(CodePreviewSample),CodeKeyword.@virtual);
                    code = ILHelper.Convert<ANode, string>(namespaceNode);
                }
            }
        }
    }
}*/