
using System.Collections.Generic;

namespace SFramework.Actor.Buff
{
    public interface ISBuffFactory
    {
        void AddBuff(ISBuff buff);
        void RemoveBuff(ISBuff buff);
        void BuffUpdate();
    }

    public class SBuffFactory : ISBuffFactory
    {
        private List<ISBuff> _buffs;
        public SBuffFactory()
        {
            _buffs = new List<ISBuff>();
        }

        public void AddBuff(ISBuff buff)
        {
            ISBuff info = FindBuffByBuffId(buff.BuffId);
            if(info == null)
            {
                
            }
            else
            {
                //as group and type deal with buff
            }
            // if (info == null)
            // {
            //     info.Duration = info.BuffData.Duration;
            //     info.BuffData.OnCreate.Apply(info);
            //     BuffInfos.AddLast(buffInfo);
            //     // sort list
            // }
            // else
            // {
            //     if(info.CurStack < buffInfo.BuffData.MaxStack)
            //     {
            //         info.CurStack++;
            //     }

            //     switch (buffInfo.BuffData.BuffUpdateTime)
            //     {
            //         case SBuffUpdateTime.Add:
            //             info.BuffData.Duration += buffInfo.BuffData.Duration;
            //             break;
            //         case SBuffUpdateTime.Replace:
            //             info.BuffData.Duration = buffInfo.BuffData.Duration;
            //             break;
            //         case SBuffUpdateTime.Keep:
            //             break;
            //     }

            //     info.BuffData.OnCreate.Apply(info);
            // }
        }

        public void RemoveBuff(ISBuff buff)
        {
            // switch (buffInfo.BuffData.BuffRemoveTime)
            // {
            //     case SBuffRemoveTime.Clear:
            //         buffInfo.BuffData.OnRemove.Apply(buffInfo);
            //         BuffInfos.Remove(buffInfo);
            //         break;
            //     case SBuffRemoveTime.Reduce:
            //         buffInfo.CurStack--;
            //         buffInfo.BuffData.OnRemove.Apply(buffInfo);
            //         if (buffInfo.CurStack <= 0)
            //         {
            //             BuffInfos.Remove(buffInfo);
            //         }
            //         else
            //         {
            //             buffInfo.Duration = buffInfo.BuffData.Duration;
            //         }
            //         break;
            // }
        }

        private ISBuff FindBuffInfoById(int id)
        {
            foreach (ISBuff buff in _buffs)
            {
                if (buff.ID == id)
                {
                    return buff;
                }
            }
            return null;
        }

        private ISBuff FindBuffByBuffId(int id)
        {
            foreach (ISBuff buff in _buffs)
            {
                if (buff.ID == id)
                {
                    return buff;
                }
            }
            return null;
        }

        public void BuffUpdate()
        {
           
        }
    }
}