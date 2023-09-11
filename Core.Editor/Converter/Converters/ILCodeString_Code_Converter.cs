using ZLCEditor.Converter.Data.Code;
namespace ZLCEditor.Converter.Converters
{
    /// <summary>
    /// 字符串数据到代码
    /// </summary>
    public class ILCodeString_Code_Converter : IConverter<ILCodeString,string>
    {

        public string Convert(ILCodeString from)
        {
            return from.content;
        }
        public object Convert(object from)
        {
            if (from is ILCodeString ilCodeString) {
                return Convert(ilCodeString);
            }
            return default;
        }
        public Type GetF()
        {
            return typeof(ILCodeString);
        }
        public Type GetT()
        {
            return typeof(string);
        }
    }
}