/*#if UNITY_EDITOR
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace XWEngine.UGUI
{
    public partial class XWProgress: MonoBehaviour
    {
        public XWProgressType type;
        /// <summary>
        /// 刷新
        /// </summary>
        [Button]
        public void Refresh()
        {
            switch(type)
            {
                case XWProgressType.Simple_Circle:
                    RefreshSimpleProgress();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// -self
        /// --fill
        /// </summary>
        private void RefreshSimpleProgress()
        {
            var bg = gameObject.GetOrAddComponent<Image>();
            var bgTrans = gameObject.GetOrAddComponent<RectTransform>();
            var size = bgTrans.rect.size;
            bg.raycastTarget = false;
            var fillTrans = transform.Find("fill");
            if (fillTrans == null)
            {
                var newObj = new GameObject("fill");
                newObj.transform.SetParent(bgTrans);
                fillTrans = newObj.GetOrAddComponent<RectTransform>();
            }
            var fill = fillTrans.gameObject.GetOrAddComponent<Image>();
            var fillRect = fillTrans as RectTransform;
            fill.raycastTarget = false;
            fillRect.anchorMin = new Vector2(0,0.5f);
            fillRect.anchorMax = new Vector2(0,0.5f);
            fillRect.pivot = new Vector2(0,0.5f);
            fillRect.sizeDelta = new Vector2(0,size.y);
            fillRect.anchoredPosition3D = Vector3.zero;
        }
    }
}
#endif*/