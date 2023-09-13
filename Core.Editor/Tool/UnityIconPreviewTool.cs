using UnityEditor;
using UnityEngine;
namespace ZLCEditor.Tool;

/// <summary>
/// 预览Unity自带图标
/// </summary>
public class UnityIconPreviewTool : EditorWindow
{
    static string[] text;
    [MenuItem("Tools/XW通用工具/UnityIconPreviewTool")]
    public static void ShowWindow()
    {
        GetWindow(typeof(UnityIconPreviewTool));
        text = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Plugins/ZLCEngine/EditorConfigs/UnityIcons.txt").text.Split("\r\n");
    }
    public Vector2 scrollPosition;
    void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        //鼠标放在按钮上的样式
        foreach (MouseCursor item in Enum.GetValues(typeof(MouseCursor))) {
            GUILayout.Button(Enum.GetName(typeof(MouseCursor), item));
            EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), item);
            GUILayout.Space(10);
        }

        //内置图标
        for (int i = 0; i < text.Length; i += 8) {
            GUILayout.BeginHorizontal();
            for (int j = 0; j < 8; j++) {
                int index = i + j;
                if (index < text.Length) {
                    try {
                        var icon = EditorGUIUtility.IconContent(text[index]);
                        icon.tooltip = text[index];
                        GUILayout.Button(icon, GUILayout.Width(50), GUILayout.Height(30));
                    }
                    catch (Exception e){
                        Debug.LogError($"没有{text[index]}\n{e}");
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
    }
}