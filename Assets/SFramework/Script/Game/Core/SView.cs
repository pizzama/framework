using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace SFramework
{
    public abstract class SView : SBundle
    {
        private SControl _control;
        public SControl Control
        {
            set { _control = value; }
            get { return _control; }
        }

        public override void Install()
        {
            init();
        }

        public override void Update()
        {
            base.Update();
            viewUpdate();

        }

        public override void FixUpdate()
        {
            base.FixUpdate();
            viewFixUpdate();

        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            viewLastUpdate();

        }

        protected virtual void viewUpdate()
        {

        }

        protected virtual void viewFixUpdate()
        {

        }

        protected virtual void viewLastUpdate()
        {

        }

    }
}