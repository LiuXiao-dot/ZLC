using Sirenix.Serialization;
namespace ZLCEditor.Attributes
{

    /// <summary>
    /// UGUI的GameObject的Preview缩略图
    /// </summary>
    public class UGUIPreviewAttribute : OdinSerializeAttribute
    {
        public int width;
        public int height;

        public UGUIPreviewAttribute(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}