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
        public SBuffGroup Group { get; set; }
        public SBuffUpdateType BuffDurationUpdateType { get; set; }
        public SBuffUpdateType BuffValueUpdateType { get; set; }
        public SBuffRemoveType BuffRemoveType { get; set; }
        public void RemoveExecute();
        public void AddExecute();
        public void Execute();
        public void Tick(float deltaTime);
        public void Create();
        public void Destroy();
    }

    public abstract class SBuff : ISBuff
    {
        private int _id;
        private int _buffId;
        private float _duration;
        private int _curStack;
        private int _maxStack;
        private string _src;
        private string _target;
        private float _value;
        private int _sort;
        private int _score;
        private SBuffGroup _group;
        private SBuffUpdateType _buffDurationUpdateType; //buff duration update method
        private SBuffUpdateType _buffValueUpdateType; //buff value update method
        private SBuffRemoveType _buffRemoveType;
        public int ID
        {
            get => _id;
            set => _id = value;
        }
        public int BuffId
        {
            get => _buffId;
            set => _buffId = value;
        }
        public float Duration
        {
            get => _duration;
            set => _duration = value;
        }
        public int CurStack
        {
            get => _curStack;
            set => _curStack = value;
        }
        public int MaxStack
        {
            get => _maxStack;
            set => _maxStack = value;
        }
        public string Src
        {
            get => _src;
            set => _src = value;
        }
        public string Target
        {
            get => _target;
            set => _target = value;
        }
        public float Value
        {
            get => _value;
            set => _value = value;
        }
        public int Sort
        {
            get => _sort;
            set => _sort = value;
        }
        public int Score
        {
            get => _score;
            set => _score = value;
        }
        public SBuffGroup Group
        {
            get => _group;
            set => _group = value;
        }
        public SBuffUpdateType BuffDurationUpdateType
        {
            get => _buffDurationUpdateType;
            set => _buffDurationUpdateType = value;
        }
        public SBuffUpdateType BuffValueUpdateType
        {
            get => _buffValueUpdateType;
            set => _buffValueUpdateType = value;
        }
        public SBuffRemoveType BuffRemoveType
        {
            get => _buffRemoveType;
            set => _buffRemoveType = value;
        }
        public abstract void AddExecute();
        public abstract void Execute();
        public abstract void RemoveExecute();
        public abstract void Tick(float deltaTime);
        public abstract void Create();
        public abstract void Destroy();
    }
}
