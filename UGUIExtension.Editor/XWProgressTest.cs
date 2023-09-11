/*#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;

namespace XWEngine.UGUI
{
    public class XWProgressTest : MonoBehaviour
    {
        public XWProgress XWProgress;

        public float progress;

        [Button("设置进度")]
        public void SetProgress()
        {
            if (XWProgress == null) return;
            XWProgress.SetProgress(progress);
        }

        [Button("模拟进度")]
        public void DoProgress()
        {
            if (XWProgress == null) return;
            progress = 0;
            CoroutineHelper.AddCoroutineWaitTime(()=>
            {
                progress += 0.01f;
                XWProgress.SetProgress(progress);
            },100,0.02f,0.1f);
        }
    }
}
#endif*/