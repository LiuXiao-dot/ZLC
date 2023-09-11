using Sirenix.OdinInspector;
using UnityEngine;
using ZLC.Utils;
namespace ZLC.AudioSystem;

/// <summary>
/// 挂在AudioSource上，提供音频播放暂停、播放完成回调等功能
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioWrap : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField]private AudioCueSO _audioCueSo;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 播放完成触发
    /// </summary>
    private Action _onFinished;

    /// <summary>
    /// 添加播放完成的事件
    /// </summary>
    /// <param name="onFinished">播放完成的事件</param>
    public void AddFinishedListener(Action onFinished)
    {
        this._onFinished += onFinished;
    }
    
    /// <summary>
    /// 播放音频
    /// 如果AudioCueSo中有多个音频，将按对应的顺序轮播音频
    /// </summary>
    /// <param name="audioCueSo"></param>
    /// <param name="setting"></param>
    /// <param name="position"></param>
    /// <param name="loop"></param>
    internal void Play(AudioCueSO audioCueSo, Vector3 position = default)
    {
        if(_audioSource.isPlaying) return;
        AudioConfigurationListSO.Instance.Get(audioCueSo.audioType).ApplyTo(_audioSource);
        _audioCueSo = audioCueSo;

        transform.SetPositionAndRotation(position,Quaternion.identity);
        PlayNextClip();
    }

    private void PlayNextClip()
    {
        if (_audioCueSo == null) {
            CoroutineHelper.StopCoroutine(PlayNextClip);
            return;
        }
        var audioClip = _audioCueSo.GetNextClip();
        _audioSource.clip = audioClip;
        _audioSource.loop = _audioCueSo.looping;
        _audioSource.time = 0f; // 重置播放位置
        _audioSource.Play();
        if (_audioCueSo.looping) {
            if (_audioCueSo.IsAudioGroup()) {
                CoroutineHelper.StopCoroutine(PlayNextClip);
                CoroutineHelper.AddCoroutineWaitTime(PlayNextClip,1,0,_audioCueSo.GetCurrentClip().length);
            }
        } else {
            CoroutineHelper.AddCoroutineWaitTime(Finish,1,0,_audioCueSo.GetCurrentClip().length);
        }
    }
    
    internal void Stop()
    {
        if(_audioSource.isPlaying)
            _audioSource.Stop();
        if (_audioCueSo.looping) {
            if (_audioCueSo.IsAudioGroup()) {
                CoroutineHelper.StopCoroutine(PlayNextClip);
            }
        } else {
            CoroutineHelper.StopCoroutine(Finish);
        }
    }

    internal void Pause()
    {
        if(_audioSource.isPlaying)
            _audioSource.Pause();
    }

    internal void Finish()
    {
        Stop();
        _onFinished?.Invoke();
        _onFinished = null;
    }
    internal void Resume()
    {
        _audioSource.UnPause();
    }
}