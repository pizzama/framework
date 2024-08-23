using System.Collections.Generic;

namespace SFramework.Game.SActor.Skill
{
    public interface ISSkillScript
    {
        public void Create(SEntity source, List<SEntity> targets = default, List<SEntity> sources = default);
        public void Execute();
        public void Tick();
        public void Finish();
    }

    public abstract class SSkillScript : ISSkillScript
    {
        protected SEntity attackSource; // the source of the attack
        protected List<SEntity> targets; //the enemies which will be attacked;
        protected List<SEntity> sources; //my friends

        public SSkillScript()
        {

        }
        public void Create(SEntity source, List<SEntity> targetValues = default, List<SEntity> sourceValues = default)
        {
            attackSource = source;
            targets = targetValues;
            sources = sourceValues;
        }

        public abstract void Execute();
        public abstract void Tick();
        public abstract void Finish();

    }
}
