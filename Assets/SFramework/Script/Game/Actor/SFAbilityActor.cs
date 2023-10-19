using SFramework.Actor;
using UnityEngine;
using SFramework.Actor.Ability;
using SFramework.StateMachine;
using System.Collections.Generic;
using System;

public class SFAbilityActor : SFEntity
{
    [SerializeField] protected SFActorController actControl;
    [SerializeField] protected SFAbility[] _abilities;
    public SimpleStateMachine<SFAbilityStates.AbilityStates> MovementMachine;
    public SimpleStateMachine<SFAbilityStates.AbilityConditions> ConditionMachine;
    private Dictionary<Type, List<AbilityEvent>> _subscribersList;

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

    public override void InitEntity()
    {
        base.InitEntity();
        _subscribersList = new Dictionary<Type, List<AbilityEvent>>();
        actControl = GetComponent<SFActorController>();
        MovementMachine = new SimpleStateMachine<SFAbilityStates.AbilityStates>(gameObject, false);
        ConditionMachine = new SimpleStateMachine<SFAbilityStates.AbilityConditions>(gameObject, false);
        initAbilities();
    }

    public override void DestroyEntity()
    {
        base.DestroyEntity();
    }

    public void AddListener(SFAbility ability, string name, Action callback)
    {
        Type eventType = ability.GetType();

        if (!_subscribersList.ContainsKey(eventType))
        {
            _subscribersList[eventType] = new List<AbilityEvent>();
        }

        if (!SubscriptionExists(eventType, name))
        {
            AbilityEvent evt = new AbilityEvent() { Name = name, Callback = callback };
            _subscribersList[eventType].Add(evt);
        }
    }

    public void RemoveListener(SFAbility ability, string name)
    {
        Type eventType = ability.GetType();

        if (!_subscribersList.ContainsKey(eventType))
        {
            return;
        }

        List<AbilityEvent> subscriberList = _subscribersList[eventType];

        for (int i = subscriberList.Count - 1; i >= 0; i--)
        {
            if (subscriberList[i].Name == name)
            {
                subscriberList.Remove(subscriberList[i]);
                if (subscriberList.Count == 0)
                {
                    _subscribersList.Remove(eventType);
                }

                return;
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

    private bool SubscriptionExists(Type type, string name)
    {
        List<AbilityEvent> receivers;

        if (!_subscribersList.TryGetValue(type, out receivers)) return false;

        bool exists = false;

        for (int i = receivers.Count - 1; i >= 0; i--)
        {
            if (receivers[i].Name == name)
            {
                exists = true;
                break;
            }
        }

        return exists;
    }
}