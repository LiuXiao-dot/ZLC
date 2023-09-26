namespace ZLC.WindowSystem
{
    /// <summary>
    /// 窗口层级设定
    /// </summary>
    [Serializable]
    public enum WindowLayer
    {
        /// <summary>
        /// 一级：主窗口
        /// </summary>
        MAIN = 0,
        /// <summary>
        /// 二级：全屏窗口
        /// </summary>
        CHILD = 1,
        /// <summary>
        /// 三级：非全屏面板 ->面板的子集为面板和弹窗
        /// </summary>
        PANEL = 2,
        /// <summary>
        /// 弹出小面板
        /// </summary>
        POPUP = 3,
        /// <summary>
        /// 加载界面层
        /// </summary>
        LOADING = 4,
        /// <summary>
        /// 消息
        /// </summary>
        MESSAGE = 5,
    }
}