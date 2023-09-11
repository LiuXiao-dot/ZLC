using ZLC.Utils;
namespace ZLCEditor.Converter.Data.Code.CodeTree.Nodes
{
    /// <summary>
    /// 类节点
    /// </summary>
    public class ClassNode : ANode, IFieldable, IMethodable, IClassable
    {
        public string className;
        public Arm arm;
        public CodeKeyword codeKeyword;
        public IList<Type> parents;

        public override void Before(ref zstring parent, ref int tabCount)
        {
            var tab = GetTab(tabCount);
            parent += $"\r\n{tab}{arm.ToString().ToLower()} ";
            AddCodeKeyword(ref parent, codeKeyword);
            if (parents != null && parents.Count > 0)
                parent += $"class {className} : " +
                    string.Join(",", parents.ToList().ConvertAll(temp =>
                    {
                        AddUsing(temp.Namespace);
                        return temp.Name;
                    })) +
                    $"\r\n{tab}{{";
            else {
                parent += $"class {className}" + $"\r\n{tab}{{";
            }
            tabCount++;
        }

        public override void After(ref zstring parent, ref int tabCount)
        {
            tabCount--;
            var tab = GetTab(tabCount);
            parent += $"\r\n{tab}}}";
        }
    }
}