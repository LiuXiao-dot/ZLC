using UnityEngine;
namespace ZLC.EventSystem.MessageQueue;

/// <summary>
/// 在主线程上运行的消息队列，依赖于Monobehaviour的Update方法
/// </summary>
/// <typeparam name="T"></typeparam>
public class AMainThreadMQ<T> : AMQ<T> where T : Enum
{
    private void Update()
    {
        if ((eventCount | taskCount) == 0) {
            return;
        }
        Act();
    }
}