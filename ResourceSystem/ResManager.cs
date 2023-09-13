using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using ZLC.Application;
using ZLC.Common;
using ZLC.IOCSystem;
using ZLC.ResSystem;
using ZLC.SceneSystem;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using ResLoader = UnityEngine.AddressableAssets.Addressables;
namespace ZLC.ResourceSystem
{
    /// <summary>
    /// 基于Addressables的资源管理器
    /// </summary>
    [Component(AppConstant.APP_LAUNCHER_MANAGER)]
    public class ResManager : IResLoader
    {
        /// <summary>
        /// 资源缓存
        /// </summary>
        private ResCache _cache;
        /// <summary>
        /// 监听Load
        /// </summary>
        private List<ILoadListener> _listeners;
        /// <summary>
        /// 是否启用了Record(同步加载不记录)
        /// </summary>
        private bool enabled;

        /// <summary>
        /// 已完成加载项
        /// </summary>
        private int _finished;
        /// <summary>
        /// 全部加载项
        /// </summary>
        private int _total;

        /// <inheritdoc />
        public void Dispose()
        {
            _cache.Dispose();
            _listeners.Clear();
        }
        /// <inheritdoc />
        public void Init()
        {
            // 初始化cache
            _cache = new ResCache();
            _listeners = new List<ILoadListener>(2);
        }
        /// <inheritdoc />
        public void Load(Action<int, int> onProgress)
        {
            var handle = ResLoader.InitializeAsync();
            handle.Completed += operationHandle =>
            {
                onProgress?.Invoke(1, 1);
            };
        }
        #region 加载监听
        /// <inheritdoc />
        public void EnableRecord()
        {
            enabled = true;
            _total = 0;
            _finished = 0;
        }

        /// <inheritdoc />
        public void DisableRecord()
        {
            enabled = false;
            if (_total == 0) {
                // 一个都不用加载，直接调用加载和结束方法
                foreach (var listener in _listeners) {
                    listener.OnLoad(_finished, _total);
                    listener.OnLoadFinshed();
                }
            }
        }

        /// <inheritdoc />
        public void AddLoadListener(ILoadListener listener)
        {
            if (_listeners.Contains(listener)) {
                Debug.LogError($"试图重复添加listener:{listener.GetType().FullName}，请修复。（打包后将跳过该检测）");
                return;
            }
            _listeners.Add(listener);
        }

        /// <inheritdoc />
        public void RemoveLoadListener(ILoadListener listener)
        {
            _listeners.Remove(listener);
        }

        /// <inheritdoc />
        public void AddTotal()
        {
            if (enabled)
                _total++;
        }

        /// <inheritdoc />
        public void AddFinished()
        {
            //if (!enabled) return;
            _finished++;
            if (_finished == _total) {
                foreach (var listener in _listeners) {
                    listener.OnLoad(_finished, _total);
                    listener.OnLoadFinshed();
                }
            } else {
                foreach (var listener in _listeners) {
                    listener.OnLoad(_finished, _total);
                }
            }

        }
        #endregion

        #region 资源加载:先检测Cache，若不能直接获取再加载
        /// <inheritdoc />
        public bool CheckAsset<T>(string path, out T result) where T : Object
        {
            if (_cache.TryGetHandle(path, out var cacheHandle)) {
                if (!cacheHandle.IsDone)
                    cacheHandle.WaitForCompletion();
                if (cacheHandle.Status == AsyncOperationStatus.Succeeded) {
                    result = (T)cacheHandle.Result;
                    return true;
                }
            }
            result = default;
            return false;
        }

        /// <inheritdoc />
        public void LoadAsset<T>(string path, Action<bool, T> callback = null) where T : Object
        {
            if (_cache.TryGetHandle(path, out var cacheHandle) && cacheHandle.IsDone) {
                callback?.Invoke(true, (T)cacheHandle.Result);
                return;
            }

            AddTotal();
            if (!cacheHandle.IsValid()) {
                cacheHandle = ResLoader.LoadAssetAsync<T>(path);
                _cache.AddHandle(path, cacheHandle);
            }
            cacheHandle.Completed += temp =>
            {
                if (cacheHandle.Status == AsyncOperationStatus.Succeeded) {
                    AddFinished();
                    callback?.Invoke(true, (T)temp.Result);
                    return;
                }
                _cache.RemoveHandle(path);
                callback?.Invoke(false, default(T));
            };
        }

        /// <inheritdoc />
        public bool LoadAssetSync<T>(string path, out T result) where T : Object
        {
            if (!_cache.TryGetHandle(path, out var handle)) {
                handle = ResLoader.LoadAssetAsync<T>(path);
                _cache.AddHandle(path, handle);
            }
            if (!handle.IsDone)
                handle.WaitForCompletion();
            if (handle.Status == AsyncOperationStatus.Failed) {
                result = default(T);
                return false;
            }
            result = (T)handle.Result;
            return true;
        }
        /// <inheritdoc />
        public void LoadPoolableGameObject(string path, Action<bool, GameObject> callback = null, int defaultCapacity = 10, int maxSize = -1)
        {
            if (_cache.TryGetHandle(path, out var cacheHandle) && cacheHandle.IsDone) {
                callback?.Invoke(true, (GameObject)cacheHandle.Result);
                return;
            }

            AddTotal();
            if (!cacheHandle.IsValid()) {
                cacheHandle = ResLoader.LoadAssetAsync<GameObject>(path);
                _cache.AddHandle(path, cacheHandle, true, defaultCapacity, maxSize);
            }
            cacheHandle.Completed += temp =>
            {
                if (cacheHandle.Status == AsyncOperationStatus.Succeeded) {
                    AddFinished();
                    callback?.Invoke(true, (GameObject)temp.Result);
                    return;
                }
                _cache.RemoveHandle(path);
                callback?.Invoke(false, default(GameObject));
            };
        }
        /// <inheritdoc />
        public bool LoadPoolableGameObjectSync(string path, int defaultCapacity = 10, int maxSize = -1)
        {
            if (!_cache.TryGetHandle(path, out var handle)) {
                handle = ResLoader.LoadAssetAsync<GameObject>(path);
                _cache.AddHandle(path, handle, true, defaultCapacity, maxSize);
            }
            if (!handle.IsDone)
                handle.WaitForCompletion();
            if (handle.Status == AsyncOperationStatus.Failed) {
                return false;
            }
            return true;
        }
        /// <inheritdoc />
        public void DestroyAsset(string path)
        {
            _cache.RemoveHandle(path);
        }

        /// <summary>
        /// 先检查是否有池，有池从池中获取，没有再直接实例化
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject InstantiateGameObjectSync(string path, Transform parent)
        {
            if (this.LoadAssetSync<GameObject>(path, out var result)) {
                return _cache.InstantiateGameObject(path, parent);
            }
            return result;
        }

        /// <inheritdoc />
        public void InstantiateGameObject(string path, Transform parent, Action<bool, GameObject> callback = null, bool poolable = false)
        {
            this.LoadAsset<GameObject>(path, (success, result) =>
            {
                if (success) {
                    var instance = _cache.InstantiateGameObject(path, parent);
                    callback?.Invoke(true, instance);
                    return;
                }
                callback?.Invoke(false, default(GameObject));
            });
        }
        #endregion

        /// <inheritdoc />
        public void ReleaseGameObject(GameObject gameObject)
        {
            _cache.ReleaseGameObject(gameObject);
        }
        /// <inheritdoc />
        public void DestroyPool(GameObject gameObject)
        {
            _cache.ClearPool(gameObject);
        }

        /// <inheritdoc />
        public void DestroyPool(string path)
        {
            _cache.ClearPool(path);
        }

        /// <inheritdoc />
        public void OpenScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Additive, Action<bool, Scene> callback = null)
        {
            var handle = ResLoader.LoadSceneAsync(sceneName, loadSceneMode);
            AddTotal();
            handle.Completed += operationHandle =>
            {
                if (operationHandle.Status == AsyncOperationStatus.Succeeded) {
                    AddFinished();
                    callback?.Invoke(true, operationHandle.Result.Scene);
                    SceneMessageQueue.Instance.SendEvent(SceneMessage.OnSceneOpen, sceneName);
                    return;
                }

                callback?.Invoke(false, default(Scene));
            };
        }

        /// <inheritdoc />
        public void CloseScene(string sceneName, bool releaseEmbeddedRes = false, Action<bool> callback = null)
        {
            if (_cache.RemoveSceneHandle(sceneName, out var sceneHandle)) {
                var sceneInstance = (SceneInstance)sceneHandle.Result;
                var handle = ResLoader.UnloadSceneAsync(sceneInstance, releaseEmbeddedRes ? UnloadSceneOptions.UnloadAllEmbeddedSceneObjects : UnloadSceneOptions.None, false);
                handle.Completed += operationHandle =>
                {
                    callback?.Invoke(operationHandle.Status == AsyncOperationStatus.Succeeded);
                    ResLoader.Release(handle);
                    SceneMessageQueue.Instance.SendEvent(SceneMessage.OnSceneClose, sceneName);
                };
                return;
            }
            callback?.Invoke(true);
        }
    }
}