using Sirenix.OdinInspector;
using UnityEditor.Presets;
using UnityEngine;
using ZLC.ConfigSystem;
using ZLC.EditorSystem;
using FilePathAttribute = ZLC.FileSystem.FilePathAttribute;
namespace ZLCEditor.Tool.ResourceFormat
{
    /// <summary>
    /// 纹理格式化
    /// </summary>
    [Tool("优化/纹理")]
    [FilePath("TextureFormatter.asset",FilePathAttribute.PathType.XWEditor,true)]
    public class TextureFormatter : SOSingleton<TextureFormatter>
    {
        public Preset Default;

        public Preset NormalMap;

        public Preset Editor_GUI_and_Legacy_GUI;

        public Preset Sprite2D_and_UI;

        public Preset Cursor;

        public Preset Cookie;

        public Preset Lightmap;

        public Preset SingleChannel;

        [Button("格式化所有纹理")]
        public void Fomat()
        {
            // todo:根据实际需求进行完善
        }
        
        [TextArea(3,20)]
        [ReadOnly] public string tip = @"- Default：默认的纹理类型格式

- Normal map：法线贴图，可将颜色通道转换为适合实时法线贴图格式

- Editor GUI and Legacy GUI：在编辑器GUI控件上使用纹理请选择此类型

- Sprite(2D and UI)：在2D游戏中使用的精灵(Sprite)或UGUI使用的纹理请选择此类型

- Cursor：鼠标光标自定义纹理类型

- Cookie：用于光照Cookie剪影类型的纹理

- Lightmap：光照贴图类型的纹理，编码格式取决于不同的平台

- Single Channel：如果原始图片文件只有一个通道，请选择此类型

纹理压缩格式默认为ASTC,关闭mipmap,readwrite等";
    
    }
}