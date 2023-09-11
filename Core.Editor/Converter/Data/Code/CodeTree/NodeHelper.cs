using UnityEngine;
using ZLCEditor.Converter.Data.Code.CodeTree.Nodes;
namespace ZLCEditor.Converter.Data.Code.CodeTree
{
    public static class NodeHelper
    {
        /// <summary>
        /// 检测keyword是否合法
        /// </summary>
        public static bool CheckCodeKeyword(CodeKeyword keyword)
        {
            if (keyword.HasFlag(CodeKeyword.@sealed) && keyword.HasFlag(CodeKeyword.@static)) {
                Debug.LogError("不能同时设定一个类为static和sealed");
                return false;
            }

            return true;
        }
        
        public static StructNode AddStruct(this IClassable parent, string className, Arm arm = Arm.Public, CodeKeyword codeKeyword = CodeKeyword.none, IList<Type> parents = null)
        {
            var parentNode = (ANode)parent;
            var child = NodeFactory.instance.CreateStruct(className, arm, codeKeyword, parents);
            parentNode.Add(child);
            return child;
        }

        
        public static EnumNode AddEnum(this IClassable parent, string className, Arm arm = Arm.Public, CodeKeyword codeKeyword = CodeKeyword.none, IList<Type> parents = null)
        {
            var parentNode = (ANode)parent;
            var child = NodeFactory.instance.CreateEnum(className, arm, codeKeyword, parents);
            parentNode.Add(child);
            return child;
        }

        public static ClassNode AddClass(this IClassable parent, string className, Arm arm = Arm.Public, CodeKeyword codeKeyword = CodeKeyword.none, IList<Type> parents = null)
        {
            var parentNode = (ANode)parent;
            var child = NodeFactory.instance.CreateClass(className, arm, codeKeyword, parents);
            parentNode.Add(child);
            return child;
        }

        public static FieldNode AddField(this IFieldable parent, Type fieldType, string fieldName, Arm arm = Arm.Public, CodeKeyword codeKeyword = CodeKeyword.none)
        {
            var parentNode = (ANode)parent;
            var child = NodeFactory.instance.CreateField(fieldType, fieldName, arm, codeKeyword);
            parentNode.Add(child);
            return child;
        }
        
        public static FieldNode AddField(this IFieldable parent, string fieldTypeName, string fieldName, Arm arm = Arm.Public, CodeKeyword codeKeyword = CodeKeyword.none)
        {
            var parentNode = (ANode)parent;
            var child = NodeFactory.instance.CreateField(fieldTypeName, fieldName, arm, codeKeyword);
            parentNode.Add(child);
            return child;
        }
        
        public static MethodNode AddMethod(this IMethodable parent,string methodName,Arm arm,IList<ParamterNode> paramters = null,Type returnType = null,CodeKeyword codeKeyword = CodeKeyword.none)
        {
            var parentNode = (ANode)parent;
            var child = NodeFactory.instance.CreateMethod(methodName,arm,paramters,returnType,codeKeyword);
            parentNode.Add(child);
            return child;
        }

        public static StatementNode AddStatement(this IStatementable parent, string statement)
        {
            var parentNode = (ANode)parent;
            var child = NodeFactory.instance.CreateStatement(statement);
            parentNode.Add(child);
            return child;
        }
    }
}