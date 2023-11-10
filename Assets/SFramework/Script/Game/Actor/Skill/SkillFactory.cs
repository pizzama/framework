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

        private Dictionary<string, Type> _skills;
        private SkillFactory()
        {
            _skills = new Dictionary<string, Type>();
        }


        public void RegisterSkill<T>(T value)where T : ISkillScript
        {
            _skills.Add(value.GetSkillID(), value.GetType());
        }

        public IEnumerable<T> GetInstancesByType<T>()
        {
            var type = typeof(T);
            return _skills.Values.Where(Instance => type.IsInstanceOfType(Instance)).Cast<T>();
        }

        public ISkillScript CreateSkill(SEntity source, string skillID, List<SEntity> targets = default, List<SEntity> sources = default)
        {
            Type skillScript = null;
            _skills.TryGetValue(skillID, out skillScript);
            if (skillScript != null)
            {
                ISkillScript script = (ISkillScript)Activator.CreateInstance(skillScript);
                script.Create(source, skillID, targets, sources);
                return script;
            }

            return null;
        }

    }
}
