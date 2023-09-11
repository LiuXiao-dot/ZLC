using UnityEngine;
namespace ZLC.Utils;

/// <summary>
/// IList的帮助类
/// </summary>
public sealed class IListHelper
{
    /// <summary>
    /// 移除全部空值
    /// </summary>
    /// <returns>true:移除成功 false:无空值可移除</returns>
    public static bool RemoveNulls<T>(IList<T> list)
    {
        try {
            if (list == null) return false;
            var length = list.Count;
            for (int i = length - 1; i >= 0; i--) {
                if (list[i] == null) {
                    list.RemoveAt(i);
                }
            }
            return list.Count == length;
        }
        catch (Exception e) {
            Debug.LogError(e);
            throw;
        }
    }
}