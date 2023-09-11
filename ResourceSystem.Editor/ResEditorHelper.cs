using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using ZLC.FileSystem;
using ZLC.Utils;
namespace ZLCEditor.ResourceSystem
{
    /// <summary>
    /// 资源管理帮助类
    /// </summary>
    public sealed class ResEditorHelper
    {
        /// <summary>
        /// 支持的文件类型
        /// </summary>
        private static HashSet<string> extensions = new HashSet<string>()
        {
            ".prefab",
            ".mat",
            ".png",
            ".unity",
            ".asset",
            ".physicsMaterial",
            ".ttf",
            ".fbx",
            ".shader",
            ".wav"
        };
        
        /// <summary>
        /// 资源打包
        /// </summary>
        public static string Build(IList<string> dirs)
        {
            try {
                // 同步AddressablesSync(dirs);
                Sync(dirs);
                // Addressables打包
                AddressableAssetSettings.BuildPlayerContent();
                // todo:其他
                // Build
                BuildPlayerOptions options = new BuildPlayerOptions();
                options.options = BuildOptions.ShowBuiltPlayer;
                BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(options));
                return "Build成功";
            }
            catch (Exception e) {
                return e.StackTrace;
            }
        }

        /// <summary>
        /// 资源同步
        /// 1.检测目录是否存在，不存在添加错误信息
        /// todo:使用多个GroupTemplateObject
        /// </summary>
        public static string Sync(IList<string> dirs)
        {
            if (!(dirs is { Count: > 0 })) {
                return "未设置目录";
            }
            var setting = AddressableAssetSettingsDefaultObject.GetSettings(true);
            var template = setting.GetGroupTemplateObject(0) as AddressableAssetGroupTemplate;

            string info;
            using (zstring.Block())
            {
                zstring log = "";
                // 创建一个group
                void createGroup(string dir,string groupName = null)
                {
                    groupName ??= FileHelper.GetDirectoryName(dir);
                    var group = setting.FindGroup(groupName);
                    if (group == null) {
                        // 创建新的Group
                        group = setting.CreateGroup(groupName,false,true,true,template.SchemaObjects);
                        log = log + "\n创建Group:" + dir;
                    }
                    
                    var fileUrls = GetInvalidFiles(dir);
                    foreach (var fileUrl in fileUrls) {
                        var fileName = Path.GetFileName(fileUrl);
                        var guid = AssetDatabase.AssetPathToGUID(fileUrl);
                        var entry = group.GetAssetEntry(guid);
                        if (entry == null) {
                            entry = setting.CreateOrMoveEntry(guid,group,false,true);
                            log = log + "\n创建Entry:" + fileUrl;
                        }
                        if(entry == null) continue; // 创建失败
                        entry.SetAddress(fileName,true);
                    }
                    var childDirs = Directory.GetDirectories(dir);
                    foreach (var childDir in childDirs) {
                        createGroup(childDir,groupName);
                    }
                }
            
                foreach (var dir in dirs) {
                    if(Directory.Exists(dir))
                        createGroup(dir);
                }
                info = log.Intern();
            }
            return info;
        }

        /// <summary>
        /// 资源整理
        /// </summary>
        public static void Sort()
        {
            
        }

        /// <summary>
        /// 资源检测
        /// </summary>
        public static void Check()
        {
            
        }
        
        /// <summary>
        /// 获取符合条件的文件
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetInvalidFiles(string dir)
        {
            return Directory.EnumerateFiles(dir).Where(IsFileMatch);
        }
        
        /// <summary>
        /// 文件筛选
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsFileMatch(string fileName)
        {
            var end = Path.GetExtension(fileName);
            return extensions.Contains(end);
        }
    }
}