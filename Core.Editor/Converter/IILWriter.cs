namespace ZLCEditor.Converter
{
    /// <summary>
    /// IL数据写入器
    /// </summary>
    public interface IILWriter<in T>
    {
        void Write(T data,params object[] args);
    }
}