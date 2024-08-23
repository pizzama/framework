using System;
using System.Collections.Generic;
using UnityEngine;

namespace PUtils
{
    public interface IReleaseAction
    {
        void UnRegister();
    }

    public struct ActionUnRegister : IReleaseAction
    {
        private Action mOnUnRegister { get; set; }

        public ActionUnRegister(Action onUnRegsiter)
        {
            mOnUnRegister = onUnRegsiter;
        }

        public void UnRegister()
        {
            mOnUnRegister?.Invoke();
            mOnUnRegister = null;
        }
    }

    public static class ReleaseActionExtension
    {
        public static IReleaseAction UnRegisterWhenGameObjectDestroyed(this IReleaseAction unRegister, GameObject gameObject)
        {
            var trigger = gameObject.GetComponent<ActionDestoryTrigger>();

            if (!trigger)
            {
                trigger = gameObject.AddComponent<ActionDestoryTrigger>();
            }

            trigger.AddUnRegister(unRegister);

            return unRegister;
        }
    }

    public class ActionDestoryTrigger : MonoBehaviour
    {
        private readonly HashSet<IReleaseAction> mUnRegisters = new HashSet<IReleaseAction>();

        public void AddUnRegister(IReleaseAction unRegister)
        {
            mUnRegisters.Add(unRegister);
        }

        public void RemoveUnRegister(IReleaseAction unRegister)
        {
            mUnRegisters.Remove(unRegister);
        }

        private void OnDestroy()
        {
            foreach (var unRegister in mUnRegisters)
            {
                unRegister.UnRegister();
            }

            mUnRegisters.Clear();
        }
    }

    public interface IPEvent
    {
        public IReleaseAction Register(Action<IPEvent> actEvent);
        public void UnRegister(Action<IPEvent> actEvent);
        public void Trigger();
    }

    public class PEvent : IPEvent
    {
        protected PEvents _parent;
        protected Action<IPEvent> mOnEvent;

        public IReleaseAction Register(Action<IPEvent> actEvent)
        {
            mOnEvent += actEvent;
            return new ActionUnRegister(() => { UnRegister(actEvent); }); ;
        }

        public void UnRegister(Action<IPEvent> actEvent)
        {
            mOnEvent -= actEvent;
        }

        public void Trigger()
        {
            mOnEvent?.Invoke(this);
        }

    }


    public class PEvents
    {
        private static PEvents mGlobalEvents = new PEvents();

        public static T Get<T>() where T : IPEvent
        {
            return mGlobalEvents.GetEvent<T>();
        }

        public static IReleaseAction RegisterGlobal<T>(Action<IPEvent> actEvent) where T:IPEvent, new()
        {
            T t = mGlobalEvents.AddEvent<T>();
            return t.Register(actEvent);
        }

        public static T Register<T>() where T : IPEvent, new()
        {
            return mGlobalEvents.AddEvent<T>();
        }

        private Dictionary<Type, IPEvent> mTypeEvents = new Dictionary<Type, IPEvent>();

        public T AddEvent<T>() where T : IPEvent, new()
        {
            T rt = GetEvent<T>();
            if (rt == null)
            {
                rt = new T();
                mTypeEvents.Add(typeof(T), rt);
            }

            return rt;
        }

        public T GetEvent<T>() where T : IPEvent
        {
            IPEvent e;

            if (mTypeEvents.TryGetValue(typeof(T), out e))
            {
                return (T)e;
            }

            return default;
        }

        public T GetOrAddEvent<T>() where T : IPEvent, new()
        {
            var eType = typeof(T);
            if (mTypeEvents.TryGetValue(eType, out var e))
            {
                return (T)e;
            }

            var t = new T();
            mTypeEvents.Add(eType, t);
            return t;
        }

    }
}