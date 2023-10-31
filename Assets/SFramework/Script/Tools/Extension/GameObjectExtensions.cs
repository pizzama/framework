using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Extension
{
    public static class GameObjectExtensions
    {
        public static Dictionary<string, GameObject> CollectAllGameObjects(this GameObject rootGameObject)
        {
            Dictionary<string, GameObject> result = new Dictionary<string, GameObject>();
            collectAllGameObject(result, rootGameObject);
            return result;
        }

        // this method is too heavy, fist using FindGameObjectsWithTag instead
        private static void collectAllGameObject(Dictionary<string, GameObject> objectMap, GameObject gameObject)
        {
            if (objectMap.ContainsKey(gameObject.name))
            {
                Debug.LogWarning($"collect gameobject {gameObject.name} has already collected, it will be ignore:");
                // objectMap[gameObject.name] = gameObject;
            }
            else
            {
                objectMap.Add(gameObject.name, gameObject);
            }

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                collectAllGameObject(objectMap, gameObject.transform.GetChild(i).gameObject);
            }
        }
    }
}
