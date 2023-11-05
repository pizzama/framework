using System.Collections;
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
            if (objectMap.ContainsKey(gameObject.name))
            {
                //Debug.LogWarning($"collect gameobject {gameObject.name} has already collected, it will be ignore:");
                 objectMap[gameObject.name] = gameObject;
            }
            else
            {
                if(gameObject.tag == careTagName)
                    objectMap.Add(gameObject.name, gameObject);
            }

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                GameObject go = gameObject.transform.GetChild(i).gameObject;
                go.CollectAllGameObject(ref objectMap, careTagName);
                
            }
        }
    }
}
