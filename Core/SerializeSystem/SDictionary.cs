using UnityEngine;
using ZLC.Extensions;
namespace ZLC.SerializeSystem;

/// <summary>
/// 可序列化的字典
/// </summary>
/// <typeparam name="K">键</typeparam>
/// <typeparam name="V">值</typeparam>
[Serializable]
public class SDictionary<K, V> : Dictionary<K, V>, ISerializationCallbackReceiver
{
    [SerializeField] private K[] keysCache;
    [SerializeField] private V[] valuesCache;

    /// <inheritdoc />
    public void OnBeforeSerialize()
    {
        if (Count == 0) return;

        keysCache = new K[Count];
        valuesCache = new V[Count];
        var index = 0;
        foreach (var kv in this) {
            keysCache[index] = kv.Key;
            valuesCache[index] = kv.Value;
            index++;
        }
    }

    /// <inheritdoc />
    public void OnAfterDeserialize()
    {
        Clear();
        if (keysCache.IsEmptyOrNull() || valuesCache.IsEmptyOrNull()) return;
        var count = keysCache.Length;
        for (int i = 0; i < count; i++) {
            Add(keysCache[i], valuesCache[i]);
        }

        keysCache = null;
        valuesCache = null;
    }
}