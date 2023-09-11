using System.Net.Mime;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using ZLC.ConfigSystem;
using ZLC.EditorSystem;
using ZLC.FileSystem;
using ZLC.SerializeSystem;
using ZLC.WindowSystem.Attribute;
using ZLCEditor.Tool;
using FilePathAttribute = ZLC.FileSystem.FilePathAttribute;
namespace ZLCEditor.WindowSystem
{
    /// <summary>
    /// 窗口工具
    /// </summary>
    [Tool("UGUI")]
    [FilePath("WindowToolSO.asset", FilePathAttribute.PathType.XWEditor,true)]
    public sealed class WindowToolSO : SOSingleton<WindowToolSO>
    {
        [LabelText("窗口目录")]
        [FolderPath]
        [Unity.Collections.ReadOnly] public string windowDirectory = "Assets/Arts/Windows";
        [LabelText("代码目录")]
        [FolderPath]
        [Unity.Collections.ReadOnly] public string codeDirectory = "Assets/AutoScript/Window";

        [LabelText("名称->组件")]
        public SDictionary<string, Type> KV = new SDictionary<string, Type>();
        [LabelText("名称->组件（忽略子组件）")]
        public SDictionary<string, Type> KV2 = new SDictionary<string, Type>();

        [FoldoutGroup("窗口->修改时间")]
        public SDictionary<string, long> modifyTimes = new SDictionary<string, long>();
        
        [LabelText("是否需要编译代码")]
        [ReadOnly] public bool isCodeDirty;

        protected override void OnEnable()
        {
            base.OnEnable();
            FileHelper.CheckDirectory(windowDirectory);
            FileHelper.CheckDirectory(codeDirectory);
            //Reset();
        }

        private void Reset()
        {
            KV = new SDictionary<string, Type>
            {
                {
                    "TF", typeof(Transform)
                },
                {
                    "BTN", typeof(Button)
                },
                {
                    "TOG", typeof(Toggle)
                },
                {
                    "IMG", typeof(MediaTypeNames.Image)
                },
                {
                    "IN", typeof(TMPro.TMP_InputField)
                },
                {
                    "TMPUI", typeof(TMPro.TextMeshProUGUI)
                },
            };
            // 查找所有ShortKey标记的类
            var temp = new List<Type>();
            ToolHelper.GetAllMarkedType<ShortKeyAttribute>(temp,ToolHelper.AssemblyFilterType.Custom | ToolHelper.AssemblyFilterType.Internal);
            foreach (var type in temp) {
                var attr = type.GetCustomAttribute<ShortKeyAttribute>();
                var shortkey = attr.key;
                shortkey = (string.IsNullOrEmpty(shortkey) ? type.Name: shortkey).ToUpper();
                KV.Add( shortkey, type);
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="time"></param>
        /// <param name="isModified"></param>
        /// <returns>true:新添加 false:已添加过</returns>
        public bool Set(string path, long time,out bool isModified)
        {
            if (modifyTimes == null) modifyTimes = new SDictionary<string, long>();
            if (modifyTimes.TryGetValue(path, out var oldTime)) {
                isModified = time != oldTime;
                modifyTimes[path] = time;
                return false;
            }
            isModified = true;
            modifyTimes.Add(path, time);
            return true;
        }

        [Button("刷新代码与组件")]
        private void GenerateAll()
        {
            WindowPostProcessor.Instance.GenerateAll(this);
        }

        [Button("清除无用界面(没有prefab了的)")]
        private void Clear()
        {
            WindowPostProcessor.Instance.Clear(this);
        }
    }
}
