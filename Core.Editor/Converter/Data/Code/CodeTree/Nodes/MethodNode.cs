namespace ZLCEditor.Converter.Data.Code.CodeTree.Nodes
{
    public class MethodNode : ANode,IStatementable
    {
        public string methodName;
        public Arm arm;
        public IEnumerable<ParamterNode> paramters;
        public Type returnType;
        public CodeKeyword codeKeyword;

        public override void Before(ref zstring parent, ref int tabCount)
        {
            var tab = GetTab(tabCount);

            parent += $"\r\n{tab}{arm.ToString().ToLower()} ";
            AddCodeKeyword(ref parent, codeKeyword);
            parent += returnType == null ? "void" : returnType.Name;
            if (returnType != null)
                AddUsing($"using {returnType.Namespace};");
            parent += $" {methodName}({(paramters == null ? "" : string.Join(" ,", paramters.ToList().ConvertAll(temp => { AddUsing($"using {temp.type.Namespace};"); return $"{temp.type.Name} {temp.name}"; })))})";
            parent += $"\r\n{tab}{{";
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