using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using ZLC.SerializeSystem;
namespace ZLCEditor.SerializeSystem;

/// <summary>
/// Property drawer for <see cref="T:System.Collections.Generic.IDictionary`2" />.
/// </summary>
public class SDictionaryDrawer<TDictionary, TKey, TValue> :
    OdinValueDrawer<TDictionary>,
    IDisposable
    where TDictionary : SDictionary<TKey, TValue>
{
    private const string CHANGE_ID = "DICTIONARY_DRAWER";
    private static readonly bool KeyIsValueType = typeof(TKey).IsValueType;
    private static GUIStyle addKeyPaddingStyle;
    private static GUIStyle listItemStyle;
    private GUIPagingHelper paging = new GUIPagingHelper();
    private GeneralDrawerConfig config;
    private LocalPersistentContext<float> keyWidthOffset;
    private bool showAddKeyGUI;
    private bool? newKeyIsValid;
    private string newKeyErrorMessage;
    private TKey newKey;
    private TValue newValue;
    private StrongDictionaryPropertyResolver<TDictionary, TKey, TValue> dictionaryResolver;
    private DictionaryDrawerSettings attrSettings;
    private bool disableAddKey;
    private GUIContent keyLabel;
    private GUIContent valueLabel;
    private float keyLabelWidth;
    private float valueLabelWidth;
    private TempKeyValuePair<TKey, TValue> tempKeyValue;
    private PropertyTree keyEntryPropertyTree;
    private IPropertyValueEntry<TKey> tempKeyEntry;
    private IPropertyValueEntry<TValue> tempValueEntry;
    private static GUIStyle oneLineMargin;
    private static GUIStyle headerMargin;

    private static GUIStyle AddKeyPaddingStyle
    {
        get {
            if (SDictionaryDrawer<TDictionary, TKey, TValue>.addKeyPaddingStyle == null)
                SDictionaryDrawer<TDictionary, TKey, TValue>.addKeyPaddingStyle = new GUIStyle((GUIStyle)"CN Box")
                {
                    overflow = new RectOffset(0, 0, 1, 0),
                    fixedHeight = 0.0f,
                    stretchHeight = false,
                    padding = new RectOffset(10, 10, 10, 10)
                };
            return SDictionaryDrawer<TDictionary, TKey, TValue>.addKeyPaddingStyle;
        }
    }

    protected override bool CanDrawValueProperty(InspectorProperty property) => property.ChildResolver is StrongDictionaryPropertyResolver<TDictionary, TKey, TValue>;

    protected override void Initialize()
    {
        SDictionaryDrawer<TDictionary, TKey, TValue>.listItemStyle = new GUIStyle(GUIStyle.none)
        {
            padding = new RectOffset(7, 20, 3, 3)
        };
        IPropertyValueEntry<TDictionary> valueEntry = this.ValueEntry;
        this.attrSettings = valueEntry.Property.GetAttribute<DictionaryDrawerSettings>() ?? new DictionaryDrawerSettings();
        this.keyWidthOffset = this.GetPersistentValue<float>("KeyColumnWidth", this.attrSettings.KeyColumnWidth);
        this.disableAddKey = valueEntry.Property.Tree.PrefabModificationHandler.HasPrefabs && valueEntry.SerializationBackend == SerializationBackend.Odin && !valueEntry.Property.SupportsPrefabModifications;
        this.keyLabel = new GUIContent(this.attrSettings.KeyLabel);
        this.valueLabel = new GUIContent(this.attrSettings.ValueLabel);
        this.keyLabelWidth = EditorStyles.label.CalcSize(this.keyLabel).x + 20f;
        this.valueLabelWidth = EditorStyles.label.CalcSize(this.valueLabel).x + 20f;
        if (this.disableAddKey)
            return;
        this.tempKeyValue = new TempKeyValuePair<TKey, TValue>();
        
        this.keyEntryPropertyTree = PropertyTree.Create((object)this.tempKeyValue);
        this.keyEntryPropertyTree.UpdateTree();
        this.tempKeyEntry = (IPropertyValueEntry<TKey>)this.keyEntryPropertyTree.GetPropertyAtPath("Key").ValueEntry;
        this.tempValueEntry = (IPropertyValueEntry<TValue>)this.keyEntryPropertyTree.GetPropertyAtPath("Value").ValueEntry;
    }

    /// <summary>Draws the property.</summary>
    protected override void DrawPropertyLayout(GUIContent label)
    {
        IPropertyValueEntry<TDictionary> valueEntry = this.ValueEntry;
        this.dictionaryResolver = valueEntry.Property.ChildResolver as StrongDictionaryPropertyResolver<TDictionary, TKey, TValue>;
        this.config = GlobalConfig<GeneralDrawerConfig>.Instance;
        this.paging.NumberOfItemsPerPage = this.config.NumberOfItemsPrPage;
        SDictionaryDrawer<TDictionary, TKey, TValue>.listItemStyle.padding.right = !valueEntry.IsEditable || this.attrSettings.IsReadOnly ? 4 : 20;
        SirenixEditorGUI.BeginIndentedVertical(SirenixGUIStyles.PropertyPadding);
        this.paging.Update(valueEntry.Property.Children.Count);
        this.DrawToolbar(valueEntry, label);
        this.paging.Update(valueEntry.Property.Children.Count);
        if (!this.disableAddKey && !this.attrSettings.IsReadOnly)
            this.DrawAddKey(valueEntry);
        GUIHelper.BeginLayoutMeasuring();
        float t;
        if (SirenixEditorGUI.BeginFadeGroup((object)UniqueDrawerKey.Create(valueEntry.Property, (OdinDrawer)this), this.Property.State.Expanded, out t)) {
            Rect rect1 = SirenixEditorGUI.BeginVerticalList(false, true);
            if (this.attrSettings.DisplayMode == DictionaryDisplayOptions.OneLine) {
                float max = rect1.width - 90f;
                rect1.xMin = this.keyWidthOffset.Value + 22f;
                rect1.xMax = rect1.xMin + 10f;
                GUIHelper.PushGUIEnabled(true);
                this.keyWidthOffset.Value += SirenixEditorGUI.SlideRect(rect1).x;
                GUIHelper.PopGUIEnabled();
                if (Event.current.type == UnityEngine.EventType.Repaint)
                    this.keyWidthOffset.Value = Mathf.Clamp(this.keyWidthOffset.Value, 30f, max);
                if (this.paging.ElementCount != 0) {
                    Rect rect2 = SirenixEditorGUI.BeginListItem(false, (GUIStyle)null);
                    GUILayout.Space(14f);
                    if (Event.current.type == UnityEngine.EventType.Repaint) {
                        GUI.Label(rect2.SetWidth(this.keyWidthOffset.Value), this.keyLabel, SirenixGUIStyles.LabelCentered);
                        GUI.Label(rect2.AddXMin(this.keyWidthOffset.Value), this.valueLabel, SirenixGUIStyles.LabelCentered);
                        SirenixEditorGUI.DrawSolidRect(rect2.AlignBottom(1f), SirenixGUIStyles.BorderColor);
                    }
                    SirenixEditorGUI.EndListItem();
                }
            }
            GUIHelper.PushHierarchyMode(false);
            this.DrawElements(valueEntry, label);
            GUIHelper.PopHierarchyMode();
            SirenixEditorGUI.EndVerticalList();
        }
        SirenixEditorGUI.EndFadeGroup();
        Rect rect = GUIHelper.EndLayoutMeasuring();
        if ((double)t > 0.00999999977648258 && Event.current.type == UnityEngine.EventType.Repaint) {
            Color borderColor = SirenixGUIStyles.BorderColor;
            --rect.yMin;
            SirenixEditorGUI.DrawBorders(rect, 1, borderColor);
            borderColor.a *= t;
            if (this.attrSettings.DisplayMode == DictionaryDisplayOptions.OneLine) {
                rect.width = 1f;
                rect.x += this.keyWidthOffset.Value + 13f;
                SirenixEditorGUI.DrawSolidRect(rect, borderColor);
            }
        }
        SirenixEditorGUI.EndIndentedVertical();
    }

    private void DrawAddKey(IPropertyValueEntry<TDictionary> entry)
    {
        if (!entry.IsEditable || this.attrSettings.IsReadOnly)
            return;
        if (SirenixEditorGUI.BeginFadeGroup((object)this, this.showAddKeyGUI)) {
            GUILayout.BeginVertical(SDictionaryDrawer<TDictionary, TKey, TValue>.AddKeyPaddingStyle);
            if (typeof(TKey) == typeof(string) && (object)this.newKey == null) {
                this.newKey = default;
                this.newKeyIsValid = new bool?();
            }
            if (!this.newKeyIsValid.HasValue)
                this.newKeyIsValid = new bool?(SDictionaryDrawer<TDictionary, TKey, TValue>.CheckKeyIsValid(entry, this.newKey, out this.newKeyErrorMessage));
            this.tempKeyEntry.Property.Tree.BeginDraw(false);
            this.tempKeyEntry.Property.Update();
            EditorGUI.BeginChangeCheck();
            this.tempKeyEntry.Property.Draw(this.keyLabel);
            if (EditorGUI.EndChangeCheck() | this.tempKeyEntry.ApplyChanges()) {
                this.newKey = this.tempKeyValue.Key;
                UnityEditorEventUtility.EditorApplication_delayCall += (Action)(() => this.newKeyIsValid = new bool?());
                GUIHelper.RequestRepaint();
            }
            this.tempValueEntry.Property.Update();
            this.tempValueEntry.Property.Draw(this.valueLabel);
            this.tempValueEntry.ApplyChanges();
            this.newValue = this.tempKeyValue.Value;
            this.tempKeyEntry.Property.Tree.InvokeDelayedActions();
            if (this.tempKeyEntry.Property.Tree.ApplyChanges()) {
                this.newKey = this.tempKeyValue.Key;
                UnityEditorEventUtility.EditorApplication_delayCall += (Action)(() => this.newKeyIsValid = new bool?());
                GUIHelper.RequestRepaint();
            }
            this.tempKeyEntry.Property.Tree.EndDraw();
            GUIHelper.PushGUIEnabled(GUI.enabled && this.newKeyIsValid.Value);
            if (GUILayout.Button(this.newKeyIsValid.Value ? "Add" : this.newKeyErrorMessage)) {
                object[] keys = new object[entry.ValueCount];
                object[] values = new object[entry.ValueCount];
                for (int index = 0; index < keys.Length; ++index)
                    keys[index] = Sirenix.Serialization.SerializationUtility.CreateCopy((object)this.tempKeyValue.Key);
                for (int index = 0; index < values.Length; ++index)
                    values[index] = Sirenix.Serialization.SerializationUtility.CreateCopy((object)this.newValue);
                this.dictionaryResolver.QueueSet(keys, values);
                UnityEditorEventUtility.EditorApplication_delayCall += (Action)(() => this.newKeyIsValid = new bool?());
                GUIHelper.RequestRepaint();
                entry.Property.Tree.DelayActionUntilRepaint((Action)(() =>
                {
                    this.newValue = default(TValue);
                    this.tempKeyValue.Value = default(TValue);
                    this.tempValueEntry.Update();
                }));
            }
            GUIHelper.PopGUIEnabled();
            GUILayout.EndVertical();
        }
        SirenixEditorGUI.EndFadeGroup();
    }

    private void DrawToolbar(IPropertyValueEntry<TDictionary> entry, GUIContent label)
    {
        SirenixEditorGUI.BeginHorizontalToolbar();
        if (entry.ListLengthChangedFromPrefab)
            GUIHelper.PushIsBoldLabel(true);
        if (this.config.HideFoldoutWhileEmpty && this.paging.ElementCount == 0) {
            if (label != null)
                GUILayout.Label(label, (GUILayoutOption[])GUILayoutOptions.ExpandWidth(false));
        } else {
            bool flag = label != null ? SirenixEditorGUI.Foldout(this.Property.State.Expanded, label) : SirenixEditorGUI.Foldout(this.Property.State.Expanded, "");
            if (!flag && this.Property.State.Expanded)
                this.showAddKeyGUI = false;
            this.Property.State.Expanded = flag;
        }
        if (entry.ListLengthChangedFromPrefab)
            GUIHelper.PopIsBoldLabel();
        GUILayout.FlexibleSpace();
        if (this.config.ShowItemCount) {
            if (entry.ValueState == PropertyValueState.CollectionLengthConflict) {
                int num1 = entry.Values.Min<TDictionary>((Func<TDictionary, int>)(x => x.Count));
                int num2 = entry.Values.Max<TDictionary>((Func<TDictionary, int>)(x => x.Count));
                GUILayout.Label(num1.ToString() + " / " + num2.ToString() + " items", EditorStyles.centeredGreyMiniLabel);
            } else
                GUILayout.Label(this.paging.ElementCount == 0 ? "Empty" : this.paging.ElementCount.ToString() + " items", EditorStyles.centeredGreyMiniLabel);
        }
        if ((!this.config.HidePagingWhileCollapsed || this.Property.State.Expanded) && (!this.config.HidePagingWhileOnlyOnePage || this.paging.PageCount != 1)) {
            bool enabled = GUI.enabled;
            bool flag = this.paging.IsEnabled && this.paging.PageCount != 1;
            GUI.enabled = enabled & flag && !this.paging.IsOnFirstPage;
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.ArrowLeft, true)) {
                if (Event.current.button == 0)
                    --this.paging.CurrentPage;
                else
                    this.paging.CurrentPage = 0;
            }
            GUI.enabled = enabled & flag;
            this.paging.CurrentPage = EditorGUILayout.IntField(this.paging.CurrentPage + 1, (GUILayoutOption[])GUILayoutOptions.Width((float)(10 + this.paging.PageCount.ToString().Length * 10))) - 1;
            GUILayout.Label(GUIHelper.TempContent("/ " + this.paging.PageCount.ToString()));
            GUI.enabled = enabled & flag && !this.paging.IsOnLastPage;
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.ArrowRight, true)) {
                if (Event.current.button == 0)
                    ++this.paging.CurrentPage;
                else
                    this.paging.CurrentPage = this.paging.PageCount - 1;
            }
            GUI.enabled = enabled && this.paging.PageCount != 1;
            if (this.config.ShowExpandButton && SirenixEditorGUI.ToolbarButton(this.paging.IsEnabled ? EditorIcons.ArrowDown : EditorIcons.ArrowUp, true))
                this.paging.IsEnabled = !this.paging.IsEnabled;
            GUI.enabled = enabled;
        }
        if (!this.disableAddKey && !this.attrSettings.IsReadOnly && SirenixEditorGUI.ToolbarButton(EditorIcons.Plus)) {
            this.showAddKeyGUI = !this.showAddKeyGUI;
            if (this.showAddKeyGUI)
                this.Property.State.Expanded = true;
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }

    private static GUIStyle OneLineMargin
    {
        get {
            if (SDictionaryDrawer<TDictionary, TKey, TValue>.oneLineMargin == null)
                SDictionaryDrawer<TDictionary, TKey, TValue>.oneLineMargin = new GUIStyle()
                {
                    margin = new RectOffset(8, 0, 0, 0)
                };
            return SDictionaryDrawer<TDictionary, TKey, TValue>.oneLineMargin;
        }
    }

    private static GUIStyle HeaderMargin
    {
        get {
            if (SDictionaryDrawer<TDictionary, TKey, TValue>.headerMargin == null)
                SDictionaryDrawer<TDictionary, TKey, TValue>.headerMargin = new GUIStyle()
                {
                    margin = new RectOffset(40, 0, 0, 0)
                };
            return SDictionaryDrawer<TDictionary, TKey, TValue>.headerMargin;
        }
    }

    private void DrawElements(IPropertyValueEntry<TDictionary> entry, GUIContent label)
    {
        for (int i = this.paging.StartIndex; i < this.paging.EndIndex; i++) {
            InspectorProperty child1 = entry.Property.Children[i];
            EditableKeyValuePair<TKey, TValue> smartValue = (child1.ValueEntry as IPropertyValueEntry<EditableKeyValuePair<TKey, TValue>>).SmartValue;
            Rect rect1 = SirenixEditorGUI.BeginListItem(false, SDictionaryDrawer<TDictionary, TKey, TValue>.listItemStyle);
            if (this.attrSettings.DisplayMode != DictionaryDisplayOptions.OneLine) {
                bool defaultValue;
                switch (this.attrSettings.DisplayMode) {
                    case DictionaryDisplayOptions.CollapsedFoldout:
                        defaultValue = false;
                        break;
                    case DictionaryDisplayOptions.ExpandedFoldout:
                        defaultValue = true;
                        break;
                    default:
                        defaultValue = SirenixEditorGUI.ExpandFoldoutByDefault;
                        break;
                }
                LocalPersistentContext<bool> persistent = child1.Context.GetPersistent<bool>((OdinDrawer)this, "Expanded", defaultValue);
                SirenixEditorGUI.BeginBox();
                SirenixEditorGUI.BeginToolbarBoxHeader();
                if (smartValue.IsInvalidKey)
                    GUIHelper.PushColor(Color.red);
                Rect rect2 = GUIHelper.GetCurrentLayoutRect().AlignLeft((float)SDictionaryDrawer<TDictionary, TKey, TValue>.HeaderMargin.margin.left);
                ++rect2.y;
                GUILayout.BeginVertical(SDictionaryDrawer<TDictionary, TKey, TValue>.HeaderMargin);
                GUIHelper.PushIsDrawingDictionaryKey(true);
                GUIHelper.PushLabelWidth(this.keyLabelWidth);
                this.DrawKeyProperty(child1.Children[0], GUIHelper.TempContent(" "));
                GUIHelper.PopLabelWidth();
                GUIHelper.PopIsDrawingDictionaryKey();
                GUILayout.EndVertical();
                if (smartValue.IsInvalidKey)
                    GUIHelper.PopColor();
                persistent.Value = SirenixEditorGUI.Foldout(rect2, persistent.Value, this.keyLabel);
                SirenixEditorGUI.EndToolbarBoxHeader();
                if (SirenixEditorGUI.BeginFadeGroup((object)persistent, persistent.Value))
                    child1.Children[1].Draw((GUIContent)null);
                SirenixEditorGUI.EndFadeGroup();
                SirenixEditorGUI.EndToolbarBox();
            } else {
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical((GUILayoutOption[])GUILayoutOptions.Width(this.keyWidthOffset.Value));
                InspectorProperty child2 = child1.Children[0];
                if (smartValue.IsInvalidKey)
                    GUIHelper.PushColor(Color.red);
                if (this.attrSettings.IsReadOnly)
                    GUIHelper.PushGUIEnabled(false);
                GUIHelper.PushIsDrawingDictionaryKey(true);
                GUIHelper.PushLabelWidth(10f);
                this.DrawKeyProperty(child2, (GUIContent)null);
                GUIHelper.PopLabelWidth();
                GUIHelper.PopIsDrawingDictionaryKey();
                if (this.attrSettings.IsReadOnly)
                    GUIHelper.PopGUIEnabled();
                if (smartValue.IsInvalidKey)
                    GUIHelper.PopColor();
                GUILayout.EndVertical();
                GUILayout.BeginVertical(SDictionaryDrawer<TDictionary, TKey, TValue>.OneLineMargin);
                GUIHelper.PushHierarchyMode(false);
                InspectorProperty child3 = child1.Children[1];
                float actualLabelWidth = GUIHelper.ActualLabelWidth;
                GUIHelper.BetterLabelWidth = 150f;
                child3.Draw((GUIContent)null);
                GUIHelper.BetterLabelWidth = actualLabelWidth;
                GUIHelper.PopHierarchyMode();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            if (entry.IsEditable && !this.attrSettings.IsReadOnly && SirenixEditorGUI.IconButton(new Rect((float)((double)rect1.xMax - 24.0 + 5.0), rect1.y + 4f + (float)(((int)rect1.height - 23) / 2), 14f, 14f), EditorIcons.X)) {
                this.dictionaryResolver.QueueRemoveKey(Enumerable.Range(0, entry.ValueCount).Select<int, object>((Func<int, object>)(n => this.dictionaryResolver.GetKey(n, i))).ToArray<object>());
                UnityEditorEventUtility.EditorApplication_delayCall += (Action)(() => this.newKeyIsValid = new bool?());
                GUIHelper.RequestRepaint();
            }
            SirenixEditorGUI.EndListItem();
        }
        if (!this.paging.IsOnLastPage || entry.ValueState != PropertyValueState.CollectionLengthConflict)
            return;
        SirenixEditorGUI.BeginListItem(false, (GUIStyle)null);
        GUILayout.Label(GUIHelper.TempContent("------"), EditorStyles.centeredGreyMiniLabel);
        SirenixEditorGUI.EndListItem();
    }

    private void DrawKeyProperty(InspectorProperty keyProperty, GUIContent keyLabel)
    {
        EditorGUI.BeginChangeCheck();
        keyProperty.Draw(keyLabel);
        bool flag1 = EditorGUI.EndChangeCheck();
        bool flag2 = SDictionaryDrawer<TDictionary, TKey, TValue>.ValuesAreDirty(keyProperty);
        if (!flag1 & flag2) {
            this.dictionaryResolver.ValueApplyIsTemporary = true;
            SDictionaryDrawer<TDictionary, TKey, TValue>.ApplyChangesToProperty(keyProperty);
            this.dictionaryResolver.ValueApplyIsTemporary = false;
        } else {
            if (!flag1 || flag2)
                return;
            SDictionaryDrawer<TDictionary, TKey, TValue>.MarkPropertyDirty(keyProperty);
        }
    }

    private static void MarkPropertyDirty(InspectorProperty keyProperty)
    {
        keyProperty.ValueEntry.WeakValues.ForceMarkDirty();
        if (!SDictionaryDrawer<TDictionary, TKey, TValue>.KeyIsValueType)
            return;
        for (int index = 0; index < keyProperty.Children.Count; ++index)
            SDictionaryDrawer<TDictionary, TKey, TValue>.MarkPropertyDirty(keyProperty.Children[index]);
    }

    private static void ApplyChangesToProperty(InspectorProperty keyProperty)
    {
        if (keyProperty.ValueEntry != null && keyProperty.ValueEntry.WeakValues.AreDirty)
            keyProperty.ValueEntry.ApplyChanges();
        if (!SDictionaryDrawer<TDictionary, TKey, TValue>.KeyIsValueType)
            return;
        for (int index = 0; index < keyProperty.Children.Count; ++index)
            SDictionaryDrawer<TDictionary, TKey, TValue>.ApplyChangesToProperty(keyProperty.Children[index]);
    }

    private static bool ValuesAreDirty(InspectorProperty keyProperty)
    {
        if (keyProperty.ValueEntry != null && keyProperty.ValueEntry.WeakValues.AreDirty)
            return true;
        if (SDictionaryDrawer<TDictionary, TKey, TValue>.KeyIsValueType) {
            for (int index = 0; index < keyProperty.Children.Count; ++index) {
                if (SDictionaryDrawer<TDictionary, TKey, TValue>.ValuesAreDirty(keyProperty.Children[index]))
                    return true;
            }
        }
        return false;
    }

    private static bool CheckKeyIsValid(
        IPropertyValueEntry<TDictionary> entry,
        TKey key,
        out string errorMessage)
    {
        if (!SDictionaryDrawer<TDictionary, TKey, TValue>.KeyIsValueType && (object)key == null) {
            errorMessage = "Key cannot be null.";
            return true;
        }
        string dictionaryKeyString = DictionaryKeyUtility.GetDictionaryKeyString((object)key);
        if (entry.Property.Children[dictionaryKeyString] == null) {
            errorMessage = "";
            return true;
        }
        errorMessage = "An item with the same key already exists.";
        return false;
    }

    public void Dispose()
    {
        if (this.keyEntryPropertyTree == null)
            return;
        this.keyEntryPropertyTree.Dispose();
        this.keyEntryPropertyTree = (PropertyTree)null;
    }
}