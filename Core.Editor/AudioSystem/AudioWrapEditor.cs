using System.Reflection;
using UnityEditor;
using UnityEngine;
using ZLC.AudioSystem;
namespace ZLCEditor.AudioSystem;

/// <summary>
/// <see cref="AudioWrap"/>的编辑器显示
/// </summary>
[CustomEditor(typeof(AudioWrap))]
public class AudioWrapEditor : Editor
{
    private SerializedProperty _audioCueSoProp;
    private void OnEnable()
    {
        _audioCueSoProp = serializedObject.FindProperty("_audioCueSo");
    }

    /// <inheritdoc />
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("播放（仅编辑器模式）")) {
            PlayInEditor();
        }
    }

    /// <summary>
    /// 播放（仅编辑器模式）
    /// </summary>
    public void PlayInEditor()
    {
        if (_audioCueSoProp.objectReferenceValue == null) {
            Debug.LogError("未设置AudioCueSO");
            return;
        }
        var play = typeof(AudioWrap).GetMethod("Play", BindingFlags.NonPublic | BindingFlags.Instance);
        play?.Invoke(target, new object[]
        {
            _audioCueSoProp.objectReferenceValue, Vector3.zero
        });
    }
}