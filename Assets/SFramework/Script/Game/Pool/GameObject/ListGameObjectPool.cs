using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Pool
{
    public class ListGameObjectPool : BaseGameObjectPool
    {
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
                returnObj = _goList[i];
                if (returnObj == null)
                {
                    _goList.Remove(returnObj);
                    continue;
                }
                if (returnObj.activeSelf == false)
                {
                    returnObj.SetActive(true);
                    return returnObj;
                }
            }

            //池中没有可分配对象了，新生成一个
            returnObj = GameObject.Instantiate(mPrefab) as GameObject;
            returnObj.transform.SetParent(mTrans);
            returnObj.SetActive(false);
            _goList.Add(returnObj);

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