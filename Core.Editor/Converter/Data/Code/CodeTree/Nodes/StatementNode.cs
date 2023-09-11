namespace ZLCEditor.Converter.Data.Code.CodeTree.Nodes
{
    /// <summary>
    /// 简单的一条语句
    /// </summary>
    public class StatementNode : ANode
    {
        public string statement;

        public override void Before(ref zstring parent, ref int tabCount)
        {
            parent = parent + "\r\n" + GetTab(tabCount) + statement;
        }
    }
}