/*#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using ZLC.Extensions;
namespace XWEngine.UGUI
{
    /// <summary>
    /// 循环列表扩展编辑器
    /// </summary>
    public partial class LoopGridLayoutGroupExtension : MonoBehaviour
    {
        /// <summary>
        /// 循环列表类型
        /// </summary>
        public enum ScrollLoopType
        {
            水平,
            垂直,
        }

        [SerializeField] private ScrollLoopType scrollType;
        [Button]
        public void Refresh()
        {
            switch (scrollType) {

                case ScrollLoopType.水平:
                    CheckComponent();
                    Refresh水平Params();
                    break;
                case ScrollLoopType.垂直:
                    CheckComponent();
                    Refresh垂直Params();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void Refresh(ScrollLoopType scrollType)
        {
            this.scrollType = scrollType;
            switch (scrollType) {

                case ScrollLoopType.水平:
                    CheckComponent();
                    Refresh水平Params();
                    break;
                case ScrollLoopType.垂直:
                    CheckComponent();
                    Refresh垂直Params();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void CheckComponent()
        {
            var layoutGroup = gameObject.GetOrAddComponent<ControllableGridLayoutGroup>();
            var contentSizeFillter = gameObject.GetOrAddComponent<ContentSizeFitter>();
            content = (RectTransform)transform;
            this.layoutGroup = layoutGroup;
        }

        private void Refresh水平Params()
        {
            var layoutGroup = gameObject.GetOrAddComponent<ControllableGridLayoutGroup>();
            var contentSizeFillter = gameObject.GetOrAddComponent<ContentSizeFitter>();
            layoutGroup.startAxis = GridLayoutGroup.Axis.Vertical;
            contentSizeFillter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        private void Refresh垂直Params()
        {
            var layoutGroup = gameObject.GetOrAddComponent<ControllableGridLayoutGroup>();
            var contentSizeFillter = gameObject.GetOrAddComponent<ContentSizeFitter>();
            layoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
            contentSizeFillter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
    }
}
#endif*/