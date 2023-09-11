/*#if UNITY_EDITOR
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace XWEngine.UGUI
{
    public partial class XWText: MonoBehaviour
    {
        private enum XWTextType
        {
            黑,
            粗黑,
        }
        
        private bool showTip;
        [InfoBox("不使用TXT命名将不会自动生成代码", "showTip")]
        [SerializeField]private XWTextType type;

        [LabelText("根据文本调整大小")]
        public bool autoSize;

        private void OnValidate()
        {
            showTip = !gameObject.name.StartsWith("TXT");
        }
        
        /// <summary>
        /// 刷新
        /// </summary>
        [Button]
        public void Refresh()
        {
            switch (type)
            {
                case XWTextType.黑:
                    Refresh黑();
                    RefreshAutoSize();
                    break;
                case XWTextType.粗黑:
                    Refresh粗黑();                    
                    RefreshAutoSize();
                    break;
                default:
                    break;
            }
        }

        private void Refresh黑()
        {
            var trans = gameObject.GetOrAddComponent<RectTransform>();
            var text = gameObject.GetOrAddComponent<TextMeshProUGUI>();
            text.color = Color.black;
            text.fontStyle = text.fontStyle & ~FontStyles.Bold;
            text.raycastTarget = false;
        }

        private void Refresh粗黑()
        {
            var trans = gameObject.GetOrAddComponent<RectTransform>();
            var text = gameObject.GetOrAddComponent<TextMeshProUGUI>();
            text.color = Color.black;
            text.fontStyle = text.fontStyle | FontStyles.Bold;
            text.raycastTarget = false;
        }

        private void RefreshAutoSize()
        {
            if (gameObject.TryGetComponent<ContentSizeFitter>(out var t) && !autoSize) {
                DestroyImmediate(t);
            } else {
                var f = gameObject.GetOrAddComponent<ContentSizeFitter>();
                f.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                f.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            }
        }
    }
}
#endif*/