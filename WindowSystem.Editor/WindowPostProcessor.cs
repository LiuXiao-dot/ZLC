using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using ZLC.ConfigSystem;
using ZLC.FileSystem;
using ZLCEditor.Converter.Data.Code;
using FilePathAttribute = ZLC.FileSystem.FilePathAttribute;
namespace ZLCEditor.WindowSystem
{
    [FilePath("WindowPostProcessor.asset", FilePathAttribute.PathType.XWEditor,true)]
    public class WindowPostProcessor : SOSingleton<WindowPostProcessor>
    {
        private bool isCodeDirty
        {
            get {
                return WindowToolSO.Instance.isCodeDirty;
            }
            set {
                WindowToolSO.Instance.isCodeDirty = value;
            }
        }

        public void GenerateAll(WindowToolSO toolSO)
        {
            try {
                var prefabGUIDs = AssetDatabase.FindAssets("t:prefab", new[]
                {
                    toolSO.windowDirectory
                });
                isCodeDirty = false;
                _windowViewDatas = GetWindowViewDatas(prefabGUIDs).ToArray();
                using (var converter = new WindowView_Code_Converter()) {
                    var windowIDSO = AssetDatabase.LoadAssetAtPath<ILEnum>("Assets/XW/EditorResources/Code/WindowID.asset");
                    windowIDSO.kvs.Clear();
                    converter.Convert(_windowViewDatas);
                    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                    Debug.Log("重新编译开始");
                    if (converter.SyncViewReference(_windowViewDatas)) {
                        Debug.Log("生成成功");
                    } else {
                        Debug.Log("请再点击一次生成");
                    }
                }
            }
            catch (Exception e) {
                Debug.LogError(e);
            }
            finally {
                EditorUtility.ClearProgressBar();
            }
        }

        private WindowViewData[] _windowViewDatas;
        private IEnumerable<WindowViewData> GetWindowViewDatas(string[] guids)
        {
            foreach (var guid in guids) {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var time = File.GetLastWriteTime(path);
                WindowToolSO.Instance.Set(path, time.Ticks, out bool isModified);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                var viewData = new WindowViewData(prefab, path, isModified);
                if (isModified) {
                    isCodeDirty = true;
                }
                yield return viewData;
            }
        }

        public void Clear(WindowToolSO toolSO)
        {
            var prefabGUIDs = AssetDatabase.FindAssets("t:prefab", new[]
            {
                toolSO.windowDirectory
            });
            var paths = prefabGUIDs.Convert<string>(temp => AssetDatabase.GUIDToAssetPath(temp.ToString())).ToHashSet();
            isCodeDirty = false;
            //var defaultDelete = false;
            toolSO.modifyTimes.Where(temp => !paths.Contains(temp.Key)).ToList().ForEach(temp =>
            {
                // 清除相关文件
                var viewData = new WindowViewData(Path.GetFileNameWithoutExtension(temp.Key), temp.Key, false);
                toolSO.modifyTimes.Remove(temp.Key);
                FileHelper.DeleteFile(viewData.GetViewPath());
                FileHelper.DeleteFile(viewData.GetCtlPath());
                if (EditorUtility.DisplayDialog("UGUI_Window", $"是否需要删除{viewData.GetSelfCtlPath()}文件", "删除", "取消")) {
                    FileHelper.DeleteFile(viewData.GetSelfCtlPath());
                }
                EditorUtility.DisplayProgressBar("UGUI_Window", $"删除中:{temp.Key}", 0);
            });
            EditorUtility.ClearProgressBar();
        }
    }
}