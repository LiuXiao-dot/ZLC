using Sirenix.OdinInspector;
using UnityEngine;
using ZLC.ConfigSystem;
using FilePathAttribute = ZLC.FileSystem.FilePathAttribute;
namespace ZLC.AudioSystem;

/// <summary>
/// 全部的音频属性配置
/// </summary>
[FilePath("AudioConfigurationListSO.asset",FilePathAttribute.PathType.XW,true)]
public class AudioConfigurationListSO : SOSingleton<AudioConfigurationListSO>
{
    [ReadOnly][SerializeField]internal AudioConfigurationSO[] configurations;

    /// <summary>
    /// 获取配置
    /// </summary>
    /// <param name="audioType"></param>
    /// <returns></returns>
    public AudioConfigurationSO Get(AudioType audioType)
    {
        return configurations[(int)audioType];
    }
}