using UnityEngine;
using ZLC.Utils;
namespace ZLC.ResourceSystem
{
    /// <summary>
    /// 池化对象
    /// </summary>
    [DisallowMultipleComponent]
    public class PooableComponent : MonoBehaviour
    {
        /// <summary>
        /// 资源池
        /// </summary>
        protected ResourcePool _pool;

        internal void SetPool(ResourcePool pool)
        {
            this._pool = pool;
        }

        internal ResourcePool GetPool()
        {
            return _pool;
        }

        /// <summary>
        /// 当被释放回池中时调用
        /// </summary>
        public virtual void OnRealse()
        {
            gameObject.SetActive(false);
            TransformHelper.Reset(transform);
        }

        /// <summary>
        /// 当被从池中取出时调用
        /// </summary>
        public virtual void OnGet()
        {
            TransformHelper.Reset(transform);
            gameObject.SetActive(true);
        }
    }
}