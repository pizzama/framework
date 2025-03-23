using SFramework;
using UnityEngine;

namespace SFramework.Game
{
    public abstract class RootEntity : SEntity, ILinker
    {
        [SerializeField]
        private RootView _parentView;

        protected RootEntity parentEntity;

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

        public void Attache(ISEntity entity)
        {
            if(entity is RootEntity root)
            {
                parentEntity = root;
                _parentView = root.ParentView;
                _parentControl = root.ParentControl;
            }

        }

        public void DeAttache()
        {
            parentEntity = null;
        }
    }
}