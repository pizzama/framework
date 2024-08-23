using SFramework;
using SFramework.Actor;
using UnityEngine;
using SFramework.Actor.Ability;
using SFramework.StateMachine;
using SFramework.Game.App;

public class SFAbilityActor : SEntity
{
    public bool InputAuthorized = false; // whether or not need inputmanager input;
    [SerializeField] protected SFActorController actControl;
    [SerializeField] protected SFAbility[] _abilities;
    public SimpleStateMachine<ActorStates> MovementMachine;
    public SimpleStateMachine<ActorConditions> ConditionMachine;

    protected virtual void Update()
    {
        foreach (SFAbility ability in _abilities)
        {
            if (ability.enabled && ability.AbilityInitialized)
            {
                ability.UpdateAbility();
            }
        }

        if(InputAuthorized)
        {
            handleInput();
        }
    }

    protected override void initEntity()
    {
        base.initEntity();
        actControl = GetComponent<SFActorController>();
        MovementMachine = new SimpleStateMachine<ActorStates>(gameObject, false);
        ConditionMachine = new SimpleStateMachine<ActorConditions>(gameObject, false);
        initAbilities();
    }

    public override void DestroyEntity()
    {
    }

    public void TriggerEvent(ActorAction actionName, object value)
    {
        foreach (SFAbility ability in _abilities)
        {
            if (ability.enabled && ability.AbilityInitialized)
            {
                ability.HandleEvent(actionName, value);
            }
        }
    }

    protected void initAbilities()
    {
        // we grab all abilities at our level
        _abilities = gameObject.GetComponents<SFAbility>();
        for (int i = 0; i < _abilities.Length; i++)
        {
            SFAbility ab = _abilities[i];
            ab.InitAbility();
        }
    }

    protected void destroyAbilities()
    {
        _abilities = gameObject.GetComponents<SFAbility>();
        for (int i = 0; i < _abilities.Length; i++)
        {
            SFAbility ab = _abilities[i];
            ab.DestroyAbility();
        }
    }

    // use system input
    protected virtual void handleInput()
    {
        if(SFInputManager.Instance.IsMove())
        {
            TriggerEvent(ActorAction.Move, SFInputManager.Instance.PrimaryMovement);
        }
    }

    public override void Recycle()
    {
    }

    public override void Show()
    {
    }
}