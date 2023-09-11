namespace ZLC.WindowSystem
{
    /// <summary>
    /// 窗口配置接口
    /// </summary>
    public interface IWindowConfig
    {
        /// <summary>
        /// 根据窗口ID创建窗口Ctl
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IWindowCtl CreateWindowCtl(int id);
    }
}