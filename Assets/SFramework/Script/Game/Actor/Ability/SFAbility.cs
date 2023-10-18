using UnityEngine;
using SFramework.StateMachine;
using SFramework.Event;

namespace SFramework.Actor.Ability
{
    public abstract class SFAbility : MonoBehaviour, ISFEventListener<SFGameEvent>
    {
        [Header("Permission")]
        public bool AbilityPermitted = true;
        public bool AbilityInitialized { get; private set; }
        protected SFActorController actControl;
        protected SFAbilityActor act;
        protected SimpleStateMachine<SFAbilityStates.AbilityStates> movementMachine;
        protected SimpleStateMachine<SFAbilityStates.AbilityConditions> conditionMachine;
        /// an array containing all the blocking movement states. If the Character is in one of these states and tries to trigger this ability, it won't be permitted. Useful to prevent this ability from being used while Idle or Swimming, for example.
        public SFAbilityStates.AbilityStates[] BlockingMovementStates;
        /// an array containing all the blocking condition states. If the Character is in one of these states and tries to trigger this ability, it won't be permitted. Useful to prevent this ability from being used while dead, for example.
        public SFAbilityStates.AbilityConditions[] BlockingConditionStates;

        public virtual void InitAbility()
        {
            actControl = GetComponent<SFActorController>();
            act = GetComponent<SFAbilityActor>();
            movementMachine = act.MovementMachine;
            conditionMachine = act.ConditionMachine;
            init();
            AbilityInitialized = true;
        }

        public virtual void DestroyAbility()
        {
            
        }

        protected abstract void init();


        public virtual void UpdateAbility()
        {

        }

        public virtual void TriggerEvent(SFGameEvent eventObject)
        {
            throw new System.NotImplementedException();
        }

        public virtual bool AbilityAuthorized
        {
            get
            {
                if (act != null)
                {
                    if ((BlockingMovementStates != null) && (BlockingMovementStates.Length > 0))
                    {
                        for (int i = 0; i < BlockingMovementStates.Length; i++)
                        {
                            if (BlockingMovementStates[i] == (act.MovementMachine.CurrentState))
                            {
                                return false;
                            }
                        }
                    }

                    if ((BlockingConditionStates != null) && (BlockingConditionStates.Length > 0))
                    {
                        for (int i = 0; i < BlockingConditionStates.Length; i++)
                        {
                            if (BlockingConditionStates[i] == (act.ConditionMachine.CurrentState))
                            {
                                return false;
                            }
                        }
                    }
                }
                return AbilityPermitted;
            }
        }
    }
}