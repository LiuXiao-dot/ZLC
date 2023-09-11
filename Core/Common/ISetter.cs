namespace ZLC.Common;
/// <summary>
/// 设置接口，用于设置应用的设置选项
/// </summary>
/// <typeparam name="T">设置项的参数类型</typeparam>
public interface ISetter<T>
{
    /// <summary>
    /// 获取值
    /// </summary>
    /// <returns></returns>
    T Get();
    /// <summary>
    /// 设置值
    /// </summary>
    /// <param name="newValue"></param>
    void Set(T newValue);
}