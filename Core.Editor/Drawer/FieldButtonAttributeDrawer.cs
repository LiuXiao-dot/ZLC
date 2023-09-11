using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using ZLCEditor.Attributes;
namespace ZLCEditor.Drawer
{
    /// <summary>
    /// FieldButtonAttribute标记对象的绘制器
    /// </summary>
    public class FieldButtonAttributeDrawer : OdinAttributeDrawer<FieldButtonAttribute>
    {
        private string name;
        private int buttonHeight;
        private string methodName;
        private ButtonStyle buttonStyle;
        private Color btnColor;
        private GUIStyle style;
        private GUIContent label;

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            this.name = Attribute.name;
            if (this.name.IsNullOrWhitespace())
            {
                if (Property.Info.GetMemberInfo() is MemberInfo memberInfo)
                {
                    this.name = memberInfo.Name;
                }
                else
                {
                    this.name = Property.ValueEntry.WeakValues[0].ToString();
                }
            }
            
            this.buttonHeight = Attribute.buttonHeight;
            this.methodName = Attribute.methodName;
            this.buttonStyle = Attribute.buttonStyle;
            this.btnColor = GUI.color;
            this.style = this.Property.Context.GetGlobal<GUIStyle>("ButtonStyle", (GUIStyle)null).Value;
            this.label = new GUIContent(this.name);
            if (this.style == null)
                this.style = (double)this.buttonHeight <= 20.0 ? EditorStyles.miniButton : SirenixGUIStyles.Button;
            if (this.buttonStyle != ButtonStyle.FoldoutButton)
                return;
            if ((double)this.buttonHeight > 20.0)
            {
                this.style = SirenixGUIStyles.ButtonLeft;
            }
            else
            {
                this.style = EditorStyles.miniButtonLeft;
            }
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            this.Property.Label = this.label;

            switch (buttonStyle)
            {
                case ButtonStyle.CompactBox:
                    break;
                case ButtonStyle.FoldoutButton:
                    break;
                case ButtonStyle.Box:
                    DrawNormalButton();
                    break;
                default:
                    break;
            }
        }

        private void DrawNormalButton()
        {
            Rect position = EditorGUI.IndentedRect((double)this.buttonHeight > 0.0
                ? GUILayoutUtility.GetRect(GUIContent.none, this.style,
                    (GUILayoutOption[])GUILayoutOptions.Height(this.buttonHeight))
                : GUILayoutUtility.GetRect(GUIContent.none, this.style));
            Color color = GUI.color;
            GUI.color = this.btnColor;
            if (GUI.Button(position, this.label != null ? this.label : GUIHelper.TempContent(string.Empty), this.style))
                this.InvokeButton();
            GUI.color = color;
        }

        private void InvokeButton()
        {
            try {
                //bool flag = this.hasReturnValue && Event.current.button == 1;
                GUIHelper.RemoveFocusControl();
                GUIHelper.RequestRepaint();
                MethodInfo methodInfo = null;
                object[] argument = null;
                object methodOwner = null;
                // true:继续查找 查找顺序：定义的类->实际类型->原类型
                bool CheckMethod(object self)
                {
                    methodInfo = methodOwner.GetType().GetMethod(methodName, new Type[]
                    {
                        self.GetType()
                    });
                    if (methodInfo == null) {
                        methodInfo = methodOwner.GetType().GetMethod(methodName);
                    } else {
                        argument = new[]
                        {
                            self
                        };
                    }

                    return methodInfo == null;
                }

                switch (this.Property.Info.GetMemberInfo()) {
                    case MethodInfo:
                        methodInfo = this.Property.Info.GetMemberInfo() as MethodInfo;
                        methodOwner = this.Property.ParentValues[0];
                        break;
                    case FieldInfo:
                        var fieldSelf = this.Property.Info.GetMemberInfo() as FieldInfo;
                        methodOwner = this.Property.SerializationRoot.ValueEntry.WeakValues[0];

                        if (CheckMethod(fieldSelf)) {
                            methodOwner = fieldSelf;
                        } else {
                            break;
                        }

                        CheckMethod(fieldSelf);
                        break;
                    default:
                        var self = this.Property.ValueEntry.WeakValues[0];
                        methodOwner = this.Property.SerializationRoot.ValueEntry.WeakValues[0];

                        if (CheckMethod(self)) {
                            methodOwner = this.Property.ParentValues[0];
                        } else {
                            break;
                        }

                        if (CheckMethod(self)) {
                            methodOwner = self;
                        } else {
                            break;
                        }

                        CheckMethod(self);
                        break;
                }

                if ((object)methodInfo == null) {
                    GUIUtility.ExitGUI();
                    return;
                }

                if (methodInfo.IsGenericMethodDefinition) {
                    Debug.LogError((object)"Cannot invoke a generic method definition.");
                } else {
                    if (this.Attribute == null || this.Attribute.dirtyOnClick) {
                        if (this.Property.ParentValueProperty != null)
                            this.Property.ParentValueProperty.RecordForUndo(
                                "Clicked Button '" + this.Property.NiceName + "'", true);
                        foreach (UnityEngine.Object unityObj in this.Property.SerializationRoot.ValueEntry.WeakValues
                                     .OfType<UnityEngine.Object>())
                            InspectorUtilities.RegisterUnityObjectDirty(unityObj);
                    }

                    if (argument != null)
                        methodInfo.Invoke(methodOwner, argument);
                    else
                        methodInfo.Invoke(methodOwner, null);
                }
            }
            catch(Exception e) {
                Debug.LogError(e);
            }
            finally
            {
                GUIUtility.ExitGUI();
            }
        }
    }
}