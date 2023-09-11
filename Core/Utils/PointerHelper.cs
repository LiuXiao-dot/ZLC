using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
namespace ZLC.Utils
{
    /// <summary>
    /// Pointer相关方法
    /// </summary>
    public sealed class PointerHelper
    {
        /// <summary>
        /// 获取当前鼠标/touch点坐标
        /// </summary>
        /// <returns></returns>
        public static Pointer GetPointer()
        {
            if (Mouse.current != null) {
                return Mouse.current;
            }

            if (Touchscreen.current != null) {
                return Touchscreen.current;
            }

            Debug.LogError("未支持pointer输入");
            return null;
        }

        /// <summary>
        /// 获取当前的point（鼠标等）坐标
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetPointerPosition()
        {
            return GetPointer()!.position.ReadValue();
        }

        /// <summary>
        /// 鼠标是否经过某个UI
        /// </summary>
        /// <returns></returns>
        public static bool IsPointerOverUI()
        {
            /*if (EventSystem.current.IsPointerOverGameObject())
            {
            // 在Input的Action Callback中使用会有warning，返回的是上一帧的结果
                return true;
            }*/
            PointerEventData pe = new PointerEventData(UnityEngine.EventSystems.EventSystem.current)
            {
                position = GetPointerPosition()
            };
            List<RaycastResult> hits = new List<RaycastResult>();
            UnityEngine.EventSystems.EventSystem.current.RaycastAll(pe, hits);
            return hits.Count > 0;
        }


        /// <summary>
        /// 获取position的世界坐标
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="position"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPosition(Camera camera, Vector2 position, LayerMask layer)
        {
            Ray ray = camera.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, layer.value)) {
                return raycastHit.point;
            } else {
                return Vector3.zero;
            }
        }

        /// <summary>
        /// 获取position的世界坐标
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="position"></param>
        /// <param name="layer"></param>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public static bool GetMouseWorldPosition(Camera camera, Vector2 position, LayerMask layer, out Vector3 worldPosition)
        {
            Ray ray = camera.ScreenPointToRay(position);
            if (IsPointerOverUI()) {
                worldPosition = Vector3.zero;
                return false;
            }

            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, layer.value)) {
                worldPosition = raycastHit.point;
                return true;
            } else {
                worldPosition = Vector3.zero;
                return false;
            }
        }
    }
}