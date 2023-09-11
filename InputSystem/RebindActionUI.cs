using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
namespace ZLC.InputSystem;

/// <summary>
/// 重新绑定输入事件
/// </summary>
public class RebindActionUI : MonoBehaviour
{
    private InputActionReference _actionReference;

    /// <summary>
    /// 待重新绑定的action
    /// </summary>
    public InputActionReference actionReference
    {
        get => _actionReference;
        set {
            _actionReference = value;
            // 更新Action文本
            UpdateBindingDisplay();
        }
    }

    private string _bindingId;

    /// <summary>
    /// 待重新弄绑定的action的id
    /// </summary>
    public string bindingId
    {
        get => _bindingId;
        set {
            _bindingId = value;
            UpdateBindingDisplay();
        }
    }

    private InputBinding.DisplayStringOptions _displayStringOptions;
    /// <summary>
    /// 字符串展示的类型设置，不同设置将会展示出不同的文本内容
    /// </summary>
    public InputBinding.DisplayStringOptions displayStringOptions
    {
        get => _displayStringOptions;
        set {
            _displayStringOptions = value;
            UpdateBindingDisplay();
        }
    }

    private TextMeshProUGUI _actionLabel;
    /// <summary>
    /// action的文本内容
    /// </summary>
    public TextMeshProUGUI actionLabel
    {
        get => _actionLabel;
        set {
            _actionLabel = value;
            UpdateActionLabel();
        }
    }

    private TextMeshProUGUI _bindingTxt;
    /// <summary>
    /// 绑定输入的文本
    /// </summary>
    public TextMeshProUGUI bindingTxt
    {
        get => _bindingTxt;
        set {
            _bindingTxt = value;
            UpdateBindingDisplay();
        }
    }

    private TextMeshProUGUI _rebindText;
    /// <summary>
    /// 重新绑定的绑定提示文本
    /// </summary>
    public TextMeshProUGUI rebindText
    {
        get => _rebindText;
        set => _rebindText = value;
    }

    private GameObject _rebindOverlay;
    /// <summary>
    /// 重新绑定输入的层级
    /// </summary>
    public GameObject rebindOverlay
    {
        get => _rebindOverlay;
        set {
            _rebindOverlay = value;
        }
    }

    private UpdateBindingUIEvent _updateBindingUIEvent;
    /// <summary>
    /// 每当有UI更新时触发
    /// </summary>
    public UpdateBindingUIEvent updateBindingUIEvent
    {
        get {
            if (_updateBindingUIEvent == null)
                _updateBindingUIEvent = new UpdateBindingUIEvent();
            return _updateBindingUIEvent;
        }
    }

    private InteractiveRebindEvent _startRebindEvent;
    /// <summary>
    /// 启动重新绑定时触发的事件
    /// </summary>
    public InteractiveRebindEvent startRebindEvent
    {
        get {
            if (_startRebindEvent == null)
                _startRebindEvent = new InteractiveRebindEvent();
            return _startRebindEvent;
        }
    }

    private InteractiveRebindEvent _stopRebindEvent;
    /// <summary>
    /// 停止重新绑定时触发的事件
    /// </summary>
    public InteractiveRebindEvent stopRebindEvent
    {
        get {
            if (_stopRebindEvent == null)
                _stopRebindEvent = new InteractiveRebindEvent();
            return _stopRebindEvent;
        }
    }

    private InputActionRebindingExtensions.RebindingOperation _rebindOperation;
    /// <summary>
    /// 正在进行绑定操作时的绑定控制器
    /// </summary>
    public InputActionRebindingExtensions.RebindingOperation onGoingRebind => _rebindOperation;

    private static List<RebindActionUI> _rebindActionUIs;

    /// <summary>
    /// 解析Action和bindindIndex
    /// </summary>
    /// <param name="action"></param>
    /// <param name="bindingIndex"></param>
    /// <returns></returns>
    public bool ResolveActionAndBinding(out InputAction action, out int bindingIndex)
    {
        bindingIndex = -1;

        action = _actionReference?.action;
        if (action == null)
            return false;

        if (string.IsNullOrEmpty(_bindingId))
            return false;

        // Look up binding index.
        var bindingId = new Guid(_bindingId);
        bindingIndex = action.bindings.IndexOf(x => x.id == bindingId);
        if (bindingIndex == -1) {
            Debug.LogError($"Cannot find binding with ID '{bindingId}' on '{action}'", this);
            return false;
        }

        return true;
    }

    /// <summary>
    /// 更新绑定的显示
    /// </summary>
    public void UpdateBindingDisplay()
    {
        var displayString = string.Empty;
        var deviceLayoutName = default(string);
        var controlPath = default(string);

        // Get display string from action.
        var action = _actionReference?.action;
        if (action != null) {
            var bindingIndex = action.bindings.IndexOf(x => x.id.ToString() == _bindingId);
            if (bindingIndex != -1)
                displayString = action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath, displayStringOptions);
        }

        // Set on label (if any).
        if (_bindingTxt != null)
            _bindingTxt.text = displayString;

        // Give listeners a chance to configure UI in response.
        _updateBindingUIEvent?.Invoke(this, displayString, deviceLayoutName, controlPath);
    }

    /// <summary>
    /// 将输入事件设定为默认值
    /// </summary>
    public void ResetToDefault()
    {
        if (!ResolveActionAndBinding(out var action, out var bindingIndex))
            return;

        if (action.bindings[bindingIndex].isComposite) {
            // It's a composite. Remove overrides from part bindings.
            for (var i = bindingIndex + 1; i < action.bindings.Count && action.bindings[i].isPartOfComposite; ++i)
                action.RemoveBindingOverride(i);
        } else {
            action.RemoveBindingOverride(bindingIndex);
        }
        UpdateBindingDisplay();
    }

    /// <summary>
    /// 启用重新绑定
    /// </summary>
    public void StartInteractiveRebind()
    {
        if (!ResolveActionAndBinding(out var action, out var bindingIndex))
            return;

        // If the binding is a composite, we need to rebind each part in turn.
        if (action.bindings[bindingIndex].isComposite) {
            var firstPartIndex = bindingIndex + 1;
            if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isPartOfComposite)
                PerformInteractiveRebind(action, firstPartIndex, allCompositeParts: true);
        } else {
            PerformInteractiveRebind(action, bindingIndex);
        }
    }

    /// <summary>
    /// 进行重新绑定
    /// </summary>
    /// <param name="action"></param>
    /// <param name="bindingIndex"></param>
    /// <param name="allCompositeParts"></param>
    private void PerformInteractiveRebind(InputAction action, int bindingIndex, bool allCompositeParts = false)
    {
        _rebindOperation?.Cancel(); // Will null out _rebindOperation.

        void CleanUp()
        {
            _rebindOperation?.Dispose();
            _rebindOperation = null;
        }
        action.Disable();

        // Configure the rebind.
        _rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
            /*todo:.WithControlsExcluding("<Mouse>")
            .WithControlsExcluding("<Mouse>/leftbutton")*/
            .WithCancelingThrough("<Keyboard>/escape")
            .OnCancel(
                operation =>
                {
                    action.Enable();
                    _stopRebindEvent?.Invoke(this, operation);
                    _rebindOverlay?.SetActive(false);
                    UpdateBindingDisplay();
                    CleanUp();
                })
            .OnComplete(
                operation =>
                {
                    action.Enable();
                    _rebindOverlay?.SetActive(false);
                    _stopRebindEvent?.Invoke(this, operation);

                    if (CheckDuplicateBinding(action, bindingIndex, allCompositeParts)) {
                        action.RemoveBindingOverride(bindingIndex);
                        CleanUp();
                        PerformInteractiveRebind(action, bindingIndex, allCompositeParts);
                        return;
                    }

                    UpdateBindingDisplay();
                    CleanUp();

                    // If there's more composite parts we should bind, initiate a rebind
                    // for the next part.
                    if (allCompositeParts) {
                        var nextBindingIndex = bindingIndex + 1;
                        if (nextBindingIndex < action.bindings.Count && action.bindings[nextBindingIndex].isPartOfComposite)
                            PerformInteractiveRebind(action, nextBindingIndex, true);
                    }
                });

        // If it's a part binding, show the name of the part in the UI.
        var partName = default(string);
        if (action.bindings[bindingIndex].isPartOfComposite)
            partName = $"Binding '{action.bindings[bindingIndex].name}'. ";

        // Bring up rebind overlay, if we have one.
        _rebindOverlay?.SetActive(true);
        if (_rebindText != null) {
            var text = !string.IsNullOrEmpty(_rebindOperation.expectedControlType)
                ? $"{partName}Waiting for {_rebindOperation.expectedControlType} input..."
                : $"{partName}Waiting for input...";
            _rebindText.text = text;
        }

        // If we have no rebind overlay and no callback but we have a binding text label,
        // temporarily set the binding text label to "<Waiting>".
        if (_rebindOverlay == null && _rebindText == null && _startRebindEvent == null && _bindingTxt != null)
            _bindingTxt.text = "<Waiting...>";

        // Give listeners a chance to act on the rebind starting.
        _startRebindEvent?.Invoke(this, _rebindOperation);

        _rebindOperation.Start();
    }

    /// <summary>
    /// 检测重复的输入绑定
    /// </summary>
    /// <param name="action"></param>
    /// <param name="bindingIndex"></param>
    /// <param name="allCompositeParts"></param>
    /// <returns></returns>
    private bool CheckDuplicateBinding(InputAction action, int bindingIndex, bool allCompositeParts = false)
    {
        var newBinding = action.bindings[bindingIndex];
        foreach (var oldBinding in action.actionMap.bindings) {
            if (newBinding.action == oldBinding.action) {
                continue;
            }
            if (newBinding.effectivePath == oldBinding.effectivePath) {
                Debug.LogWarning($"重复绑定{newBinding.effectivePath}");
                return true;
            }
        }

        if (allCompositeParts) {
            for (var i = 1; i < bindingIndex; i++) {
                if (action.bindings[i].effectivePath == newBinding.overridePath) {
                    Debug.LogWarning($"重复绑定{newBinding.effectivePath}");
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 添加Action改变监听
    /// </summary>
    protected void OnEnable()
    {
        if (_rebindActionUIs == null)
            _rebindActionUIs = new List<RebindActionUI>();
        _rebindActionUIs.Add(this);
        if (_rebindActionUIs.Count == 1)
            UnityEngine.InputSystem.InputSystem.onActionChange += OnActionChange;
    }

    /// <summary>
    /// 移除Action改变监听
    /// </summary>
    protected void OnDisable()
    {
        _rebindOperation?.Dispose();
        _rebindOperation = null;

        _rebindActionUIs.Remove(this);
        if (_rebindActionUIs.Count == 0) {
            _rebindActionUIs = null;
            UnityEngine.InputSystem.InputSystem.onActionChange -= OnActionChange;
        }
    }
    
    /// <summary>
    /// 当输入绑定发生改变时，需要刷新UI
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="change"></param>
    private static void OnActionChange(object obj, InputActionChange change)
    {
        if (change != InputActionChange.BoundControlsChanged)
            return;

        var action = obj as InputAction;
        var actionMap = action?.actionMap ?? obj as InputActionMap;
        var actionAsset = actionMap?.asset ?? obj as InputActionAsset;

        for (var i = 0; i < _rebindActionUIs.Count; ++i) {
            var component = _rebindActionUIs[i];
            var referencedAction = component.actionReference?.action;
            if (referencedAction == null)
                continue;

            if (referencedAction == action ||
                referencedAction.actionMap == actionMap ||
                referencedAction.actionMap?.asset == actionAsset)
                component.UpdateBindingDisplay();
        }
    }

    /// <summary>
    /// 更新Action文本
    /// </summary>
    public void UpdateActionLabel()
    {
        if (_actionLabel != null) {
            var action = _actionReference?.action;
            _actionLabel.text = action != null ? action.name : string.Empty;
        }
    }
}