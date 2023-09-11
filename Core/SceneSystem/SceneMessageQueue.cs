using UnityEngine;
using ZLC.EventSystem.MessageQueue;
namespace ZLC.SceneSystem;

/// <inheritdoc />
public class SceneMessageQueue : AMainThreadMQ<SceneMessage>
{
    /// <summary>
    /// 单例
    /// </summary>
    public static SceneMessageQueue Instance
    {
        get{
            if(_instance == null)
            {
                var newObject = new GameObject("SceneMessageQueue");
                DontDestroyOnLoad(newObject);
                _instance = newObject.AddComponent<SceneMessageQueue>();
            }
            return _instance;
        }
    }
    private static SceneMessageQueue _instance;
}