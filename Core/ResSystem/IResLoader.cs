using UnityEngine;
using UnityEngine.SceneManagement;
using ZLC.Common;
using Object = UnityEngine.Object;
namespace ZLC.ResSystem;

/// <summary>
/// 资源加载器
/// </summary>
/// <remarks>用于加载资源（可以包含下载）,包含资源的同步与异步加载方式，支持资源池以及加载进度监控</remarks>
public interface IResLoader : IManager, ILoader
{
    /// <summary>
    /// 开启记录功能，开启后，才会对资源加载进行记录，并在进行资源加载后调用listener的对应方法
    /// </summary>
    void EnableRecord();

    /// <summary>
    /// 关闭记录功能,在关闭时，如果整个记录过程中全部记载资源数量为0，会直接调用一次listener的OnLoadFinshed
    /// </summary>
    void DisableRecord();

    /// <summary>
    /// 添加加载监听器
    /// </summary>
    /// <param name="listener"></param>
    void AddLoadListener(ILoadListener listener);

    /// <summary>
    /// 移除加载监听器
    /// </summary>
    /// <param name="listener"></param>
    void RemoveLoadListener(ILoadListener listener);

    /// <summary>
    /// 添加一次record total
    /// </summary>
    void AddTotal();
    /// <summary>
    /// 添加一次record finished
    /// </summary>
    void AddFinished();

    /// <summary>
    /// 检测资源是否已加载
    /// </summary>
    /// <param name="path"></param>
    /// <param name="result"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>true:已加载 false:未加载</returns>
    bool CheckAsset<T>(string path, out T result) where T : Object;

    /// <summary>
    /// 异步加载资源。
    /// </summary>
    /// <param name="path">资源路径</param>
    /// <param name="callback">加载完成回调</param>
    /// <typeparam name="T"></typeparam>
    void LoadAsset<T>(string path, Action<bool, T> callback = null) where T : Object;

    /// <summary>
    /// 同步加载资源。
    /// </summary>
    /// <param name="path">资源路径</param>
    /// <param name="result">加载结果</param>
    /// <typeparam name="T">资源类型</typeparam>
    /// <returns></returns>
    bool LoadAssetSync<T>(string path, out T result) where T : Object;

    /// <summary>
    /// 加载GameObject并创建池
    /// </summary>
    void LoadPoolableGameObject(string path, Action<bool, GameObject> callback = null, int defaultCapacity = 10, int maxSize = -1);
    /// <summary>
    /// 同步加载GameObject并创建池
    /// </summary>
    /// <param name="path"></param>
    /// <param name="defaultCapacity"></param>
    /// <param name="maxSize"></param>
    bool LoadPoolableGameObjectSync(string path, int defaultCapacity = 10, int maxSize = -1);

    /// <summary>
    /// Asset(如SO)在内存中只有一份，删除采用直接销毁handle的方式,被复制的Asset(如ScriptableObject)需要自行删除
    /// </summary>
    /// <param name="path">资源路径</param>
    void DestroyAsset(string path);

    /// <summary>
    /// 同步加载GameObject.
    /// (如果有Cache机制，同步实例化时，直接可以获取Cache并实例化)
    /// </summary>
    /// <param name="path">资源路径</param>
    /// <param name="parent">作为parent的子对象实例化GameObject</param>
    /// <returns></returns>
    GameObject InstantiateGameObjectSync(string path, Transform parent);

    /// <summary>
    /// 异步实例化GameObject。实例化完成执行callback方法.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="parent"></param>
    /// <param name="callback"></param>
    /// <param name="poolable">将对象池化</param>
    void InstantiateGameObject(string path, Transform parent, Action<bool, GameObject> callback = null, bool poolable = false);

    /// <summary>
    /// 释放对象（不销毁引用）,若要销毁引用，调用DestroyAsset。
    /// 1.非池对象，直接销毁
    /// 2.池对象，释放回池中
    /// </summary>
    /// <param name="gameObject"></param>
    void ReleaseGameObject(GameObject gameObject);

    /// <summary>
    /// 销毁包含该gameObject的池
    /// </summary>
    /// <param name="gameObject"></param>
    void DestroyPool(GameObject gameObject);

    /// <summary>
    /// 销毁池
    /// </summary>
    /// <param name="path"></param>
    void DestroyPool(string path);

    /// <summary>
    /// 打开场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="loadSceneMode"></param>
    /// <param name="callback"></param>
    void OpenScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Additive, Action<bool, Scene> callback = null);

    /// <summary>
    /// 关闭场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="releaseEmbeddedRes"></param>
    /// <param name="callback"></param>
    void CloseScene(string sceneName, bool releaseEmbeddedRes = false, Action<bool> callback = null);
}