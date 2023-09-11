namespace ZLC.WindowSystem
{
    /// <summary>
    /// 窗口关闭前触发
    /// </summary>
    public interface IBeforeWindowClose
    {
        /// <summary>
        /// true:不退出
        /// false:退出
        /// </summary>
        /// <returns></returns>
        bool BeforeWindowClose();
    }
}