using UnityEngine;
using UnityEngine.Audio;
namespace ZLC.AudioSystem;

/// <summary>
/// 音频参数设置
/// </summary>
public class AudioConfigurationSO : ScriptableObject
{
    /// <summary>
    /// 音频混合组（分为主音频、背景音乐、音效等）
    /// </summary>
    public AudioMixerGroup OutputAudioMixerGroup = null;
    /// <summary>
    /// 音频优先级
    /// </summary>
    [SerializeField] private PriorityLevel _priorityLevel = PriorityLevel.Standard;
    /// <summary>
    /// 音频优先级
    /// </summary>
    public int Priority
    {
        get { return (int)_priorityLevel; }
        set { _priorityLevel = (PriorityLevel)value; }
    }

    /// <summary>
    /// 静音
    /// </summary>
    [Header("Sound properties 属性")]
    public bool Mute = false;
    /// <summary>
    /// 音频源的音量（0.0 到 1.0）。
    /// </summary>
    [Range(0f, 1f)] public float Volume = 1f;
    /// <summary>
    /// 音频源的音高
    /// </summary>
    [Range(-3f, 3f)] public float Pitch = 1f;
    /// <summary>
    /// 以立体声方式（左声道或右声道）平移正在播放的声音。仅适用于单声道或立体声
    /// </summary>
    [Range(-1f, 1f)] public float PanStereo = 0f;
    /// <summary>
    /// 将来自 AudioSource 的信号混合到与混响区关联的全局混响中的量。
    /// </summary>
    [Range(0f, 1.1f)] public float ReverbZoneMix = 1f;
    
    /// <summary>
    /// 设置 3D 空间化计算（衰减、多普勒效应等）对该 AudioSource 的影响程度。0.0 使声音变成全 2D 效果，1.0 使其变成全 3D。
    /// </summary>
    [Header("Spatialisation 3D音效设置")]
    [Range(0f, 1f)] public float SpatialBlend = 1f;
    /// <summary>
    /// 设置/获取 AudioSource 随距离衰减的方式。
    /// </summary>
    public AudioRolloffMode RolloffMode = AudioRolloffMode.Logarithmic;
    /// <summary>
    /// 在最小距离内，AudioSource 将停止增大音量。
    /// </summary>
    [Range(0.1f, 5f)] public float MinDistance = 0.1f;
    /// <summary>
    /// （对数衰减）MaxDistance 为声音停止衰减的距离。
    /// </summary>
    [Range(5f, 100f)] public float MaxDistance = 50f;
    /// <summary>
    /// 设置扬声器空间中 3D 立体声或多声道声音的扩散角度（以度为单位）。
    /// </summary>
    [Range(0, 360)] public int Spread = 0;
    /// <summary>
    /// 设置该 AudioSource 的多普勒缩放。
    /// </summary>
    [Range(0f, 5f)] public float DopplerLevel = 1f;

    /// <summary>
    /// 直通效果（从滤波器组件或全局监听器滤波器应用）。
    /// </summary>
    [Header("Ignores 忽略选项")]
    public bool BypassEffects = false;
    /// <summary>
    /// 如果设置，则不将 AudioListener 上的全局效果应用于 AudioSource 生成的音频信号。不适用于 AudioSource 正在混合器组中播放的情况。
    /// </summary>
    public bool BypassListenerEffects = false;
    /// <summary>
    /// 	如果设置，则不将来自 AudioSource 的信号路由到与混响区关联的全局混响。
    /// </summary>
    public bool BypassReverbZones = false;
    /// <summary>
    /// 这使得音频源不考虑音频监听器的音量。
    /// </summary>
    public bool IgnoreListenerVolume = false;
    /// <summary>
    /// 即使 AudioListener.pause 设置为 true，也允许 AudioSource 播放。这对于暂停菜单中的菜单元素声音或背景音乐很有用。
    /// </summary>
    public bool IgnoreListenerPause = false;
    
    /// <summary>
    /// 优先级
    /// </summary>
    private enum PriorityLevel
    {
        Highest = 0,
        High = 64,
        Standard = 128,
        Low = 194,
        VeryLow = 256,
    }
    
    /// <summary>
    /// 为AudioSource应用配置属性
    /// </summary>
    /// <param name="audioSource">音频源</param>
    public void ApplyTo(AudioSource audioSource)
    {
        audioSource.outputAudioMixerGroup = this.OutputAudioMixerGroup;
        audioSource.mute = this.Mute;
        audioSource.bypassEffects = this.BypassEffects;
        audioSource.bypassListenerEffects = this.BypassListenerEffects;
        audioSource.bypassReverbZones = this.BypassReverbZones;
        audioSource.priority = this.Priority;
        audioSource.volume = this.Volume;
        audioSource.pitch = this.Pitch;
        audioSource.panStereo = this.PanStereo;
        audioSource.spatialBlend = this.SpatialBlend;
        audioSource.reverbZoneMix = this.ReverbZoneMix;
        audioSource.dopplerLevel = this.DopplerLevel;
        audioSource.spread = this.Spread;
        audioSource.rolloffMode = this.RolloffMode;
        audioSource.minDistance = this.MinDistance;
        audioSource.maxDistance = this.MaxDistance;
        audioSource.ignoreListenerVolume = this.IgnoreListenerVolume;
        audioSource.ignoreListenerPause = this.IgnoreListenerPause;
    }
}