namespace ZLCEditor.Converter.Data
{
    /// <summary>
    /// 继承该接口表示用于生成代码的中间数据
    /// </summary>
    public interface IILCode : IILData
    {
        string GetPath();
    }
}