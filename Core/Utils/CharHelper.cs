namespace ZLC.Utils
{
    /// <summary>
    /// 字符的帮助类
    /// </summary>
    public sealed class CharHelper
    {
        /// <summary>
        /// 变为大写
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static char ToUpper(char c)
        {
            if (c is >= 'a' and <= 'z')
                c &= '\uFFDF';
            return c;                                                                                   
        }
        
        /// <summary>
        /// 变为小写
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static char ToLowwer(char c)        {
            if (c is >= 'A' and <= 'Z')
                c |= '\u0020';
            return c;
        }
    }
}