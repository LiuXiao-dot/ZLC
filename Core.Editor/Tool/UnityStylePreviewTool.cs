using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace ZLCEditor.Tool;

/// <summary>
/// 预览Unity自带的GUIStyle
/// </summary>
public class UnityStylePreviewTool : EditorWindow
{

    static List<GUIStyle> styles = null;
    [MenuItem("Tools/XW通用工具//styles预览")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<UnityStylePreviewTool>("styles");

        styles = new List<GUIStyle>();
        foreach (PropertyInfo fi in typeof(EditorStyles).GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)) {
            object o = fi.GetValue(null, null);
            if (o.GetType() == typeof(GUIStyle)) {
                styles.Add(o as GUIStyle);
            }
        }
    }

    public Vector2 scrollPosition = Vector2.zero;
    void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        for (int i = 0; i < styles.Count; i++) {
            GUILayout.Label("EditorStyles." + styles[i].name, styles[i]);
        }
        GUILayout.EndScrollView();
    }
}