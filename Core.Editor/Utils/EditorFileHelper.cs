using UnityEditor;
namespace ZLCEditor.Utils
{
    /// <summary>
    /// 文件相关操作
    /// </summary>
    public sealed class EditorFileHelper
    {
        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="uri"></param>
        public static void DeleteFile(string uri)
        {
            if (File.Exists(uri)) {
                AssetDatabase.DeleteAsset(uri);
            }
        }
    }
}