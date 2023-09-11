namespace ZLCEditor.Converter
{
    public interface IConverter
    {
        object Convert(object from);
        Type GetF();
        Type GetT();
    }
    
    /// <summary>
    /// 转换器
    /// </summary>
    public interface IConverter<in F, out T> : IConverter
    {
        T Convert(F from);
    }
}