using Sirenix.OdinInspector;
using ZLC.SerializeSystem;
namespace ZLCEditor.Converter.Data.Code
{
    /// <summary>
    /// 枚举中间数据
    /// </summary>
    public class ILEnum : AILCode
    {
        [LabelText("命名空间")]
        public string @namespace;

        [LabelText("访问修饰符")]
        public Arm arm;

        [LabelText("启用Flag")]
        public bool isFlag;

        [LabelText("数据")]
        public SDictionary<string, int> kvs = new SDictionary<string, int>();


        [Button("格式化数据")]
        public void FormalizeEnumValues()
        {
            if (kvs != null) {
                var keys = kvs.Keys.ToArray();
                var length = keys.Length;
                if (isFlag) {
                    if (length >= 1)
                        kvs[keys[0]] = 0;
                    var value = 1;
                    for (int i = 1; i < length; i++) {
                        kvs[keys[i]] = value;
                        value <<= 1;
                    }
                } else {
                    for (int i = 0; i < length; i++) {
                        kvs[keys[i]] = i;
                    }
                }
            }
        }

        private void Reset()
        {
            kvs = new SDictionary<string, int>();
        }
    }
}