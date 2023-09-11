using ZLCEditor.Converter.Data.Code.CodeTree.Nodes;
namespace ZLCEditor.Converter.Data.Code.CodeTree
{
    /// <summary>
    /// 节点工厂
    /// 要使用代码节点，需要一下代码范围内使用：
    /// using(new NodeFactory())
    /// {
    /// }
    /// </summary>
    public class NodeFactory : IDisposable
    {
        public static NodeFactory instance;
        private NodePool _pool;

        public NodeFactory()
        {
            _pool = new NodePool();
            instance = this;
        }

        public string CreateSimpleClass(string @namespace, IList<string> usings, string className, Arm arm, IList<string> parent, IList<string> content)
        {
            return $@"{string.Join(";\r\n", usings)}
namespace {@namespace}
{{
    {arm} class {className} : {string.Join(",", parent)}
    {{
        {string.Join("\r\n", content)}
    }}
}}";
        }

        /// <summary>
        /// 创建命名空间节点(命名空间节点为根节点)
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <returns></returns>
        public NamespaceNode CreateNamespace(string namespaceName)
        {
            var namespaceNode = _pool.Get<NamespaceNode>();
            namespaceNode.name = namespaceName;
            return namespaceNode;
        }
        
        public ClassNode CreateClass(string className, Arm arm = Arm.Public, CodeKeyword codeKeyword = CodeKeyword.none, IList<Type> parents = null)
        {
            if (!NodeHelper.CheckCodeKeyword(codeKeyword)) {
                return default;
            }
            var classNode = _pool.Get<ClassNode>();
            classNode.className = className;
            classNode.arm = arm;
            classNode.codeKeyword = codeKeyword;
            classNode.parents = parents;
            
            return classNode;
        }
        
        public StructNode CreateStruct(string className, Arm arm = Arm.Public, CodeKeyword codeKeyword = CodeKeyword.none, IList<Type> parents = null)
        {
            if (!NodeHelper.CheckCodeKeyword(codeKeyword)) {
                return default;
            }
            var classNode = _pool.Get<StructNode>();
            classNode.className = className;
            classNode.arm = arm;
            classNode.codeKeyword = codeKeyword;
            classNode.parents = parents;
            
            return classNode;
        }
        
        public EnumNode CreateEnum(string className, Arm arm = Arm.Public, CodeKeyword codeKeyword = CodeKeyword.none, IList<Type> parents = null)
        {
            if (!NodeHelper.CheckCodeKeyword(codeKeyword)) {
                return default;
            }
            var classNode = _pool.Get<EnumNode>();
            classNode.enumName = className;
            classNode.arm = arm;
            classNode.codeKeyword = codeKeyword;
            classNode.parents = parents;
            
            return classNode;
        }

        public FieldNode CreateField(Type fieldType,string fieldName, Arm arm = Arm.Public, CodeKeyword codeKeyword = CodeKeyword.none)
        {
            if (!NodeHelper.CheckCodeKeyword(codeKeyword)) {
                return default;
            }
            var fieldNode = _pool.Get<FieldNode>();
            fieldNode.fieldType = fieldType;
            fieldNode.fieldName = fieldName;
            fieldNode.arm = arm;
            fieldNode.codeKeyword = codeKeyword;
            return fieldNode;
        }
        
        public FieldNode CreateField(string fieldTypeName,string fieldName, Arm arm = Arm.Public, CodeKeyword codeKeyword = CodeKeyword.none)
        {
            if (!NodeHelper.CheckCodeKeyword(codeKeyword)) {
                return default;
            }
            var fieldNode = _pool.Get<FieldNode>();
            fieldNode.fieldTypeName = fieldTypeName;
            fieldNode.fieldName = fieldName;
            fieldNode.arm = arm;
            fieldNode.codeKeyword = codeKeyword;
            return fieldNode;
        }

        public MethodNode CreateMethod(string methodName,Arm arm,IEnumerable<ParamterNode> paramters = null,Type returnType = null,CodeKeyword codeKeyword = CodeKeyword.none)
        {
            if (!NodeHelper.CheckCodeKeyword(codeKeyword)) {
                return default;
            }
            var methodNode = _pool.Get<MethodNode>();
            methodNode.methodName = methodName;
            methodNode.arm = arm;
            methodNode.paramters = paramters;
            methodNode.returnType = returnType;
            methodNode.codeKeyword = codeKeyword;
            return methodNode;
        }

        public StatementNode CreateStatement(string statement)
        {
            var statementNode = _pool.Get<StatementNode>();
            statementNode.statement = statement;
            return statementNode;
        }

        public ParamterNode CreateParamterNode(Type type, string name)
        {
            var node = _pool.Get<ParamterNode>();
            node.type = type;
            node.name = name;
            return node;
        }

        ~NodeFactory()
        {
            Dispose();
        }
        
        public void Dispose()
        {
            _pool?.Dispose();
            instance = null;
        }
        public UsingNode CreateUsingNode(string usingStatement)
        {
            var node = _pool.Get<UsingNode>();
            node.usingStatement = usingStatement;
            return node;
        }

        public void ReleaseNode(ANode node)
        {
            _pool.Release(node);
        }
    }
}