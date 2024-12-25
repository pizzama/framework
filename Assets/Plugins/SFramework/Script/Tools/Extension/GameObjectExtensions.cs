using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Extension
{
    public static class GameObjectExtensions
    {
        public static Dictionary<string, GameObject> CollectAllGameObjects(this GameObject rootGameObject, string careTagName = "Untagged")
        {
            Dictionary<string, GameObject> result = new Dictionary<string, GameObject>();
            rootGameObject.CollectAllGameObject(ref result, careTagName);
            return result;
        }

        // this method is too heavy, fist using FindGameObjectsWithTag instead
        public static void CollectAllGameObject(this GameObject gameObject, ref Dictionary<string, GameObject> objectMap, string careTagName = "Untagged")
        {
            if (gameObject.tag == careTagName)
            {
                if (objectMap.ContainsKey(gameObject.name))
                {
                    Debug.LogWarning($"collect gameobject {gameObject.name} has already collected, it will be ignore:");
                    // objectMap[gameObject.name] = gameObject;
                }
                else
                    objectMap.Add(gameObject.name, gameObject);
            }

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                GameObject go = gameObject.transform.GetChild(i).gameObject;
                go.CollectAllGameObject(ref objectMap, careTagName);
            }
        }

        public static List<T> CollectAllComponent<T>(this Transform parent) where T : Component
        {
            List<T> compones = new List<T>();
            if (parent.TryGetComponent(out T comp))
            {
                compones.Add(comp);
            }
            parent.CollectAllComponent<T>(compones);
            return compones;
        }

        public static void CollectAllComponent<T>(this Transform parent, List<T> components)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (child.TryGetComponent(out T comp))
                {
                    components.Add(comp);
                }
                child.CollectAllComponent<T>(components);
            }
        }

        public static T GetEntityComponent<T>(this Component component) where T : Component
        {
            T tcompoent = component.GetComponent<T>();
            if (tcompoent is ISEntity root)
            {
                root.Show();
            }

            return tcompoent;
        }

        public static void UnRigistEventTrigger(this UIBehaviour _ui, EventTriggerType _eventTriggerType)
        {
            if (_ui == null)
            {
                Debug.LogError("invalid UIBehaviour can not be a trigger content.");
                return;
            }

            EventTrigger eventTrigger = _ui.GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                return;
            }

            var entrys = eventTrigger.triggers;
            for (int i = entrys.Count - 1; i > 0; i--)
            {
                var entry = entrys[i];
                if (entry.eventID == _eventTriggerType)
                    entrys.Remove(entry);
            }

        }

        public static void UnRigistAllEventTrigger(this UIBehaviour _ui)
        {
            if (_ui == null)
            {
                Debug.LogError("invalid UIBehaviour can not be a trigger content.");
                return;
            }

            EventTrigger eventTrigger = _ui.GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                return;
            }

            eventTrigger.triggers.Clear();
        }

        /// <summary>
        /// 注册ui 增加eventtrigger的行为
        /// 如按钮增加 btn.RigistEventTrigger(EventTriggerType.PointerDown, handle);
        /// </summary>
        /// <param name="_ui"></param>
        /// <param name="_eventTriggerType"></param>
        /// <param name="_callback"></param>
        public static void RigistEventTrigger(this UIBehaviour _ui, EventTriggerType _eventTriggerType, Action<PointerEventData> _callback)
        {
            if (_ui == null)
            {
                Debug.LogError("invalid UIBehaviour can not be a trigger content.");
                return;
            }
            else
            {
                EventTrigger eventTrigger = _ui.GetComponent<EventTrigger>();
                if (eventTrigger == null)
                {
                    eventTrigger = _ui.gameObject.AddComponent<EventTrigger>();
                }

                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = _eventTriggerType;
                entry.callback.AddListener((_pointData) =>
                {
                    var baseData = _pointData as PointerEventData;
                    _callback?.Invoke(baseData);
                });
                eventTrigger.triggers.Add(entry);
            }
        }
        public static void RigistEventTrigger(this UIBehaviour _ui, EventTriggerType _eventTriggerType, Action _callback)
        {
            if (_ui == null)
            {
                Debug.LogError("invalid UIBehaviour can not be a trigger content.");
                return;
            }
            else
            {
                EventTrigger eventTrigger = _ui.GetComponent<EventTrigger>();
                if (eventTrigger == null)
                {
                    eventTrigger = _ui.gameObject.AddComponent<EventTrigger>();
                }



                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = _eventTriggerType;
                entry.callback.AddListener((_pointData) =>
                {
                    //var baseData = _pointData as PointerEventData;
                    _callback?.Invoke();
                });
                eventTrigger.triggers.Add(entry);
            }
        }
    }
}
