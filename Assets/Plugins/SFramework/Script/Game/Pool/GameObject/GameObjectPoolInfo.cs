using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Pool
{
    public class GameObjectPoolInfo : MonoBehaviour
    {
        /// <summary>
        /// 对象显示的持续时间，若=0，则不隐藏
        /// </summary>
        [HideInInspector] public float Lifetime = 0;
        /// <summary>
        /// 所属对象池的唯一id
        /// </summary>
        [HideInInspector] public string PoolName;

        void OnEnable()
        {
            if (Lifetime > 0)
            {
                StartCoroutine(CountDown(Lifetime));
            }
        }

        IEnumerator CountDown(float lifetime)
        {
            yield return new WaitForSeconds(lifetime);
            //将对象加入对象池
            GameObjectPoolManager.Instance.Return(PoolName, gameObject);
        }
    }
}
