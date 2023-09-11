namespace ZLC.PofileSystem;

/// <summary>
/// 日志模式
/// </summary>
[Flags]
public enum LogMode
{
    /// <summary>
    /// 关闭所有日志
    /// </summary>
    None = 0,
    /// <summary>
    /// Unity默认日志
    /// </summary>
    Default = 1,
    /// <summary>
    /// 日志保存到文件中
    /// </summary>
    File = 2,
}