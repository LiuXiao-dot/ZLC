using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using ZLCEditor.Attributes;
using ZLCEditor.Data;
namespace ZLCEditor.Tool.Config
{
    /// <summary>
    /// ScriptableObject创建工具
    /// </summary>
    [Serializable]
    [Generator()]
    public class ScriptableObjectCreateTool
    {
        /// <summary>
        /// 所有ScriptableObject类型（不包含抽象类）
        /// </summary>
        [Searchable] [SerializeField] [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        private List<TempButton> allScriptableObjectTypes = new List<TempButton>();

        /// <summary>
        /// 全部的type
        /// </summary>
        private List<Type> types;

        public ScriptableObjectCreateTool()
        {
            types = new List<Type>();
            EditorHelper.GetAllChildType<ScriptableObject>(types,EditorHelper.AssemblyFilterType.Custom | EditorHelper.AssemblyFilterType.Internal);
            var length = types.Count;
            for (int i = 0; i < length; i++)
            {
                allScriptableObjectTypes.Add(new TempButton(){index = i,name = types[i].Name});
            }
        }

        public void OnClickTempButton(TempButton temp)
        {
            var type = types[temp.index];
            var path = EditorUtility.SaveFilePanelInProject("创建ScriptableObject",type.Name,"asset",$"创建{type.Name}的实例");
            if(string.IsNullOrEmpty(path)) return;
            var ins = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(ins,path);
        }

        public static void CreateScriptableObject<T>(string defaultPath, string name, Action<T, string> beforeSave = null) where T : ScriptableObject
        {
            var path = EditorUtility.SaveFilePanelInProject("创建ScriptableObject",name,"asset","创建SO实例",defaultPath);
            //UnityEngine.Debug.Log($"0创建{path}");
            if (string.IsNullOrEmpty(path)) return;
            var ins = ScriptableObject.CreateInstance<T>();
            beforeSave?.Invoke(ins,path);
            AssetDatabase.CreateAsset(ins,path);
            //UnityEngine.Debug.Log($"1创建{path}");
            AssetDatabase.Refresh();
        }
    }
}