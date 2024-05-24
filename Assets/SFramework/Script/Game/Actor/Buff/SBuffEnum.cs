namespace SFramework.Actor.Buff
{
    public enum SBuffUpdateType
    {
        Add,
        Replace,
        Keep,
    }

    public enum SBuffRemoveType
    {
        Clear,
        Reduce,
    }

    public struct SBuffConfig
    {
        //base data
        public int Id;
        public string Name;
        public string Desc;
        public string Icon;
        public int Priority;
        public int MaxStack;
        public string[] Tags;

        //time info
        public float Duration; // if <= 0, no duration
        public float TickTime;
    }
}
