using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PUtils
{
    public class QueueGameObjectPool : BaseGameObjectPool
    {
        /// <summary>
        /// 队列，存放对象池中没有用到的对象，即可分配对象
        /// </summary>
        protected Queue mQueue;

        protected override void newInit()
        {
            mQueue = new Queue();
        }

        public override GameObject Request()
        {
            return Request(0); //不会通过对象池自动释放。只能通过对象自己来释放
        }

        /// <summary>
        /// 生成一个对象
        /// </summary>
        /// <param name="position">起始坐标</param>
        /// <param name="lifetime">对象存在的时间</param>
        /// <returns>生成的对象</returns>
        public override GameObject Request(float lifetime)
        {
            if (mPrefab == null)
            {
                throw new MissingComponentException();
            }
            GameObject returnObj;
            if (mQueue.Count > 0)
            {
                //池中有待分配对象
                returnObj = (GameObject)mQueue.Dequeue();
            }
            else
            {
                //池中没有可分配对象了，新生成一个
                returnObj = GameObject.Instantiate(mPrefab) as GameObject;
                returnObj.transform.SetParent(mTrans);
                returnObj.SetActive(false);
            }
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
            returnObj.SetActive(true);
            return returnObj;
        }

        /// <summary>
        /// “删除对象”放入对象池
        /// </summary>
        /// <param name="obj">对象</param>
        public override void Return(GameObject obj)
        {
            //待分配对象已经在对象池中  
            if (mQueue.Contains(obj))
            {
                return;
            }
            if (mQueue.Count > mMaxCount)
            {
                //当前池中object数量已满，直接销毁
                GameObject.Destroy(obj);
            }
            else
            {
                //放入对象池，入队
                mQueue.Enqueue(obj);
                obj.SetActive(false);
            }
        }

        /// <summary>
        /// 销毁该对象池
        /// </summary>
        public virtual void Destroy()
        {
            mQueue.Clear();
        }
    }

}
