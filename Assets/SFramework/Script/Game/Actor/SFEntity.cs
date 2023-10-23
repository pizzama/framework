using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Actor
{
    public interface ISFEntity
    {
        string EntityId { get; }
        GameObject Instance { get; }
        void SetEntityData(string entityId, SView view);
        void InitEntity();
        void Recycle();
        void Show();
        void DestroyEntity();
        void Attached(ISFEntity childEntity);
        void Detached(ISFEntity childEntity);
    }

    public class SFEntity : MonoBehaviour, ISFEntity
    {
        [SerializeField]
        private string _entityId;
        public string EntityId { get => _entityId; }

        [SerializeField]
        private SView _parentView;

        public SView ParentView { get => _parentView; }

        public GameObject Instance => throw new System.NotImplementedException();
        protected virtual void Start()
        {
            InitEntity();
        }

        private void OnDestroy()
        {
            DestroyEntity();
        }

        public virtual void InitEntity()
        {

        }

        public void Attached(ISFEntity childEntity)
        {
        }

        public void Detached(ISFEntity childEntity)
        {
        }

        public virtual void DestroyEntity()
        {
        }

        public void SetEntityData(string entityId, SView view)
        {
            _entityId = entityId;
            _parentView = view;
        }

        public void Recycle()
        {
        }

        public void Show()
        {
        }
    }
}
