namespace ZLC.ExceptionSystem;

/// <summary>
/// 单例被重复创建异常
/// </summary>
public class MultiSingletonException : ApplicationException
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="exist">已存在的单例实例</param>
    /// <param name="multi">新创建的重复实例</param>
    public MultiSingletonException(object exist, object multi) : base($"类型:{exist.GetType()} 已存在:{exist} 新的:{multi}")
    {
    }
}