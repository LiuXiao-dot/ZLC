using Sirenix.Utilities;
using ZLCEditor.Converter.Data.Code;
namespace ZLCEditor.Converter.Converters
{
    /// <summary>
    /// 字典数据转换为Enum代码
    /// </summary>
    public class ILEnum_Code_Converter : IConverter<ILEnum, string>
    {
        public string Convert(ILEnum from)
        {
            var kvs = from.kvs;
            var content = "";
            kvs.ForEach(temp =>
                content += $"\t\t{temp.Key} = {temp.Value},\n"
            );
            var code = $@"namespace {from.@namespace}
{{
    {from.arm.ToString().ToLower()} enum {from.fileName}
    {{
{content}
    }}
}}";
            return code;
        }
        public Type GetF()
        {
            return typeof(ILEnum);
        }
        public Type GetT()
        {
            return typeof(string);
        }
        public object Convert(object from)
        {
            if (from is ILEnum ilEnum) {
                return Convert(ilEnum);
            }
            return default;
        }
    }
}