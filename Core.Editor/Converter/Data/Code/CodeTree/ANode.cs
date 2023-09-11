using ZLC.Utils;
using ZLCEditor.Converter.Data.Code.CodeTree.Nodes;
namespace ZLCEditor.Converter.Data.Code.CodeTree
{
    /// <summary>
    /// 基础节点
    /// </summary>
    public abstract class ANode : IStatementable
    {
        public ANode parent;
        public List<ANode> childs;

        private void SetParent(ANode parent)
        {
            this.parent = parent;
        }

        public void Add(ANode newChild)
        {
            if (childs == null) {
                childs = new List<ANode>(1);
            }
            if (newChild is FieldNode newFiled) {
                if (childs.Exists(temp =>
                    {
                        if (temp is FieldNode oldField) {
                            return oldField.fieldName == newFiled.fieldName;
                        }
                        return false;
                    })) {
                    return;
                }
            }
            childs.Add(newChild);
            newChild.SetParent(this);
        }

        public virtual void Before(ref zstring parent, ref int tabCount)
        {
        }

        public virtual void After(ref zstring parent, ref int tabCount)
        {
        }

        protected zstring GetTab(int tabCount)
        {
            zstring tab = "";
            for (int i = 0; i < tabCount; i++) {
                tab += "\t";
            }
            return tab;
        }

        protected void AddCodeKeyword(ref zstring parent, CodeKeyword keyword)
        {
            foreach (CodeKeyword temp in Enum.GetValues(typeof(CodeKeyword))) {
                if (temp == CodeKeyword.none)
                    continue;
                if (keyword.HasFlag(temp)) {
                    parent += temp.ToString().ToLower() + " ";
                }
            }
        }

        public void AddUsing(string usingStatement)
        {
            var tempParent = this;
            for (int i = 0; i < 100; i++) {
                if (tempParent != null) {
                    if (tempParent is NamespaceNode) {
                        var namespaceNode = (NamespaceNode)tempParent;
                        namespaceNode.AddUsingNode(usingStatement);
                        return;
                    }
                    tempParent = tempParent.parent;
                } else {
                    return;
                }
            }
        }
    }
}