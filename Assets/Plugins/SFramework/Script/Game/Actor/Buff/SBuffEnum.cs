namespace SFramework.Actor.Buff
{
    public enum SBuffGroup
    {
        None = 0,
    }

    public enum SBuffUpdateType
    {
        Add,
        Replace,
        ReplaceUsingHigh,
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
