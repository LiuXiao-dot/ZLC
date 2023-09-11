using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;
using ZLC.ConfigSystem;
using ZLC.EditorSystem;
using FilePathAttribute = ZLC.FileSystem.FilePathAttribute;
namespace ZLCEditor.Tool.ResourceFormat
{
    /// <summary>
    /// 音频格式化
    /// </summary>
    [Tool("优化/音频")]
    [FilePath("AudioFormatter.asset",FilePathAttribute.PathType.XWEditor,true)]
    public class AudioFormatter : SOSingleton<AudioFormatter>
    {
        /// <summary>
        /// 简单音效
        /// </summary>
        [LabelText("简单音效")]
        public Preset simpleSFX;
        /// <summary>
        /// 复杂音效
        /// </summary>
        [LabelText("复杂音效")]
        public Preset complexSFX;
        /// <summary>
        /// 背景音乐
        /// </summary>
        [LabelText("背景音乐")]
        public Preset music;

        /// <summary>
        /// 重命名工具
        /// </summary>
        public FolderFileRenameHelper rename;
        
        /// <summary>
        /// 格式化全部音频
        /// </summary>
        [Button("格式化全部音频")]
        public void Format()
        {
            if(simpleSFX == null || complexSFX == null || music == null)
                return;
            var audioUris = AssetDatabase.FindAssets("t:AudioClip");
            foreach (var audioUri in audioUris) {
                var uri = AssetDatabase.GUIDToAssetPath(audioUri);
                var audioClip = AssetImporter.GetAtPath(uri);
                Preset targetPreset;
                //EditorUtility.SetDirty(audioClip);
                if (uri.Contains("MUSIC")) {
                    // 长音效或者音乐
                    targetPreset = music;
                } else {
                    using (var read = File.OpenRead(uri)) {
                        var fileSize = read.Length;
                        if (fileSize <= 200 * 1024) {
                            targetPreset = simpleSFX;
                        }else {
                            targetPreset = complexSFX;
                        }
                    }
                }
                // 检测是否需要修改
                if (targetPreset.CanBeAppliedTo(audioClip)) {
                    targetPreset.ApplyTo(audioClip);
                } else {
                    Debug.LogError($"当前预设{targetPreset.name}无法被应用到{audioClip.assetPath}");
                }
            }
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 重命名音频文件
        /// </summary>
        [Button("重命名")]
        public void Rename()
        {
            rename.Excute();
        }
        
        [TextArea(3,20)]
        [ReadOnly] public string tip = @"- 简短音效导入后小于200kb，采用Decompress on Load模式

- 对于复杂音效，大小大于200kb，长度超过5秒的音效采用Compressed In Memory模式

- 对于长度较长的音效或背景音乐则采用Streaming模式，虽然会有CPU额外开销，但节省内存并且加载不卡顿

- 根据平台选择合理的音频设置，原始音频资源尽量采用未压缩WAV格式

- 移动平台对音乐音效统一采用单通道设置（Force to Mono）,并将音乐采样频率设置为22050Hz

- 移动平台大多数声音尽量采用Vorbis压缩设置，IOS平台或不打算循环的声音可以选择MP3格式，对于简短、常用的音效，可以采用解码速度快的ADPCM格式（PCM为未压缩格式）

- 音频片段加载类型说明

- 当实现静音功能时，不要简单的将音量设置为0，应销毁音频（AudioSource）组件，将音频从内存中卸载。";
    }
}