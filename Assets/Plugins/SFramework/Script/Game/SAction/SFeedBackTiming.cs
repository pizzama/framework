using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SFeedBackTiming
{
    [Tooltip("播放的初始延迟时间单位是秒")]
    public float InitialDelay = 0f;
    [Tooltip("两次播放的间隔时间单位是秒")]
    public float CoolDownDuration = 0f;
    [Tooltip("重复次数如果为0则表示无限循环")]
    public int NumberOfRepeats = 0;
}
