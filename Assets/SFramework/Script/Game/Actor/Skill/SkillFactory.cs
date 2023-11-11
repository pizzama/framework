using System;
using System.Collections.Generic;
using System.Linq;

namespace SFramework.Game.SActor.Skill
{
    public sealed class SkillFactory
    {
        private static readonly Lazy<SkillFactory> lazy =
            new Lazy<SkillFactory>(() => new SkillFactory());
        public static SkillFactory Instance { get { return lazy.Value; } }

        private IOCContainer _skills;
        private SkillFactory()
        {
            _skills = new IOCContainer();
        }


        public void RegisterSkillScript(ISkillScript value)
        {
            _skills.Register(value);
        }

        public ISkillScript GetSkillScript(string key)
        {
            return _skills.Get<ISkillScript>(key);
        }

    }
}
