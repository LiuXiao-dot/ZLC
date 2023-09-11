using UnityEngine;
namespace ZLC.SerializeSystem;

/// <summary>
/// 支持序列化不同对象类型的list
/// </summary>
[Serializable]
public class SList : List<object>, ISerializationCallbackReceiver
{
    /// <summary>
    /// 每个数据的类型
    /// </summary>
    public SType[] types;
    /// <summary>
    /// 每个数据的实际数据
    /// </summary>
    public string[] temps;

    /// <inheritdoc />
    public void OnBeforeSerialize()
    {
        var count = this.Count;
        temps = new string[count];
        types = new SType[count];
        for (var i = 0; i < count; i++)
        {
            temps[i] = JsonUtility.ToJson(this[i]);
            types[i] = this[i].GetType();
        }
    }

    /// <inheritdoc />
    public void OnAfterDeserialize()
    {
        if(temps == null) return;
        var count = temps.Length;
        for (var i = 0; i < count; i++)
        {
            Add(JsonUtility.FromJson(temps[i],types[i]));
        }

        temps = null;
        types = null;
    }
}