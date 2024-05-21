using UnityEngine;

namespace SFramework.Actor.Buff
{
    public class SBuffInfo
    {
        public SBuffData BuffData;
        public GameObject Creator;
        public GameObject Target;
        public float Duration;
        public float TickTime;
        public int CurStack;
    }

    public class SBuffDamageInfo
    {
        public GameObject Creator;
        public GameObject Target;
        public float Damage;
    }
}
