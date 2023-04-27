using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PUtils
{
    public abstract class BaseGameObjectPool : IPool<GameObject>
    {
        /// <summary>
        /// 默认最大容量
        /// </summary>
        protected const int mDefaultMaxCount = 10;
        /// <summary>
        /// 对象池中存放最大数量
        /// </summary>
        protected int mMaxCount;

        /// <summary>
        /// 该对象池的transform
        /// </summary>
        protected Transform mTrans;
        /// <summary>
        /// 每个对象池的名称，当唯一id
        /// </summary>
        protected string mPoolName;

        /// <summary>
        /// 对象预设
        /// </summary>
        protected GameObject mPrefab;

        public BaseGameObjectPool()
        {
            mMaxCount = mDefaultMaxCount;
            newInit();
        }

        protected virtual void newInit()
        {

        }
        public void Prewarm(int num)
        {
            mMaxCount = num;
        }

        public virtual GameObject Request()
        {
            throw new System.NotImplementedException();
        }

        public virtual GameObject Request(float lifetime)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Return(GameObject member)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Init(string poolName, Transform trans)
        {
            mPoolName = poolName;
            mTrans = trans;
        }

        public GameObject prefab
        {
            set
            {
                mPrefab = value;
            }
        }

        protected GameObject addLifeTimeInfo(GameObject returnObj, float lifetime)
        {
            //使用PrefabInfo脚本保存returnObj的一些信息
            GameObjectPoolInfo info = returnObj.GetComponent<GameObjectPoolInfo>();
            if (info == null)
            {
                info = returnObj.AddComponent<GameObjectPoolInfo>();
            }
            info.poolName = mPoolName;
            if (lifetime > 0)
            {
                info.lifetime = lifetime;
            }

            return returnObj;
        }

    }
}