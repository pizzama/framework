using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Pool
{
    public class ListGameObjectPool : BaseGameObjectPool
    {
        //可以管理所有对象池里边的对象。
        private List<GameObject> _goList = new List<GameObject>();

        public override GameObject Request(float lifetime)
        {
            if (mPrefab == null)
            {
                throw new MissingComponentException();
            }

            int key = mPrefab.GetInstanceID();

            GameObject returnObj = null;

            for (int i = _goList.Count - 1; i >= 0; i--)
            {
                var temp = _goList[i];
                if (temp == null)
                {
                    _goList.Remove(temp);
                    continue;
                }
                if (temp.activeSelf == false)
                {
                    returnObj = temp;
                }
            }

            if(returnObj == null)
            {
                //池中没有可分配对象了，新生成一个
                returnObj = GameObject.Instantiate(mPrefab) as GameObject;
                returnObj.transform.SetParent(mTrans);
                returnObj.SetActive(false);
                _goList.Add(returnObj);
            }

            returnObj = addLifeTimeInfo(returnObj, lifetime);
            returnObj.SetActive(true);
            return returnObj;
        }

        public override void Return(GameObject go)
        {
            if (go == null)
                return;
            if (_goList.Count > mMaxCount)
                _goList.Remove(go);
            else
                go.SetActive(false);
        }

    }
}