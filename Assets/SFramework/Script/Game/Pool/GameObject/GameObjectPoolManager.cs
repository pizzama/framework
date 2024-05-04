using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Pool
{
    public class GameObjectPoolManager
    {
        private static GameObjectPoolManager _instance;
        public static GameObjectPoolManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObjectPoolManager();
                }

                return _instance;
            }
        }
        private GameObjectPoolManager() { }
        /// <summary>
        /// 存放所有的对象池
        /// </summary>
        private Dictionary<string, BaseGameObjectPool> _poolDic = new Dictionary<string, BaseGameObjectPool>();
        /// <summary>
        /// 对象池在场景中的父控件
        /// </summary>
        private Transform _parentTrans;

        /// <summary>
        /// 创建一个新的对象池
        /// </summary>
        /// <typeparam name="T">对象池类型</typeparam>
        /// <param name="poolName">对象池名称，唯一id</param>
        /// <returns>对象池对象</returns>
        private GameObject _allPool;
        public T CreateGameObjectPool<T>(string poolName) where T : BaseGameObjectPool, new()
        {
            if (_poolDic.ContainsKey(poolName))
            {
                return (T)_poolDic[poolName];
            }
            //生成一个新的GameObject存放所有的对象池对象
            if (_allPool == null)
                _allPool = new GameObject("[AllPool]");
            _parentTrans = _allPool.transform;
            GameObject obj = new GameObject(poolName);
            obj.transform.SetParent(_parentTrans);
            T pool = new T();
            pool.Init(poolName, obj.transform);
            _poolDic.Add(poolName, pool);
            return pool;
        }

        public bool HasPool(string pooName)
        {
            return _poolDic.ContainsKey(pooName);
        }

        /// <summary>
        /// 从对象池中取出新的对象
        /// </summary>
        /// <param name="poolName">对象池名称</param>
        /// <param name="position">对象新坐标</param>
        /// <param name="lifeTime">对象显示时间</param>
        /// <returns>新对象</returns>
        public GameObject Request(string poolName, float lifeTime)
        {
            if (_poolDic.ContainsKey(poolName))
            {
                return _poolDic[poolName].Request(lifeTime);
            }
            return null;
        }

        public GameObject Request<T>(string poolName, GameObject prefab, float lifeTime) where T : BaseGameObjectPool, new()
        {
            if (prefab == null)
                throw new NotFoundException("Prefab Not be null");
            GameObject ob = Request(poolName, lifeTime);
            if(ob == null)
            {
                T pool = CreateGameObjectPool<T>(poolName);
                pool.Prefab = prefab;
                return pool.Request(lifeTime);
            }
            else
            {
                if(!ob.activeSelf)
                    ob.SetActive(true);
                return ob;
            }
        }

        /// <summary>
        /// 将对象存入对象池中
        /// </summary>
        /// <param name="poolName">对象池名称</param>
        /// <param name="go">对象</param>
        public void Return(string poolName, GameObject go)
        {
            if (_poolDic.ContainsKey(poolName))
            {
                _poolDic[poolName].Return(go);
            }
        }

        public void Return(GameObject go)
        {
            GameObjectPoolInfo info = go.GetComponent<GameObjectPoolInfo>();
            Return(info.PoolName, go);
        }

        public int GetPoolCount()
        {
            return _poolDic.Count;
        }

        /// <summary>
        /// 销毁所有对象池操作
        /// </summary>
        public void Destroy()
        {
            _poolDic.Clear();
            GameObject.Destroy(_parentTrans);
        }
    }
}