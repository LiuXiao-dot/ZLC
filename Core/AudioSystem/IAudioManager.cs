using UnityEngine;
using ZLC.Common;
using ZLC.ResSystem;
namespace ZLC.AudioSystem;

/// <summary>
/// 音频管理器
/// </summary>
public interface IAudioManager : IManager
{
    /// <summary>
    /// 设置<paramref name="parameterName"/>的音量大小为<paramref name="normalizedVolume"/>
    /// </summary>
    /// <param name="parameterName"></param>
    /// <param name="normalizedVolume"></param>
    void SetGroupVolume(string parameterName, float normalizedVolume);
    /// <summary>
    /// 获取<paramref name="parameterName"/>的音量大小
    /// </summary>
    /// <param name="parameterName"></param>
    /// <returns></returns>
    float GetGroupVolume(string parameterName);
    /// <summary>
    /// 播放音频
    /// </summary>
    /// <param name="audioCueSo">音频资源</param>
    /// <param name="position">音频播放位置</param>
    /// <returns></returns>
    AudioWrap Play(AudioCueSO audioCueSo, Vector3 position = default);
    /// <summary>
    /// 播放音频
    /// (使用<see cref="IResLoader"/>的DestroyAsset方法销毁资源
    /// </summary>
    /// <param name="audioName">音频资源的名字</param>
    /// <param name="position">音频播放位置</param>
    /// <returns></returns>
    AudioWrap Play(string audioName, Vector3 position = default);
    /// <summary>
    /// 暂停播放
    /// </summary>
    /// <param name="audioWrap"></param>
    void Pause(AudioWrap audioWrap);
    /// <summary>
    /// 完成播放
    /// </summary>
    /// <param name="audioWrap"></param>
    void Finish(AudioWrap audioWrap);
    /// <summary>
    /// 停止播放(与finish一样会完成播放，但是不会触发回调)
    /// </summary>
    /// <param name="audioWrap"></param>
    void Stop(AudioWrap audioWrap);
    /// <summary>
    /// 恢复播放
    /// </summary>
    /// <param name="audioWrap"></param>
    void Resume(AudioWrap audioWrap);
}