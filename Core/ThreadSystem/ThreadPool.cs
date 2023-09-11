using UnityEngine;
namespace ZLC.ThreadSystem;

 /// <summary>
    /// 线程池
    /// </summary>
    public class ThreadPool : MonoBehaviour
    {
        private static ThreadPool _instance;
        /// <summary>
        /// 单例
        /// </summary>
        public static ThreadPool Instance
        {
            get
            {
                if(_instance == null)
                {
                    var obj = new GameObject("ThreadPool");
                    _instance = obj.AddComponent<ThreadPool>();
                }
                return _instance;
            }
        }

        /// <summary>
        /// 空闲线程
        /// </summary>
        private Queue<Thread> freeThreads;

        /// <summary>
        /// 使用中线程
        /// </summary>
        private List<Thread> usedThreads;

        private Dictionary<Thread, ThreadWrapper> threadCallbacks;

        private void Awake()
        {
            freeThreads = new Queue<Thread>();
            usedThreads = new List<Thread>();
            threadCallbacks = new Dictionary<Thread, ThreadWrapper>();
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public Thread Obtain()
        {
            if(freeThreads.Count == 0)
            {
                ThreadWrapper newStart = new ThreadWrapper();
                var newThread = new Thread(newStart.Run);
                threadCallbacks.Add(newThread, newStart);
                newThread.Name = "AThread" + (freeThreads.Count + usedThreads.Count);
                usedThreads.Add(newThread);
                return newThread;
            }

            var thread = freeThreads.Dequeue();
            usedThreads.Add(thread);
            return thread;
        }

        /// <summary>
        /// 只允许自身释放
        /// </summary>
        /// <param name="thread"></param>
        public void Free(Thread thread)
        {
            usedThreads.Remove(thread);
            if (Thread.CurrentThread != thread)
            {
                thread.Abort();
                return;
            }
            threadCallbacks[thread].Pause();
            freeThreads.Enqueue(thread);
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Release()
        {
            foreach (var thread in freeThreads)
            {
                thread.Abort();
            }
            foreach (var thread in usedThreads)
            {
                thread.Abort();
            }
        }

        /// <summary>
        /// 获取封装器
        /// </summary>
        /// <param name="thread"></param>
        /// <returns></returns>
        public ThreadWrapper GetWrapper(Thread thread)
        {
            return threadCallbacks[thread];
        }

        private void OnDestroy()
        {
            Release();
        }

        private void OnApplicationQuit()
        {
            Release();
        }
    }