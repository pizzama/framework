namespace SFramework.Actor.Buff
{
    public interface ISBuff
    {
        public int ID { get; set; }
        public int BuffId { get; set; }
        public float Duration { get; set; }
        public int CurStack { get; set; }
        public int MaxStack { get; set; }
        public string Src { get; set; }
        public string Target { get; set; }
        public float Value { get; set; }
        public int Sort { get; set; }
        public int Group { get; set; }
        public SBuffUpdateType BuffUpdateType { get; set; }
        public SBuffRemoveType BuffRemoveType { get; set; }
    }

    public class SBuff : ISBuff
    {
        public int ID { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int BuffId { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public float Duration { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int CurStack { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int MaxStack { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Src { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Target { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public float Value { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Sort { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Group { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public SBuffUpdateType BuffUpdateType { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public SBuffRemoveType BuffRemoveType { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
