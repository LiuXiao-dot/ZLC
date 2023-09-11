using System.Reflection;
using UnityEditor;
using UnityEngine;
using ZLC.ConfigSystem;
using FilePathAttribute = ZLC.FileSystem.FilePathAttribute;
namespace ZLCEditor;

public class SOSingletonEditor
{
    public static Action<ScriptableObject> onSOSingletonCreated; 
        
    public static void Check<T>() where T : SOSingleton<T>
    {
        var _instance = AssetDatabase.LoadAssetAtPath<T>(typeof(T).GetCustomAttribute<FilePathAttribute>().GetPath());
        if (_instance == null) {
            _instance = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(_instance, typeof(T).GetCustomAttribute<FilePathAttribute>().GetPath());
        }
    }
    
    public static void Check(Type type)
    {
        var _instance = AssetDatabase.LoadAssetAtPath(type.GetCustomAttribute<FilePathAttribute>().GetPath(),type);
        if (_instance == null) {
            _instance = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(_instance, type.GetCustomAttribute<FilePathAttribute>().GetPath());
            onSOSingletonCreated?.Invoke((ScriptableObject)_instance);
        }
    }
}