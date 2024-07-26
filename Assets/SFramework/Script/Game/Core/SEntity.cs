using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;
using UnityEngine;

namespace SFramework
{
    public interface ISEntity
    {
        string EntityId { get; }
        GameObject Instance { get; }
        void Recycle();
        void Show();
        void DestroyEntity();
        void Attached(ISEntity childEntity);
        void Detached(ISEntity childEntity);
    }

    public abstract class SEntity : MonoBehaviour, ISEntity
    {
        [SerializeField]
        private string _entityId;
        public string EntityId { get => _entityId; set => _entityId = value; }
        public GameObject Instance => throw new System.NotImplementedException();
        protected virtual void Start()
        {
            initEntity();
        }

        private void OnDestroy()
        {
            DestroyEntity();
        }

        protected virtual void initEntity()
        {
        }

        public void Attached(ISEntity childEntity)
        {
        }

        public void Detached(ISEntity childEntity)
        {
        }

        public abstract void DestroyEntity();

        public void SetEntity(string entityId)
        {
            _entityId = entityId;

        }

        public abstract void Recycle();
        public abstract void Show();

        public bool Checked()
        {
            return true;
        }

    }
}
