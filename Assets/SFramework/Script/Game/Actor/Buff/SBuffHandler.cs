using System.Collections.Generic;
using SFramework.Actor.Buff;
using UnityEngine;

namespace SFramework.Actor.Buff
{
    public class SBuffHandler : MonoBehaviour
    {
        public LinkedList<SBuffInfo> BuffInfos = new LinkedList<SBuffInfo>();

        public void AddBuff(SBuffInfo buffInfo)
        {
            SBuffInfo info = FindBuffInfoById(buffInfo.BuffData.Id);
            if (info == null)
            {
                info.Duration = info.BuffData.Duration;
                info.BuffData.OnCreate.Apply(info);
                BuffInfos.AddLast(buffInfo);
                // sort list
            }
            else
            {
                if(info.CurStack < buffInfo.BuffData.MaxStack)
                {
                    info.CurStack++;
                }

                switch (buffInfo.BuffData.BuffUpdateTime)
                {
                    case SBuffUpdateTime.Add:
                        info.BuffData.Duration += buffInfo.BuffData.Duration;
                        break;
                    case SBuffUpdateTime.Replace:
                        info.BuffData.Duration = buffInfo.BuffData.Duration;
                        break;
                    case SBuffUpdateTime.Keep:
                        break;
                }

                info.BuffData.OnCreate.Apply(info);
            }
        }

        public void RemoveBuff(SBuffInfo buffInfo)
        {
            switch (buffInfo.BuffData.BuffRemoveTime)
            {
                case SBuffRemoveTime.Clear:
                    buffInfo.BuffData.OnRemove.Apply(buffInfo);
                    BuffInfos.Remove(buffInfo);
                    break;
                case SBuffRemoveTime.Reduce:
                    buffInfo.CurStack--;
                    buffInfo.BuffData.OnRemove.Apply(buffInfo);
                    if (buffInfo.CurStack <= 0)
                    {
                        BuffInfos.Remove(buffInfo);
                    }
                    else
                    {
                        buffInfo.Duration = buffInfo.BuffData.Duration;
                    }
                    break;
            }
        }

        private SBuffInfo FindBuffInfoById(int id)
        {
            foreach (var buffInfo in BuffInfos)
            {
                if (buffInfo.BuffData.Id == id)
                {
                    return buffInfo;
                }
            }
            return null;
        }
    }
}
