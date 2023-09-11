namespace ZLCEditor.Converter.Data.Code.CodeTree.Nodes
{
    /// <summary>
    /// 命名空间节点
    /// </summary>
    public class NamespaceNode : ANode, IClassable
    {
        public string name;
        public HashSet<UsingNode> usingNodes;

        public void AddUsingNode(string usingStatement)
        {
            if (usingNodes == null) {
                usingNodes = new HashSet<UsingNode>();
            }
            if (!usingStatement.Contains("using ")) {
                usingStatement = $"using {usingStatement};";
            }
            var newNode = NodeFactory.instance.CreateUsingNode(usingStatement);
            if (usingNodes.Contains(newNode)) {
                NodeFactory.instance.ReleaseNode(newNode);
            } else {
                usingNodes.Add(newNode);
            }
        }
        
        public override void Before(ref zstring parent, ref int tabCount)
        {  
            parent += $"\r\nnamespace {name}\r\n{{";
            tabCount++;
        }

        public override void After(ref zstring parent, ref int tabCount)
        {
            tabCount--;
            if (usingNodes != null && usingNodes.Count > 0) {
                parent = string.Join("\r\n", usingNodes.ToList().ConvertAll(temp => temp.usingStatement)) + parent;
            } 
            parent += "\r\n}";
        }
    }
}