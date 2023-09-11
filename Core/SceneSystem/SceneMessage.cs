using ZLC.EventSystem.MessageQueue;
namespace ZLC.SceneSystem;
/// <summary>
/// 场景消息
/// </summary>
[MessageQueue]
public enum SceneMessage
{
    /// <summary>
    /// 场景打开时调用
    /// 参数：string 场景名
    /// </summary>
    OnSceneOpen = 0,
    /// <summary>
    /// 场景关闭时调用
    /// 参数：string 场景名
    /// </summary>
    OnSceneClose = 1,
}