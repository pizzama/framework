using SFramework.Actor;
using UnityEngine;
using SFramework.Actor.Ability;
using SFramework.StateMachine;
public class SFAbilityActor : SFEntity
{
    [SerializeField] protected SFActorController actControl;
    [SerializeField] protected SFAbility[] _abilities;
    public SimpleStateMachine<SFAbilityStates.AbilityStates> MovementMachine;
    public SimpleStateMachine<SFAbilityStates.AbilityConditions> ConditionMachine;
    protected virtual void Start()
    {
        initialization();
    }

    protected virtual void Update()
    {
        foreach (SFAbility ability in _abilities)
        {
            if (ability.enabled && ability.AbilityInitialized)
            {
                ability.UpdateAbility();
            }
        }
    }

    private void initialization()
    {
        actControl = GetComponent<SFActorController>();
        MovementMachine = new SimpleStateMachine<SFAbilityStates.AbilityStates>(gameObject, false);
        ConditionMachine = new SimpleStateMachine<SFAbilityStates.AbilityConditions>(gameObject, false);
        cacheAbilities();
    }

    protected void cacheAbilities()
    {
        // we grab all abilities at our level
        _abilities = gameObject.GetComponents<SFAbility>();
        for (int i = 0; i < _abilities.Length; i++)
        {
            SFAbility ab = _abilities[i];
            ab.InitAbility();
        }
    }
}