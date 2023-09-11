namespace ZLC.ExceptionSystem;

/// <summary>
/// 通用异常信息
/// </summary>
public class ZLCException : ApplicationException
{
    private int _errorCode;

    /// <inheritdoc />
    public ZLCException(string message) : base(message)
    {
    }

    /// <inheritdoc />
    public ZLCException(ErrorCode errorCode)
    {
        this._errorCode = (int)errorCode;
    }

    /// <inheritdoc />
    public ZLCException(int errorCode)
    {
        this._errorCode = errorCode;
    }
}