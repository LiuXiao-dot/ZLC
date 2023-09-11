namespace ZLC.Extensions;
/// <summary>
/// 数组扩展
/// </summary>
public static class ArrayExtension
{
    /// <summary>
    /// 是否为空或者只包含0个数据
    /// </summary>
    /// <param name="array">数组</param>
    /// <returns>是否包含数据</returns>
    public static bool IsEmptyOrNull(this Array array)
    {
        return array == null || array.Length == 0;
    }
}