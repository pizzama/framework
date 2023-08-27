using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Game.Actor
{
    public interface ISFEntity
    {
        string EntityId{get;}
        string EntityAssetName{get;}
        GameObject Instance{get;}
        void Init(string entityId, string entityAssetName);
        void Recycle();
        void Show();
        void Destory();
        void Attached(ISFEntity childEntity);
        void Detached(ISFEntity childEntity);
    }

    public class SFEntity : MonoBehaviour, ISFEntity
    {
        private string _entityId;
        public string EntityId { get => _entityId; }

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

        public void Init(string entityId, string entityAssetName)
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
