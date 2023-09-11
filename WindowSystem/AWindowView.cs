using Sirenix.OdinInspector;
using UnityEngine;
using ZLC.Application;
using ZLC.UISystem;
namespace ZLC.WindowSystem
{
    /// <summary>
    /// 窗口的View控制器,实际View根据Prefab自动生成
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class AWindowView : MonoBehaviour
    {
        /// <summary>
        /// 窗口ID
        /// </summary>
        [ReadOnly]public int windowID;
        
        /// <summary>
        /// 窗口层级
        /// </summary>
        public WindowLayer windowLayer;

        /// <summary>
        /// 窗口背景
        /// </summary>
        public WindowBg windowBg;

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public void Close()
        {
            IAppLauncher.Get<IWindowManager>().CloseWindow(windowID);
        }
    }
}