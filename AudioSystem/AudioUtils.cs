namespace ZLC.AudioSystem;

/// <summary>
/// 音频工具方法
/// </summary>
public sealed class AudioUtils
{
    /// <summary>
    /// 将AudioMixer的数值转换为0~1的范围
    /// </summary>
    /// <param name="mixerValue"></param>
    /// <returns></returns>
    /// <remarks>
    /// AudioMixer的音量数值为-80~0转换后，-80对应0，0对应1.
    /// </remarks>
    public static float MixerValueToNormalized(float mixerValue)
    {
        return 1f + (mixerValue / 80f);
    }
    /// <summary>
    /// 将0~1范围的数值转换为AudioMixer的数值
    /// </summary>
    /// <param name="normalizedValue"></param>
    /// <returns></returns>
    ///     /// <remarks>
    /// AudioMixer的音量数值为-80~0转换时，-80对应0，0对应1.
    /// </remarks>
    public static float NormalizedToMixerValue(float normalizedValue)
    {
        return (normalizedValue - 1f) * 80f;
    }
}