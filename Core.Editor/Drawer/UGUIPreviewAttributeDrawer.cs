using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using ZLCEditor.Attributes;
namespace ZLCEditor.Drawer
{
    [AllowGUIEnabledForReadonly]
    public sealed class UGUIPreviewAttributeDrawer<GameObject> : OdinAttributeDrawer<UGUIPreviewAttribute, GameObject>
    {
        /// <summary>Draws the property.</summary>
        protected override void DrawPropertyLayout(GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            var obj = (GameObject)this.ValueEntry.WeakSmartValue;
            
            //Sirenix.Utilities.Editor.ObjectFieldAlignment alignment = !this.Attribute.AlignmentHasValue ? GlobalConfig<GeneralDrawerConfig>.Instance.SquareUnityObjectAlignment : (Sirenix.Utilities.Editor.ObjectFieldAlignment) this.Attribute.Alignment;
            //this.ValueEntry.WeakSmartValue = (object) SirenixEditorFields.UnityPreviewObjectField(label, , this.ValueEntry.BaseValueType, this.ValueEntry.Property.GetAttribute<AssetsOnlyAttribute>() == null, (double) this.Attribute.Height == 0.0 ? GlobalConfig<GeneralDrawerConfig>.Instance.SquareUnityObjectFieldHeight : this.Attribute.Height, alignment);
            if (!EditorGUI.EndChangeCheck())
                return;
            this.ValueEntry.Values.ForceMarkDirty();
        }
    }
}
