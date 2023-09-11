using ZLCEditor.Attributes;
namespace ZLCEditor.Data
{
    [FieldButton("OnClickTempButton")]
    [Serializable]
    public class TempButton
    {
        public int index;
        public string name;

        public override string ToString()
        {
            return name;
        }
    }
}