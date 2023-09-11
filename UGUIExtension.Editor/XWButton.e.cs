/*#if UNITY_EDITOR
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace XWEngine.UGUI
{
    public partial class XWButton: MonoBehaviour
    {
        public enum XWButtonType
        {
            矩形_文字,
            圆形_图片,
        }
        private bool showTip;
        [InfoBox("不使用BTN命名将不会自动生成代码", "showTip")]
        public XWButtonType type;


        private void OnValidate()
        {
            showTip = !gameObject.name.StartsWith("BTN");
        }

        /// <summary>
        /// 刷新
        /// </summary>
        [Button]
        public void Refresh()
        {
            switch (type)
            {
                case XWButtonType.矩形_文字:
                    RefreshButton();
                    RefreshRectText();
                    break;
                case XWButtonType.圆形_图片:
                    RefreshButton();
                    RefreshCircleIcon();
                    break;
                default:
                    break;
            }
        }

        private void RefreshButton()
        {
            var bg = gameObject.GetOrAddComponent<Image>();
            var bgTrans = gameObject.GetOrAddComponent<RectTransform>();
            var size = bgTrans.rect.size;
            bg.raycastTarget = true;

            // button组件检查
            var btn = gameObject.GetOrAddComponent<Button>();
            btn.transition = Selectable.Transition.Animation;

            // Animator组件检查
            var animator = gameObject.GetOrAddComponent<Animator>();
            RuntimeAnimatorController animatorController = null;
            var animatorControllers = AssetDatabase.FindAssets("BTN_simpleText t:AnimatorController");
            if (animatorControllers.Length > 0)
            {
                animatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(AssetDatabase.GUIDToAssetPath(animatorControllers[0]));
            }

            animator.runtimeAnimatorController = animatorController;
        }

        private void RefreshRectText()
        {
            // 检查子对象的text
            var textTrans = transform.Find("text");
            if (textTrans == null)
            {
                var newObj = new GameObject("text");
                newObj.transform.SetParent(gameObject.transform);
                textTrans = newObj.GetOrAddComponent<RectTransform>();
            }
            if (!textTrans.gameObject.TryGetComponent<TextMeshProUGUI>(out var text))
            {
                // 子元素除初始化不强制刷新大小位置
                text = textTrans.gameObject.GetOrAddComponent<TextMeshProUGUI>();
                text.alignment = TextAlignmentOptions.Center;
                text.enableAutoSizing = true;
                text.color = Color.black;
                var textRect = textTrans as RectTransform;
                text.raycastTarget = false;
                textRect.anchorMin = new Vector2(0.2f, 0.2f);
                textRect.anchorMax = new Vector2(0.8f, 0.8f);
                textRect.pivot = new Vector2(0.5f, 0.5f);
                textRect.sizeDelta = Vector2.zero;
                textRect.anchoredPosition3D = Vector3.zero;
            }
        }

        private void RefreshCircleIcon()
        {
            // 检查子对象的text
            var iconTrans = transform.Find("icon");
            if (iconTrans == null)
            {
                var newObj = new GameObject("icon");
                newObj.transform.SetParent(gameObject.transform);
                iconTrans = newObj.GetOrAddComponent<RectTransform>();
            }
            if (!iconTrans.gameObject.TryGetComponent<Image>(out var icon))
            {
                // 子元素除初始化不强制刷新大小位置
                icon = iconTrans.gameObject.GetOrAddComponent<Image>();
                var iconRect = iconTrans as RectTransform;
                icon.raycastTarget = false;
                iconRect.anchorMin = new Vector2(0.2f, 0.2f);
                iconRect.anchorMax = new Vector2(0.8f, 0.8f);
                iconRect.pivot = new Vector2(0.5f, 0.5f);
                iconRect.sizeDelta = Vector2.zero;
                iconRect.anchoredPosition3D = Vector3.zero;
            }
        }
    }
}
#endif*/