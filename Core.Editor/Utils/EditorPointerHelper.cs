using UnityEngine;
namespace ZLCEditor.Utils
{
    /// <summary>
    /// Pointer相关方法
    /// </summary>
    public sealed class EditorPointerHelper
    {
        public static Vector3 GetMousePosToScene()
        {
            UnityEditor.SceneView sceneView = UnityEditor.SceneView.currentDrawingSceneView;
            //当前屏幕坐标,左上角(0,0)右下角(camera.pixelWidth,camera.pixelHeight)
            Vector2 mousePos = Event.current.mousePosition;
            //retina 屏幕需要拉伸值
            float mult = UnityEditor.EditorGUIUtility.pixelsPerPoint;
            //转换成摄像机可接受的屏幕坐标,左下角是(0,0,0);右上角是(camera.pixelWidth,camera.pixelHeight,0)
            mousePos.y = sceneView.camera.pixelHeight - mousePos.y * mult;
            mousePos.x *= mult;
            //近平面往里一些,才能看到摄像机里的位置
            Vector3 fakePoint = mousePos;
            fakePoint.z = 20;
            Vector3 point = sceneView.camera.ScreenToWorldPoint(fakePoint);
            return point;
        }
    }
}