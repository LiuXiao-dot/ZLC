using UnityEngine;
using UnityEngine.InputSystem;
using ZLC.Application;
using ZLC.Common;
using ZLC.IOCSystem;
using ZLC.ResSystem;
namespace ZLC.InputSystem;

/// <summary>
/// 输入管理器
/// </summary>
[Component(AppConstant.APP_LAUNCHER_MANAGER)]
public class InputManager : IInputManager
{
    /// <summary>
    /// 输入行为资产
    /// </summary>
    public InputActionAsset actions;

    /// <inheritdoc />
    ~InputManager()
    {
        Dispose();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        // 保存当前输入设置
        var rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(AssemblyConstant.INPUT_KEY, rebinds);
    }

    /// <inheritdoc />
    public void Init()
    {
        var resLoader = IAppLauncher.Get<IResLoader>();
        if (resLoader == null) {
            Debug.LogError("缺少IResLoader，请先创建IResLoader再初始化InputManager");
            return;
        }
        if (resLoader.LoadAssetSync(AssemblyConstant.INPUT_ACTION_ASSET_NAME, out actions)) {
            // 加载输入设置
            var rebinds = PlayerPrefs.GetString(AssemblyConstant.INPUT_KEY);
            if (!string.IsNullOrEmpty(rebinds))
                actions.LoadBindingOverridesFromJson(rebinds);
        } else {
            Debug.LogError("加载InputActionAsset失败");
        }
    }
    
    
}