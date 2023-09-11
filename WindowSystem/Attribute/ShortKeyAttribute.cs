using System;
namespace ZLC.WindowSystem.Attribute
{
    /// <summary>
    /// key,需要全大写，根据key值自动生成UI代码
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ShortKeyAttribute : System.Attribute
    {
        /// <summary>
        /// KEY
        /// </summary>
        public string key;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">key</param>
        public ShortKeyAttribute(string key = null)
        {
            this.key = key;
        }
    }
}