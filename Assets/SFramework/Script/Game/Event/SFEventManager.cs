using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Event
{

    public interface ISFEventListenerBase { };

    public interface ISFEventListener<T> : ISFEventListenerBase
    {
        void TriggerEvent(T eventType);
    }

    public struct SFGameEvent
    {
        public object EventObj;
        public SFGameEvent(object newObject)
        {
            EventObj = newObject;
        }
        static SFGameEvent e;
        public static void Trigger(object newObject)
        {
            e.EventObj = newObject;
            SFEventManager.TriggerEvent(e);
        }
    }

    public class SFEventManager
    {
        private static Dictionary<Type, List<ISFEventListenerBase>> _subscribersList;
        static SFEventManager()
        {
            _subscribersList = new Dictionary<Type, List<ISFEventListenerBase>>();
        }

        public static void AddListener<TEvent>(ISFEventListener<TEvent> listener) where TEvent : struct
        {
            Type eventType = typeof(TEvent);

            if (!_subscribersList.ContainsKey(eventType))
            {
                _subscribersList[eventType] = new List<ISFEventListenerBase>();
            }

            if (!SubscriptionExists(eventType, listener))
            {
                _subscribersList[eventType].Add(listener);
            }
        }

        public static void RemoveListener<TEvent>(ISFEventListener<TEvent> listener) where TEvent : struct
        {
            Type eventType = typeof(TEvent);

            if (!_subscribersList.ContainsKey(eventType))
            {
                return;
            }

            List<ISFEventListenerBase> subscriberList = _subscribersList[eventType];

            for (int i = subscriberList.Count - 1; i >= 0; i--)
            {
                if (subscriberList[i] == listener)
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

        public static void TriggerEvent<TEvent>(TEvent newEvent) where TEvent : struct
        {
            List<ISFEventListenerBase> list;
            if (!_subscribersList.TryGetValue(typeof(TEvent), out list))
                return;

            for (int i = list.Count - 1; i >= 0; i--)
            {
                (list[i] as ISFEventListener<TEvent>).TriggerEvent(newEvent);
            }
        }

        private static bool SubscriptionExists(Type type, ISFEventListenerBase receiver)
        {
            List<ISFEventListenerBase> receivers;

            if (!_subscribersList.TryGetValue(type, out receivers)) return false;

            bool exists = false;

            for (int i = receivers.Count - 1; i >= 0; i--)
            {
                if (receivers[i] == receiver)
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }
    }
}
