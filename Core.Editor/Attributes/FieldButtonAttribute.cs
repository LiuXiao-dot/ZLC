using Sirenix.OdinInspector;
namespace ZLCEditor.Attributes
{
    /// <summary>
    /// 标记字段，并调用设定的方法,暂时只支持无参数的方法调用
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Method,AllowMultiple = false, Inherited = false)]
    public class FieldButtonAttribute : ShowInInspectorAttribute
    {
        public string name;
        public int buttonHeight;
        public string methodName;
        public ButtonStyle buttonStyle;
        public bool dirtyOnClick = false;

        /// <summary>
        /// 只有标记方法时可以使用无参构造
        /// </summary>
        public FieldButtonAttribute(string methodName = null, ButtonSizes sizes = ButtonSizes.Medium, string name = null, ButtonStyle buttonStyle = ButtonStyle.Box)
        {
            this.name = name;
            this.buttonHeight = (int)sizes;
            this.methodName = methodName;
            this.buttonStyle = buttonStyle;
        }
    }
}