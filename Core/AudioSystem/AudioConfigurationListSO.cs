using Sirenix.OdinInspector;
using UnityEngine;
using ZLC.ConfigSystem;
using FilePathAttribute = ZLC.FileSystem.FilePathAttribute;
namespace ZLC.AudioSystem;

/// <summary>
/// ȫ������Ƶ��������
/// </summary>
[FilePath("AudioConfigurationListSO.asset",FilePathAttribute.PathType.XW,true)]
public class AudioConfigurationListSO : SOSingleton<AudioConfigurationListSO>
{
    [ReadOnly][SerializeField]internal AudioConfigurationSO[] configurations;

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <param name="audioType"></param>
    /// <returns></returns>
    public AudioConfigurationSO Get(AudioType audioType)
    {
        return configurations[(int)audioType];
    }
}