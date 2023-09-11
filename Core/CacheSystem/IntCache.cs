namespace ZLC.CacheSystem;

/// <summary>
/// 整型的class，防止装箱拆箱
/// </summary>
/// <remarks>使用方式：将int强制转换为IntCache，将会从池中获取一个IntCache,在不使用时，调用<see cref="Release"/>释放回池中</remarks>
public class IntCache
{
    /// <summary>
    /// 池
    /// </summary>
    private static ObjectPool<IntCache> _pool;

    /// <summary>
    /// 实际值
    /// </summary>
    public int value;

    /// <summary>
    /// 释放IntCache回池
    /// </summary>
    public void Release()
    {
        CheckPool();
        _pool.Release(this);
    }
        
    /// <summary>
    /// IntCache转换为int
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static explicit operator int(IntCache source)
    {
        return source.value;
    }
        
    /// <summary>
    /// int转换为IntCache
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static explicit operator IntCache(int source)
    {
        CheckPool();
        var cache = _pool.Get();
        cache.value = source;
        return cache;
    }

    /// <summary>
    /// 检测池，如果没有创建过池，则创建一个新的池，否则不创建
    /// </summary>
    private static void CheckPool()
    {
        _pool ??= new ObjectPool<IntCache>(() => new IntCache());
    }
}