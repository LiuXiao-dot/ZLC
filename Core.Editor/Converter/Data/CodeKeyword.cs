namespace ZLCEditor.Converter.Data
{
    [Flags]
    public enum CodeKeyword
    {
        none = 0,
        @static = 1,
        @partial = 2,
        @abstract = 4,
        @sealed = 8,
        @virtual = 16,
        @override = 32,
        @readonly = 64,
        @const = 128,
    }
}