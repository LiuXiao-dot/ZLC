using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using ResLoader = UnityEngine.AddressableAssets.Addressables;

namespace ZLC.ResourceSystem
{
    /// <summary>
    /// 资源缓存
    /// </summary>
    internal class ResCache : IDisposable
    {
        /// <summary>
        /// 对象池
        /// </summary>
        private Dictionary<string, ResourcePool> _pools;
        /// <summary>
        /// 一般资源的handle
        /// </summary>
        private Dictionary<string, AsyncOperationHandle> _handles;
        /// <summary>
        /// 场景的handle
        /// </summary>
        private Dictionary<string, AsyncOperationHandle> _scenes;

        internal ResCache()
        {
            _pools = new Dictionary<string, ResourcePool>();
            _handles = new Dictionary<string, AsyncOperationHandle>();
            _scenes = new Dictionary<string, AsyncOperationHandle>();
        }
        
        /// <summary>
        /// 1.替换掉无效的handle
        /// 2.添加handle
        /// 3.加载完后添加池判断
        /// </summary>
        /// <param name="path"></param>
        /// <param name="handle"></param>
        /// <param name="defaultCapacity">默认池的大小</param>
        /// <param name="forcePooable">强制池化</param>
        /// <param name="maxSize"></param>
        internal void AddHandle(string path, AsyncOperationHandle handle, bool forcePooable = false, int defaultCapacity = 10, int maxSize = -1)
        {
            if (_handles.TryGetValue(path, out var oldHandle)) {
                if (oldHandle.IsValid()) {
                    // 不添加
                    Contract.Requires(handle.Equals(oldHandle));
                } else {
                    _handles[path] = handle;
                    handle.Completed += operationHandle =>
                    {
                        // 判断是否是GameObject与是否需要池
                        if (operationHandle.Status == AsyncOperationStatus.Failed)
                            _handles.Remove(path);
                        else {
                            if (forcePooable || (handle.Result is GameObject result && result.TryGetComponent<PooableComponent>(out var pooableAsset))) {
                                Contract.Requires(handle.Result is GameObject, $"使用了forcePooable，但该选项目前只支持了GameObject.{path}");
                                AddPool(path, handle,defaultCapacity,maxSize);
                            }
                        }
                    };
                }
            } else {
                _handles.Add(path,handle);
                handle.Completed += operationHandle =>
                {
                    // 判断是否是GameObject与是否需要池
                    if (operationHandle.Status == AsyncOperationStatus.Failed)
                        _handles.Remove(path);
                    else {
                        if (forcePooable || (handle.Result is GameObject result && result.TryGetComponent<PooableComponent>(out var pooableAsset))) {
                            Contract.Requires(handle.Result is GameObject, $"使用了forcePooable，但该选项目前只支持了GameObject.{path}");
                            AddPool(path, handle,defaultCapacity,maxSize);
                        }
                    }
                };
            }
        }

        /// <summary>
        /// 添加池
        /// </summary>
        /// <param name="path"></param>
        /// <param name="handle"></param>
        /// <param name="defaultCapacity"></param>
        /// <param name="maxSize"></param>
        internal void AddPool(string path, AsyncOperationHandle handle, int defaultCapacity = 10, int maxSize = -1)
        {
            if (!_pools.ContainsKey(path)) {
                _pools.Add(path, new ResourcePool(handle.Convert<GameObject>(), defaultCapacity, maxSize));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal bool TryGetHandle(string path, out AsyncOperationHandle handle)
        {
            return _handles.TryGetValue(path, out handle);
        }

        /// <summary>
        /// 移除cache
        /// </summary>
        /// <param name="path"></param>
        internal void RemoveHandle(string path)
        {
            if (_handles.TryGetValue(path, out var handle)) {
                _handles.Remove(path);
                if (handle.IsValid())
                    ResLoader.Release(handle);
            }
        }

        /// <summary>
        /// 优先从池中获取
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        internal GameObject InstantiateGameObject(string path, Transform parent)
        {
            Contract.Requires(_handles.ContainsKey(path), "试图实例化未加载的GameObject");
            if (_pools.TryGetValue(path, out var pool)) {
                return pool.Get(parent);
            } else {
                return GameObject.Instantiate((GameObject)_handles[path].Result, parent);
            }
        }

        /// <summary>
        /// 当是池中对象时，PooableGameObject会释放GameObject到池中
        /// </summary>
        /// <param name="gameObject"></param>
        internal void ReleaseGameObject(GameObject gameObject)
        {
            if (gameObject.TryGetComponent<PooableComponent>(out var component)) {
                var pool = component.GetPool();
                if (pool != null) {
                    pool.Release(gameObject);
                    return;
                }
            }
            GameObject.Destroy(gameObject);
        }

        internal void ClearPool(string path)
        {

        }

        internal void ClearPool(GameObject gameObject)
        {

        }

        internal void AddSceneHandle(string sceneName, AsyncOperationHandle handle)
        {
            if (_scenes.TryGetValue(sceneName, out var oldHandle)) {
                if (oldHandle.IsValid()) {
                    // 不添加
                    Contract.Requires(handle.Equals(oldHandle));
                } else {
                    _scenes[sceneName] = handle;
                }
            }
        }

        /// <summary>
        /// 场景的handle要用于卸载场景，由ResManager进行销毁
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="handle"></param>
        internal bool RemoveSceneHandle(string sceneName,out AsyncOperationHandle handle)
        {
            if (_scenes.TryGetValue(sceneName, out handle)) {
                return _scenes.Remove(sceneName);
            }
            return false;
        }
        
        public void Dispose()
        {
            // 释放全部handle
            foreach (var pool in _pools) {
                pool.Value.Dispose();
            }
            _pools.Clear();
            foreach (var handle in _handles) {
                if(handle.Value.IsValid())
                    ResLoader.Release(handle.Value);
            }
            _handles.Clear();
            foreach (var handle in _scenes) {
                if(handle.Value.IsValid())
                    ResLoader.Release(handle.Value);
            }
            _scenes.Clear();
        }
    }
}