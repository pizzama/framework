using System;

using System.Collections;

using System.Collections.Generic;

using UnityEngine;

namespace SFramework.Pool
{

    public struct PoolID
    {
        public int id;

        public float time;

        public PoolID(int id_, float time_)
        {
            id = id_;
            time = time_;
        }
    }


    public class MonosPool : MonoBehaviour
    {
        public static Dictionary<int, Stack<Component>> _poolGo = new Dictionary<int, Stack<Component>>();
        public static List<Component> returnTransform = new List<Component>();
        public static List<PoolID> returnTime = new List<PoolID>();
        // Use this for initialization
        void Awake()
        {
            this.name = "[MonosPool]";
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            //check is auto recyle
            if (returnTransform.Count > 0)
            {
                int c = returnTransform.Count - 1;
                for (int i = c; i > -1; i--)
                {
                    var pd = returnTime[i];
                    if (pd.time < Time.time)
                    {
                        Component C = returnTransform[i];
                        _poolGo[pd.id].Push(C);
                        C.gameObject.SetActive(false);
                        returnTransform.RemoveAt(i);
                        returnTime.RemoveAt(i);
                    }
                }

            }
        }

        //根据需要初始化每个component对象池的大小
        public static void Prewarm<T>(T key, int count) where T : Component
        {
            Stack<Component> temp;
            int name = key.GetHashCode();
            //int name = key.GetInstanceID();
            if (!_poolGo.TryGetValue(name, out temp))
            {
                temp = new Stack<Component>(count);
                _poolGo.Add(name, temp);
            }

            for (int i = 0; i < count; i++)
            {
                T go;
                //   go = GameObject.Instantiate<T>(prefab).GetComponent<T>();
                go = Instantiate<T>(key, Vector3.one * 99999, Quaternion.identity);
                go.hideFlags = HideFlags.HideInHierarchy;
                temp.Push(go);
                go.gameObject.SetActive(false);
            }
        }

        public static T Request<T>(T key, Vector3 pos, Quaternion rot) where T : Component
        {
            T go = Request(key);
            go.transform.SetPositionAndRotation(pos, rot);
            //go = Instantiate<T>(key, pos, rot);
            return go;
        }

        //可以自动回收  设置自动回收的时间
        public static T Request<T>(T key, Vector3 pos, Quaternion rot, float t) where T : Component
        {
            int name = key.GetHashCode();
            T go = Request(key, pos, rot);
            returnTransform.Add(go);
            returnTime.Add(new PoolID(name, Time.time + t));
            go.gameObject.SetActive(true);
            return go;
        }

        // 使用示例
        // Transform tf=MonoPool.Request<Transform>(keyTf);

        public static T Request<T>(T key) where T : Component
        {
            Stack<Component> qc;
            T go;
            int name = key.GetHashCode();
            if (_poolGo.TryGetValue(name, out qc))
            {
                if (qc.Count > 0)
                    go = (T)qc.Pop();
                else
                    go = Instantiate<T>(key);
            }
            else
            {
                qc = new Stack<Component>();
                _poolGo.Add(name, qc);
                go = Instantiate<T>(key);
                // go.hideFlags = HideFlags.HideInHierarchy;
            }
            go.gameObject.SetActive(true);
            return go;
        }

        public static void Return<T>(T go, T key, Action act = null) where T : Component
        {
            if (act != null)
                act();
            int name = key.GetHashCode();
            if (!_poolGo.ContainsKey(name))
            {
                _poolGo.Add(name, new Stack<Component>());
            }
            _poolGo[name].Push(go);
            go.gameObject.SetActive(false);
        }

        public ulong CombineTwoInt(int a, int b)
        {
            uint ua = (uint)a;
            ulong ub = (uint)b;
            return ub << 32 | ua;
        }
        public void DecombineTwoInt(ulong c, out int a, out int b)
        {
            a = (int)(c & 0xFFFFFFFFUL);
            b = (int)(c >> 32);
        }
    }
}