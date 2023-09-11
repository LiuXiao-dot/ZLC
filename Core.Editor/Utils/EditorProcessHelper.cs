using System.Diagnostics;
using Debug = UnityEngine.Debug;
namespace ZLCEditor.Utils
{
    /// <summary>
    /// 调用Windows的cmd
    /// </summary>
    public sealed class EditorProcessHelper
    {
        /// <summary>
        /// 运行
        /// </summary>
        /// <returns></returns>
        public static void RunCommand(string command, string arguments, string workingDirectory = null)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = command; //确定程序名
            startInfo.Arguments = arguments; //指定程式命令行
            startInfo.UseShellExecute = false; //是否使用Shell
            startInfo.RedirectStandardInput = true; //重定向输入
            startInfo.RedirectStandardOutput = true; //重定向输出
            startInfo.RedirectStandardError = true; //重定向输出错误
            startInfo.CreateNoWindow = true; //设置不显示窗口
            startInfo.ErrorDialog = true; // 不能启动时显示错误对话框
            if (workingDirectory != null) startInfo.WorkingDirectory = workingDirectory;
            startInfo.StandardOutputEncoding = System.Text.UTF8Encoding.UTF8;
            startInfo.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;
            process.Start();
            string res = "";
            if (!process.StandardOutput.EndOfStream)
                res = process.StandardOutput.ReadToEnd();
            if (process.StandardError.EndOfStream)
                res += $"[error:{process.StandardError.ReadToEnd()}]";
            Debug.Log(res);
            process.WaitForExit();
            process.Close();
        }
    }
}
