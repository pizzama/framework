using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;

namespace SFramework.Game.SActor.Skill
{
    public abstract class SkillScript
    {
        protected SEntity attackSource; // the source of the attack
        protected string attackSkillId; // 
        private List<SEntity> _targets; //the enemies which will be attacked;
        private List<SEntity> _sources; //my friends
        public SkillScript(SEntity source, string skillId, List<SEntity> targets = default, List<SEntity> sources = default)
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
