using UnityEngine;
namespace ZLCEditor.Converter.Data.Code
{
    /// <summary>
    /// 代码的字符串
    /// </summary>
    public class ILCodeString : AILCode
    {
        [TextArea(minLines: 5, maxLines: 20)]
        public string content;
    }
}