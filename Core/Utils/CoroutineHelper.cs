using System.Collections;
using UnityEngine;
namespace ZLC.Utils
{
    /// <summary>
    /// 协程管理
    /// </summary>
    public sealed class CoroutineHelper : MonoBehaviour
    {
        private static CoroutineHelper instance
        {
            get
            {
                if (_instance == null)
                {
                    var newObj = new GameObject("CoroutineHelper");
                    _instance = newObj.AddComponent<CoroutineHelper>();
                    DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
        }

        private static CoroutineHelper _instance;

        private static Dictionary<int, IEnumerator> coroutines = new Dictionary<int, IEnumerator>();
        
        /// <summary>
        /// 添加一个协程并运行
        /// </summary>
        /// <param name="enumrator"></param>
        /// <returns></returns>
        public static Coroutine AddCoroutine(IEnumerator enumrator)
        {
            return instance.StartCoroutine(enumrator);
        }
        
        /// <summary>
        /// 移除一个协程并停止运行
        /// </summary>
        /// <param name="enumrator"></param>
        public static void RemoveCoroutine(IEnumerator enumrator)
        {
            instance.StopCoroutine(enumrator);
        }

        /// <summary>
        /// 每interval秒执行一次，执行loopTime次，loopTime小于0时为循环
        /// (delayTime为0时会立刻执行一次)
        /// </summary>
        /// <param name="action"></param>
        /// <param name="loopTime"></param>
        /// <param name="interval"></param>
        /// <param name="delayTime"></param>
        public static void AddCoroutineWaitTime(Action action,int loopTime,float interval,float delayTime = 0)
        {
            var routine = ActionCoroutineWaitTime(action, loopTime,interval,delayTime);
            var hashCode = action.GetHashCode();
            if (coroutines.ContainsKey(hashCode))
            {
                Debug.LogWarning("暂不支持同一个action同时在多个协程中执行");
                return;
            }
            coroutines.Add(action.GetHashCode(),routine);
            instance.StartCoroutine(routine);
        }

        private static IEnumerator ActionCoroutineWaitTime(Action action,int loopTime,float interval,float delatTime)
        {
            yield return new WaitForSeconds(delatTime);
            var inter = new WaitForSeconds(interval);
            while (loopTime != 0)
            {
                action();
                yield return inter;
                loopTime--;
            }

            coroutines.Remove(action.GetHashCode());
        }


        /// <summary>
        /// 每帧执行一次，执行loopTime次，loopTime小于0时为循环
        /// </summary>
        /// <param name="action"></param>
        /// <param name="loopTime"></param>
        /// <param name="delayTime"></param>
        public static void AddCoroutineTime(Action action,int loopTime,float delayTime = 0)
        {
            var routine = ActionCoroutineTime(action, loopTime,delayTime);
            var hashCode = action.GetHashCode();
            if (coroutines.ContainsKey(hashCode))
            {
                Debug.LogWarning("暂不支持同一个action同时在多个协程中执行");
                return;
            }
            coroutines.Add(action.GetHashCode(),routine);
            instance.StartCoroutine(routine);
        }

        private static IEnumerator ActionCoroutineTime(Action action,int loopTime,float delatTime)
        {
            yield return new WaitForSeconds(delatTime);
            while (loopTime != 0)
            {
                action();
                yield return null;
                loopTime--;
            }

            coroutines.Remove(action.GetHashCode());
        }


        /// <summary>
        /// 每帧执行一次，执行loopTime次，loopTime小于0时为循环
        /// </summary>
        /// <param name="action"></param>
        /// <param name="loopTime"></param>
        /// <param name="delayFrame"></param>
        public static void AddCoroutineFrame(Action action,int loopTime,int delayFrame = 0)
        {
            var routine = ActinoCoroutineFrame(action, loopTime,delayFrame);
            var hashCode = action.GetHashCode();
            if (coroutines.ContainsKey(hashCode))
            {
                Debug.LogWarning("暂不支持同一个action同时在多个协程中执行");
                return;
            }
            coroutines.Add(action.GetHashCode(),routine);
            instance.StartCoroutine(routine);
        }

        private static IEnumerator ActinoCoroutineFrame(Action action,int loopTime,int delayFrame)
        {
            for (int i = 0; i < delayFrame; i++)
            {
                yield return null;
            }
            while (loopTime != 0)
            {
                action();
                yield return null;
                loopTime--;
            }

            coroutines.Remove(action.GetHashCode());
        }


        /// <summary>
        /// 每帧执行一次，执行loopTime次，loopTime小于0时为循环
        /// </summary>
        /// <param name="action"></param>
        /// <param name="loopTime"></param>
        /// <param name="delayFrame"></param>
        public static void AddCoroutineEndFrame(Action action,int loopTime,int delayFrame = 0)
        {
            var routine = ActionCoroutineEndFrame(action, loopTime,delayFrame);
            var hashCode = action.GetHashCode();
            if (coroutines.ContainsKey(hashCode))
            {
                Debug.LogWarning("暂不支持同一个action同时在多个协程中执行");
                return;
            }
            coroutines.Add(action.GetHashCode(),routine);
            instance.StartCoroutine(routine);
        }

        private static IEnumerator ActionCoroutineEndFrame(Action action,int loopTime,int delayFrame)
        {
            var wait = new WaitForEndOfFrame();
            for (int i = 0; i < delayFrame; i++)
            {
                yield return null;
            }
            while (loopTime != 0)
            {
                action();
                yield return wait;
                loopTime--;
            }

            coroutines.Remove(action.GetHashCode());
        }
        
        /// <summary>
        /// 停止一个协程
        /// </summary>
        /// <param name="action"></param>
        public static void StopCoroutine(Action action)
        {
            var hashCode = action.GetHashCode();
            if (coroutines.TryGetValue(hashCode,out var temp))
            {
                instance.StopCoroutine(temp);
                coroutines.Remove(hashCode);
            }
        }
    }
}
