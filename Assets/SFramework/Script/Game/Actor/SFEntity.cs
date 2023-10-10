using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Actor
{
    public interface ISFEntity
    {
        string EntityId{get;}
        string EntityAssetName{get;}
        GameObject Instance{get;}
        void SetData(string entityId, string entityAssetName);
        void Recycle();
        void Show();
        void Destory();
        void Attached(ISFEntity childEntity);
        void Detached(ISFEntity childEntity);
    }

    public class SFEntity : MonoBehaviour, ISFEntity
    {
        [SerializeField]
        private string _entityId;
        public string EntityId { get => _entityId; }
        [SerializeField]
        private string _entityAssetName;
        public string EntityAssetName { get => _entityAssetName; }

        public GameObject Instance => throw new System.NotImplementedException();

        public void Attached(ISFEntity childEntity)
        {
        }

        public void Detached(ISFEntity childEntity)
        {
        }

        public void Destory()
        {
        }

        public void SetData(string entityId, string entityAssetName)
        {
            _entityId = entityId;
            _entityAssetName = entityAssetName;
        }

        public void Recycle()
        {
        }

        public void Show()
        {
        }
    }
}
