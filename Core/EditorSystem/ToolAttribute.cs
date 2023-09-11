namespace ZLC.EditorSystem;

/// <summary>
/// 创建一个工具所需要的数据
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ToolAttribute : Attribute
{
    /// <summary>
    /// 路径
    /// </summary>
    public readonly string path;
    /// <summary>
    /// true:会调用InitTool方法，传入OdinMenuTree进行初始化
    /// </summary>
    public readonly bool needInit;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="needInit">true:会调用InitTool方法，传入OdinMenuTree进行初始化</param>
    public ToolAttribute(string path, bool needInit = false)
    {
        this.path = path;
        this.needInit = needInit;
    }
}