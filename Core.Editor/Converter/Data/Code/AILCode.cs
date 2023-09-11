using Sirenix.OdinInspector;
using UnityEngine;
namespace ZLCEditor.Converter.Data.Code
{
    public abstract class AILCode : ScriptableObject,IILCode
    {

        [LabelText("名称")]
        public string fileName;
        
        [LabelText("自定义路径")]
        public bool customPath;
        
        [FolderPath]
        [LabelText("代码路径")]
        [Tooltip("不填的话将使用默认的路径")]
        [ShowIf("customPath")]
        public string url;
        
        public virtual string GetPath()
        {
            if (!customPath)
                return Path.Combine(ConvertToolSO.Instance.codeGenPath,$"{fileName}.g.cs");
            return Path.Combine(url,$"{fileName}.g.cs");
        }
    }
}