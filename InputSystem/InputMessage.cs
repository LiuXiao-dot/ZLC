using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using ZLC.EventSystem.MessageQueue;
namespace ZLC.InputSystem;

/// <summary>
/// 输入系统相关的消息
/// </summary>
public enum InputMessage
{
    
}

/// <summary>
/// 输入系统详细队列
/// </summary>
public class InputMQ : AMainThreadMQ<InputMessage>
{
    private static InputMQ _instance;
    /// <summary>
    /// 单例
    /// </summary>
    public static InputMQ Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // 创建单例
            var go = new GameObject("InputMQ",typeof(InputMQ));
            _instance = go.GetComponent<InputMQ>();
            DontDestroyOnLoad(go);
            return _instance;
        }
    }
}

/// <summary>
/// 每当有UI更新时触发
/// </summary>
[Serializable]
public class UpdateBindingUIEvent : UnityEvent<RebindActionUI, string, string, string>
{
}
/// <summary>
/// 启动/停止 重新绑定时触发的事件。
/// </summary>
[Serializable]
public class InteractiveRebindEvent : UnityEvent<RebindActionUI, InputActionRebindingExtensions.RebindingOperation>
{
}