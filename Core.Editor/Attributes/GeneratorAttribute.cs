namespace ZLCEditor.Attributes
{
    /// <summary>
    /// 生成器.
    /// 用于标记生成器类，会被调用Generate方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class GeneratorAttribute : Attribute
    {
        /// <summary>
        /// 0表示每次调用都执行，其他的level自定义
        /// </summary>
        public int genLevel;
        public GeneratorAttribute(int genLevel = 0)
        {
            this.genLevel = genLevel;
        }
    }
}