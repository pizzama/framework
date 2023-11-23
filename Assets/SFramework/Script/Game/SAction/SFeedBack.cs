using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.SAction
{
    [System.Serializable]
    public class SFeedBack
    {
        [Tooltip("在inspector里显示这个组件的名字")]
        public string Label = "SFeedback";
        [Tooltip("FeedBack发生的概率 100完全发生, 0完全不发生依次类推")]
        public float Probability = 100f;
        public SFeedBackTiming Timing;

#if UNITY_EDITOR
        public virtual Color FeedbackColor { get { return Color.white; } }
#endif
		public virtual void Play()
		{
        }

        public virtual void Stop()
		{
        }
    }
}
