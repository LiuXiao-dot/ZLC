namespace ZLCEditor.Converter.Data.Code.CodeTree.Nodes
{
    /// <summary>
    /// Struct节点
    /// </summary>
    public class StructNode : ClassNode
    {
        public override void Before(ref zstring parent, ref int tabCount)
        {
            var tab = GetTab(tabCount);
            parent += $"\r\n{tab}{arm.ToString().ToLower()} ";
            AddCodeKeyword(ref parent, codeKeyword);
            if (parents != null && parents.Count > 0)
                parent += $"struct {className} : IEquatable<{className}>," +
                    string.Join(",", parents.ToList().ConvertAll(temp =>
                    {
                        AddUsing(temp.Namespace);
                        return temp.Name;
                    })) +
                    $"\r\n{tab}{{";
            else {
                parent += $"struct {className} : IEquatable<{className}>" + $"\r\n{tab}{{";
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