using UnityEngine;
using UnityEngine.UI;

namespace XWEngine.UGUI
{
    /// <summary>
    /// 循环列表扩展：依赖：ScrollRect,ControllableGridLayoutGroup
    /// (若要生效，需要比可见范围的物品多3行子物品)
    /// 若开启无限循环，scrollRect需要设置为Unresticted
    /// </summary>
    public partial class ScrollLoopExtension : MonoBehaviour
    {
        /// <summary>
        /// 滚动列表
        /// </summary>
        [SerializeField]
        private ScrollRect scrollRect;
        /// <summary>
        /// 循环列表扩展
        /// </summary>
        [SerializeField]
        private LoopGridLayoutGroupExtension _loopGridLayoutGroupExtension; 
        
        private void Awake()
        {
            scrollRect.onValueChanged.AddListener(_loopGridLayoutGroupExtension.OnScroll);
            if(_loopGridLayoutGroupExtension.GetIsLimited())
                scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
        }

        private void OnDestroy()
        {
            scrollRect.onValueChanged.RemoveListener(_loopGridLayoutGroupExtension.OnScroll);
        }

        public LoopGridLayoutGroupExtension GetLoopGridLayoutGroupExtension()
        {
            return this._loopGridLayoutGroupExtension;
        }
    }
}