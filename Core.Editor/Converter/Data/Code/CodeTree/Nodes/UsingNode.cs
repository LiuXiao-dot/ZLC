namespace ZLCEditor.Converter.Data.Code.CodeTree.Nodes
{
    public class UsingNode : ANode, IEquatable<UsingNode>
    {
        public string usingStatement;
        public bool Equals(UsingNode other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return usingStatement == other.usingStatement;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UsingNode)obj);
        }
        public override int GetHashCode()
        {
            return (usingStatement != null ? usingStatement.GetHashCode() : 0);
        }
    }
}