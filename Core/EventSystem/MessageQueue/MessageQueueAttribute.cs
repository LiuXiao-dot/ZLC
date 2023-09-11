namespace ZLC.EventSystem.MessageQueue;

/// <summary>
/// 消息队列的特性
/// </summary>
[AttributeUsage(AttributeTargets.Enum)]
public class MessageQueueAttribute : Attribute
{
    /// <summary>
    /// 是否是主线程运行的消息队列
    /// </summary>
    public bool isMainThread;
    /// <summary>
    /// 消息队列构造函数
    /// </summary>
    /// <param name="isMainThread">是否是主线程运行的消息队列</param>
    public MessageQueueAttribute(bool isMainThread = true)
    {
        this.isMainThread = isMainThread;
    }
}