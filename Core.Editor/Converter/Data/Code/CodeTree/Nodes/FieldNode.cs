namespace ZLCEditor.Converter.Data.Code.CodeTree.Nodes
{
    public class FieldNode : ANode
    {
        public string fieldName;
        public Arm arm;
        public CodeKeyword codeKeyword;
        public Type fieldType;
        public string fieldTypeName;

        public override void Before(ref zstring parent, ref int tabCount)
        {
            var tab = GetTab(tabCount);
            parent += $"\r\n{tab}{arm.ToString().ToLower()} ";
            AddCodeKeyword(ref parent, codeKeyword);
            parent += (fieldType == null ? fieldTypeName : fieldType.Name) + " " + fieldName + ";";
            if(fieldType != null)
                AddUsing($"using {fieldType.Namespace};");
        }

    }
}