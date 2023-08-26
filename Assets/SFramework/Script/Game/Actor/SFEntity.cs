using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Game.Actor
{
    public interface ISFEntity
    {
        /// <summary>
        /// ��ȡʵ���š�
        /// </summary>
        string Id{get;}

        /// <summary>
        /// ��ȡʵ����Դ���ơ�
        /// </summary>
        string EntityAssetName{get;}

        /// <summary>
        /// ��ȡʵ��ʵ����
        /// </summary>
        GameObject Instance{get;}

        /// <summary>
        /// ʵ���ʼ����
        /// </summary>
        /// <param name="entityId">ʵ���š�</param>
        /// <param name="entityAssetName">ʵ����Դ���ơ�</param>
        void Init(int entityId, string entityAssetName);

        /// <summary>
        /// ʵ����ա�
        /// </summary>
        void Recycle();

        /// <summary>
        /// ʵ����ʾ��
        /// </summary>
        /// <param name="userData">�û��Զ������ݡ�</param>
        void Show(object userData);

        /// <summary>
        /// ʵ�����ء�
        /// </summary>
        /// <param name="isShutdown">�Ƿ��ǹر�ʵ�������ʱ������</param>
        /// <param name="userData">�û��Զ������ݡ�</param>
        void Hide(bool isShutdown, object userData);

        /// <summary>
        /// ʵ�帽����ʵ�塣
        /// </summary>
        /// <param name="childEntity">���ӵ���ʵ�塣</param>
        /// <param name="userData">�û��Զ������ݡ�</param>
        void Attached(ISFEntity childEntity, object userData);

        /// <summary>
        /// ʵ������ʵ�塣
        /// </summary>
        /// <param name="childEntity">�������ʵ�塣</param>
        /// <param name="userData">�û��Զ������ݡ�</param>
        void Detached(ISFEntity childEntity, object userData);

        /// <summary>
        /// ʵ�帽����ʵ�塣
        /// </summary>
        /// <param name="parentEntity">�����ӵĸ�ʵ�塣</param>
        /// <param name="userData">�û��Զ������ݡ�</param>
        void AttachTo(ISFEntity parentEntity, object userData);

        /// <summary>
        /// ʵ������ʵ�塣
        /// </summary>
        /// <param name="parentEntity">������ĸ�ʵ�塣</param>
        /// <param name="userData">�û��Զ������ݡ�</param>
        void DetachFrom(ISFEntity parentEntity, object userData);

        /// <summary>
        /// ʵ����ѯ��
        /// </summary>
        /// <param name="elapseSeconds">�߼�����ʱ�䣬����Ϊ��λ��</param>
        /// <param name="realElapseSeconds">��ʵ����ʱ�䣬����Ϊ��λ��</param>
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
