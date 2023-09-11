#if LUBAN_ENABLE
using System;
using System.Diagnostics;
using System.IO;
using Editor;
using Sirenix.OdinInspector;
using UnityEditor;
using XWEngine;
namespace XWEditor.Tool
{
    /// <summary>
    /// Luban使用工具
    /// </summary>
    [Tool("配置/配置表生成工具")]
    [XWFilePath("LubanTool.asset",XWFilePathAttribute.PathType.XWEditor)]
    public class LubanTool : SOSingleton<LubanTool>
    {
        /// <summary>
        /// .bat文件路径
        /// </summary>
        [ReadOnly]public string genPath;
        /// <summary>
        /// 调用外部bat生成配置表
        /// </summary>l
        [Button(ButtonSizes.Large, Name = "生成文档")]
        public void Generate()
        {
            if(string.IsNullOrEmpty(genPath) || !File.Exists(genPath))
                genPath = $"{GameConstant.BasePath}/Packages/XW/.Tools/Luban/gen_code_bin.bat";
            
            var process = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            if (File.Exists(genPath))
            {
                info.FileName = genPath;
            } else {
                UnityEngine.Debug.LogError("缺少生成工具或需要配置环境变量");
                return;
            }
            info.CreateNoWindow = false;
            info.WindowStyle = ProcessWindowStyle.Normal;
            process.StartInfo = info;
            EditorUtility.DisplayCancelableProgressBar("生成文档",$"文档生成中{genPath}",0);
            try
            {
                process.Start();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
                throw;
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
      
        }
    }
}
#endif