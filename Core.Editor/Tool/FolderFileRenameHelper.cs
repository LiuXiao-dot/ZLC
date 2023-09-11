using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using ZLC.FileSystem;
namespace ZLCEditor.Tool
{
    /// <summary>
    /// 文件重命名工具
    /// </summary>
    [Serializable]
    [Tooltip("把文件夹下的所有文件的文件名将所有from修改为to")]
    public class FolderFileRenameHelper
    {
        [FolderPath]
        [LabelText("文件夹名称")]
        public string uri;
        public string from;
        public string to;
        public void Excute()
        {
            FileHelper.GetAllFiles(uri).ForEach(temp =>
            {
                AssetDatabase.RenameAsset(temp, Path.GetFileName(temp).Replace(from,to));
            });
            AssetDatabase.Refresh();
        }
    }
}