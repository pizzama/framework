using System.Collections.Generic;

namespace SFramework.Actor.Buff
{
    public interface ISBuffFactory
    {
        void AddBuff(ISBuff buff);
        void RemoveBuff(ISBuff buff);
        void BuffTick();
    }

    public class SBuffFactory : ISBuffFactory
    {
        private List<ISBuff> _buffs;

        public SBuffFactory()
        {
            _buffs = new List<ISBuff>();
        }

        public ISBuff CreateBuff()
        {
            return null;
        }

        public void AddBuff(ISBuff buff)
        {
            ISBuff info = FindBuffByBuffId(buff.BuffId);
            if (info == null)
            {
                buff.AddExecute();
                _buffs.Add(buff);
            }
            else
            {
                //as group and type deal with buff
                dealWithDuration(ref info, ref buff);
                dealWithValue(ref info, ref buff);
                info.AddExecute();
            }
        }

        public void RemoveBuff(ISBuff buff)
        {
            ISBuff info = FindBuffByBuffId(buff.BuffId);
            if (info == null)
            {
                //as group and type deal with buff
                switch (buff.BuffRemoveType)
                {
                    case SBuffRemoveType.Clear:
                        info.RemoveExecute();
                        _buffs.Remove(info);
                        break;
                    case SBuffRemoveType.Reduce:
                        info.RemoveExecute();
                        info.CurStack--;
                        if (info.CurStack <= 0)
                        {
                            _buffs.Remove(info);
                        }
                        break;
                }
            }
        }

        public ISBuff FindBuffByBuffId(int id)
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

        public List<ISBuff> FilterBuffByGroup(SBuffGroup group)
        {
            List<ISBuff> buffs = new List<ISBuff>();
            foreach (ISBuff buff in _buffs)
            {
                if (buff.Group == group)
                {
                    buffs.Add(buff);
                }
            }

            return buffs;
        }

        public void BuffTick() { }

        protected virtual void dealWithDuration(ref ISBuff srcBuff, ref ISBuff targetBuff)
        {
            switch (srcBuff.BuffDurationUpdateType)
            {
                case SBuffUpdateType.Add:
                    srcBuff.Duration += targetBuff.Duration;
                    break;
                case SBuffUpdateType.Replace:
                    srcBuff.Duration = targetBuff.Duration;
                    break;
                case SBuffUpdateType.ReplaceUsingHigh:
                    if (srcBuff.Duration <= targetBuff.Duration)
                    {
                        srcBuff.Duration = targetBuff.Duration;
                    }
                    break;
                case SBuffUpdateType.Keep:
                    break;
            }
        }

        protected virtual void dealWithValue(ref ISBuff srcBuff, ref ISBuff targetBuff)
        {
            switch (srcBuff.BuffValueUpdateType)
            {
                case SBuffUpdateType.Add:
                    srcBuff.Value += targetBuff.Value;
                    break;
                case SBuffUpdateType.Replace:
                    srcBuff.Value = targetBuff.Value;
                    break;
                case SBuffUpdateType.ReplaceUsingHigh:
                    if (srcBuff.Value <= targetBuff.Value)
                    {
                        srcBuff.Value = targetBuff.Value;
                    }
                    break;
                case SBuffUpdateType.Keep:
                    break;
            }
        }
    }
}
