using Sirenix.OdinInspector;
using UnityEngine;
using ZLC.ConfigSystem;
using ZLC.EditorSystem;
using ZLC.SerializeSystem;
using FilePathAttribute = ZLC.FileSystem.FilePathAttribute;
namespace ZLC.WindowSystem
{
    /// <summary>
    /// 窗口配置
    /// 自动生成代码
    /// </summary>
    [Tool("UGUI/运行时配置")]
    [FilePath("WindowConfigsSO.asset",FilePathAttribute.PathType.XW,true)]
    public class WindowConfigsSO : SOSingleton<WindowConfigsSO>
    {
        /// <summary>
        /// 窗口层级对应的sorting order                    
        /// </summary>
        [Header("窗口层级(该层级的最小Order in Layer值)")]
        [ShowInInspector]
        [ReadOnly]
        public readonly SDictionary<WindowLayer, int> layers = new SDictionary<WindowLayer, int>()
        {
            { WindowLayer.MAIN, 0 },
            { WindowLayer.CHILD, 10 },
            { WindowLayer.PANEL, 30 },
            { WindowLayer.POPUP, 50 },
            { WindowLayer.LOADING, 70 },
            { WindowLayer.MESSAGE, 90 },
        };
        /// <summary>
        /// 默认加载窗口id
        /// </summary>
        public int defaultLoadingWindowID = 0;
        /// <summary>
        /// 主界面窗口id
        /// </summary>
        public int mainWindowID = 1;
    }
}