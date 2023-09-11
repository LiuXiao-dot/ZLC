using System.Runtime.Serialization.Json;
using Sirenix.Utilities;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using FilePathAttribute = ZLC.FileSystem.FilePathAttribute;
namespace ZLCEditor
{
    public sealed class EditorHelper
    {
        /// <summary>
        /// 检测->创建目标路径的目录
        /// </summary>
        /// <param name="filePath"></param>
        public static void CreateDir(string filePath)
        {
            var dir = GetDir(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        /// <summary>
        /// 获取文件的文件夹路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetDir(string filePath)
        {
            return filePath.Replace(Path.GetFileName(filePath), "");
        }

        /// <summary>
        /// 检测T是否使用FilePathAttribute标记，
        /// 如果标记了，将检测是否存在T的实例，
        /// 如果存在检测路径是否正确，
        /// 否则会在目标路径创建一个T的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void CheckAsset<T>() where T : ScriptableObject
        {
            var type = typeof(T);
            // 生成一个实例
            var xWFilePathAttribute = type.GetCustomAttribute<FilePathAttribute>();
            if (xWFilePathAttribute == null) return;

            var paths = AssetDatabase.FindAssets($"t:{type.Name}");
            var sos = paths.Select(temp =>
                AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(temp)));
            IEnumerable<ScriptableObject> scriptableObjects = sos as ScriptableObject[] ?? sos.ToArray();
            if (scriptableObjects.Any())
            {
                if (scriptableObjects.Count() > 1)
                {
                    Debug.LogError($"存在多个{typeof(T).GetNiceName()}");
                    return;
                }

                var oldPath = AssetDatabase.GetAssetPath(scriptableObjects.First());
                if (oldPath != xWFilePathAttribute.filePath)
                    AssetDatabase.MoveAsset(oldPath, xWFilePathAttribute.filePath);
            }
            else
            {
                var newSO = ScriptableObject.CreateInstance(type);
                /*AssetDatabase.CreateAsset(newSO,Path.GetFileName(xWFilePathAttribute.filePath));
                AssetDatabase.MoveAsset(Path.GetFileName(xWFilePathAttribute.filePath), xWFilePathAttribute.filePath);*/
                AssetDatabase.CreateAsset(newSO, xWFilePathAttribute.filePath);
            }
        }

        /// <summary>
        /// 检测并创建文件夹，并在目标路径生成T的实例   
        /// </summary>
        /// <param name="t"></param>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        public static void CreateAsset<T>(T t, string path) where T : UnityEngine.Object
        {
            CreateDir(path);
            AssetDatabase.CreateAsset(t, path);
        }

        /// <summary>
        /// 查找所有资产
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> FindAssets<T>() where T : UnityEngine.Object
        {
            return FindAssets<T>(null);
        }
        
        /// <summary>
        /// 查找searchFolders下的所有资产
        /// </summary>
        /// <param name="searchFolders"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> FindAssets<T>(string[] searchFolders) where T : UnityEngine.Object
        {
            return FindAssets<T>(typeof(T).Name,searchFolders);
        }

        /// <summary>
        /// 查找所有资产
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> FindAssets<T>(string typeName, string[] serachFolders) where T : UnityEngine.Object
        {
            var ts = AssetDatabase.FindAssets($"t:{typeName}",serachFolders);
            return ts.Select(temp => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(temp))).ToList();
        }

        /// <summary>
        /// 使用SearchService查找资产
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static void SearchPackagesAssets<T>(string searchText, Action<IList<T>> callback) where T : UnityEngine.Object
        {
            Action<SearchContext, IList<SearchItem>> OnSearchCompleted = (SearchContext context, IList<SearchItem> items) =>
            {
                var assets = new List<T>();
                foreach (var item in items) {
                    var path = SearchUtils.GetAssetPath(item);
                    var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                    assets.Add(asset);
                }
                callback(assets);
            };
            // 刷新ZLC提供的程序集
            SearchService.Request(searchText,OnSearchCompleted,SearchFlags.Default | SearchFlags.Packages | SearchFlags.WantsMore);
        }
        
        /*public static void CombineWindows()
        {
            Type sceneViewType = typeof(SceneView);
            //创建最外层容器
            object containerInstance = ContainerWindowWrap.CreateInstance();
            //创建分屏容器
            object splitViewInstance = SplitViewWrap.CreateInstance();
            //设置根容器
            ContainerWindowWrap.SetRootView(containerInstance, splitViewInstance);
    
            //tool面板与timeline面板分割面板
            object window_sceneSplitView = SplitViewWrap.CreateInstance();
            SplitViewWrap.SetPosition(window_sceneSplitView, new Rect(0, 0, 1920, 1080));
            //设置垂直状态
            SplitViewWrap.SetVertical(window_sceneSplitView, false);
            object sceneDockArea = DockAreaWrap.CreateInstance();
            var sceneWidth = 1080 * 1080 / 1920;
            DockAreaWrap.SetPosition(sceneDockArea, new Rect(0, 0, sceneWidth, 1080));
            var sceneWindow = ScriptableObject.CreateInstance(sceneViewType) as SceneView;
            sceneWindow.orthographic = true;
            sceneWindow.in2DMode = true;
            
            DockAreaWrap.AddTab(sceneDockArea, sceneWindow);
            SplitViewWrap.AddChild(window_sceneSplitView, sceneDockArea);
    
            //添加timeline窗体
            object windowDock = DockAreaWrap.CreateInstance();
            DockAreaWrap.SetPosition(windowDock, new Rect(sceneWidth, 0, 1920 - sceneWidth, 1080));
            EditorWindow windowEditorWindow = (EditorWindow)ScriptableObject.CreateInstance(typeof(WindowEditorWindow));
            //windowEditorWindow.minSize = new Vector2(1920 - sceneWidth, 1080);
            DockAreaWrap.AddTab(windowDock, windowEditorWindow);
            SplitViewWrap.AddChild(window_sceneSplitView, windowDock);
    
            //添加tool_timeline切割窗体
            SplitViewWrap.AddChild(splitViewInstance, window_sceneSplitView);
    
            EditorWindowWrap.MakeParentsSettingsMatchMe(sceneWindow);
            EditorWindowWrap.MakeParentsSettingsMatchMe(windowEditorWindow);
    
            ContainerWindowWrap.SetPosition(containerInstance, new Rect(0, 0, 1920, 1080));
            SplitViewWrap.SetPosition(splitViewInstance, new Rect(0, 0, 1920, 1080));
            ContainerWindowWrap.Show(containerInstance, 0, true, false, true);
            ContainerWindowWrap.OnResize(containerInstance);
        }*/
    }
}