namespace ZLC.ThreadSystem;

/// <summary>
/// 线程封装
/// </summary>
public sealed class ThreadWrapper
{
    private Action action;
    private ManualResetEvent resetEvent;

    /// <inheritdoc cref="object"/>
    public ThreadWrapper()
    {
        resetEvent = new ManualResetEvent(false);
    }

    /// <summary>
    /// 运行
    /// </summary>
    public void Run()
    {
        action?.Invoke();
    }

    /// <summary>
    /// 加入新的事件
    /// </summary>
    /// <param name="newAction"></param>
    public void Join(Action newAction)
    {
        action += newAction;
    }

    /// <summary>
    /// 不重复添加已加的事件
    /// </summary>
    public void JoinCover(Action newAction)
    {
        action -= newAction;
        action += newAction;
    }

    /// <summary>
    /// 暂停
    /// </summary>
    public void Pause()
    {
        resetEvent.Reset();
        resetEvent.WaitOne();
    }

    /// <summary>
    /// 恢复
    /// </summary>
    public void Resume()
    {
        resetEvent.Set();
    }
}