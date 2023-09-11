using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ZLCEditor.Menu
{
    public static class DefaultControls
    {
        private static IFactoryControls _currentFactory = DefaultRuntimeFactory.Default;
        /// <summary>
        /// 当前的工厂
        /// </summary>
        public static IFactoryControls factory
        {
            get { return _currentFactory; }
            set { _currentFactory = value; }
        }

        /// <summary>
        /// 创建GameObject的工厂接口.
        /// 使用该接口可以使MenuOption和Editor使用ObjectFactory和默认预设正常工作.
        /// </summary>
        public interface IFactoryControls
        {
            /// <summary>
            /// 创建GameObject
            /// </summary>
            /// <param name="name">GameObject的name</param>
            /// <param name="components">GameObject上添加的组件</param>
            /// <returns></returns>
            GameObject CreateGameObject(string name, params Type[] components);
        }

        /// <summary>
        /// 默认的工厂
        /// </summary>
        /// <remarks>
        /// 仅仅创建一个GameObject并把对应的组件添加给这个GameObject
        /// </remarks>
        private class DefaultRuntimeFactory : IFactoryControls
        {
            public static IFactoryControls Default = new DefaultRuntimeFactory();

            public GameObject CreateGameObject(string name, params Type[] components)
            {
                return new GameObject(name, components);
            }
        }

        /// <summary>
        /// 默认资源
        /// </summary>
        public struct Resources
        {
            /// <summary>
            /// 默认图片:用与按钮等
            /// </summary>
            public Sprite standard;

            /// <summary>
            /// 默认背景图片
            /// </summary>
            public Sprite background;

            /// <summary>
            /// 默认输入框背景图片
            /// </summary>
            public Sprite inputField;

            /// <summary>
            /// 默认的可被拖动的点图片
            /// </summary>
            public Sprite knob;

            /// <summary>
            /// 默认选择框图片
            /// </summary>
            public Sprite checkmark;

            /// <summary>
            /// 默认下拉框图片
            /// </summary>
            public Sprite dropdown;

            /// <summary>
            /// 默认遮罩图片
            /// </summary>
            public Sprite mask;
        }

        private const float kWidth = 160f;
        private const float kThickHeight = 30f;
        private const float kThinHeight = 20f;
        private static Vector2 s_ThickElementSize = new Vector2(kWidth, kThickHeight);
        private static Vector2 s_ThinElementSize = new Vector2(kWidth, kThinHeight);
        private static Vector2 s_ImageElementSize = new Vector2(100f, 100f);
        private static Color s_DefaultSelectableColor = new Color(1f, 1f, 1f, 1f);
        private static Color s_PanelColor = new Color(1f, 1f, 1f, 0.392f);
        private static Color s_TextColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);

        // Helper methods at top

        #region UI
        private static GameObject CreateUIElementRoot(string name, Vector2 size, params Type[] components)
        {
            GameObject child = factory.CreateGameObject(name, components);
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            return child;
        }

        private static GameObject CreateUIObject(string name, GameObject parent, params Type[] components)
        {
            GameObject go = factory.CreateGameObject(name, components);
            SetParentAndAlign(go, parent);
            return go;
        }

        private static void SetDefaultTextValues(Text lbl)
        {
            // Set text values we want across UI elements in default controls.
            // Don't set values which are the same as the default values for the Text component,
            // since there's no point in that, and it's good to keep them as consistent as possible.
            lbl.color = s_TextColor;

            // Reset() is not called when playing. We still want the default font to be assigned
            //lbl.AssignDefaultFont();
        }
        #endregion

        private static GameObject CreateElementRoot(string name, Vector3 scale, params Type[] components)
        {
            GameObject child = factory.CreateGameObject(name, components);
            child.transform.localScale = scale;
            return child;
        }

        private static GameObject CreateObject(string name, GameObject parent, params Type[] components)
        {
            GameObject go = factory.CreateGameObject(name, components);
            SetParentAndAlign(go, parent);
            return go;
        }

        private static void SetDefaultColorTransitionValues(Selectable slider)
        {
            ColorBlock colors = slider.colors;
            colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
            colors.pressedColor = new Color(0.698f, 0.698f, 0.698f);
            colors.disabledColor = new Color(0.521f, 0.521f, 0.521f);
        }

        private static void SetParentAndAlign(GameObject child, GameObject parent)
        {
            if (parent == null)
                return;

            child.transform.SetParent(parent.transform, false);

            SetLayerRecursively(child, parent.layer);
        }

        private static void SetLayerRecursively(GameObject go, int layer)
        {
            go.layer = layer;
            Transform t = go.transform;
            for (int i = 0; i < t.childCount; i++)
                SetLayerRecursively(t.GetChild(i).gameObject, layer);
        }

        public static GameObject CreateDefaultGameLauncher(Resources resources)
        {
            var root = CreateElementRoot("GameLauncher", Vector3.one);
            return root;
        }
    }
}