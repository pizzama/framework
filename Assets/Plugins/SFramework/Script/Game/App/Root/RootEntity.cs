using SFramework;
using UnityEngine;

namespace SFramework.Game
{
    public abstract class RootEntity : SEntity
    {
        [SerializeField]
        private RootView _parentView;

        public RootView ParentView { get => _parentView; }
        [SerializeField]
        private RootControl _parentControl;

        public RootControl ParentControl { get => _parentControl; }

        public void SetParentView(RootView view)
        {
            _parentView = view;
            _parentControl = (RootControl)view.Control;
        }

        public void SetEntityData(string eId, RootView view)
        {
            EntityId = eId;
            SetParentView(view);
        }

        public void Attached(RootEntity childEntity)
        {
            SetParentView(childEntity.ParentView);
        }

        public void Attached(RootView childEntity)
        {
            SetParentView(childEntity);
        }

        public void DeAttached(RootEntity childEntity)
        {

        }
    }
}