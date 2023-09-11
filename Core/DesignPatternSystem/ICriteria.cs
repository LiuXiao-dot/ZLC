namespace ZLC.DesignPatternSystem;

/// <summary>
/// 过滤器模式
/// </summary>
public interface ICriteria<T>
{
    /// <summary>
    /// 过滤<paramref name="sources"/>
    /// </summary>
    /// <param name="sources">源数据</param>
    /// <returns>过滤后的结果</returns>
    IEnumerable<T> MeetCriteria(IEnumerable<T> sources);
}