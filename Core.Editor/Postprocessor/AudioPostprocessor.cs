using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using ZLC.AudioSystem;
namespace ZLCEditor.Postprocessor;

/// <summary>
/// 音频相关后处理
/// </summary>
public class AudioPostprocessor
{
    [InitializeOnLoadMethod]
    private static void CheckAudioConfigurationListSO()
    {
        if(AudioConfigurationListSO.Instance != null) return;
        void OnAudioCOnfigurationListSoLoaded(ScriptableObject so)
        {
            if(so is not AudioConfigurationListSO) return;
            EditorApplication.delayCall += () =>
            {
                SOSingletonEditor.onSOSingletonCreated -= OnAudioCOnfigurationListSoLoaded;
                
                var names = Enum.GetNames(typeof(ZLC.AudioSystem.AudioType));
                var length = names.Length;
                var path = typeof(AudioConfigurationListSO).GetCustomAttribute<ZLC.FileSystem.FilePathAttribute>().GetPath();
                var assets = AssetDatabase.LoadAllAssetsAtPath(path);
                for (int i = 0; i < assets.Length; i++) {
                    if (assets[i].name == "AudioConfigurationListSO") continue;
                    AssetDatabase.RemoveObjectFromAsset(assets[i]);
                }

                var configurations = new AudioConfigurationSO[length];
                AudioConfigurationListSO.Instance.configurations = configurations;
                for (int i = 0; i < length; i++) {
                    var temp = ScriptableObject.CreateInstance<AudioConfigurationSO>();
                    temp.hideFlags = HideFlags.None;
                    temp.name = names[i];
                    AssetDatabase.AddObjectToAsset(temp, path);
                    configurations[i] = temp;
                }
                AssetDatabase.ImportAsset(path);
            };
        }
        SOSingletonEditor.onSOSingletonCreated -= OnAudioCOnfigurationListSoLoaded;
        SOSingletonEditor.onSOSingletonCreated += OnAudioCOnfigurationListSoLoaded;
    }
}