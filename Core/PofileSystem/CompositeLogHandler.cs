using UnityEngine;
using Object = UnityEngine.Object;
namespace ZLC.PofileSystem;
/// <summary>
/// 复合日志
/// </summary>
public class CompositeLogHandler : ILogHandler
{
    private List<ILogHandler> handlers;

    /// <summary>
    /// 复合日志构造函数
    /// </summary>
    public CompositeLogHandler()
    {
        this.handlers = new List<ILogHandler>();
    }
    
    /// <summary>
    /// 添加子日志Handler
    /// </summary>
    /// <param name="handler"></param>
    public void AddHandler(ILogHandler handler)
    {
        handlers.Add(handler);
    }

    /// <inheritdoc />
    public void LogFormat(LogType logType, Object context, string format, params object[] args)
    {
        foreach (var handler in handlers) {
            handler.LogFormat(logType, context, format, args);
        }
    }
    /// <inheritdoc />
    public void LogException(Exception exception, Object context)
    {
        foreach (var handler in handlers) {
            handler.LogException(exception,context);
        }
    }
}
