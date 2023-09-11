using UnityEngine;
namespace ZLC.Utils
{
    /// <summary>
    /// Transform帮助类
    /// </summary>
    public sealed class TransformHelper
    {
        /// <summary>
        /// 重置Transform的position,scale,rotation 
        /// </summary>
        public static void Reset(Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// 遍历全部transform
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static IEnumerable<Transform> ForEach(Transform transform)
        {
            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++) {
                var child = transform.GetChild(i);
                yield return child;
                var temp = ForEach(child);
                foreach (var childTemp in temp) {
                    yield return childTemp;
                }
            }
        }
    }
}