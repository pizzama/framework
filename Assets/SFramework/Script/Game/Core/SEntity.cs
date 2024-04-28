using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework
{
    public interface ISEntity
    {
        string EntityId { get; }
        GameObject Instance { get; }
        void SetEntityData(string entityId, SView view);
        void Recycle();
        void Show();
        void DestroyEntity();
        void Attached(ISEntity childEntity);
        void Detached(ISEntity childEntity);
    }

    public class SEntity : MonoBehaviour, ISEntity
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

        public bool Checked()
        {
            return true;
        }

    }
}
