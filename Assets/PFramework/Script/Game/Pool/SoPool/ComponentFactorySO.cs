using System.Collections.Generic;
using UnityEngine;

namespace PUtils
{
    public abstract class ComponentFactorySO<T> : FactorySO<T>
    {
        protected Transform _parent;
        protected List<Transform> _targets;

        public void SetParent(Transform t)
        {
            _parent = t;
        }

        public List<Transform> GetTargets()
        {
            if (_targets == null)
                _targets = new List<Transform>();
            return _targets;
        }

        public void AddTarget(Transform t)
        {
            if (_targets == null)
                _targets = new List<Transform>();
            if (!_targets.Contains(t))
                _targets.Add(t);
        }

        public void SetTargets(List<Transform> t)
        {
            _targets = t;
        }

        public abstract void Refresh();

        public abstract void Reset();
    }
}
