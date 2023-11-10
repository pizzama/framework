using System;
using UnityEngine;

public sealed class SkillFactory
{
    private static readonly Lazy<SkillFactory> lazy =
        new Lazy<SkillFactory>(() => new SkillFactory());
    public static SkillFactory Instance { get { return lazy.Value; } }
}
