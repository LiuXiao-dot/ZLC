namespace ZLC.DesignPatternSystem;

/// <summary>
/// 控制器
/// </summary>
public interface ICtl
{
    /// <summary>
    /// 视图
    /// </summary>
    IView View { get; set; }
    /// <summary>
    /// 数据
    /// </summary>
    IModel Model { get; set; }
}
/// <summary>
/// 视图
/// </summary>
public interface IView
{
    /// <summary>
    /// 视图的数据
    /// </summary>
    IViewModel Model { get; set; }
}
/// <summary>
/// 视图数据
/// </summary>
/// <remarks>用于刷新视图的数据</remarks>
public interface IViewModel
{
}
/// <summary>
/// 数据
/// </summary>
public interface IModel
{
}