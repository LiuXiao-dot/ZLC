using UnityEngine;
namespace ZLCEditor.WindowSystem
{
    public struct WindowViewData
    {
        public GameObject prefab;
        public string prefabPath;
        public string prefabName;
        public bool needGenerate;

        public WindowViewData(GameObject prefab, string prefabPath, bool needGenerate)
        {
            this.prefab = prefab;
            this.prefabPath = prefabPath;
            this.prefabName = prefab.name;
            this.needGenerate = needGenerate;
        }

        public WindowViewData(string prefabName, string prefabPath, bool needGenerate)
        {
            this.prefabName = prefabName;
            this.prefab = null;
            this.prefabPath = prefabPath;
            this.needGenerate = needGenerate;
        }

        public string GetName()
        {
            return prefabName;
        }

        public string GetViewName()
        {
            return $"{prefabName}_View";
        }

        public string GetCtlName()
        {
            return $"{prefabName}_Ctl";
        }
        
        public string GetViewPath()
        {
            return Path.Combine(WindowToolSO.Instance.codeDirectory, $"View/{GetViewName()}.cs");
        }
        
        /// <summary>
        /// 自动创建并刷新Ctl的代码
        /// </summary>
        /// <returns></returns>
        public string GetCtlPath()
        {
            return Path.Combine(WindowToolSO.Instance.codeDirectory, $"AutoCtl/{GetCtlName()}.g.cs");
        }

        /// <summary>
        /// 只会创建一次，不会再刷新代码
        /// </summary>
        /// <returns></returns>
        public string GetSelfCtlPath()
        {
            return Path.Combine(WindowToolSO.Instance.codeDirectory, $"SelfCtl/{GetCtlName()}.cs");
        }
    }
}