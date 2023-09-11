namespace ZLC.WindowSystem
{
    /// <summary>
    /// 窗口控制器
    /// </summary>
    public interface IWindowCtl
    {
        void SetValue(object args);
        void SetView(AWindowView view);
        void Open();
        void Pause();
        void Resume();
        void Close();
        void ReOpen();
        AWindowView GetView();
        int GetWindowID();
    }
}