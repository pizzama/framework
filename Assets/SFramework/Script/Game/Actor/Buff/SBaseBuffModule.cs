using UnityEngine;

namespace SFramework.Actor.Buff
{
    public abstract class SABaseBuffModule : ScriptableObject
    {
        public abstract void Apply(SBuffInfo info, SBuffDamageInfo damageInfo = null);
    }
}
 