namespace ZLC.EventSystem;

/// <summary>
/// 通用事件
/// </summary>
public struct Event
{
    /// <summary>
    /// 事件类型
    /// </summary>
    public int operate;

    /// <summary>
    /// 参数
    /// </summary>
    public object data;

    /// <summary>
    /// 事件基类的构造函数
    /// </summary>
    /// <param name="operate">事件</param>
    /// <param name="data">事件的参数</param>
    public Event(int operate, object data)
    {
        this.operate = operate;
        this.data = data;
    }
}

/// <summary>
/// 被订阅者
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISubscribee<T>
{
    /// <summary>
    /// 订阅事件
    /// </summary>
    /// <param name="subscriber">订阅者</param>
    /// <param name="operate">要订阅的事件ID</param>
    void Subscribe(ISubscriber<T> subscriber, T operate);
    /// <summary>
    /// 订阅事件
    /// </summary>
    /// <param name="subscriber">订阅者</param>
    /// <param name="operates">要订阅的事件ID</param>
    void Subscribe(ISubscriber<T> subscriber, IEnumerable<T> operates);
    /// <summary>
    /// 取消订阅
    /// </summary>
    /// <param name="subscriber">订阅者</param>
    /// <param name="operate">要订阅的事件</param>
    void Unsubscribe(ISubscriber<T> subscriber, T operate);
    /// <summary>
    /// 取消订阅
    /// </summary>
    /// <param name="subscriber">订阅者</param>
    /// <param name="operates">要取消订阅的事件ID</param>
    void Unsubscribe(ISubscriber<T> subscriber, IEnumerable<T> operates);
    /// <summary>
    /// 发送事件
    /// </summary>
    /// <param name="operate">要发送的事件ID</param>
    /// <param name="args">事件参数</param>
    void SendEvent(T operate, object args);
}

/// <summary>
/// 订阅者
/// </summary>
public interface ISubscriber<T>
{
    /// <summary>
    /// 接受到消息
    /// </summary>
    /// <param name="subEvent"></param>
    void OnMessage(Event subEvent);
}