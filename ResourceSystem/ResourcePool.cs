using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using ResLoader = UnityEngine.AddressableAssets.Addressables;
namespace ZLC.ResourceSystem
{
    /// <summary>
    /// GameObject资源池
    /// </summary>
    public class ResourcePool : IDisposable
    {
        /// <summary>
        /// 资源的handle，销毁池时，需要释放handle
        /// </summary>
        private readonly AsyncOperationHandle<GameObject> _handle;
        /// <summary>
        /// 当前池中的对象
        /// </summary>
        public int CountInactive
        {
            get {
                return _stack.Count;
            }
        }

        /// <summary>
        /// 池一共创建过的对象数量
        /// </summary>
        public int CountAll { get; private set; }

        /// <summary>
        /// 池中对象的最大数量，-1：不限大小
        /// </summary>
        private int _maxSize;
        
        /// <summary>
        /// 存放池中对象的集合
        /// </summary>
        private Stack<GameObject> _stack;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="defaultCapacity"></param>
        /// <param name="maxSize"></param>
        public ResourcePool(AsyncOperationHandle<GameObject> handle, int defaultCapacity = 10, int maxSize = -1)
        {
            _handle = handle;
            _stack = new Stack<GameObject>(defaultCapacity);
            _maxSize = maxSize;
        }
        
        /// <summary>
        /// 获取GameObject
        /// </summary>
        /// <returns></returns>
        public GameObject Get()
        {
            if (_stack.Count != 0) return _stack.Pop();
            ++CountAll;
            return GameObject.Instantiate(_handle.Result);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject Get(Transform parent)
        {
            if (_stack.Count != 0) {
                var result = _stack.Pop();
                result.transform.SetParent(parent);
                var component = result.GetComponent<PooableComponent>();
                component.OnGet();
                return result;
            }
            ++CountAll;
            return GameObject.Instantiate(_handle.Result,parent);
        }

        /// <summary>
        /// 释放element到池中。
        /// 超过要保存的上限时，直接销毁
        /// </summary>
        /// <param name="element"></param>
        public void Release(GameObject element)
        {
            if (this._stack.Count > 0 && this._stack.Contains(element)) {
                Debug.LogError("Trying to release an object that has already been released to the pool.");
                return;
            }
            if (this._maxSize == -1 || this.CountInactive < this._maxSize)
            {
                var component = element.GetComponent<PooableComponent>();
                component.OnRealse();
                this._stack.Push(element);
            }
            else
            {
                GameObject.Destroy(element);
            }
        }
        
        /// <summary>
        /// 清空池
        /// </summary>
        public void Clear()
        {
            foreach (var gameObject in _stack) {
                GameObject.Destroy(gameObject);
            }
            _stack.Clear();
            this.CountAll = 0;
        }
        
        /// <summary>
        /// 组件被销毁时销毁池,并释放handle
        /// </summary>
        public void Dispose()
        {
            Clear();
            ResLoader.Release(_handle);
        }
    }
}