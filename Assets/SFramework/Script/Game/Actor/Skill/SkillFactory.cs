using System;

public sealed class SkillFactory
{
    private static readonly Lazy<SkillFactory> lazy =
        new Lazy<SkillFactory>(() => new SkillFactory());
    public static SkillFactory Instance { get { return lazy.Value; } }
    private SkillFactory()
    {
        
    }

    private void initAllScript()
    {

    }
}
