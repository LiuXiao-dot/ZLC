namespace ZLC.FileSystem;

/// <summary>
/// 文件路径
/// </summary>
public class FilePathAttribute : Attribute
{
    /// <summary>
    /// 用户使用的运行时路径
    /// </summary>
    public const string XWPATH = "Assets/ZLC_Configs/Configs";
    /// <summary>
    /// 用户使用的编辑器路径
    /// </summary>
    public const string XWEDITORPATH = "Assets/ZLC_Configs/EditorConfigs";
    /// <summary>
    /// 框架内置运行时路径
    /// </summary>
    internal const string IN_XWPATH = "Assets/Plugins/ZLCEngine/Configs";
    /// <summary>
    /// 框架内置编辑器路径
    /// </summary>
    internal const string IN_XWEDITORPATH = "Assets/Plugins/ZLCEngine/EditorConfigs";
    /// <summary>
    /// 路径类型
    /// </summary>
    public enum PathType
    {
        /// <summary>
        /// XW的EditorResources目录
        /// </summary>
        XWEditor,
        /// <summary>
        /// XW的Resources目录
        /// </summary>
        XW,
        /// <summary>
        /// 相对与项目的绝对路径
        /// </summary>
        Absolute
    }
    /// <summary>
    /// 文件路径
    /// </summary>
    public string filePath;
    
    internal FilePathAttribute(string filePath, PathType pathType = PathType.Absolute, bool isInternal = true)
    {
        this.filePath = isInternal ? GetInternalPath(filePath, pathType) : GetPath(filePath, pathType);
    }

    /// <inheritdoc />
    public FilePathAttribute(string filePath, PathType pathType = PathType.Absolute) : this(filePath, pathType, false)
    {
    }

    /// <summary>
    /// 获取内部路径
    /// </summary>
    /// <param name="filePath">路径</param>
    /// <param name="pathType">路径类型</param>
    /// <returns></returns>
    internal static string GetInternalPath(string filePath, PathType pathType)
    {
        switch (pathType) {
            case PathType.XWEditor:
                FileHelper.CheckDirectory(IN_XWEDITORPATH);
                return $"{IN_XWEDITORPATH}/{filePath}";
            case PathType.XW:
                FileHelper.CheckDirectory(IN_XWPATH);
                return $"{IN_XWPATH}/{filePath}";
            case PathType.Absolute:
                return filePath;
            default:
                return filePath;
        }
    }

    /// <summary>
    /// 获取完整路径
    /// </summary>
    /// <returns></returns>
    public string GetPath()
    {
        return filePath;
    }

    /// <summary>
    /// 获取完整路径
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="pathType"></param>
    /// <returns></returns>
    public static string GetPath(string filePath, PathType pathType)
    {
        switch (pathType) {
            case PathType.XWEditor:
                FileHelper.CheckDirectory(XWEDITORPATH);
                return $"{XWEDITORPATH}/{filePath}";
            case PathType.XW:
                FileHelper.CheckDirectory(XWPATH);
                return $"{XWPATH}/{filePath}";
            case PathType.Absolute:
                return filePath;
            default:
                return filePath;
        }
    }
}