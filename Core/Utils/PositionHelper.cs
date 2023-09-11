using UnityEngine;
namespace ZLC.Utils
{
    /// <summary>
    ///坐标变换相关的方法
    /// </summary>
    public sealed class PositionHelper
    {
        
        /// <summary>
        /// 获取z=0的鼠标的世界坐标
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPosition()
        {
            if (Camera.main != null) {
                Vector3 vec = GetMouseWorldPositionWithZ(PointerHelper.GetPointerPosition(), Camera.main);
                vec.z = 0f;
                return vec;
            }
            Debug.LogError("无主相机");
            return Vector3.zero;
        }

        /// <summary>
        /// 获取有z轴的鼠标的世界坐标
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPositionWithZ()
        {
            if (Camera.main != null) return GetMouseWorldPositionWithZ(PointerHelper.GetPointerPosition(), Camera.main);
            Debug.LogError("无主相机");
            return Vector3.zero;
        }

        /// <summary>
        /// 获取有z轴的鼠标的世界坐标
        /// </summary>
        /// <param name="worldCamera"></param>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(PointerHelper.GetPointerPosition(), worldCamera);
        }

        /// <summary>
        /// 获取有Z轴的鼠标的世界坐标
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <param name="worldCamera"></param>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        /// <summary>
        /// 根据角度获取方向向量
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector3 GetVectorFromAngle(int angle)
        {
            // angle = 0 -> 360
            float angleRad = angle * (Mathf.PI / 180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        /// <summary>
        /// 根据角度获取方向向量
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector3 GetVectorFromAngle(float angle)
        {
            // angle = 0 -> 360
            float angleRad = angle * (Mathf.PI / 180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        /// <summary>
        /// 根据方向向量或去角度
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static float GetAngleFromVectorFloat(Vector3 dir)
        {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n;
        }

        /// <summary>
        /// 根据方向向量获取角度
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static float GetAngleFromVectorFloatXZ(Vector3 dir)
        {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n;
        }

        /// <summary>
        /// 根据方向向量获取角度
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static int GetAngleFromVector(Vector3 dir)
        {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            int angle = Mathf.RoundToInt(n);

            return angle;
        }

        /// <summary>
        /// 根据方向向量获取角度
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static int GetAngleFromVector180(Vector3 dir)
        {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            int angle = Mathf.RoundToInt(n);

            return angle;
        }


        /// <summary>
        /// 将<paramref name="vec"/>旋转<paramref name= "vecRotation"/>
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="vecRotation"></param>
        /// <returns></returns>
        public static Vector3 ApplyRotationToVector(Vector3 vec, Vector3 vecRotation)
        {
            return ApplyRotationToVector(vec, GetAngleFromVectorFloat(vecRotation));
        }

        /// <summary>
        /// 将<paramref name="vec"/>旋转<paramref name= "angle"/>
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector3 ApplyRotationToVector(Vector3 vec, float angle)
        {
            return Quaternion.Euler(0, 0, angle) * vec;
        }

        /// <summary>
        /// 将<paramref name="vec"/>旋转<paramref name= "angle"/>
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector3 ApplyRotationToVectorXZ(Vector3 vec, float angle)
        {
            return Quaternion.Euler(0, angle, 0) * vec;
        }

        /// <summary>
        /// 将<paramref name="vec"/>的每一位都进行RoundToInt操作
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static Vector3Int RoundToInt(Vector3 vec)
        {
            return new Vector3Int(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y), Mathf.RoundToInt(vec.z));
        }

        /// <summary>
        /// 将<paramref name="a"/>与<paramref name="b"/>的每一位分别相乘得到一个新的向量
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 EachMultiply(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
    }
}