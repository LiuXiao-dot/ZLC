namespace ZLCEditor.Converter
{
    /// <summary>
    /// 转换帮助器
    /// 1.Editor中的Converter类如果需要在Runtime环境下使用，可以移动到Runtime目录下
    /// </summary>
    public sealed class ILHelper
    {
        private static ILFactory _instance;

        /// <summary>
        /// 转换帮助器单例
        /// </summary>
        public static ILFactory Instance
        {
            get {
                return _instance ??= new ILFactory();
            }
            set {
                _instance = value;
            }
        }

        public static T Convert<F,T>(F from)
        {
            var converter = Instance.GetConverter<F, T>();
            if (converter == null)
                return default(T);
            return converter.Convert(from);
        }
        
        public static object Convert(object from, Type to)
        {
            var converter = (IConverter)Instance.GetConverter(from.GetType(),to);
            if (converter == null)
                return default;
            return converter.Convert(from);
        }
    }
}
