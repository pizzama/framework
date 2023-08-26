using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Game.Actor
{
    public interface ISFEntity
    {
        /// <summary>
        /// 获取实体编号。
        /// </summary>
        string Id{get;}

        /// <summary>
        /// 获取实体资源名称。
        /// </summary>
        string EntityAssetName{get;}

        /// <summary>
        /// 获取实体实例。
        /// </summary>
        GameObject Instance{get;}

        /// <summary>
        /// 实体初始化。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <param name="entityAssetName">实体资源名称。</param>
        void Init(int entityId, string entityAssetName);

        /// <summary>
        /// 实体回收。
        /// </summary>
        void Recycle();

        /// <summary>
        /// 实体显示。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        void Show(object userData);

        /// <summary>
        /// 实体隐藏。
        /// </summary>
        /// <param name="isShutdown">是否是关闭实体管理器时触发。</param>
        /// <param name="userData">用户自定义数据。</param>
        void Hide(bool isShutdown, object userData);

        /// <summary>
        /// 实体附加子实体。
        /// </summary>
        /// <param name="childEntity">附加的子实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        void Attached(ISFEntity childEntity, object userData);

        /// <summary>
        /// 实体解除子实体。
        /// </summary>
        /// <param name="childEntity">解除的子实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        void Detached(ISFEntity childEntity, object userData);

        /// <summary>
        /// 实体附加子实体。
        /// </summary>
        /// <param name="parentEntity">被附加的父实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        void AttachTo(ISFEntity parentEntity, object userData);

        /// <summary>
        /// 实体解除子实体。
        /// </summary>
        /// <param name="parentEntity">被解除的父实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        void DetachFrom(ISFEntity parentEntity, object userData);

        /// <summary>
        /// 实体轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        void Update(float elapseSeconds, float realElapseSeconds);


    }

    public class SFEntity : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
