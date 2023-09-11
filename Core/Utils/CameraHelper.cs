using UnityEngine;
namespace ZLC.Utils
{
    /// <summary>
    /// 相机工具方法
    /// </summary>
    public sealed class CameraHelper
    {
        /// <summary>
        /// 计算视锥体在给定距离处的高度
        /// </summary>
        public static float GetFrustumHeight(float distance,float fov)
        {
            return 2.0f * distance * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
        }

        /// <summary>
        /// 获取指定视锥体高度需要的距离
        /// </summary>
        /// <param name="frustumHeight"></param>
        /// <param name="fov"></param>
        /// <returns></returns>
        public static float GetDistance(float frustumHeight,float fov)
        {
            return frustumHeight * 0.5f / Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
        }

        /// <summary>
        /// 获取fieldOfView
        /// </summary>
        /// <returns></returns>
        public static float GetFov(float frustumHeight,float distance)
        {
            return 2.0f * Mathf.Atan(frustumHeight * 0.5f / distance) * Mathf.Rad2Deg;
        }
    }
}