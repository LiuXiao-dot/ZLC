namespace ZLC.CacheSystem;

/// <summary>
/// 浮点型的class，防止装箱拆箱
/// </summary>
/// <remarks>使用方式：将float强制转换为FloatCache，将会从池中获取一个FloatCache,在不使用时，调用<see cref="Release"/>释放回池中</remarks>
public class FloatCache
{
    /// <summary>
    /// 池
    /// </summary>
    private static ObjectPool<FloatCache> _pool;

    /// <summary>
    /// 实际值
    /// </summary>
    public float value;

    /// <summary>
    /// 释放FloatCache回池
    /// </summary>
    public void Release()
    {
        CheckPool();
        _pool.Release(this);
    }

    /// <summary>
    /// FloatCache转换为float
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static implicit operator float(FloatCache source)
    {
        return source.value;
    }

    /// <summary>
    /// float转换为FloatCache
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static implicit operator FloatCache(float source)
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
        _pool ??= new ObjectPool<FloatCache>(() => new FloatCache());
    }
}