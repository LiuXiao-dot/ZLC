using UnityEngine;
using ZLC.Application;
using ZLC.FileSystem;
namespace ZLC.PofileSystem;

/// <summary>
/// 文件日志
/// </summary>
public class FileLogHandler : ILogHandler
{
    private readonly Dictionary<int, StreamWriter> _writers = new Dictionary<int, StreamWriter>();
    private string url;

    /// <summary>
    /// 构造函数
    /// </summary>
    public FileLogHandler()
    {
        url = $"{AppConstant.BasePath}/log";
        FileHelper.CheckDirectory(url);
        Debug.Log($"FileLog:{url}");
    }

    /// <inheritdoc />
    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;

        // Get or create the writer for the current thread
        var writer = GetOrCreateWriter(threadId);

        // Write the log message to the file
        writer.WriteLine($"\n{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{logType}] {format}", args);
        writer.Flush();
    }

    /// <inheritdoc />
    public void LogException(Exception exception, UnityEngine.Object context)
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;

        // Get or create the writer for the current thread
        var writer = GetOrCreateWriter(threadId);

        // Write the exception message to the file
        writer.WriteLine($"\n{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [Exception] {exception.Message}\n{exception.StackTrace}");
        writer.Flush();
    }

    private StreamWriter GetOrCreateWriter(int threadId)
    {
        // Check if the writer for the current thread already exists
        if (_writers.TryGetValue(threadId, out var writer)) {
            return writer;
        }

        // Create a new writer for the current thread

        var filePath = $"{url}/log_{threadId}.txt";
        writer = new StreamWriter(filePath, true);
        _writers.Add(threadId, writer);

        return writer;
    }

    /// <summary>
    /// 关闭所有日志文件流
    /// </summary>
    public void CloseAllWriters()
    {
        foreach (var writer in _writers.Values) {
            writer?.Dispose();
        }

        _writers.Clear();
    }

    /// <summary>
    /// 获取文件路径
    /// </summary>
    /// <returns></returns>
    public string GetUrl()
    {
        return url;
    }

    /// <summary>
    /// 析构函数
    /// </summary>
    ~FileLogHandler()
    {
        CloseAllWriters();
    }
}