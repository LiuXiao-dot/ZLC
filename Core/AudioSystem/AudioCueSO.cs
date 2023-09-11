using UnityEngine;
namespace ZLC.AudioSystem;

/// <summary>
/// 音频资源:一个AudioCueSO可以包含一种声音的多个AudioClip
/// todo:检测卸载AudioCueSO时是否回自动释放AudioClip
/// </summary>
public class AudioCueSO : ScriptableObject
{
    /// <summary>
    /// 是否循环播放
    /// </summary>
    public bool looping = false;
    /// <summary>
    /// 音频类型
    /// </summary>
    public AudioType audioType;
    /// <summary>
    /// 播放顺序
    /// </summary>
    public SequenceMode sequenceMode = SequenceMode.RandomNoImmediateRepeat;
    /// <summary>
    /// 音频
    /// </summary>
    public AudioClip[] audioClips;

    private int _nextClipToPlay;
    private int _lastClipPlayed;

    private void OnEnable()
    {
        _nextClipToPlay = -1;
        _lastClipPlayed = -1;
    }

    /// <summary>
    /// 是否是音频组（包含多个音频）
    /// </summary>
    /// <returns></returns>
    public bool IsAudioGroup()
    {
        return audioClips.Length > 1;
    }

    /// <summary>
    /// 获取当前音频
    /// </summary>
    /// <returns></returns>
    public AudioClip GetCurrentClip()
    {
        if (_lastClipPlayed == -1) return audioClips[0];
        return audioClips[_lastClipPlayed];
    }
    
    /// <summary>
    /// 获取下一个音频
    /// </summary>
    /// <returns>音频剪辑</returns>
    public AudioClip GetNextClip()
    {
        // 只有一个音频，直接返回
        if (audioClips.Length == 1)
            return audioClips[0];

        if (_nextClipToPlay == -1)
        {
            // 初始化
            _nextClipToPlay = (sequenceMode == SequenceMode.Sequential) ? 0 : UnityEngine.Random.Range(0, audioClips.Length);
        }
        else
        {
            // 选择下一个音频
            switch (sequenceMode)
            {
                case SequenceMode.Random:
                    _nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
                    break;

                case SequenceMode.RandomNoImmediateRepeat:
                    do
                    {
                        _nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
                    } while (_nextClipToPlay == _lastClipPlayed);
                    break;

                case SequenceMode.Sequential:
                    _nextClipToPlay = (int)Mathf.Repeat(++_nextClipToPlay, audioClips.Length);
                    break;
            }
        }

        _lastClipPlayed = _nextClipToPlay;

        return audioClips[_nextClipToPlay];
    }
    
    /// <summary>
    /// 播放顺序
    /// </summary>
    public enum SequenceMode
    {
        /// <summary>
        /// 随机播放
        /// </summary>
        Random,
        /// <summary>
        /// 随机播放，但是不会连续随机到相同的音频
        /// </summary>
        RandomNoImmediateRepeat,
        /// <summary>
        /// 顺序播放
        /// </summary>
        Sequential,
    }
}