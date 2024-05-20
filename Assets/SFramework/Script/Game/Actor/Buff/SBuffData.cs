using UnityEngine;

namespace SFramework.Actor.Buff
{
    [CreateAssetMenu(menuName = "framework/Buff/BuffData")]
    public class SBuffData : ScriptableObject
    {
        //base data
        public string Id;
        public string Name;
        public string Desc;
        public string Icon;
        public int Priority;
        public int MaxStack;
        public string[] Tags;

        //time info
        public float Duration; // if <= 0, no duration
        public float TickTime;

        //update method
        public SBuffUpdateTime BuffUpdateTime;
        public SBuffRemoveTime BuffRemoveTime;
        //call back
        public SABaseBuffModule OnCreate;
        public SABaseBuffModule OnRemove;
        public SABaseBuffModule OnUpdate;
        //call back other
        public SABaseBuffModule OnHit;
        public SABaseBuffModule OnBeHurt;
        public SABaseBuffModule OnKill;
        public SABaseBuffModule OnBeKill;

    }
}
