using System.Text;
using ZLC.Application;
using ZLC.FileSystem;
namespace ZLCEditor.Converter
{
    public class CodeFileWriter : IILWriter<string>, IDisposable
    {
        public static CodeFileWriter instance;

        public CodeFileWriter()
        {
            instance = this;
        }
        
        /// <summary>
        /// 自动生成的注释
        /// </summary>
        private const string autoGen = "// <auto-generated/>";

        public void Write(string data, params object[] arguments)
        {
            Write(data, arguments[0].ToString());
        }

        public void Write(string data, string path)
        {
            path = Path.Combine(AppConstant.BasePath, path);
            FileHelper.CheckDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, @$"{autoGen}
{data}", Encoding.UTF8);
        }

        ~CodeFileWriter()
        {
            Dispose();
        }
        public void Dispose()
        {
            instance = null;
        }
    }
}