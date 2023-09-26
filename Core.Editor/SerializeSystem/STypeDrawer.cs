using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;
using ZLC.SerializeSystem;
using ZLC.Utils;
using ZLCEditor.Tool;
namespace ZLCEditor.SerializeSystem;

/// <summary>
/// <see cref="SType"/>的编辑器
/// </summary>
[CustomPropertyDrawer(typeof(SType))]
public class STypeDrawer : PropertyDrawer
{
    private class STypeSelect : AdvancedDropdown
    {
        private string selected;
        private string[] _types;
        private SerializedProperty fullNameProp;
        private SerializedProperty prop;
        private SerializedProperty tempNameProp;

        public STypeSelect(AdvancedDropdownState state, SerializedProperty fullNameProp, SerializedProperty tempNameProp,SerializedProperty prop) : base(state)
        {
            this.fullNameProp = fullNameProp;
            this.prop = prop;
            this.tempNameProp = tempNameProp;
            var maximumSize = GetType().GetProperty("maximumSize", BindingFlags.NonPublic | BindingFlags.Instance);
            maximumSize.SetValue(this, new Vector2(1080f, 450f));
            InitTypes();
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem("类型选择");

            foreach (var type in _types) {
                root.AddChild(new AdvancedDropdownItem(type));
            }
            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            selected = item.name;
            fullNameProp.stringValue = selected;
            tempNameProp.stringValue = Type.GetType(selected).Name;
            fullNameProp.serializedObject.ApplyModifiedProperties();
            var boxedValue = prop.GetType().GetProperty("boxedValue");
            boxedValue.SetValue(prop,new SType());
        }

        private void InitTypes()
        {
            if (_types != null) return;
            var typeList = new List<string>();
            void AddTypes(DefaultAsset[] dlls, AssemblyDefinitionAsset[] assemblies)
            {
                foreach (var dll in dlls) {
                    var assembly = Assembly.Load(dll.name);
                    var types = assembly.GetTypes();
                    foreach (var type in types) {
                        typeList.Add(type.AssemblyQualifiedName);
                    }
                }
                foreach (var assemblyAsset in assemblies) {
                    var assembly = Assembly.Load(assemblyAsset.name);
                    var types = assembly.GetTypes();
                    foreach (var type in types) {
                        typeList.Add(type.AssemblyQualifiedName);
                    }
                }
            }
            var filterType = EditorHelper.AssemblyFilterType.Custom | EditorHelper.AssemblyFilterType.Internal;
            Action<EditorHelper.AssemblyFilterType> getAllChildType = singleType =>
            {
                var assemblyForToolSO = AssemblyForToolSO.Instance;
                switch (singleType) {
                    case EditorHelper.AssemblyFilterType.Custom:
                        AddTypes(assemblyForToolSO.selfDlls, assemblyForToolSO.selfAssemblies);
                        break;
                    case EditorHelper.AssemblyFilterType.Unity:
                        AddTypes(assemblyForToolSO.unityDlls, assemblyForToolSO.unityAssemblies);
                        break;
                    case EditorHelper.AssemblyFilterType.Other:
                        AddTypes(assemblyForToolSO.otherDlls, assemblyForToolSO.otherAssemblies);
                        break;
                    case EditorHelper.AssemblyFilterType.Internal:
                        AddTypes(assemblyForToolSO.defaultDlls, assemblyForToolSO.defaultAssemblies);
                        break;
                    default:
                        Debug.LogError($"filterType数据分割错误.source:{filterType} single:{singleType}");
                        break;
                }
            };
            EnumHelper.ForEachFlag(filterType, getAllChildType);

            _types = typeList.ToArray();
        }
    }

    //private SerializedProperty _fullName;
    private GUIContent value = new GUIContent("value");
    private SerializedProperty fullNameProp;
    private SerializedProperty tempNameProp;
    private static string[] _types;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        // 一个文本一个下拉箭头
        if (fullNameProp == null) {
            fullNameProp = property.FindPropertyRelative("fullName");
            tempNameProp = property.FindPropertyRelative("tempName");
        }
        value.text = tempNameProp.stringValue;
        value.tooltip = fullNameProp.stringValue;
        if (EditorGUILayout.DropdownButton(value, FocusType.Passive)) {
            var dropdown = new STypeSelect(new AdvancedDropdownState(), fullNameProp, tempNameProp, property);
            dropdown.Show(position);
        }
        EditorGUI.EndProperty();
    }
}