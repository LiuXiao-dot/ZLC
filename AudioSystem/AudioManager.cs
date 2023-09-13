using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using ZLC.Application;
using ZLC.Common;
using ZLC.ConfigSystem;
using ZLC.EventSystem;
using ZLC.IOCSystem;
using ZLC.ResSystem;
using ZLC.SceneSystem;
namespace ZLC.AudioSystem;

/// <inheritdoc cref="ZLC.AudioSystem.IAudioManager" />
[Component(AppConstant.APP_LAUNCHER_MANAGER)]
[Component(AppConstant.APP_LAUNCHER_PRELOADERS)]
public class AudioManager : IAudioManager, ILoader, ISubscriber<SceneMessage>
{
    private Transform _root;
    private AudioMixer _audioMixer;
    private IResLoader _resLoader;

    /// <inheritdoc />
    public void Dispose()
    {

    }
    /// <inheritdoc />
    public void Init()
    {
        _resLoader = IAppLauncher.Get<IResLoader>();
        SceneMessageQueue.Instance.Subscribe(this, SceneMessage.OnSceneOpen);
    }
    /// <inheritdoc />
    public void SetGroupVolume(string parameterName, float normalizedVolume)
    {
        bool volumeSet = _audioMixer.SetFloat(parameterName, AudioUtils.NormalizedToMixerValue(normalizedVolume));
        if (!volumeSet)
            Debug.LogError($"AudioMixer没有参数:{parameterName}");
    }
    /// <inheritdoc />
    public float GetGroupVolume(string parameterName)
    {
        if (_audioMixer.GetFloat(parameterName, out float rawVolume)) {
            return AudioUtils.MixerValueToNormalized(rawVolume);
        } else {
            Debug.LogError($"AudioMixer没有参数:{parameterName}");
            return 0f;
        }
    }

    /// <inheritdoc />
    public void Load(Action<int, int> onProgress = null)
    {
        int progress = 0;
        _resLoader.LoadPoolableGameObject(AudioConstant.AUDIO_PREFAB, (success, result) =>
        {
            progress++;
            onProgress?.Invoke(progress, 2);
        });
        IAppLauncher.Get<IResLoader>().LoadAsset<AudioMixer>(AudioConstant.AUDIO_MIXER, (success, result) =>
        {
            if (success) {
                _audioMixer = result;
            }
            progress++;
            onProgress?.Invoke(progress, 2);
        });
    }

    /// <inheritdoc />
    public AudioWrap Play(AudioCueSO audioCueSo,Vector3 position = default)
    {
        var audioObj = _resLoader.InstantiateGameObjectSync(AudioConstant.AUDIO_PREFAB, _root);
        audioObj.transform.SetPositionAndRotation(position, Quaternion.identity);
        var audioWrap = audioObj.GetComponent<AudioWrap>();
        audioWrap.Play(audioCueSo, position);
        return audioWrap;
    }
    /// <inheritdoc />
    public AudioWrap Play(string audioName, Vector3 position = default)
    {
        _resLoader.LoadAssetSync(audioName, out AudioCueSO audioCueSo);
        return Play(audioCueSo, position);
    }

    /// <inheritdoc />
    public void Pause(AudioWrap audioWrap)
    {
        audioWrap.Pause();
    }

    /// <inheritdoc />
    public void Finish(AudioWrap audioWrap)
    {
        audioWrap.Finish();
    }

    /// <inheritdoc />
    public void Stop(AudioWrap audioWrap)
    {
        audioWrap.Stop();
    }
    /// <inheritdoc />
    public void Resume(AudioWrap audioWrap)
    {
        audioWrap.Resume();
    }

    /// <inheritdoc />
    public void OnMessage(EventSystem.Event inGameEvent)
    {
        switch ((SceneMessage)inGameEvent.operate) {
            case SceneMessage.OnSceneOpen:
                // 判断是否加载了UI场景，并在UI场景中初始化AudioManager
                if (inGameEvent.data.Equals(AppConfigSO.Instance.uiSceneName)) {
                    SceneMessageQueue.Instance.Unsubscribe(this, SceneMessage.OnSceneOpen);
                    var uiScene = SceneManager.GetSceneByName(Path.GetFileNameWithoutExtension(AppConfigSO.Instance.uiSceneName));
                    var uiScenePrefab = uiScene.GetRootGameObjects();
                    _root = uiScenePrefab[0].transform.Find("AudioManager");
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        SceneMessageQueue.Instance.Callback(inGameEvent);
    }
}