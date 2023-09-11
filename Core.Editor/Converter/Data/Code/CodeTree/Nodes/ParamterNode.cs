namespace ZLCEditor.Converter.Data.Code.CodeTree.Nodes
{
    /// <summary>
    /// 方法参数节点
    /// todo:支持更多参数类型 如:ref，in, out
    /// </summary>
    public class ParamterNode : ANode
    {
        public Type type;
        public string name;
    }
}