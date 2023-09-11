using ZLC.Utils;
namespace ZLCEditor.Converter.Data.Code.CodeTree
{
    /// <summary>
    /// 节点数据转换为字符串
    /// </summary>
    public class Node_String_Converter : IConverter<ANode, string>
    {
        private void Analysis(ANode node, ref zstring parent, ref int tabCount)
        {
            var childs = node.childs;
            node.Before(ref parent, ref tabCount);

            if (childs != null) {
                foreach (var child in childs) {
                    Analysis(child, ref parent, ref tabCount);
                }
            }
            node.After(ref parent, ref tabCount);
        }

        public string Convert(ANode from)
        {
            string result;
            using (zstring.Block()) {
                zstring code = "";
                int tabCount = 0;
                Analysis(from, ref code, ref tabCount);
                result = code.Intern();
            }
            return result;
        }
        public object Convert(object from)
        {
            if (from is ANode node) {
                return Convert(node);
            }
            return default;
        }
        public Type GetF()
        {
            return typeof(ANode);
        }
        public Type GetT()
        {
            return typeof(string);
        }
    }
}