using UnityEngine;
namespace ZLC.Extensions;
/// <summary>
/// GameObject扩展
/// </summary>
public static class GameObjectExtension
{
    /// <summary>
    /// 获取或添加Component
    /// </summary>
    /// <typeparam name="T">组件</typeparam>
    /// <returns>添加的组件</returns>
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        return gameObject.TryGetComponent(out T a) ? a : gameObject.AddComponent<T>();
    }
}