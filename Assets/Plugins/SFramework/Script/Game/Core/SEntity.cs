using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;
using UnityEngine;

namespace SFramework
{
    public interface ILinker
    {
        void Attache(ISEntity entity);
        void DeAttache();
    }

    public interface ISEntity
    {
        string EntityId { get; }
        GameObject Instance { get; }
        void Recycle();
        void Show();
    }

    public abstract class SEntity : MonoBehaviour, ISEntity
    {
        [SerializeField]
        private string _entityId;
        public string EntityId { get => _entityId; set => _entityId = value; }
        public GameObject Instance => throw new System.NotImplementedException();

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
