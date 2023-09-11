using ZLC.CacheSystem;
namespace ZLCEditor.Converter.Data.Code.CodeTree
{
    public class NodePool : IDisposable
    {
        private Dictionary<Type, ObjectPool<ANode>> _pools = new Dictionary<Type, ObjectPool<ANode>>();

        private ObjectPool<ANode> GetPool(Type type)
        {
            if (!_pools.TryGetValue(type, out var pool)) {
                pool = new ObjectPool<ANode>(() =>
                {
                    return (ANode)Activator.CreateInstance(type);
                });
                _pools.Add(type, pool);
            }
            return pool;
        }

        /// <summary>
        /// 获取指定类型的节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public ANode Get(Type type)
        {
            return GetPool(type).Get();
        }

        public T Get<T>() where T : ANode
        {
            return (T)Get(typeof(T));
        }

        /// <summary>
        /// 释放节点到对应类型的池中
        /// </summary>
        /// <param name="node"></param>
        public void Release(ANode node)
        {
            GetPool(node.GetType()).Release(node);
        }

        ~NodePool()
        {
            Dispose();
        }
        public void Dispose()
        {
            foreach (var objectPool in _pools) {
                objectPool.Value.Dispose();
            }
        }
    }
}