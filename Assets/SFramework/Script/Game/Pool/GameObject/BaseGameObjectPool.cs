using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Pool
{
    public abstract class BaseGameObjectPool : IPool<GameObject>
    {
        //设置物体的隐藏位置
        private float _defaultPosX = 99999f;
        private float _defaultPosY = 99999f;
        private float _defaultPosZ = 99999f;

        public float DefaultPosX { get { return _defaultPosX; } set { _defaultPosX = value; } }
        public float DefaultPosY { get { return _defaultPosY; } set { _defaultPosY = value; } }
        public float DefaultPosZ { get { return _defaultPosZ; } set { _defaultPosZ = value; } }
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
            return Request(0);
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

        public GameObject Prefab
        {
            set
            {
                mPrefab = value;
            }

            get
            {
                return mPrefab;
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
            info.PoolName = mPoolName;
            if (lifetime > 0)
            {
                info.Lifetime = lifetime;
            }

            return returnObj;
        }

        protected void SetDefaultPosition(Transform ta)
        {
            ta.localPosition = new Vector3(_defaultPosX, _defaultPosY, _defaultPosZ);
            ta.transform.localScale = new Vector3(1,1,1);
        }

    }
}