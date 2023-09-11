using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
namespace ZLC.EventSystem.MessageQueue;

/// <summary>
/// AMessageQueue:消息队列抽象类
/// </summary>
public class AMQ<T> : MonoBehaviour, ISubscribee<T> where T : Enum
{
    /// <summary>
    /// 消息池
    /// </summary>
    protected Queue<Event> queue = new Queue<Event>();
    /// <summary>
    /// 消息订阅者
    /// </summary>
    protected Dictionary<T, List<ISubscriber<T>>> listeners = new Dictionary<T, List<ISubscriber<T>>>();
    /// <summary>
    /// 消息数量
    /// </summary>
    protected int eventCount = 0;
    /// <summary>
    /// 当前消息剩余订阅执行数量
    /// </summary>
    protected int taskCount = 0;
    /// <summary>
    /// 当前执行中的消息
    /// </summary>
    private Event currentEvent;

    /// <inheritdoc cref="ISubscribee{T}"/>
    public void Subscribe(ISubscriber<T> subscriber, T operate)
    {
        if (!listeners.ContainsKey(operate)) {
            listeners.Add(operate, new List<ISubscriber<T>>());
        }

        if (listeners.TryGetValue(operate, out List<ISubscriber<T>> operateListeners) &&
            !operateListeners.Contains(subscriber)) {
            operateListeners.Add(subscriber);
        } else {
            Debug.LogWarning($"重复添加监听,listener:{subscriber.GetType()} operate:{operate}");
        }
    }

    /// <inheritdoc cref="ISubscribee{T}"/>
    public void Subscribe(ISubscriber<T> subscriber, IEnumerable<T> operates)
    {
        foreach (var operate in operates) {
            Subscribe(subscriber, operate);
        }
    }

    /// <inheritdoc cref="ISubscribee{T}"/>
    public void Unsubscribe(ISubscriber<T> subscriber, T operate)
    {
        if (listeners.TryGetValue(operate, out List<ISubscriber<T>> operateListeners) &&
            operateListeners.Contains(subscriber)) {
            operateListeners.Remove(subscriber);
        }
    }
    
    /// <inheritdoc cref="ISubscribee{T}"/>
    public void Unsubscribe(ISubscriber<T> subscriber, IEnumerable<T> operates)
    {
        foreach (var operate in operates) {
            Unsubscribe(subscriber, operate);
        }
    }
    /// <inheritdoc cref="ISubscribee{T}"/>
    public virtual void SendEvent(T operate, object args)
    {
        try {
            queue.Enqueue(new Event(UnsafeUtility.As<T,int>(ref operate),args));
            eventCount++;
        }
        catch (InvalidCastException e) {
            Debug.LogError(e);
        }
    }
    
    /// <summary>
    /// 执行
    /// </summary>
    protected void Act()
    {
        if (taskCount > 0 || queue.Count == 0 || !queue.TryDequeue(out currentEvent)) return;
        try {
            if (listeners.TryGetValue(UnsafeUtility.As<int,T>(ref currentEvent.operate), out List<ISubscriber<T>> currentListeners)) {
                var length = currentListeners.Count;
                taskCount = length;

                for (int i = length - 1; i >= 0; i--) {
                    if (currentListeners[i] == null) {
                        currentListeners.RemoveAt(i);
                        Callback(currentEvent);
                    }
                }
                length = currentListeners.Count;
                for (int i = 0; i < length; i++) {
                    currentListeners[i].OnMessage(currentEvent);
                }
            }
        }
        catch (Exception e) {
            Debug.LogError(e);
        }
    }
#if ZLC_DEBUG
    [SerializeField] private string currentEventName;
    private string _empty = "无";
#endif
    /// <summary>
    /// 消息执行完成后需要调用回调
    /// </summary>
    /// <param name="message"></param>
    public void Callback(Event message)
    {
        if (!message.Equals(currentEvent)) {
            return;
        }
        taskCount--;
        if (taskCount != 0) return;
        eventCount--;
#if ZLC_DEBUG
        currentEventName = _empty;
#endif
    }
}