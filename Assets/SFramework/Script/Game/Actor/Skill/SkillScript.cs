using System.Collections.Generic;

namespace SFramework.Game.SActor.Skill
{
    public interface ISkillScript
    {
        public void Create(SEntity source, string skillId, List<SEntity> targets = default, List<SEntity> sources = default);
        public void PreExecute();
        public void Execute();
        public void AfterExecute();
    }

    public abstract class SkillScript : ISkillScript
    {
        protected SEntity attackSource; // the source of the attack
        protected string attackSkillId; // 
        private List<SEntity> _targets; //the enemies which will be attacked;
        private List<SEntity> _sources; //my friends

        public string SkillID { get {return attackSkillId; } }
        public SkillScript()
        {

        }
        public void Create(SEntity source, string skillId, List<SEntity> targets = default, List<SEntity> sources = default)
        {
            attackSource = source;
            attackSkillId = skillId;
            _targets = targets;
            _sources = sources;
        }
        public abstract void PreExecute();
        public abstract void Execute();
        public abstract void AfterExecute();

    }
}
