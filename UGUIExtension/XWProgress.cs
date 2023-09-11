using UnityEngine;
using ZLC.WindowSystem.Attribute;

namespace XWEngine.UGUI
{
    /// <summary>
    /// 进度条
    /// 仅进度条，无文本
    /// </summary>
    [ShortKey]
    public partial class XWProgress : MonoBehaviour
    {
        /// <summary>
        /// 进度条类型
        /// </summary>
        public enum XWProgressType
        {
            Simple_Circle,
            Loading,
        }
        
        private RectTransform fill;
        private float totalLength;

        private void Awake()
        {
            fill = transform.Find("fill") as RectTransform;
            fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,0);
            totalLength = (transform as RectTransform).rect.size.x;
        }

        public void SetProgress(float progress)
        {
            fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, progress * totalLength);
        }

        private void OnRectTransformDimensionsChange()
        {
            totalLength = (transform as RectTransform).rect.size.x;
        }
    }
}
