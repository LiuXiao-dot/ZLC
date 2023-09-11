using UnityEngine;
namespace ZLC.FileSystem;

/// <summary>
/// 文件相关操作
/// </summary>
public sealed class FileHelper
{
    /// <summary>
    /// 检测文件夹是否存在，如果不存在，则会创建
    /// </summary>
    /// <param name="url">文件夹路径</param>
    public static void CheckDirectory(string url)
    {
        if (Directory.Exists(url)) {
            return;
        }

        Directory.CreateDirectory(url);
#if ZLC_DEBUG
        Debug.Log($"创建文件夹:{url}");
#endif
    }

    /// <summary>
    /// 获取文件夹的名称
    /// </summary>
    /// <param name="url">文件夹路径</param>
    /// <returns>文件夹名称</returns>
    public static string GetDirectoryName(string url)
    {
        var splits = url.Split('/');
        return splits[^1];
    }


    /// <summary>
    /// 删除文件夹下的的所有文件
    /// </summary>
    /// <param name="url">文件夹路径</param>
    public static void ClearDirectory(string url)
    {
        if (!Directory.Exists(url)) {
            #if ZLC_DEBUG
            Debug.LogError($"待删除路径{url}不存在");
            #endif
            return;
        }
        var files = Directory.EnumerateFiles(url);
        foreach (var file in files) {
            File.Delete(file);
        }
    }

    /// <summary>
    /// 获取url目录下的所有子文件（包含子文件夹中的文件）
    /// </summary>
    /// <returns>每个文件的路径</returns>
    public static IEnumerable<string> GetAllFiles(string url)
    {
        var files = Directory.GetFiles(url);
        foreach (var file in files) {
            yield return file;
        }
        var directories = Directory.GetDirectories(url);
        foreach (var dir in directories) {
            var temp = GetAllFiles(dir);
            foreach (var child in temp) {
                yield return child;
            }
        }
    }

    /// <summary>
    /// 文件是否在directory目录下
    /// </summary>
    /// <returns></returns>
    public static bool IsFileInDirectory(string directory, string file)
    {
        return file.Contains(directory);
    }

    /// <summary>
    /// 删除指定文件
    /// </summary>
    /// <param name="url">文件路径</param>
    public static void DeleteFile(string url)
    {
        if (File.Exists(url)) {
            File.Delete(url);
        }
    }
}