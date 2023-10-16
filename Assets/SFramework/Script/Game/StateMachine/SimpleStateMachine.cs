using System;
using System.Collections.Generic;
using UnityEngine;
using SFramework.Event;

namespace SFramework.StateMachine
{
    public interface ISFStateMachine
    {
        bool IsTriggerCallback { get; set; }
    }


    public class SimpleStateMachine<T> : ISFStateMachine where T : struct, IComparable, IConvertible, IFormattable
    {
        public bool IsTriggerCallback { get; set; }
        public GameObject Target;
        public T CurrentState { get; protected set; }
        public T PreviousState { get; protected set; }
        public delegate void OnStateChangeDelegate();
        public OnStateChangeDelegate OnStateChange;

        public SimpleStateMachine(GameObject target, bool IsTriggerCallback)
        {
            this.Target = target;
            this.IsTriggerCallback = IsTriggerCallback;
        }

        public virtual void ChangeState(T newState)
        {
            if (EqualityComparer<T>.Default.Equals(newState, CurrentState))
            {
                return;
            }

            // we store our previous character movement state
            PreviousState = CurrentState;
            CurrentState = newState;

            OnStateChange?.Invoke();

            if (IsTriggerCallback)
            {
                SFEventManager.TriggerEvent(new SFStateChangeEvent<T>(this));
            }
        }
    }

    public struct SFStateChangeEvent<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        public GameObject Target;
        public SimpleStateMachine<T> TargetStateMachine;
        public T NewState;
        public T PreviousState;

        public SFStateChangeEvent(SimpleStateMachine<T> stateMachine)
        {
            Target = stateMachine.Target;
            TargetStateMachine = stateMachine;
            NewState = stateMachine.CurrentState;
            PreviousState = stateMachine.PreviousState;
        }
    }
}